using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.RequestUtility;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Application.Interfaces.Mobile;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Enums;
using TTVMAMobileWebMiddleware.Domain.Helpers;
using TTVMAMobileWebMiddleware.Domain.Requests;
using TTVMAMobileWebMiddleware.Domain.Views;

namespace TTVMAMobileWebMiddleware.Application.Services;

/// <summary>
/// Service implementation for citizen validation operations
/// </summary>
public class CitizenService : ICitizenService
{
    private readonly MOBDbContext _context;
    private readonly DLSDbContext _dlsContext;
    private readonly IExternalApiService _externalApiService;
    private readonly INotificationService _notificationService;
    private readonly ILogger<CitizenService> _logger;

    public CitizenService(
        MOBDbContext context,
        DLSDbContext dlsContext,
        IExternalApiService externalApiService,
        INotificationService notificationService,
        ILogger<CitizenService> logger)
    {
        _context = context;
        _dlsContext = dlsContext;
        _externalApiService = externalApiService;
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Links online citizen to existing local citizen and approves
    /// </summary>
    public async Task<CitizenLinkResponse> LinkAndApproveAsync(CitizenLinkRequest request, int userId, CancellationToken ct = default)
    {
        // Input validation
        if (request == null)
        {
            var ex = new Exception("Request is required");
            ex.HelpLink = "request_required";
            throw ex;
        }

        if (request.CitizenOnlineId <= 0)
        {
            var ex = new Exception("CitizenOnlineId must be greater than 0");
            ex.HelpLink = "invalid_citizen_online_id";
            throw ex;
        }

        if (request.CitizenLocalId <= 0)
        {
            var ex = new Exception("CitizenLocalId must be greater than 0");
            ex.HelpLink = "invalid_citizen_local_id";
            throw ex;
        }

        if (userId <= 0)
        {
            var ex = new Exception("UserId must be greater than 0");
            ex.HelpLink = "invalid_user_id";
            throw ex;
        }

        using var transaction = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            // 1. Validate online citizen exists
            var onlineCitizen = await _context.Citizens
                .FirstOrDefaultAsync(c => c.Id == request.CitizenOnlineId && (c.IsDeleted == null || c.IsDeleted == false), ct);

            if (onlineCitizen == null)
            {
                var ex = new Exception($"Online citizen {request.CitizenOnlineId} not found");
                ex.HelpLink = "online_citizen_not_found";
                throw ex;
            }

            // Business rule validation: Check if citizen is deleted
            if (onlineCitizen.IsDeleted == true)
            {
                var ex = new Exception($"Online citizen {request.CitizenOnlineId} is deleted");
                ex.HelpLink = "citizen_is_deleted";
                throw ex;
            }

            // Business rule validation: Check if citizen is already approved
            if (onlineCitizen.ApprovalStatusId == (int)CitizenStatus.Approved)
            {
                var ex = new Exception($"Citizen {request.CitizenOnlineId} is already approved");
                ex.HelpLink = "citizen_already_approved";
                throw ex;
            }

            // 2. Validate local citizen exists
            var localCitizen = await _dlsContext.Citizens
                .FirstOrDefaultAsync(c => c.Id == request.CitizenLocalId && c.IsDeleted != true, ct);

            if (localCitizen == null)
            {
                var ex = new Exception($"Local citizen {request.CitizenLocalId} not found");
                ex.HelpLink = "local_citizen_not_found";
                throw ex;
            }

            // Business rule validation: Validate link method
            if (!Enum.IsDefined(typeof(LinkMethod), request.LinkMethod))
            {
                var ex = new Exception($"Invalid link method: {request.LinkMethod}");
                ex.HelpLink = "invalid_link_method";
                throw ex;
            }

            // Business rule validation: Validate confidence score range (0.0 to 1.0)
            if (request.Confidence < 0.0m || request.Confidence > 1.0m)
            {
                var ex = new Exception($"Confidence score must be between 0.0 and 1.0. Current value: {request.Confidence}");
                ex.HelpLink = "invalid_confidence_score";
                throw ex;
            }

            // 3. Check if link already exists
            var existingLink = await _context.CitizenLinks
                .FirstOrDefaultAsync(cl => cl.CitizenOnlineId == request.CitizenOnlineId, ct);

            if (existingLink != null)
            {
                var ex = new Exception($"Citizen {request.CitizenOnlineId} is already linked");
                ex.HelpLink = "citizen_already_linked";
                throw ex;
            }
            // 4. Create CitizenLink record
            var citizenLink = new CitizenLink
            {
                CitizenOnlineId = request.CitizenOnlineId,
                CitizenLocalId = request.CitizenLocalId,
                LinkMethod = request.LinkMethod,
                Confidence = request.Confidence,
                LinkedByUserId = userId,
                LinkedAtUtc = DateTime.UtcNow,
                DecisionNote = request.DecisionNote
            };

            _context.CitizenLinks.Add(citizenLink);

            // 5. Augment missing optional fields in local citizen (non-destructive)
            bool localUpdated = false;
            if (string.IsNullOrEmpty(localCitizen.Email) && !string.IsNullOrEmpty(onlineCitizen.Email))
            {
                localCitizen.Email = onlineCitizen.Email;
                localUpdated = true;
            }

            if (string.IsNullOrEmpty(localCitizen.Phone) && !string.IsNullOrEmpty(onlineCitizen.Phone))
            {
                localCitizen.Phone = onlineCitizen.Phone;
                localUpdated = true;
            }

            if (localUpdated)
            {
                localCitizen.ModifiedDate = DateTime.UtcNow;
                localCitizen.ModifiedUserId = userId;
            }

            // 6. Set online citizen status to Approved
            onlineCitizen.IsValid = true;
            onlineCitizen.ApprovalStatusId = (int)CitizenStatus.Approved;
            onlineCitizen.ValidationDate = DateTime.UtcNow;
            onlineCitizen.ValidationUserId = userId;

            // 6.1. Create notification for status update (Approved)
            await CreateNotificationAsync(
                onlineCitizen.UserId,
                "Citizen Application Linked",
                "Your citizen application has been linked and approved successfully.",
                ct);

            // 7. Sync DL snapshot if exists
            var dlSnapshot = await GetDrivingLicenseSnapshotAsync(request.CitizenLocalId, ct);


            // 8. Get the driving license and driving license details from DLS database
            // Only add to mobile database if citizen is linked and driving license exists in DLS
            var dlsDrivingLicense = await _dlsContext.DrivingLicenses
                .Include(dl => dl.DrivingLicenseDetails)
                .Where(dl => dl.CitizenId == request.CitizenLocalId &&
                            dl.DrivingLicenseStatusId != null &&
                            !dl.IsDeleted)
                .OrderByDescending(dl => dl.IssuanceDate)
                .FirstOrDefaultAsync(ct);

            // 9. Add driving license and details to mobile database if they exist in DLS
            DrivingLicense? mobileDrivingLicense = null;
            if (dlsDrivingLicense != null)
            {
                mobileDrivingLicense = new DrivingLicense
                {
                    CitizenId = request.CitizenOnlineId,
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
                    CreatedDate = DateTime.UtcNow,
                    CreatedUserId = userId,
                    CitizenFullName = dlsDrivingLicense.CitizenFullName,
                    IsInternational = dlsDrivingLicense.IsInternational,
                    IsMigrated = false,
                    DrivingLicenseTypeId = dlsDrivingLicense.DrivingLicenseTypeId
                };

                _context.DrivingLicenses.Add(mobileDrivingLicense);
            }

            // Batch save: Save all changes (onlineCitizen, citizenLink, mobileDrivingLicense, notification) in one call
            await _context.SaveChangesAsync(ct);

            // Add driving license details if they exist (after mobileDrivingLicense.Id is available)
            if (dlsDrivingLicense != null && mobileDrivingLicense != null)
            {
                var dlsDetails = dlsDrivingLicense.DrivingLicenseDetails
                    .Where(d => !d.IsDeleted)
                    .ToList();

                if (dlsDetails.Any())
                {
                    List<DrivingLicenseDetail> mobileDetails = new List<DrivingLicenseDetail>();
                    foreach (var dlsDetail in dlsDetails)
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
                            CreatedDate = DateTime.UtcNow,
                            CreatedUserId = userId,
                            StatusId = dlsDetail.StatusId,
                            ProcessVarietyTypeId = dlsDetail.ProcessVarietyTypeId
                        };

                        mobileDetails.Add(mobileDetail);
                    }
                    _context.DrivingLicenseDetails.AddRange(mobileDetails);

                    // Save driving license details
                    await _context.SaveChangesAsync(ct);
                }
            }

