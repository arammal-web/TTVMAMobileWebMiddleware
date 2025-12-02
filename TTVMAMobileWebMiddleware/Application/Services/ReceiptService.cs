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
using ApplicationStatus = TTVMAMobileWebMiddleware.Domain.Enums.ApplicationStatus;

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
                var feeCategoriesDLS = await _dLSDbContext.FeeCategories
                    .Where(fc => categoryIds.Contains(fc.Id))
                    .ToDictionaryAsync(fc => fc.Id, fc => fc, ct);

                var feeCategoriesMob = await _context.FeeCategories
                    .Where(fc => categoryIds.Contains(fc.Id))
                    .ToDictionaryAsync(fc => fc.Id, fc => fc, ct);

                var sequence = await GetNextSequenceAsync("Receipt", 0, currentYear, ct);
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
                var receiptsToCreateDSL = new List<Receipt>();
                var detailsToCreateDLS = new List<ReceiptDetail>();
                var receiptsToCreateMOB = new List<ReceiptMOB>();
                var detailsToCreateMON = new List<ReceiptDetailMOB>();
                await CreateDLSReceiptWithDetails(entity,
                    receiptResponseDto,
                    groupedDetails,
                    currentYear,
                    feeCategoriesDLS,
                    sequenceFormatted,
                    CitizenFullName,
                    DrivingLicenseId,
                    receiptsToCreateDSL,
                    detailsToCreateDLS,
                    ct);
                return await CreateMobileReceiptsWithDetails(entity,
                      receiptResponseDto,
                      groupedDetails,
                      currentYear,
                      feeCategoriesMob,
                      sequenceFormatted,
                      CitizenFullName,
                      DrivingLicenseId,
                      receiptsToCreateMOB,
                      detailsToCreateMON,
                      ct);
            }
            catch (Exception ex)
            {
                var exception = new Exception($"Error creating receipt with details for application {entity?.Receipt?.ApplicationId}");
                exception.HelpLink = "receipt_creation_error";
                throw exception;
            }
        }



        private async Task<List<ReceiptResponseDto>> CreateMobileReceiptsWithDetails(ReceiptWithDetailRequest entity, List<ReceiptResponseDto> receiptResponseDto, List<IGrouping<int?, ReceiptDetail>> groupedDetails, int currentYear, Dictionary<int, Domain.Entities.Mobile.FeeCategory> feeCategories, string sequenceFormatted, string CitizenFullName, int? DrivingLicenseId, List<ReceiptMOB> receiptsToCreate, List<ReceiptDetailMOB> detailsToCreate, CancellationToken ct)
        {
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
                var categoryReceipt = new ReceiptMOB
                {
                    ApplicationId = entity.Receipt.ApplicationId,
                    ReceiptNumber = sequenceFormatted,
                    ReceiptNumberIntegration = receiptNumberIntegration,
                    Description = entity.Receipt.Description,
                    ReceiptStatusId = (int)ReceiptStatuses.PendingPayment,
                    StructureId = entity.Receipt.StructureId,
                    TotalAmount = group.Sum(x => x.Amount), // Calculate total from details
                    IsPaid = false,
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
            using var transaction = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                // Add all receipts at once
                _context.Receipts.AddRange(receiptsToCreate);
                await _context.SaveChangesAsync(ct);

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
                await _context.SaveChangesAsync(ct);

                await transaction.CommitAsync(ct);

                return receiptResponseDto;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }

        private async Task<List<ReceiptResponseDto>> CreateDLSReceiptWithDetails(ReceiptWithDetailRequest entity, List<ReceiptResponseDto> receiptResponseDto, List<IGrouping<int?, ReceiptDetail>> groupedDetails, int currentYear, Dictionary<int, Domain.Entities.DLS.FeeCategory> feeCategories, string sequenceFormatted, string CitizenFullName, int? DrivingLicenseId, List<Receipt> receiptsToCreate, List<ReceiptDetail> detailsToCreate, CancellationToken ct)
        {
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
                    IsPaid = false,
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
                return receiptResponseDto;

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        }
        /// <inheritdoc/>
        public async Task<SyncResult> SyncReceiptsAndDrivingLicensesAsync(int userId, CancellationToken ct = default)
        {
            var result = new SyncResult();

            try
            {
                _logger.LogInformation("Starting sync of receipts and driving licenses for mobile app citizens");
                 try
                {
                    // Sync Receipts and ReceiptDetails
                    await SyncReceiptsAsync( userId, result, ct);

                    _logger.LogInformation(
                        "Sync completed successfully. Receipts: {Receipts}, ReceiptDetails: {ReceiptDetails}, " +
                        "DrivingLicenses: {DrivingLicenses}, DrivingLicenseDetails: {DrivingLicenseDetails}",
                        result.ReceiptsSynced, result.ReceiptDetailsSynced,
                        result.DrivingLicensesSynced, result.DrivingLicenseDetailsSynced);
                }
                catch (Exception ex)
                {
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

        private async Task SyncReceiptsAsync( int userId, SyncResult result, CancellationToken ct)
        { 
            // Get unpaid receipts from mobile database for these applications
            var unpaidMobileReceipts = await _context.Receipts
                .Where(r =>  r.IsPaid == false &&
                           (r.IsDeleted == null || r.IsDeleted == false))
                .ToListAsync(ct);

            // Check if these receipts are paid in DLS database
            if (unpaidMobileReceipts.Any())
            {
                var receiptNumberIntegrations = unpaidMobileReceipts
                    .Select(r => r.ReceiptNumberIntegration)
                    .Distinct()
                    .ToList();

                var paidDlsReceipts = await _dLSDbContext.Receipts
                    .Where(r => receiptNumberIntegrations.Contains(r.ReceiptNumberIntegration) &&
                               r.IsPaid == true &&
                               (r.IsDeleted == null || r.IsDeleted == false))
                    .ToListAsync(ct);

                // Process each paid DLS receipt
                var citizensToSyncDrivingLicenses = new HashSet<int>(); // Track online citizen IDs

                foreach (var dlsReceipt in paidDlsReceipts)
                {
                    try
                    {
                        // Find corresponding mobile receipt
                        var mobileReceipt = unpaidMobileReceipts
                            .FirstOrDefault(r => r.ReceiptNumberIntegration == dlsReceipt.ReceiptNumberIntegration);

                        if (mobileReceipt == null)
                            continue;

                        // Call PayReceiptAsync with format: {year}-{ReceiptNumber}
                        // PayReceiptAsync expects format where first part is year and last part is ReceiptNumber
 
                        var paySuccess = await PayReceiptAsync(dlsReceipt.ReceiptCategorySequenceNumber, userId, ct);

                        if (paySuccess)
                        {
                            // Get application to find citizen ID
                            var application = await _context.Applications
                                .Where(a => a.Id == mobileReceipt.ApplicationId &&
                                           (a.IsDeleted == null || a.IsDeleted == false))
                                .Select(a => new { a.OwnerId })
                                .FirstOrDefaultAsync(ct);

                            if (application != null && application.OwnerId != 0)
                            {
                                citizensToSyncDrivingLicenses.Add(application.OwnerId);
                            }

                            _logger.LogInformation(
                                "Successfully paid receipt {ReceiptNumberIntegration} for application {ApplicationId}",
                                mobileReceipt.ReceiptNumberIntegration, mobileReceipt.ApplicationId);
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Errors.Add($"Error paying receipt {dlsReceipt.ReceiptNumberIntegration}: {ex.Message}");
                        _logger.LogError(ex, "Error paying receipt {ReceiptNumberIntegration}", dlsReceipt.ReceiptNumberIntegration);
                    }
                }

                // Sync driving licenses for citizens whose receipts were successfully paid
                if (citizensToSyncDrivingLicenses.Any())
                {
                    // Get citizen mappings for these online citizens
                    var citizenMappings = await _context.CitizenLinks
                        .Where(cl => citizensToSyncDrivingLicenses.Contains(cl.CitizenOnlineId))
                        .Select(cl => new { cl.CitizenOnlineId, cl.CitizenLocalId })
                        .ToListAsync(ct);

                    var mobileCitizensForSync = citizenMappings
                        .Select(c => (OnlineCitizenId: c.CitizenOnlineId, LocalCitizenId: c.CitizenLocalId))
                        .ToList();

                    if (mobileCitizensForSync.Any())
                    {
                        await SyncDrivingLicensesAsync(mobileCitizensForSync, userId, result, ct);
                    }
                }
            }

        }

        private async Task SyncDrivingLicensesAsync(List<(int OnlineCitizenId, int LocalCitizenId)> mobileCitizens, int userId, SyncResult result, CancellationToken ct)
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
                        .FirstOrDefaultAsync(dl => dl.CitizenId == mapping.OnlineCitizenId &&
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

        private async Task<int> GetNextSequenceAsync(string tableName, int? structureId = null, int? year = null, CancellationToken ct = default)
        {

            using var transaction = await _dLSDbContext.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(tableName))
                {
                    var ex = new Exception("Table name cannot be null or empty.");
                    ex.HelpLink = "table_name_cannot_be_null_or_empty";
                    throw ex;
                }
                if (year == null)
                {
                    year = DateTime.Now.Year;
                }
                if (structureId == null)
                {
                    structureId = 0; // Assuming 0 is a valid default for StructureId
                }
                var sequence = await _context.Set<SequenceNumber>()
                    .Where(s => s.TableName == tableName
                             && s.YearValue == year)
                    .FirstOrDefaultAsync();

                int nextValue = (sequence?.MaxValue ?? 0) + 1;

                if (sequence == null)
                {
                    var insertSql = $@"
                        INSERT INTO STR.SequenceNumber (TableName, YearValue, StructureId, MaxValue)
                        VALUES (@p0, @p1, @p2, @p3)
                    ";

                    await _context.Database.ExecuteSqlRawAsync(insertSql,
                        tableName,
                        year,
                        structureId,
                        nextValue);
                }
                else
                {
                    sequence.MaxValue = nextValue;
                    _context.Set<SequenceNumber>().Update(sequence);
                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return nextValue;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        private async Task<bool> PayReceiptAsync(string ReceiptNumber, int userId, CancellationToken ct = default)
        {
            List<ReceiptMOB> receipts = new List<ReceiptMOB>();

            string[] receiptNumberSplit = ReceiptNumber.Split('-');
            if (receiptNumberSplit.Count() > 1)
            {
                receipts = await _context.Receipts
                 .Where(r => r.ReceiptNumber == receiptNumberSplit.Last() && r.CreatedDate.Year.ToString() == receiptNumberSplit.First() && (r.IsDeleted == null || r.IsDeleted == false))
                 .ToListAsync(ct);
            }
            if (!receipts.Any())
                throw new Exception("receipt not found.") { HelpLink = "receipt_not_found" };

            // Validate all receipts have the same ApplicationId
            var applicationIds = receipts.Select(r => r.ApplicationId).Distinct().ToList();
            if (applicationIds.Count > 1)
                throw new Exception("receipts have different application IDs.") { HelpLink = "receipts_different_application_ids" };


            // Use the first receipt for application validation (they should all have the same ApplicationId)
            var receipt = receipts.First();

            var app = await _context.Applications.Where(a => a.Id == receipt.ApplicationId && a.IsDeleted == false)
                .FirstOrDefaultAsync()
                ?? throw new Exception("application not found for the receipt.") { HelpLink = "application_not_found_for_receipt" };

            if (app.ApplicationApprovalStatusId != (int)ApplicationStatus.Approved)
                throw new Exception($"cannot pay: application is not approved (current status: {app.ApplicationApprovalStatus?.StatusDesc ?? $"{app.ApplicationApprovalStatusId}"}).")
                { HelpLink = "application_status_not_approved" };


            using var tx = await _context.Database.BeginTransactionAsync(ct);
            try
            {
                // (1) App → Committed
                app.ApplicationApprovalStatusId = (int)ApplicationStatus.Committed;
                app.ApplicationApprovalStatusDate = DateTime.UtcNow;
                app.ModifiedDate = DateTime.UtcNow;
                app.ModifiedUserId = userId;
                // (3) All Receipts with same receipt number → Paid
                foreach (var receiptToUpdate in receipts)
                {
                    receiptToUpdate.IsPaid = true;
                    receiptToUpdate.PaidDate = DateTime.UtcNow;
                    receiptToUpdate.PaymentProviderNumber = ReceiptNumber;
                    receiptToUpdate.PaymentProviderDate = DateTime.UtcNow;
                    receiptToUpdate.PaymentProviderData = ReceiptNumber;
                    receiptToUpdate.Notes = "Paid On Mobile";
                    receiptToUpdate.ModifiedDate = DateTime.UtcNow;
                    receiptToUpdate.ModifiedUserId = userId;
                    receiptToUpdate.ReceiptStatusId = (int)ReceiptStatuses.Completed;
                    receiptToUpdate.ReceiptStatusDate = DateTime.UtcNow;

                }
                await _context.SaveChangesAsync();
                await tx.CommitAsync(ct);


                return true;
            }
            catch
            {
                await tx.RollbackAsync(ct);
                throw;
            }
        }


    }
}
