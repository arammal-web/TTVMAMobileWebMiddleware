
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Shared.RequestUtility;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Enums;
using TTVMAMobileWebMiddleware.Domain.Views;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using static System.Net.Mime.MediaTypeNames;

namespace TTVMAMobileWebMiddleware.Application.Services
{
    /// <inheritdoc/>
    public class ReceiptService : IReceiptService
    {
        private readonly MOBDbContext _context;
        private readonly DLSDbContext _dLSDbContext;
        private readonly IMemoryCache _cache;
        private readonly ISequenceGeneratorService _sequenceService;
        private readonly IExternalApiService _externalApiService;

        public ReceiptService(MOBDbContext context, DLSDbContext dLSDbContext,
                                IMemoryCache cache,
                                IExternalApiService externalApiService,
                                ISequenceGeneratorService sequenceService
            )
        {
            _context = context;
            _cache = cache;
            _sequenceService = sequenceService;
            _dLSDbContext = dLSDbContext;
        }

        /// <inheritdoc/>

        /// <inheritdoc/>
        public async Task<List<ReceiptResponseDto>> CreateWithDeatailsAsync(ReceiptWithDetailRequest entity)
        {
            try
            {


                var receiptResponseDto = new List<ReceiptResponseDto>();
                // Group receipt details by ItemCategoryId
                var groupedDetails = entity.ReceiptDetails
                    .GroupBy(d => d.ItemCategoryId)
                    .ToList();

                // Get all fee categories for the grouped details
                var categoryIds = groupedDetails
                    .Where(g => g.Key.HasValue)
                    .Select(g => g.Key.Value)
                    .ToList();

                var currentYear = DateTime.UtcNow.Year; // Get last 2 digits of year
                var structureId = entity.Receipt.StructureId?.ToString("D3") ?? "000";

                // Move expensive operations outside transaction
                var feeCategories = await _context.FeeCategories
                    .Where(fc => categoryIds.Contains(fc.Id))
                    .ToDictionaryAsync(fc => fc.Id, fc => fc);

                var sequence = await _sequenceService.GetNextSequenceAsync("Receipt", entity.Receipt.StructureId, currentYear);
                var sequenceFormatted = sequence.ToString("D7");

                // Prepare all receipts and details before transaction
                var receiptsToCreate = new List<ReceiptMOB>();
                var detailsToCreate = new List<ReceiptDetailMOB>();

                foreach (var group in groupedDetails)
                {
                    var categoryId = group.Key;
                    var details = group.ToList();

                    // Get fee category info
                    var feeCategory = categoryId.HasValue && feeCategories.ContainsKey(categoryId.Value)
                        ? feeCategories[categoryId.Value]
                        : null;

                    // Create receipt number based on category 
                    var categoryCode = feeCategory?.Sequence;
                    // Add leading zero if categoryCode is a single digit
                    var formattedCategoryCode = categoryCode?.ToString().PadLeft(2, '0') ?? "00";
                    string ReceiptCategorySequenceNumber = $"{currentYear}-{formattedCategoryCode}-{sequenceFormatted}";
                    string receiptNumberIntegration = $"{currentYear}{formattedCategoryCode}{sequenceFormatted}";

                    // Get application data with citizen information in a single query
                    var applicationData = await _context.Applications
                        .Where(x => x.Id == entity.Receipt.ApplicationId)
                        .Select(x => new
                        {
                            OwnerId = x.OwnerId,
                            Citizen = x.Citizen != null ? new
                            {
                                FirstName = x.Citizen.FirstNameSecondLang ?? string.Empty,
                                FatherName = x.Citizen.FathersNameSecondLang ?? string.Empty,
                                LastName = x.Citizen.LastNameSecondLang ?? string.Empty
                            } : null
                        })
                        .FirstOrDefaultAsync()
                        ?? throw new Exception($"Application not found with ID: {entity.Receipt.ApplicationId}");



                    var CitizenFullName = applicationData.Citizen != null
                        ? $"{applicationData.Citizen.FirstName} {applicationData.Citizen.FatherName} {applicationData.Citizen.LastName}".Trim()
                        : string.Empty;

                    // Get DrivingLicenseId from existing receipt for this application, or from application itself
                    var DrivingLicenseId = await _context.Receipts
                        .Where(x => x.ApplicationId == entity.Receipt.ApplicationId && x.DrivingLicenseId != null)
                        .OrderByDescending(x => x.CreatedDate)
                        .Select(x => x.DrivingLicenseId)
                        .FirstOrDefaultAsync();

                    // If no driving license found in receipts, get from the application's driving license
                    if (!DrivingLicenseId.HasValue)
                    {
                        DrivingLicenseId = await _context.DrivingLicenses
                            .Where(x => x.ApplicationId == entity.Receipt.ApplicationId && (x.IsDeleted == null || x.IsDeleted == false))
                            .Select(x => (int?)x.Id)
                            .FirstOrDefaultAsync();
                    }

                    // Create new receipt for this category
                    var categoryReceipt = new ReceiptMOB
                    {
                        ApplicationId = entity.Receipt.ApplicationId,
                        ReceiptNumber = sequenceFormatted,
                        ReceiptNumberIntegration = receiptNumberIntegration,
                        Description = entity.Receipt.Description,
                        ReceiptStatusId = (int)ReceiptStatuses.PendingPayment,
                        StructureId = entity.Receipt.StructureId,
                        TotalAmount = group.Sum(x => x.Amount), // Calculate total from details
                        IsPaid = true,
                        PaidDate = DateTime.UtcNow,
                        IsPosted = false,
                        IsDeleted = false,
                        CitizenFullName = CitizenFullName,
                        Notes = entity.Receipt.Notes,
                        CreatedDate = DateTime.UtcNow,
                        CreatedUserId = entity.Receipt.CreatedUserId,
                        ReceiptStatusDate = DateTime.UtcNow,
                        ReceiptCategorySequenceNumber = ReceiptCategorySequenceNumber,
                        DrivingLicenseId = DrivingLicenseId
                    };

                    receiptsToCreate.Add(categoryReceipt);
                }

                // Start transaction only for database operations
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    // Add all receipts at once
                    _context.Receipts.AddRange(receiptsToCreate);
                    await _context.SaveChangesAsync();

                    // Create receipt details for each receipt 
                    for (int i = 0; i < receiptsToCreate.Count; i++)
                    {
                        var receipt = receiptsToCreate[i];
                        var group = groupedDetails[i];
                        var details = group.ToList();

                        foreach (var detail in details)
                        {
                            var receiptDetail = new ReceiptDetailMOB
                            {
                                ReceiptId = receipt.Id,
                                ItemId = detail.ItemId,
                                ProcessId = detail.ProcessId,
                                BPVarietyId = detail.BPVarietyId,
                                ItemDescriptionAR = detail.ItemDescriptionAR,
                                ItemDescriptionEN = detail.ItemDescriptionEN,
                                ItemCode = detail.ItemCode,
                                ItemTypeId = detail.ItemTypeId,
                                ItemCategoryId = detail.ItemCategoryId,
                                Amount = detail.Amount,
                                Notes = detail.Notes,
                                IsDeleted = false,
                                CreatedDate = DateTime.UtcNow,
                                CreatedUserId = detail.CreatedUserId,

                            };

                            detailsToCreate.Add(receiptDetail);
                        }

                    }

                    // Add all details at once
                    _context.ReceiptDetails.AddRange(detailsToCreate);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    // Requery all created receipts with navigation data
                    var createdReceiptIds = receiptsToCreate.Select(r => r.Id).ToList();
                    foreach (var createReciptId in createdReceiptIds)
                    {
                        receiptResponseDto.Add(await GetByIdAsync(createReciptId));
                    }

                    _externalApiService.CreateReceiptWithDetailsAsync(entity);
                    // Return the first created receipt
                    //return receiptsToCreate.FirstOrDefault() ?? entity.Receipt;
                    return receiptResponseDto;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            catch (Exception wee)
            {

                throw;
            }
        }

        private async Task<ReceiptResponseDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await _context.Receipts
                    .AsNoTracking()
                    .Where(r => r.Id == id)
                    .Select(r => new ReceiptResponseDto
                    {
                        Id = r.Id,
                        ApplicationId = r.ApplicationId,
                        ApplicationNumber = r.ApplicationId != null ? r.Application.ApplicationNumber : null,
                        ReceiptNumber = r.ReceiptNumber,
                        ReceiptCategorySequenceNumber = r.ReceiptCategorySequenceNumber,
                        Description = r.Description,
                        ReceiptStatusId = r.ReceiptStatusId,
                        ReceiptStatusEn = r.ReceiptStatusId != null ? r.ReceiptStatus.StatusDesc : null,
                        ReceiptStatusAr = r.ReceiptStatusId != null ? r.ReceiptStatus.StatusDescAr : null,
                        ReceiptStatusFr = r.ReceiptStatusId != null ? r.ReceiptStatus.StatusDescFr : null,
                        ReceiptStatusDate = r.ReceiptStatusDate,
                        StructureId = r.StructureId,
                        TotalAmount = r.TotalAmount,
                        IsPaid = r.IsPaid,
                        PaidDate = r.PaidDate,
                        PaymentProviderNumber = r.PaymentProviderNumber,
                        PaymentProviderDate = r.PaymentProviderDate,
                        PaymentProviderData = r.PaymentProviderData,
                        DataHash = r.DataHash,
                        IsPosted = r.IsPosted,
                        PostedDate = r.PostedDate,
                        PostedUserId = r.PostedUserId,
                        CitizenFullName = r.CitizenFullName,
                        Notes = r.Notes,
                        IsDeleted = r.IsDeleted,
                        DeletedDate = r.DeletedDate,
                        DeletedUserId = r.DeletedUserId,
                        CreatedDate = r.CreatedDate,
                        CreatedUserId = r.CreatedUserId,
                        ModifiedDate = r.ModifiedDate,
                        ModifiedUserId = r.ModifiedUserId,
                        ApplicationProcessFee = null,
                        Fee = null,
                        DrivingLicenseNumber = r.DrivingLicenseId != null ?
                                                 _context.DrivingLicenses
                                                 .AsNoTracking()
                                                 .Where(dl => dl.Id == r.DrivingLicenseId)
                                                 .Select(dl => dl.DrivingLicenseNumber)
                                                 .FirstOrDefault() : null,
                        CitizenId = _context.DrivingLicenses
                                                 .AsNoTracking()
                                                 .Where(dl => dl.Id == r.DrivingLicenseId)
                                                 .Select(dl => dl.CitizenId)
                                                 .FirstOrDefault(),

                        ReceiptDetails = r.ReceiptDetails
                            .Where(d => d.IsDeleted != true)
                            .Select(d => new ReceiptDetailResponseDto
                            {
                                Id = d.Id,
                                ReceiptId = d.ReceiptId,
                                ItemId = d.ItemId,
                                ProcessId = d.ProcessId,
                                BPVarietyId = d.BPVarietyId,
                                ItemDescriptionAR = d.ItemDescriptionAR,
                                ItemDescriptionEN = d.ItemDescriptionEN,
                                ItemCode = d.ItemCode,
                                ItemTypeId = d.ItemTypeId,
                                ItemCategoryId = d.ItemCategoryId,
                                ItemCategoryEn = d.ItemCategoryId != null ? d.FeeCategory.NameEn : null,
                                ItemCategoryAr = d.ItemCategoryId != null ? d.FeeCategory.NameAr : null,
                                ItemCategoryFr = d.ItemCategoryId != null ? d.FeeCategory.NameFr : null,
                                Amount = d.Amount,
                                Notes = d.Notes,
                                IsDeleted = d.IsDeleted,
                                DeletedDate = d.DeletedDate,
                                DeletedUserId = d.DeletedUserId,
                                CreatedDate = d.CreatedDate,
                                CreatedUserId = d.CreatedUserId,
                                ModifiedDate = d.ModifiedDate,
                                ModifiedUserId = d.ModifiedUserId
                            })
                            .ToList()
                    })
                    .FirstOrDefaultAsync(ct);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
