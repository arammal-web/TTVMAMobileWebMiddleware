using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.Utilities;
using System.Linq;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Enums;
using TTVMAMobileWebMiddleware.Domain.Views; 

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
        private readonly ILogger<ReceiptService> _logger;

        public ReceiptService(MOBDbContext context, DLSDbContext dLSDbContext,
                                IMemoryCache cache,
                                IExternalApiService externalApiService,
                                ISequenceGeneratorService sequenceService,
                                ILogger<ReceiptService> logger
            )
        {
            _context = context;
            _cache = cache;
            _sequenceService = sequenceService;
            _dLSDbContext = dLSDbContext;
            _externalApiService = externalApiService;
            _logger = logger;
        }

        /// <inheritdoc/>

        /// <inheritdoc/>
        public async Task<List<ReceiptResponseDto>> CreateWithDetailsAsync(ReceiptWithDetailRequest entity, CancellationToken ct = default)
        {
            // Input validation
            if (entity == null)
            {
                var ex = new Exception("Receipt entity is required");
                ex.HelpLink = "receipt_entity_required";
                throw ex;
            }

            if (entity.Receipt == null)
            {
                var ex = new Exception("Receipt is required");
                ex.HelpLink = "receipt_required";
                throw ex;
            }

            if (entity.ReceiptDetails == null || !entity.ReceiptDetails.Any())
            {
                var ex = new Exception("ReceiptDetails are required and cannot be empty");
                ex.HelpLink = "receipt_details_required";
                throw ex;
            }

            if (string.IsNullOrEmpty(entity.Receipt.ApplicationId))
            {
                var ex = new Exception("Receipt ApplicationId is required");
                ex.HelpLink = "receipt_application_id_required";
                throw ex;
            }

            if (!entity.Receipt.StructureId.HasValue || entity.Receipt.StructureId.Value <= 0)
            {
                var ex = new Exception("Receipt StructureId must be greater than 0");
                ex.HelpLink = "receipt_structure_id_invalid";
                throw ex;
            }

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
                var feeCategories = await _dLSDbContext.FeeCategories
                    .Where(fc => categoryIds.Contains(fc.Id))
                    .ToDictionaryAsync(fc => fc.Id, fc => fc, ct);

                var sequence = await _sequenceService.GetNextSequenceAsync("Receipt", 0, currentYear,ct);
                var sequenceFormatted = sequence.ToString("D7");

                // Get application data with citizen information in a single query (outside transaction)
                var applicationData = await _dLSDbContext.Applications
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
                    .FirstOrDefaultAsync(ct);

                if (applicationData == null)
                {
                    var ex = new Exception($"Application {entity.Receipt.ApplicationId} not found");
                    ex.HelpLink = "application_not_found";
                    throw ex;
                }

                var CitizenFullName = applicationData.Citizen != null
                    ? $"{applicationData.Citizen.FirstName} {applicationData.Citizen.FatherName} {applicationData.Citizen.LastName}".Trim()
                    : string.Empty;

                // Get DrivingLicenseId from existing receipt for this application, or from application itself (outside transaction)
                var DrivingLicenseId = await _dLSDbContext.Receipts
                    .Where(x => x.ApplicationId == entity.Receipt.ApplicationId && x.DrivingLicenseId != null)
                    .OrderByDescending(x => x.CreatedDate)
                    .Select(x => x.DrivingLicenseId)
                    .FirstOrDefaultAsync(ct);

                // If no driving license found in receipts, get from the application's driving license
                if (!DrivingLicenseId.HasValue)
                {
                    DrivingLicenseId = await _dLSDbContext.DrivingLicenses
                        .Where(x => x.ApplicationId == entity.Receipt.ApplicationId && (x.IsDeleted == null || x.IsDeleted == false))
                        .Select(x => (int?)x.Id)
                        .FirstOrDefaultAsync(ct);
                }

                // Prepare all receipts and details before transaction
                var receiptsToCreate = new List<Receipt>();
                var detailsToCreate = new List<ReceiptDetail>();

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

                    // Create new receipt for this category
                    var categoryReceipt = new Receipt
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
                using var transaction = await _dLSDbContext.Database.BeginTransactionAsync(ct);
                try
                {
                    // Add all receipts at once
                    _dLSDbContext.Receipts.AddRange(receiptsToCreate);
                    await _dLSDbContext.SaveChangesAsync(ct);

                    // Create receipt details for each receipt 
                    for (int i = 0; i < receiptsToCreate.Count; i++)
                    {
                        var receipt = receiptsToCreate[i];
                        var group = groupedDetails[i];
                        var details = group.ToList();

                        foreach (var detail in details)
                        {
                            var receiptDetail = new ReceiptDetail
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
                    _dLSDbContext.ReceiptDetails.AddRange(detailsToCreate);
                    await _dLSDbContext.SaveChangesAsync(ct);

                    await transaction.CommitAsync(ct);
                    
                    // Load all created receipts with navigation data in a single query (fixes N+1 problem)
                    var createdReceiptIds = receiptsToCreate.Select(r => r.Id).ToList();
                    var receipts = await _dLSDbContext.Receipts
                        .AsNoTracking()
                        .Where(r => createdReceiptIds.Contains(r.Id))
                        .Include(r => r.ReceiptStatus)
                        .Include(r => r.Application)
                        .Include(r => r.ReceiptDetails)
                            .ThenInclude(d => d.FeeCategory)
                        .ToListAsync(ct);

                    // Load all driving licenses in a single query to avoid N+1
                    var drivingLicenseIds = receipts
                        .Where(r => r.DrivingLicenseId.HasValue)
                        .Select(r => r.DrivingLicenseId.Value)
                        .Distinct()
                        .ToList();
                    
                    var drivingLicenses = drivingLicenseIds.Any()
                        ? await _dLSDbContext.DrivingLicenses
                            .AsNoTracking()
                            .Where(dl => drivingLicenseIds.Contains(dl.Id))
                            .ToDictionaryAsync(dl => dl.Id, dl => dl, ct)
                        : new Dictionary<int, DrivingLicenseABP>();

                    // Map receipts to DTOs
                    foreach (var receipt in receipts)
                    {
                        var drivingLicense = receipt.DrivingLicenseId.HasValue && drivingLicenses.ContainsKey(receipt.DrivingLicenseId.Value)
                            ? drivingLicenses[receipt.DrivingLicenseId.Value]
                            : null;

                        var receiptDto = new ReceiptResponseDto
                        {
                            Id = receipt.Id,
                            ApplicationId = receipt.ApplicationId,
                            ApplicationNumber = receipt.ApplicationId != null ? receipt.Application?.ApplicationNumber : null,
                            ReceiptNumber = receipt.ReceiptNumber,
                            ReceiptCategorySequenceNumber = receipt.ReceiptCategorySequenceNumber,
                            Description = receipt.Description,
                            ReceiptStatusId = receipt.ReceiptStatusId,
                            ReceiptStatusEn = receipt.ReceiptStatusId != null ? receipt.ReceiptStatus?.StatusDesc : null,
                            ReceiptStatusAr = receipt.ReceiptStatusId != null ? receipt.ReceiptStatus?.StatusDescAr : null,
                            ReceiptStatusFr = receipt.ReceiptStatusId != null ? receipt.ReceiptStatus?.StatusDescFr : null,
                            ReceiptStatusDate = receipt.ReceiptStatusDate,
                            StructureId = receipt.StructureId,
                            TotalAmount = receipt.TotalAmount,
                            IsPaid = receipt.IsPaid,
                            PaidDate = receipt.PaidDate,
                            PaymentProviderNumber = receipt.PaymentProviderNumber,
                            PaymentProviderDate = receipt.PaymentProviderDate,
                            PaymentProviderData = receipt.PaymentProviderData,
                            DataHash = receipt.DataHash,
                            IsPosted = receipt.IsPosted,
                            PostedDate = receipt.PostedDate,
                            PostedUserId = receipt.PostedUserId,
                            CitizenFullName = receipt.CitizenFullName,
                            Notes = receipt.Notes,
                            IsDeleted = receipt.IsDeleted,
                            DeletedDate = receipt.DeletedDate,
                            DeletedUserId = receipt.DeletedUserId,
                            CreatedDate = receipt.CreatedDate,
                            CreatedUserId = receipt.CreatedUserId,
                            ModifiedDate = receipt.ModifiedDate,
                            ModifiedUserId = receipt.ModifiedUserId,
                            ApplicationProcessFee = null,
                            Fee = null,
                            DrivingLicenseNumber = drivingLicense?.DrivingLicenseNumber,
                            CitizenId = drivingLicense?.CitizenId,
                            ReceiptDetails = receipt.ReceiptDetails
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
                                    ItemCategoryEn = d.ItemCategoryId != null ? d.FeeCategory?.NameEn : null,
                                    ItemCategoryAr = d.ItemCategoryId != null ? d.FeeCategory?.NameAr : null,
                                    ItemCategoryFr = d.ItemCategoryId != null ? d.FeeCategory?.NameFr : null,
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
                        };
                        receiptResponseDto.Add(receiptDto);
                    }

                    return receiptResponseDto;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(ct);
                    throw;
                }
            }
            catch (Exception ex)
            {
                var exception = new Exception($"Error creating receipt with details for application {entity?.Receipt?.ApplicationId}");
                exception.HelpLink = "receipt_creation_error";
                throw exception;
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
                var exception = new Exception($"Error retrieving receipt {id}");
                exception.HelpLink = "receipt_retrieval_error";
                throw exception;
            }
        }

        /// <inheritdoc/>
        public async Task<SyncResult> SyncReceiptsAndDrivingLicensesAsync(int userId, CancellationToken ct = default)
        {
            var result = new SyncResult();

            try
            {
                _logger.LogInformation("Starting sync of receipts and driving licenses for mobile app citizens");

                // Get all mobile app citizens (those with CitizenLink)
                var mobileCitizens = await _context.CitizenLinks
                    .Select(cl => new { cl.CitizenOnlineId, cl.CitizenLocalId })
                    .ToListAsync(ct);

                var mobileCitizensList = mobileCitizens
                    .Select(c => (OnlineCitizenId: c.CitizenOnlineId, LocalCitizenId: c.CitizenLocalId))
                    .ToList();

                result.CitizensProcessed = mobileCitizensList.Count;

                if (!mobileCitizensList.Any())
                {
                    _logger.LogInformation("No mobile app citizens found to sync");
                    return result;
                }

                var localCitizenIds = mobileCitizensList.Select(c => c.LocalCitizenId).ToList();
                var onlineCitizenIds = mobileCitizensList.Select(c => c.OnlineCitizenId).ToList();

                // Get all applications for mobile app citizens
                var mobileApplications = await _context.Applications
                    .Where(a => onlineCitizenIds.Contains(a.OwnerId) && (a.IsDeleted == null || a.IsDeleted == false))
                    .Select(a => a.Id)
                    .ToListAsync(ct);

                result.ApplicationsProcessed = mobileApplications.Count;

                using var transaction = await _context.Database.BeginTransactionAsync(ct);
                try
                {
                    // Sync Receipts and ReceiptDetails
                    await SyncReceiptsAsync(mobileApplications, userId, result, ct);

                    // Sync DrivingLicenses and DrivingLicenseDetails
                    await SyncDrivingLicensesAsync(mobileCitizensList, userId, result, ct);

                    await _context.SaveChangesAsync(ct);
                    await transaction.CommitAsync(ct);

                    _logger.LogInformation(
                        "Sync completed successfully. Receipts: {Receipts}, ReceiptDetails: {ReceiptDetails}, " +
                        "DrivingLicenses: {DrivingLicenses}, DrivingLicenseDetails: {DrivingLicenseDetails}",
                        result.ReceiptsSynced, result.ReceiptDetailsSynced,
                        result.DrivingLicensesSynced, result.DrivingLicenseDetailsSynced);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(ct);
                    result.Errors.Add($"Transaction rolled back: {ex.Message}");
                    _logger.LogError(ex, "Error during sync transaction");
                    throw;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Sync failed: {ex.Message}");
                _logger.LogError(ex, "Error syncing receipts and driving licenses");
            }

            return result;
        }

        private async Task SyncReceiptsAsync(List<string> applicationIds, int userId, SyncResult result, CancellationToken ct)
        {
            if (!applicationIds.Any())
                return;


            // Get all receipts from DLS for these applications
            var dlsReceipts = await _dLSDbContext.Receipts
                .Include(r => r.ReceiptDetails)
                .Where(r => applicationIds.Contains(r.ApplicationId) &&
                           (r.IsDeleted == null || r.IsDeleted == false))
                .ToListAsync(ct);

            foreach (var dlsReceipt in dlsReceipts)
            {
                try
                {
                    // Check if receipt already exists in mobile database
                    var existingReceipt = await _context.Receipts
                        .FirstOrDefaultAsync(r => r.ApplicationId == dlsReceipt.ApplicationId &&
                                                  r.ReceiptNumberIntegration == dlsReceipt.ReceiptNumberIntegration &&
                                                  (r.IsDeleted == null || r.IsDeleted == false), ct);

                    ReceiptMOB mobileReceipt;
                    if (existingReceipt == null)
                    {
                        // Create new receipt
                        mobileReceipt = new ReceiptMOB
                        {
                            ApplicationId = dlsReceipt.ApplicationId,
                            ReceiptNumber = dlsReceipt.ReceiptNumber,
                            ReceiptNumberIntegration = dlsReceipt.ReceiptNumberIntegration,
                            ReceiptCategorySequenceNumber = dlsReceipt.ReceiptCategorySequenceNumber,
                            Description = dlsReceipt.Description,
                            ReceiptStatusId = dlsReceipt.ReceiptStatusId,
                            ReceiptStatusDate = dlsReceipt.ReceiptStatusDate,
                            StructureId = dlsReceipt.StructureId,
                            TotalAmount = dlsReceipt.TotalAmount,
                            IsPaid = dlsReceipt.IsPaid,
                            PaidDate = dlsReceipt.PaidDate,
                            PaymentProviderNumber = dlsReceipt.PaymentProviderNumber,
                            PaymentProviderDate = dlsReceipt.PaymentProviderDate,
                            PaymentProviderData = dlsReceipt.PaymentProviderData,
                            DataHash = dlsReceipt.DataHash,
                            IsPosted = dlsReceipt.IsPosted,
                            PostedDate = dlsReceipt.PostedDate,
                            PostedUserId = dlsReceipt.PostedUserId,
                            CitizenFullName = dlsReceipt.CitizenFullName,
                            Notes = dlsReceipt.Notes,
                            IsDeleted = false,
                            CreatedDate = dlsReceipt.CreatedDate,
                            CreatedUserId = dlsReceipt.CreatedUserId ?? userId,
                            ModifiedDate = dlsReceipt.ModifiedDate,
                            ModifiedUserId = dlsReceipt.ModifiedUserId,
                            DrivingLicenseId = dlsReceipt.DrivingLicenseId
                        };

                        _context.Receipts.Add(mobileReceipt);
                        await _context.SaveChangesAsync(ct); // Save to get the ID
                        result.ReceiptsSynced++;
                    }
                    else
                    {
                        // Update existing receipt
                        mobileReceipt = existingReceipt;
                        existingReceipt.ReceiptStatusId = dlsReceipt.ReceiptStatusId;
                        existingReceipt.ReceiptStatusDate = dlsReceipt.ReceiptStatusDate;
                        existingReceipt.TotalAmount = dlsReceipt.TotalAmount;
                        existingReceipt.IsPaid = dlsReceipt.IsPaid;
                        existingReceipt.PaidDate = dlsReceipt.PaidDate;
                        existingReceipt.PaymentProviderNumber = dlsReceipt.PaymentProviderNumber;
                        existingReceipt.PaymentProviderDate = dlsReceipt.PaymentProviderDate;
                        existingReceipt.PaymentProviderData = dlsReceipt.PaymentProviderData;
                        existingReceipt.DataHash = dlsReceipt.DataHash;
                        existingReceipt.IsPosted = dlsReceipt.IsPosted;
                        existingReceipt.PostedDate = dlsReceipt.PostedDate;
                        existingReceipt.PostedUserId = dlsReceipt.PostedUserId;
                        existingReceipt.ModifiedDate = DateTime.UtcNow;
                        existingReceipt.ModifiedUserId = userId;
                    }

                    // Sync ReceiptDetails
                    var dlsDetails = dlsReceipt.ReceiptDetails
                        .Where(d => d.IsDeleted == false)
                        .ToList();

                    foreach (var dlsDetail in dlsDetails)
                    {
                        var existingDetail = await _context.ReceiptDetails
                            .FirstOrDefaultAsync(d => d.ReceiptId == mobileReceipt.Id &&
                                                      d.ItemId == dlsDetail.ItemId &&
                                                      d.ProcessId == dlsDetail.ProcessId &&
                                                      d.BPVarietyId == dlsDetail.BPVarietyId &&
                                                      d.IsDeleted == false, ct);

                        if (existingDetail == null)
                        {
                            var mobileDetail = new ReceiptDetailMOB
                            {
                                ReceiptId = mobileReceipt.Id,
                                ItemId = dlsDetail.ItemId,
                                ProcessId = dlsDetail.ProcessId,
                                BPVarietyId = dlsDetail.BPVarietyId,
                                ItemDescriptionAR = dlsDetail.ItemDescriptionAR,
                                ItemDescriptionEN = dlsDetail.ItemDescriptionEN,
                                ItemCode = dlsDetail.ItemCode,
                                ItemTypeId = dlsDetail.ItemTypeId,
                                ItemCategoryId = dlsDetail.ItemCategoryId,
                                Amount = dlsDetail.Amount,
                                Notes = dlsDetail.Notes,
                                IsDeleted = false,
                                CreatedDate = dlsDetail.CreatedDate,
                                CreatedUserId = dlsDetail.CreatedUserId,
                                ModifiedDate = dlsDetail.ModifiedDate,
                                ModifiedUserId = dlsDetail.ModifiedUserId
                            };

                            _context.ReceiptDetails.Add(mobileDetail);
                            result.ReceiptDetailsSynced++;
                        }
                        else
                        {
                            // Update existing detail
                            existingDetail.Amount = dlsDetail.Amount;
                            existingDetail.ItemDescriptionAR = dlsDetail.ItemDescriptionAR;
                            existingDetail.ItemDescriptionEN = dlsDetail.ItemDescriptionEN;
                            existingDetail.ItemCode = dlsDetail.ItemCode;
                            existingDetail.Notes = dlsDetail.Notes;
                            existingDetail.ModifiedDate = DateTime.UtcNow;
                            existingDetail.ModifiedUserId = userId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Error syncing receipt {dlsReceipt.Id}: {ex.Message}");
                    _logger.LogError(ex, "Error syncing receipt {ReceiptId}", dlsReceipt.Id);
                }
            }
        }

        private async Task SyncDrivingLicensesAsync(
            List<(int OnlineCitizenId, int LocalCitizenId)> mobileCitizens, int userId, SyncResult result, CancellationToken ct)
        {
            var localCitizenIds = mobileCitizens.Select(c => c.LocalCitizenId).ToList();

            // Get all driving licenses from DLS for these local citizens
            var dlsDrivingLicenses = await _dLSDbContext.DrivingLicenses
                .Include(dl => dl.DrivingLicenseDetails)
                .Where(dl => localCitizenIds.Contains(dl.CitizenId) &&
                            dl.DrivingLicenseStatusId != null &&
                            !dl.IsDeleted)
                .ToListAsync(ct);

            foreach (var dlsDrivingLicense in dlsDrivingLicenses)
            {
                try
                {
                    // Find the corresponding online citizen ID
                    var mapping = mobileCitizens.FirstOrDefault(c => c.LocalCitizenId == dlsDrivingLicense.CitizenId);
                    if (mapping == default)
                        continue;

                    // Check if driving license already exists in mobile database
                    var existingDrivingLicense = await _context.DrivingLicenses
                        .FirstOrDefaultAsync(dl =>  dl.CitizenId == mapping.OnlineCitizenId &&
                                                   dl.DrivingLicenseNumber == dlsDrivingLicense.DrivingLicenseNumber &&
                                                   !dl.IsDeleted, ct);

                    DrivingLicense mobileDrivingLicense;
                    if (existingDrivingLicense == null)
                    {
                        // Create new driving license
                        mobileDrivingLicense = new DrivingLicense
                        {
                            CitizenId = mapping.OnlineCitizenId,
                            ApplicationId = dlsDrivingLicense.ApplicationId,
                            DrivingLicenseNumber = dlsDrivingLicense.DrivingLicenseNumber,
                            DrivingLicenseStatusId = dlsDrivingLicense.DrivingLicenseStatusId,
                            DrivingLicenseStatusDate = dlsDrivingLicense.DrivingLicenseStatusDate,
                            Description = dlsDrivingLicense.Description,
                            IssuanceDate = dlsDrivingLicense.IssuanceDate,
                            ExpiryDate = dlsDrivingLicense.ExpiryDate,
                            SAI = dlsDrivingLicense.SAI,
                            LicenseCodes = dlsDrivingLicense.LicenseCodes,
                            IssueAuthorityId = dlsDrivingLicense.IssueAuthorityId,
                            StructureId = dlsDrivingLicense.StructureId,
                            NumberOfPoints = dlsDrivingLicense.NumberOfPoints,
                            IsBlocked = dlsDrivingLicense.IsBlocked,
                            BlockingAuthority = dlsDrivingLicense.BlockingAuthority,
                            BlockingReason = dlsDrivingLicense.BlockingReason,
                            IsOldDrivingLicense = dlsDrivingLicense.IsOldDrivingLicense,
                            OldDrivingLicenseImage = dlsDrivingLicense.OldDrivingLicenseImage,
                            IsByPasssTest = dlsDrivingLicense.IsByPasssTest,
                            ByPassTestAuthority = dlsDrivingLicense.ByPassTestAuthority,
                            ByPassTestReason = dlsDrivingLicense.ByPassTestReason,
                            IsPrinted = dlsDrivingLicense.IsPrinted,
                            PrintingMachine = dlsDrivingLicense.PrintingMachine,
                            CardSerialNumber = dlsDrivingLicense.CardSerialNumber,
                            LicensePrintedData = dlsDrivingLicense.LicensePrintedData,
                            DataHash = dlsDrivingLicense.DataHash,
                            PrintedUserId = dlsDrivingLicense.PrintedUserId,
                            PrintedDate = dlsDrivingLicense.PrintedDate,
                            PrintedStructureId = dlsDrivingLicense.PrintedStructureId,
                            Notes = dlsDrivingLicense.Notes,
                            IsDeleted = false,
                            CreatedDate = dlsDrivingLicense.CreatedDate,
                            CreatedUserId = dlsDrivingLicense.CreatedUserId ?? userId,
                            CitizenFullName = dlsDrivingLicense.CitizenFullName,
                            IsInternational = dlsDrivingLicense.IsInternational,
                            IsMigrated = false,
                            DrivingLicenseTypeId = dlsDrivingLicense.DrivingLicenseTypeId
                        };

                        _context.DrivingLicenses.Add(mobileDrivingLicense);
                        await _context.SaveChangesAsync(ct); // Save to get the ID
                        result.DrivingLicensesSynced++;
                    }
                    else
                    {
                        // Update existing driving license
                        mobileDrivingLicense = existingDrivingLicense;
                        existingDrivingLicense.DrivingLicenseStatusId = dlsDrivingLicense.DrivingLicenseStatusId;
                        existingDrivingLicense.DrivingLicenseStatusDate = dlsDrivingLicense.DrivingLicenseStatusDate;
                        existingDrivingLicense.IssuanceDate = dlsDrivingLicense.IssuanceDate;
                        existingDrivingLicense.ExpiryDate = dlsDrivingLicense.ExpiryDate;
                        existingDrivingLicense.SAI = dlsDrivingLicense.SAI;
                        existingDrivingLicense.LicenseCodes = dlsDrivingLicense.LicenseCodes;
                        existingDrivingLicense.NumberOfPoints = dlsDrivingLicense.NumberOfPoints;
                        existingDrivingLicense.IsBlocked = dlsDrivingLicense.IsBlocked;
                        existingDrivingLicense.BlockingAuthority = dlsDrivingLicense.BlockingAuthority;
                        existingDrivingLicense.BlockingReason = dlsDrivingLicense.BlockingReason;
                        existingDrivingLicense.IsPrinted = dlsDrivingLicense.IsPrinted;
                        existingDrivingLicense.CardSerialNumber = dlsDrivingLicense.CardSerialNumber;
                        existingDrivingLicense.LicensePrintedData = dlsDrivingLicense.LicensePrintedData;
                        existingDrivingLicense.DataHash = dlsDrivingLicense.DataHash;
                        existingDrivingLicense.Notes = dlsDrivingLicense.Notes;
                        existingDrivingLicense.ModifiedDate = DateTime.UtcNow;
                        existingDrivingLicense.ModifiedUserId = userId;
                    }

                    // Sync DrivingLicenseDetails
                    var dlsDetails = dlsDrivingLicense.DrivingLicenseDetails
                        .Where(d => !d.IsDeleted)
                        .ToList();

                    foreach (var dlsDetail in dlsDetails)
                    {
                        var existingDetail = await _context.DrivingLicenseDetails
                            .FirstOrDefaultAsync(d => d.DrivingLicenseId == mobileDrivingLicense.Id &&
                                                      d.ApplicationId == dlsDetail.ApplicationId &&
                                                      d.ProcessId == dlsDetail.ProcessId &&
                                                      d.BPVarietyId == dlsDetail.BPVarietyId &&
                                                      !d.IsDeleted, ct);

                        if (existingDetail == null)
                        {
                            var mobileDetail = new DrivingLicenseDetail
                            {
                                DrivingLicenseId = mobileDrivingLicense.Id,
                                ApplicationId = dlsDetail.ApplicationId,
                                ProcessId = dlsDetail.ProcessId,
                                BPVarietyId = dlsDetail.BPVarietyId,
                                IssuingDate = dlsDetail.IssuingDate,
                                ExpiryDate = dlsDetail.ExpiryDate,
                                Description = dlsDetail.Description,
                                StructureId = dlsDetail.StructureId,
                                Notes = dlsDetail.Notes,
                                IsDeleted = false,
                                CreatedDate = dlsDetail.CreatedDate,
                                CreatedUserId = dlsDetail.CreatedUserId,
                                StatusId = dlsDetail.StatusId,
                                ProcessVarietyTypeId = dlsDetail.ProcessVarietyTypeId
                            };

                            _context.DrivingLicenseDetails.Add(mobileDetail);
                            result.DrivingLicenseDetailsSynced++;
                        }
                        else
                        {
                            // Update existing detail
                            existingDetail.IssuingDate = dlsDetail.IssuingDate;
                            existingDetail.ExpiryDate = dlsDetail.ExpiryDate;
                            existingDetail.Description = dlsDetail.Description;
                            existingDetail.Notes = dlsDetail.Notes;
                            existingDetail.StatusId = dlsDetail.StatusId;
                            existingDetail.ProcessVarietyTypeId = dlsDetail.ProcessVarietyTypeId;
                            existingDetail.ModifiedDate = DateTime.UtcNow;
                            existingDetail.ModifiedUserId = userId;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Errors.Add($"Error syncing driving license {dlsDrivingLicense.Id}: {ex.Message}");
                    _logger.LogError(ex, "Error syncing driving license {DrivingLicenseId}", dlsDrivingLicense.Id);
                }
            }
        }

    }
}