            // Save DLS context changes if local citizen was updated
            if (localUpdated)
                await _dlsContext.SaveChangesAsync(ct);

            await transaction.CommitAsync(ct);

            _logger.LogInformation(
                "Citizen {OnlineId} linked to local citizen {LocalId} with method {Method}",
                request.CitizenOnlineId, request.CitizenLocalId, request.LinkMethod);

            return new CitizenLinkResponse
            {
                CitizenOnlineId = request.CitizenOnlineId,
                CitizenLocalId = request.CitizenLocalId,
                Status = "Approved",
                Message = "Citizen linked and approved successfully",
                DrivingLicenseSnapshot = dlSnapshot
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            var exception = new Exception($"Error linking citizen {request.CitizenOnlineId}");
            exception.HelpLink = "citizen_link_error";
            throw exception;
        }
    }

    /// <summary>
    /// Creates a new local citizen in DLS and links to online citizen
    /// </summary>
    public async Task<CitizenLinkResponse> CreateLocalAndApproveAsync(CitizenCreateLocalRequest request, int userId, CancellationToken ct = default)
    {
        // Input validation
        if (request == null)
        {
            var ex = new Exception("Request is required");
            ex.HelpLink = "request_required";
            throw ex;
        }

        if (request.CitizenOnlineId <= 0)
        {
            var ex = new Exception("CitizenOnlineId must be greater than 0");
            ex.HelpLink = "invalid_citizen_online_id";
            throw ex;
        }

        //if (userId <= 0)
        //{
        //    var ex = new Exception("UserId must be greater than 0");
        //    ex.HelpLink = "invalid_user_id";
        //    throw ex;
        //}

        using var transaction = await _context.Database.BeginTransactionAsync(ct);
        try
        {
            // 1. Get the citizen from mobile database
            var onlineCitizen = await _context.Citizens
                .Include(c => c.CitizenAddresses)
                .Include(c => c.CitizenIdentityDocuments)
                .Include(c => c.CitizenSignatures)
                .Include(c => c.CitizenFaceImages)
                .FirstOrDefaultAsync(c => c.Id == request.CitizenOnlineId && (c.IsDeleted == null || c.IsDeleted == false), ct);

            if (onlineCitizen == null)
            {
                var ex = new Exception($"Online citizen {request.CitizenOnlineId} not found");
                ex.HelpLink = "online_citizen_not_found";
                throw ex;
            }
            // 2. Get the address and detach from change tracker before modifying
            var addresses = onlineCitizen.CitizenAddresses
                .Where(a => !a.IsDeleted.HasValue || !a.IsDeleted.Value)
                .ToList();
            // Detach addresses from change tracker before modifying IDs
            foreach (var address in addresses)
            {
                _context.Entry(address).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            ResetIds(addresses);

            // 3. Get list of identity documents and detach from change tracker
            var identityDocuments = onlineCitizen.CitizenIdentityDocuments
                .Where(d => !d.IsDeleted.HasValue || !d.IsDeleted.Value)
                .ToList();
            foreach (var doc in identityDocuments)
            {
                _context.Entry(doc).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            ResetIds(identityDocuments);

            // 4. Get list of Signatures and detach from change tracker
            var signatures = onlineCitizen.CitizenSignatures
                .Where(s => !s.IsDeleted.HasValue || !s.IsDeleted.Value)
                .ToList();
            foreach (var signature in signatures)
            {
                _context.Entry(signature).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            ResetIds(signatures);

            // 5. Get list of FaceImages and detach from change tracker
            var faceImages = onlineCitizen.CitizenFaceImages
                .Where(f => !f.IsDeleted.HasValue || !f.IsDeleted.Value)
                .ToList();
            foreach (var faceImage in faceImages)
            {
                _context.Entry(faceImage).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            ResetIds(faceImages);

            // 6. Create a copy of the citizen for the API call (to avoid modifying tracked entity)
            // Serialize and deserialize to create a deep copy with Id = 0
            var citizenJson = System.Text.Json.JsonSerializer.Serialize(onlineCitizen, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            });
            var citizenCopy = System.Text.Json.JsonSerializer.Deserialize<Citizen>(citizenJson);
            if (citizenCopy == null)
            {
                var ex = new Exception("Failed to create citizen copy for API call");
                ex.HelpLink = "citizen_copy_creation_failed";
                throw ex;
            }
            citizenCopy.Id = 0;

            // 7. Fill the request CitizenWithDetailsRequest to call the API create-with-address-doc
            var citizenWithDetailsRequest = new CitizenWithDetailsRequest
            {
                Citizen = citizenCopy,
                Address = addresses,
                Document = identityDocuments,
                Signatures = signatures,
                FaceImages = faceImages
            };

            // Call external API to create citizen in DLS
            var apiResponse = await _externalApiService.CreateCitizenWithAddressAndDocumentsAsync(
                citizenWithDetailsRequest, ct);

            if (!apiResponse.IsSuccess)
            {
                var ex = new Exception(
                         $"Failed to create citizen in DLS: {apiResponse.ErrorMessage}");
                ex.HelpLink = "citizen_creation_failed_in_dls";
                throw ex;
            }

            // Parse the response to get the created citizen ID
            int localCitizenId;
            Citizen? citizenABP = null;
            try
            {
                if (string.IsNullOrEmpty(apiResponse.ResponseContent))
                {
                    var ex = new Exception("Empty response from DLS API");
                    ex.HelpLink = "dls_api_response_empty";
                    throw ex;
                }

                // Parse JSON response to extract the result object
                var responseJson = JsonDocument.Parse(apiResponse.ResponseContent);

                // Check if the response has a "result" property
                if (responseJson.RootElement.TryGetProperty("result", out var resultElement))
                {
                    // Deserialize the result object to CitizenABP
                    var resultJson = resultElement.GetRawText();
                    citizenABP = JsonConvert.DeserializeObject<Citizen>(resultJson);

                    if (citizenABP != null)
                    {
                        localCitizenId = citizenABP.Id;
                    }
                    else
                    {
                        var ex = new Exception(
                                 $"Failed to deserialize citizen from response: {apiResponse.ResponseContent}");
                        ex.HelpLink = "citizen_deserialize_failed";
                        throw ex;
                    }
                }
                else
                {
                    // Fallback: Try to parse directly as CitizenABP or extract ID from root
                    if (responseJson.RootElement.TryGetProperty("Id", out var citizenIdElement))
                    {
                        localCitizenId = citizenIdElement.GetInt32();
                        // Try to deserialize the entire response as CitizenABP
                        citizenABP = JsonConvert.DeserializeObject<Citizen>(apiResponse.ResponseContent);
                    }
                    else if (responseJson.RootElement.TryGetProperty("id", out var idElement))
                    {
                        localCitizenId = idElement.GetInt32();
                        // Try to deserialize the entire response as CitizenABP
                        citizenABP = JsonConvert.DeserializeObject<Citizen>(apiResponse.ResponseContent);
                    }
                    else if (responseJson.RootElement.ValueKind == JsonValueKind.Number)
                    {
                        localCitizenId = responseJson.RootElement.GetInt32();
                    }
                    else
                    {
                        // Try to parse as integer directly from string
                        if (!int.TryParse(apiResponse.ResponseContent.Trim(), out localCitizenId))
                        {
                            var ex = new Exception(
                                     $"Could not parse citizen ID from response: {apiResponse.ResponseContent}");
                            ex.HelpLink = "citizen_id_parse_failed";
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception px)
            {
                var ex = new Exception(
                         $"Could not parse citizen ID from DLS API response: {px.Message}");
                ex.HelpLink = "citizen_id_parse_failed";
                throw ex;
            }
            // Reload onlineCitizen without navigation properties to avoid tracking issues
            // This ensures we're working with a clean tracked entity when saving
            var onlineCitizenForSave = await _context.Citizens
                .FirstOrDefaultAsync(c => c.Id == request.CitizenOnlineId && (c.IsDeleted == null || c.IsDeleted == false), ct);

            if (onlineCitizenForSave == null)
            {
                var ex = new Exception($"Online citizen {request.CitizenOnlineId} not found for saving");
                ex.HelpLink = "online_citizen_not_found_for_save";
                throw ex;
            }
            DrivingLicenseSnapshot? dlSnapshot = await UpdateStatusAndAddLinkNew(request, userId, onlineCitizenForSave, localCitizenId, ct);
            await transaction.CommitAsync(ct);

            _logger.LogInformation(
                "Citizen {OnlineId} created in DLS as local citizen {LocalId}",
                request.CitizenOnlineId, localCitizenId);

            return new CitizenLinkResponse
            {
                CitizenOnlineId = request.CitizenOnlineId,
                CitizenLocalId = localCitizenId,
                Status = "Approved",
                Message = "Citizen created in DLS and approved successfully",
                DrivingLicenseSnapshot = dlSnapshot
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(ct);
            var exception = new Exception($"Error creating local citizen for {request.CitizenOnlineId}");
            exception.HelpLink = "citizen_create_local_error";
            throw exception;
        }
    }


    /// <summary>
    /// Reviews and merges citizen data for medium-confidence matches
    /// </summary>
    public async Task<CitizenLinkResponse> ReviewAndMergeAsync(CitizenLinkRequest request, int userId, CancellationToken ct = default)
    {
        // For medium matches, officer has already reviewed side-by-side
        // This is essentially the same as LinkAndApprove, but with merge notes
        // The actual merge logic (picking local-wins for identity keys) is handled
        // by the officer's decision in the request
        return await LinkAndApproveAsync(request, userId, ct);
    }

    /// <summary>
    /// Approves or rejects an online citizen
    /// </summary>
    public async Task<bool> ApproveOnlineCitizenAsync(CitizenApproveRequest request, int userId, CancellationToken ct = default)
    {
        // Input validation
        if (request == null)
        {
            var ex = new Exception("Request is required");
            ex.HelpLink = "request_required";
            throw ex;
        }

        if (request.CitizenOnlineId <= 0)
        {
            var ex = new Exception("CitizenOnlineId must be greater than 0");
            ex.HelpLink = "invalid_citizen_online_id";
            throw ex;
        }

        if (userId <= 0)
        {
            var ex = new Exception("UserId must be greater than 0");
            ex.HelpLink = "invalid_user_id";
            throw ex;
        }

        var onlineCitizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == request.CitizenOnlineId && (c.IsDeleted == null || c.IsDeleted == false), ct);

        if (onlineCitizen == null)
        {
            var ex = new Exception($"Online citizen {request.CitizenOnlineId} not found");
            ex.HelpLink = "online_citizen_not_found";
            throw ex;
        }
        if (request.IsApproved)
        {
            onlineCitizen.IsValid = true;
            onlineCitizen.ApprovalStatusId = (int)CitizenStatus.Approved;
            onlineCitizen.ValidationDate = DateTime.UtcNow;
            onlineCitizen.ValidationUserId = userId;

            // Create notification for status update (Approved)
            await CreateNotificationAsync(
                onlineCitizen.UserId,
                "Citizen Application Approved",
                "Your citizen application has been approved successfully.",
                ct);
        }
        else
        {
            onlineCitizen.IsValid = false;
            onlineCitizen.ApprovalStatusId = (int)CitizenStatus.Rejected;
            onlineCitizen.ValidationDate = DateTime.UtcNow;
            onlineCitizen.ValidationUserId = userId;
            onlineCitizen.Notes = $"Rejected: {request.RejectionReason}";

            // Create notification for status update (Rejected)
            await CreateNotificationAsync(
                onlineCitizen.UserId,
                "Citizen Application Rejected",
                $"Your citizen application has been rejected. Reason: {request.RejectionReason ?? "No reason provided"}",
                ct);
        }
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Citizen {CitizenId} {Action} by user {UserId}",
            request.CitizenOnlineId, request.IsApproved ? "approved" : "rejected", userId);

        return true;
    }


    /// <summary>
    /// Searches for citizens in local DLS using matching algorithm
    /// </summary>
    public async Task<CitizenSearchResponse> SearchLocalAsync(CitizenSearchRequest request, CancellationToken ct = default)
    {
        // Input validation
        if (request == null)
        {
            var ex = new Exception("Search request is required");
            ex.HelpLink = "search_request_required";
            throw ex;
        }

        // Validate that at least one search criteria is provided
        var hasSearchCriteria = !string.IsNullOrWhiteSpace(request.NationalId) ||
                               !string.IsNullOrWhiteSpace(request.PassportNumber) ||
                               !string.IsNullOrWhiteSpace(request.RegistrationNumber) ||
                               !string.IsNullOrWhiteSpace(request.FirstNameAr) ||
                               !string.IsNullOrWhiteSpace(request.FirstNameEn) ||
                               !string.IsNullOrWhiteSpace(request.LastNameAr) ||
                               !string.IsNullOrWhiteSpace(request.LastNameEn) ||
                               !string.IsNullOrWhiteSpace(request.Mobile);

        if (!hasSearchCriteria)
        {
            var ex = new Exception("At least one search criteria is required (NationalId, PassportNumber, RegistrationNumber, Name, or Mobile)");
            ex.HelpLink = "search_criteria_required";
            throw ex;
        }

        var stopwatch = Stopwatch.StartNew();
        var queryId = QueryIdGenerator.GenerateQueryId();

        try
        {
            // 1. Normalize query
            var normalized = NormalizeQuery(request);
            var hypocorismSet = HypocorismHelper.BuildHypocorismSet(
                normalized.FirstNameAr,
                normalized.FirstNameEn);

            // 2. Build candidate pool from Local DLS (document-first search)
            var candidates = new HashSet<CitizenABP>();

            // Exact key searches (document-first as per DLS behavior)
            if (!string.IsNullOrEmpty(normalized.NationalId))
            {
                var byNid = await _dlsContext.Citizens
                    .AsNoTracking()
                    .Include(x => x.CitizenAddresses)
                    .Include(x => x.CitizenIdentityDocuments)
                    .Where(c => c.NationalId == normalized.NationalId && c.IsDeleted != true)
                    .ToListAsync(ct);
                foreach (var c in byNid) candidates.Add(c);
            }

            if (!string.IsNullOrEmpty(normalized.PassportNumber))
            {
                var byPassport = await _dlsContext.Citizens
                    .AsNoTracking()
                    .Include(x => x.CitizenAddresses)
                    .Include(x => x.CitizenIdentityDocuments)
                    .Where(c => c.PassportNumber == normalized.PassportNumber && c.IsDeleted != true)
                    .ToListAsync(ct);
                foreach (var c in byPassport) candidates.Add(c);
            }

            if (!string.IsNullOrEmpty(normalized.RegistrationNumber))
            {
                var byReg = await _dlsContext.Citizens
                    .AsNoTracking()
                    .Include(x => x.CitizenAddresses)
                    .Include(x => x.CitizenIdentityDocuments)
                    .Where(c => c.RegisterId == normalized.RegistrationNumber && c.IsDeleted != true)
                    .ToListAsync(ct);
                foreach (var c in byReg) candidates.Add(c);
            }

            // Name-driven expansion (Arabic/Latin)
            if (HasNameData(normalized))
            {
                // Arabic triplet + DoB
                if (!string.IsNullOrEmpty(normalized.FirstNameAr) &&
                    !string.IsNullOrEmpty(normalized.FatherNameAr) &&
                    !string.IsNullOrEmpty(normalized.LastNameAr) &&
                    !string.IsNullOrEmpty(normalized.DateOfBirth))
                {
                    var byArabicNameDob = await _dlsContext.Citizens
                        .AsNoTracking()
                        .Include(x => x.CitizenAddresses)
                        .Include(x => x.CitizenIdentityDocuments)
                        .Where(c =>
                                ((c.FirstNameSecondLang == normalized.FirstNameEn &&
                                  c.LastNameSecondLang == normalized.LastNameEn) ||
                                 (c.FirstName == normalized.FirstNameAr &&
                                  c.FathersName == normalized.FatherNameAr &&
                                  c.LastName == normalized.LastNameAr)) &&
                                c.DateOfBirth.HasValue &&
                                normalized.DateOfBirth != null &&
                                ParseNormalizedDate(normalized.DateOfBirth).HasValue &&
                                c.DateOfBirth.Value.Date == ParseNormalizedDate(normalized.DateOfBirth).Value.Date &&
                                c.IsDeleted != true)
                        .ToListAsync(ct);
                    foreach (var c in byArabicNameDob) candidates.Add(c);
                }

                // Latin pair + DoB
                if (!string.IsNullOrEmpty(normalized.FirstNameEn) &&
                    !string.IsNullOrEmpty(normalized.LastNameEn))
                {
                    var byLatinNameDob = await _dlsContext.Citizens
                        .AsNoTracking()
                        .Include(x => x.CitizenAddresses)
                        .Include(x => x.CitizenIdentityDocuments)
                        .Where(c =>
                           ((c.FirstNameSecondLang == normalized.FirstNameEn &&
                            c.LastNameSecondLang == normalized.LastNameEn) ||
                            (c.FirstName == normalized.FirstNameEn &&
                            c.LastName == normalized.LastNameEn) ||
                            (c.FirstName == normalized.FirstNameAr &&
                            c.FathersName == normalized.FatherNameAr &&
                            c.LastName == normalized.LastNameAr)) &&
                            (c.IsDeleted == false || c.IsDeleted == null))
                        .ToListAsync(ct);
                    foreach (var c in byLatinNameDob) candidates.Add(c);
                }

                // Hypocorism variants
                foreach (var variantAr in hypocorismSet.NameVariantsAr)
                {
                    if (!string.IsNullOrEmpty(normalized.DateOfBirth))
                    {
                        var byVariantAr = await _dlsContext.Citizens
                            .AsNoTracking()
                            .Include(x => x.CitizenAddresses)
                            .Include(x => x.CitizenIdentityDocuments)
                            .Where(c =>
                                c.FirstName == variantAr &&
                                c.DateOfBirth.HasValue &&
                                normalized.DateOfBirth != null &&
                                ParseNormalizedDate(normalized.DateOfBirth).HasValue &&
                                c.DateOfBirth.Value.Date == ParseNormalizedDate(normalized.DateOfBirth).Value.Date &&
                                (c.IsDeleted == null || c.IsDeleted == false))
                            .ToListAsync(ct);
                        foreach (var c in byVariantAr) candidates.Add(c);
                    }
                }

                foreach (var variantEn in hypocorismSet.NameVariantsEn)
                {
                    if (!string.IsNullOrEmpty(normalized.DateOfBirth))
                    {
                        var byVariantEn = await _dlsContext.Citizens
                            .AsNoTracking()
                            .Include(x => x.CitizenAddresses)
                            .Include(x => x.CitizenIdentityDocuments)
                            .Where(c =>
                                c.FirstNameSecondLang == variantEn &&
                                c.DateOfBirth.HasValue &&
                                normalized.DateOfBirth != null &&
                                ParseNormalizedDate(normalized.DateOfBirth).HasValue &&
                                c.DateOfBirth.Value.Date == ParseNormalizedDate(normalized.DateOfBirth).Value.Date &&
                                (c.IsDeleted == null || c.IsDeleted == false))
                            .ToListAsync(ct);
                        foreach (var c in byVariantEn) candidates.Add(c);
                    }
                }
            }

            // Historical mobile (low weight auxiliary)
            if (!string.IsNullOrEmpty(normalized.Mobile))
            {
                var byMobile = await _dlsContext.Citizens
                    .AsNoTracking()
                    .Include(x => x.CitizenAddresses)
                    .Include(x => x.CitizenIdentityDocuments)
                    .Where(c => c.Phone == normalized.Mobile && c.IsDeleted != true)
                    .ToListAsync(ct);
                foreach (var c in byMobile) candidates.Add(c);
            }

            // 3. Deduplicate by ID
            var uniqueCandidates = candidates.DistinctBy(c => c.Id).ToList();

            // 4. Score each candidate
            var scoredCandidates = new List<(CitizenABP candidate, double score, List<string> reasons, Dictionary<string, object> fields)>();

            foreach (var candidate in uniqueCandidates)
            {
                var (score, reasons, fields) = ScoreCandidate(candidate, normalized, hypocorismSet);
                scoredCandidates.Add((candidate, score, reasons, fields));
            }

            // 5. Rank and apply thresholds
            var ranked = scoredCandidates
                .OrderByDescending(x => x.score)
                .Select(x => new CitizenCandidate
                {
                    CitizenLocalId = x.candidate.Id,
                    Score = x.score,
                    Confidence = x.score >= 0.90 ? "HIGH" : x.score >= 0.75 ? "MEDIUM" : "LOW",
                    MatchReasons = x.reasons,
                    FieldsMatched = x.fields,
                    citizens = uniqueCandidates.Where(cc => cc.Id == x.candidate.Id).FirstOrDefault()
                })
                .ToList();

            stopwatch.Stop();

            return new CitizenSearchResponse
            {
                QueryId = queryId,
                Candidates = ranked,
                Audit = new SearchAuditInfo
                {
                    TookMs = (int)stopwatch.ElapsedMilliseconds,
                    Normalized = true,
                    HypocorismApplied = hypocorismSet.Applied
                }
            };
        }
        catch (Exception ex)
        {
            var exception = new Exception("Error searching for citizens");
            exception.HelpLink = "citizen_search_error";
            throw exception;
        }
    }
    public async Task<(IEnumerable<CitizenListItemDto> items, PaginationMetaData metaData)> SearchMobileCitizen(Pagination pagination, int? status = null, CancellationToken ct = default)
    {
        // Normalize status input 

        // Build base query
        var query = _context.Citizens
            .AsNoTracking()
            .Where(x => x.IsValid == false && (x.ApprovalStatusId == (int)CitizenStatus.PendingValidation || status == null) && (x.IsDeleted == false || x.IsDeleted == null));



        var totalCount = await query.CountAsync(ct);

        // Project to DTO with status names using join
        var items = await query
            .Join(_context.Statuses,
                citizen => citizen.ApprovalStatusId,
                status => status.ID,
                (citizen, status) => new CitizenListItemDto
                {
                    Id = citizen.Id,
                    UserId = citizen.UserId,
                    FirstName = citizen.FirstName,
                    LastName = citizen.LastName,
                    MiddleName = citizen.MiddleName,
                    DateOfBirth = citizen.DateOfBirth,
                    NationalId = citizen.NationalId,
                    Phone = citizen.Phone,
                    Email = citizen.Email,
                    ApprovalStatusId = citizen.ApprovalStatusId,
                    ApprovalStatusEn = status.StatusDesc,
                    ApprovalStatusAr = status.StatusDescAr,
                    ApprovalStatusFr = status.StatusDescFr,
                    CreatedDate = citizen.CreatedDate,
                    ModifiedDate = citizen.ModifiedDate
                })
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        var metaData = PageList<CitizenListItemDto>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize).MetaData;
        return (items, metaData);
    }

    public async Task<Citizen> GetOnlineCitizenById(int citizenId, CancellationToken ct = default)
    {
        return await _context.Citizens
            .Include(x => x.CitizenIdentityDocuments)
            .ThenInclude(x => x.Document)
            .Where(x => x.Id == citizenId && x.IsValid == false && x.ApprovalStatusId == (int)CitizenStatus.PendingValidation && (x.IsDeleted == false || x.IsDeleted == null))
            .FirstOrDefaultAsync();

    }


    /// <summary>
    /// Safely parses a normalized date string (YYYY-MM-DD) to DateTime
    /// </summary>
    private static DateTime? ParseNormalizedDate(string? dateStr)
    {
        if (string.IsNullOrWhiteSpace(dateStr))
            return null;

        // Try parsing YYYY-MM-DD format first
        if (DateTime.TryParseExact(dateStr, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out var parsedDate))
        {
            return parsedDate;
        }

        // Fallback to general parsing
        if (DateTime.TryParse(dateStr, System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None, out parsedDate))
        {
            return parsedDate;
        }

        return null;
    }

    private NormalizedQuery NormalizeQuery(CitizenSearchRequest request)
    {
        return new NormalizedQuery
        {
            NationalId = NameNormalizer.NormalizeDocumentNo(request.NationalId),
            PassportNumber = NameNormalizer.NormalizeDocumentNo(request.PassportNumber),
            RegistrationNumber = NameNormalizer.NormalizeRegistrationNo(request.RegistrationNumber),
            FirstNameAr = NameNormalizer.NormalizeArabicName(request.FirstNameAr),
            FatherNameAr = NameNormalizer.NormalizeArabicName(request.FatherNameAr),
            LastNameAr = NameNormalizer.NormalizeArabicName(request.LastNameAr),
            MotherNameAr = NameNormalizer.NormalizeArabicName(request.MotherNameAr),
            FirstNameEn = NameNormalizer.NormalizeLatinName(request.FirstNameEn),
            LastNameEn = NameNormalizer.NormalizeLatinName(request.LastNameEn),
            DateOfBirth = NameNormalizer.NormalizeDateOfBirth(request.DateOfBirth),
            Mobile = NameNormalizer.NormalizePhone(request.Mobile)
        };
    }

    private bool HasNameData(NormalizedQuery query)
    {
        return !string.IsNullOrEmpty(query.FirstNameAr) ||
               !string.IsNullOrEmpty(query.FirstNameEn) ||
               !string.IsNullOrEmpty(query.LastNameAr) ||
               !string.IsNullOrEmpty(query.LastNameEn);
    }

    private (double score, List<string> reasons, Dictionary<string, object> fields) ScoreCandidate(CitizenABP candidate, NormalizedQuery query, HypocorismSet hypocorismSet)
    {
        double score = 0.0;
        var reasons = new List<string>();
        var fields = new Dictionary<string, object>();

        // Normalize candidate names for fair comparison with normalized query
        var candidateFirstNameAr = NameNormalizer.NormalizeArabicName(candidate.FirstName);
        var candidateFatherNameAr = NameNormalizer.NormalizeArabicName(candidate.FathersName);
        var candidateLastNameAr = NameNormalizer.NormalizeArabicName(candidate.LastName);
        var candidateFirstNameEn = NameNormalizer.NormalizeLatinName(candidate.FirstNameSecondLang);
        var candidateLastNameEn = NameNormalizer.NormalizeLatinName(candidate.LastNameSecondLang);

        // 4.1 Deterministic keys (short-circuit to high)
        if (!string.IsNullOrEmpty(query.NationalId) &&
            candidate.NationalId == query.NationalId)
        {
            score = Math.Max(score, 1.00);
            reasons.Add("NATIONAL_ID");
            fields["nationalId"] = true;
        }

        if (!string.IsNullOrEmpty(query.PassportNumber) &&
            candidate.PassportNumber == query.PassportNumber)
        {
            score = Math.Max(score, 1.00);
            reasons.Add("PASSPORT");
            fields["passportNo"] = true;
        }

        if (!string.IsNullOrEmpty(query.RegistrationNumber) &&
            candidate.RegisterId == query.RegistrationNumber)
        {
            score = Math.Max(score, 0.95);
            reasons.Add("REGISTRATION");
            fields["registrationNo"] = true;
        }

        // 4.2 Composite: Arabic triplet + DoB
        if (!string.IsNullOrEmpty(query.FirstNameAr) &&
            !string.IsNullOrEmpty(query.FatherNameAr) &&
            !string.IsNullOrEmpty(query.LastNameAr) &&
            !string.IsNullOrEmpty(query.DateOfBirth))
        {
            // Check for exact match first (highest confidence)
            bool exactMatchAr = candidateFirstNameAr == query.FirstNameAr &&
                               candidateFatherNameAr == query.FatherNameAr &&
                               candidateLastNameAr == query.LastNameAr;

            var nameArScore = exactMatchAr ? 1.0 : MatchingAlgorithms.TripletScoreAr(
                candidateFirstNameAr, candidateFatherNameAr, candidateLastNameAr,
                query.FirstNameAr, query.FatherNameAr, query.LastNameAr);

            var queryDob = ParseNormalizedDate(query.DateOfBirth);
            if (nameArScore >= 0.92 && candidate.DateOfBirth.HasValue && queryDob.HasValue && candidate.DateOfBirth.Value.Date == queryDob.Value.Date)
            {
                score = Math.Max(score, exactMatchAr ? 0.95 : 0.90);
                reasons.Add("NAME_AR_DOB_COMPOSITE");
                fields["nameAr"] = $"{query.FirstNameAr} {query.FatherNameAr} {query.LastNameAr} (normalized)";
                fields["dob"] = true;
            }
        }


        // 4.3 Composite: Latin pair + DoB
        if (!string.IsNullOrEmpty(query.FirstNameEn) &&
            !string.IsNullOrEmpty(query.LastNameEn) &&
            !string.IsNullOrEmpty(query.DateOfBirth))
        {
            // Check for exact match first (highest confidence)
            bool exactMatchEn = candidateFirstNameEn == query.FirstNameEn &&
                               candidateLastNameEn == query.LastNameEn;

            var nameEnScore = exactMatchEn ? 1.0 : MatchingAlgorithms.PairScoreEn(
                candidateFirstNameEn, candidateLastNameEn,
                query.FirstNameEn, query.LastNameEn);

            var queryDobEn = ParseNormalizedDate(query.DateOfBirth);
            if (nameEnScore >= 0.90 && candidate.DateOfBirth.HasValue && queryDobEn.HasValue && candidate.DateOfBirth.Value.Date == queryDobEn.Value.Date)
            {
                score = Math.Max(score, exactMatchEn ? 0.80 : 0.75);
                reasons.Add("NAME_EN_DOB_COMPOSITE");
                fields["nameEn"] = $"{query.FirstNameEn} {query.LastNameEn} (normalized)";
                fields["dob"] = true;
            }
        }

        // 4.4 Hypocorism variants
        if (hypocorismSet.Applied && !string.IsNullOrEmpty(query.DateOfBirth))
        {
            foreach (var variantAr in hypocorismSet.NameVariantsAr)
            {
                var queryDobVarAr = ParseNormalizedDate(query.DateOfBirth);
                // Compare normalized candidate name with normalized variant
                var normalizedVariantAr = NameNormalizer.NormalizeArabicName(variantAr);
                if (candidateFirstNameAr == normalizedVariantAr &&
                    candidate.DateOfBirth.HasValue &&
                    queryDobVarAr.HasValue &&
                    candidate.DateOfBirth.Value.Date == queryDobVarAr.Value.Date)
                {
                    score = Math.Max(score, 0.90);
                    reasons.Add("HYPOCORISM_VARIANT");
                    fields["nameAr"] = $"{query.FirstNameAr} ~ {variantAr} (hypocorism variant)";
                    fields["dob"] = true;
                    break;
                }
            }

            foreach (var variantEn in hypocorismSet.NameVariantsEn)
            {
                var queryDobVarEn = ParseNormalizedDate(query.DateOfBirth);
                // Compare normalized candidate name with normalized variant
                var normalizedVariantEn = NameNormalizer.NormalizeLatinName(variantEn);
                if (candidateFirstNameEn == normalizedVariantEn &&
                    candidate.DateOfBirth.HasValue &&
                    queryDobVarEn.HasValue &&
                    candidate.DateOfBirth.Value.Date == queryDobVarEn.Value.Date)
                {
                    score = Math.Max(score, 0.90);
                    reasons.Add("HYPOCORISM_VARIANT");
                    fields["nameEn"] = $"{query.FirstNameEn} ~ {variantEn} (hypocorism variant)";
                    fields["dob"] = true;
                    break;
                }
            }
        }

        // 4.5 Fuzzy matching (weighted)
        if (score < 0.90) // Only if no hard match
        {
            var fuzzyAr = MatchingAlgorithms.TripletScoreAr(
                candidateFirstNameAr, candidateFatherNameAr, candidateLastNameAr,
                query.FirstNameAr, query.FatherNameAr, query.LastNameAr);

            var fuzzyEn = MatchingAlgorithms.PairScoreEn(
                candidateFirstNameEn, candidateLastNameEn,
                query.FirstNameEn, query.LastNameEn);

            // Weight Arabic higher (0.7) than Latin (0.3)
            // Use the higher of the two scores if one is missing
            double weightedFuzzy;
            if (fuzzyAr > 0 && fuzzyEn > 0)
            {
                weightedFuzzy = (fuzzyAr * 0.7) + (fuzzyEn * 0.3);
            }
            else if (fuzzyAr > 0)
            {
                weightedFuzzy = fuzzyAr;
            }
            else if (fuzzyEn > 0)
            {
                weightedFuzzy = fuzzyEn;
            }
            else
            {
                weightedFuzzy = 0;
            }

            if (weightedFuzzy >= 0.85)
            {
                score = Math.Max(score, Math.Min(weightedFuzzy, 0.89));
                reasons.Add("FUZZY_NAME");
                if (fuzzyAr >= 0.85)
                    fields["nameAr"] = "close";
                if (fuzzyEn >= 0.85)
                    fields["nameEn"] = "close";
            }
            else if (weightedFuzzy > 0 && score == 0)
            {
                // Give partial credit for any fuzzy match, even if below threshold
                score = Math.Max(score, Math.Min(weightedFuzzy * 0.8, 0.74)); // Cap at 0.74 for low confidence
                reasons.Add("FUZZY_NAME");
                if (fuzzyAr > 0.5)
                    fields["nameAr"] = "partial";
                if (fuzzyEn > 0.5)
                    fields["nameEn"] = "partial";
            }
        }

        // 4.6 Auxiliary signals
        if (!string.IsNullOrEmpty(query.Mobile) && candidate.Phone == query.Mobile)
        {
            score = Math.Min(score + 0.03, 1.00);
            reasons.Add("MOBILE_AUX");
            fields["mobile"] = true;
        }

        score = Math.Min(score, 1.00);
        return (score, reasons, fields);
    }
    public async Task<bool> RejectCitizen(int id, string reason, CancellationToken ct = default)
    {
        // Input validation
        if (id <= 0)
        {
            var ex = new Exception("Citizen ID must be greater than 0");
            ex.HelpLink = "invalid_citizen_id";
            throw ex;
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            var ex = new Exception("Rejection reason is required");
            ex.HelpLink = "rejection_reason_required";
            throw ex;
        }

        try
        {
            var citizenToUpdate = await _context.Citizens
                .FirstOrDefaultAsync(
                    a => (a.Id == id) && (a.IsDeleted == null || a.IsDeleted == false),
                    ct);

            if (citizenToUpdate == null)
            {
                var ex = new Exception("citizen not found");
                ex.HelpLink = "citizen_not_found";
                throw ex;
            }
            citizenToUpdate.IsValid = false;
            citizenToUpdate.ApprovalStatusId = (int)CitizenStatus.Rejected;
            citizenToUpdate.ModifiedDate = DateTime.UtcNow;

            ///to do add notification 
            //await _context.SaveChangesAsync(ct);
            Notification notification = new Notification
            {
                UserId = citizenToUpdate.UserId,
                Title = "Citizen Application Rejected",
                Message = $"Your citizen application has been rejected. Reason: {reason}",
                IsRead = false,
                CreatedDate = DateTime.UtcNow
            };
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync(ct);
            //_notificationService.AddAsync(notification);
            return true;
        }
        catch (Exception ex)
        {
            var exception = new Exception($"Error rejecting citizen {id}");
            exception.HelpLink = "citizen_reject_error";
            throw exception;
        }
    }


    /// <summary>
    /// Gets driving license snapshot for a local citizen
    /// </summary>
    private async Task<DrivingLicenseSnapshot?> GetDrivingLicenseSnapshotAsync(int citizenLocalId, CancellationToken ct)
    {
        var dl = await _dlsContext.DrivingLicenses
        .Include(dl => dl.DrivingLicenseDetails)
        .Where(dl => dl.CitizenId == citizenLocalId &&
                    dl.DrivingLicenseStatusId != null && // Active license
                    !dl.IsDeleted)
        .OrderByDescending(dl => dl.IssuanceDate)
        .FirstOrDefaultAsync(ct);

        if (dl == null)
            return null;

        var categories = dl.DrivingLicenseDetails
            .Where(d => !d.IsDeleted)
            .Select(d => d.BPVarietyId.ToString())
            .ToList();

        // Get structure name if needed
        string? structureName = null;
        if (dl.StructureId.HasValue)
        {
            var structure = await _dlsContext.Structures
                .FirstOrDefaultAsync(s => s.Id == dl.StructureId.Value, ct);
            structureName = structure?.NameAr ?? structure?.Name;
        }

        return new DrivingLicenseSnapshot
        {
            LicenseNumber = dl.DrivingLicenseNumber,
            Categories = categories,
            IssuanceDate = dl.IssuanceDate,
            ExpiryDate = dl.ExpiryDate,
            StructureId = dl.StructureId,
            StructureName = structureName
        };
    }

    /// <summary>
    /// Resets the Id property to 0 for all items in a collection
    /// </summary>
    private static void ResetIds<T>(IEnumerable<T> items) where T : class
    {
        foreach (var item in items)
        {
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null && idProperty.CanWrite)
            {
                idProperty.SetValue(item, 0);
            }
        }
    }

    /// <summary>
    /// Creates a notification for a user and saves it to the mobile database
    /// </summary>
    private async Task CreateNotificationAsync(long userId, string title, string message, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedDate = DateTime.UtcNow
        };
        await _notificationService.AddAsync(notification, ct);
    }
    private async Task<DrivingLicenseSnapshot?> UpdateStatusAndAddLinkNew(CitizenCreateLocalRequest request, int userId, Citizen onlineCitizen, int localCitizenId, CancellationToken ct)
    {

        //// 8. Create CitizenLink record first
        var citizenLink = new CitizenLink
        {
            CitizenOnlineId = request.CitizenOnlineId,
            CitizenLocalId = localCitizenId,
            LinkMethod = LinkMethod.Composite, // Default for created citizens
            Confidence = 1.0m, // Full confidence for newly created
            LinkedByUserId = userId,
            LinkedAtUtc = DateTime.UtcNow,
            DecisionNote = "Citizen created in DLS via create-local endpoint"
        };

        _context.CitizenLinks.Add(citizenLink);
        await _context.SaveChangesAsync(ct);

        // 9. Set online citizen status to Approved
        onlineCitizen.IsValid = true;
        onlineCitizen.ApprovalStatusId = (int)CitizenStatus.Approved;
        onlineCitizen.ValidationDate = DateTime.UtcNow;
        onlineCitizen.ValidationUserId = userId;

        // 9.1. Create notification for status update (Approved)
        await CreateNotificationAsync(
            onlineCitizen.UserId,
            "Citizen Application Approved",
            "Your citizen application has been created in DLS and approved successfully.",
            ct);

        // 10. Get DL snapshot if exists
        var dlSnapshot = await GetDrivingLicenseSnapshotAsync(localCitizenId, ct);

        // 11. Final save and commit transaction
        await _context.SaveChangesAsync(ct);
        return dlSnapshot;
    }

}

