using Azure.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Shared.RequestUtility;
using Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Enums;
using TTVMAMobileWebMiddleware.Domain.Views;
using static System.Net.Mime.MediaTypeNames;
using ApplicationStatus = TTVMAMobileWebMiddleware.Domain.Enums.ApplicationStatus;

namespace TTVMAMobileWebMiddleware.Application.Services;

/// <summary>
/// Service implementation for application approval operations
/// </summary>
public class ApplicationService : IApplicationService
{
    private readonly MOBDbContext _context;
    private readonly DLSDbContext _dlsdbContext;
    private readonly ISequenceGeneratorService _sequenceService;
    private readonly IExternalApiService _externalApiService;
    private readonly IReceiptService _receiptService;

    private readonly ILogger<ApplicationService> _logger;
    private readonly IMemoryCache _cache;

    public ApplicationService(
        MOBDbContext context,
        DLSDbContext dlsdbContext,
        IMemoryCache cache,
        ISequenceGeneratorService sequenceService,
        IExternalApiService externalApiService,
        IReceiptService receiptService,
        ILogger<ApplicationService> logger)
    {
        _context = context;
        _dlsdbContext = dlsdbContext;
        _sequenceService = sequenceService;
        _externalApiService = externalApiService;
        _receiptService = receiptService;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all non-deleted Application records with pagination.
    /// </summary>
    /// <param name="pagination">Pagination parameters</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>A tuple containing a list of Application records and pagination metadata.</returns>
    public async Task<(IEnumerable<ApplicationMob> items, PaginationMetaData metaData)> GetAllAsync(Pagination pagination, CancellationToken ct = default)
    {
        var query = _context.Applications
            .AsNoTracking()
            .Include(a => a.Citizen)
            .ThenInclude(c => c.CitizenAddresses)
            .Where(a => a.IsDeleted != true && a.ApplicationApprovalStatusId == (int)ApplicationStatus.Pending);
        var cacheKey = "ApplicationCount_All";

        var totalCount = await _cache.GetOrCreateAsync(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);
            return query.CountAsync(ct);
        });

        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
                               .Take(pagination.PageSize)
                               .ToListAsync(ct);

        var metaData = PageList<ApplicationMob>.ToPageList(items, totalCount, pagination.PageNumber, pagination.PageSize).MetaData;
        return (items, metaData);
    }


    /// <summary>
    /// Retrieves a specific Application by its ID.
    /// </summary>
    /// <param name="id">The ID of the Application to retrieve.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The Application record with the specified ID, or null if not found.</returns>
    public async Task<ApplicationMob?> GetByIdAsync(string id, CancellationToken ct = default)
    {
        return await _context.Applications
            .Include(a => a.Citizen)
            .ThenInclude(c => c.CitizenAddresses)
            .Where(a => (a.ApplicationNumber == id || a.Id == id) && a.IsDeleted != true && a.ApplicationApprovalStatusId == (int)ApplicationStatus.Pending)
            .Select(a => new ApplicationMob
            {
                Id = a.Id,
                ApplicationTypeId = a.ApplicationTypeId,
                StatusId = a.StatusId,
                CreatedDate = a.CreatedDate,
                ApplicationApprovalStatus = a.ApplicationApprovalStatus != null ? new Domain.Entities.Mobile.Status
                {
                    ID = a.ApplicationApprovalStatus.ID,
                    StatusDesc = a.ApplicationApprovalStatus.StatusDesc,
                    StatusDescAr = a.ApplicationApprovalStatus.StatusDescAr,
                    StatusDescFr = a.ApplicationApprovalStatus.StatusDescFr
                } : null,
                ApplicationProcesses = a.ApplicationProcesses
                .Select(proc => new Domain.Entities.Mobile.ApplicationProcess
                {
                    ApplicationId = proc.ApplicationId,
                    ProcessId = proc.ProcessId,
                    BPVarietyId = proc.BPVarietyId,
                    ApplicationProcessCheckLists = proc.ApplicationProcessCheckLists
                    .Select(cl => new Domain.Entities.Mobile.ApplicationProcessCheckList
                    {
                        ApplicationId = cl.ApplicationId,
                        ProcessId = proc.ProcessId,
                        BPVarietyId = cl.BPVarietyId,
                        ChekListId = cl.ChekListId,
                        IsDocRequired = cl.IsDocRequired
                    }).ToList(),
                    ApplicationProcessConditions = proc.ApplicationProcessConditions
                    .Select(c => new Domain.Entities.Mobile.ApplicationProcessCondition
                    {
                        ApplicationId = c.ApplicationId,
                        ProcessId = c.ProcessId,
                        BPVarietyId = c.BPVarietyId,
                        ConditionId = c.ConditionId,
                        ConditionGroupId = c.ConditionGroupId,
                        ConditionCode = c.ConditionCode,
                        CitizenAge = c.CitizenAge,
                        Age1 = c.Age1,
                        Age2 = c.Age2,
                        Value1 = c.Value1,
                        Value2 = c.Value2,
                        Value3 = c.Value3,
                        LeadTime = c.LeadTime,
                        IsStopProcess = c.IsStopProcess,
                        IsUserNotification = c.IsUserNotification
                    }).ToList(),
                    ApplicationProcessDocuments = proc.ApplicationProcessDocuments
                    .Select(doc => new Domain.Entities.Mobile.ApplicationProcessDocument
                    {
                        Id = doc.Id,
                        ApplicationId = doc.ApplicationId,
                        ProcessId = doc.ProcessId,
                        BPVarietyId = doc.BPVarietyId,
                        DocumentId = doc.DocumentId,
                        DocFilePath = doc.DocFilePath,
                        DocFileData = doc.DocFileData,
                        DocFileExt = doc.DocFileExt
                    }).ToList(),
                    ApplicationProcessFees = proc.ApplicationProcessFees
                    .Select(fee => new Domain.Entities.Mobile.ApplicationProcessFee
                    {
                        FeeId = fee.FeeId,
                        ProcessId = fee.ProcessId,
                        BPVarietyId = fee.BPVarietyId,
                        ApplicationId = fee.ApplicationId,
                        FeeNameEn = fee.FeeNameEn,
                        FeeNameAr = fee.FeeNameAr,
                        FeeNameFr = fee.FeeNameFr,
                        FeeValue = fee.FeeValue,
                        FeeTax = fee.FeeTax,
                        IsPaid = fee.IsPaid
                    }).ToList(),
                    VarietyBusinessProcess = proc.VarietyBusinessProcess != null ? new Domain.Entities.Mobile.VarietyBusinessProcess
                    {
                        ProcessId = proc.VarietyBusinessProcess.ProcessId,
                        BPVarietyId = proc.VarietyBusinessProcess.BPVarietyId
                    } : null
                }).ToList()
            })
            .FirstOrDefaultAsync(ct);
    }
    /// <summary>
    /// Retrieves a specific Application with details by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public async Task<ApplicationDto?> GetByIdWithDetailAsync(string id, CancellationToken ct = default)
    {
        return await _context.Applications
            .AsNoTracking()
            .Where(a => (a.ApplicationNumber == id || a.Id == id) && a.IsDeleted != true && a.ApplicationApprovalStatusId == (int)ApplicationStatus.Pending)
            .Select(a => new ApplicationDto
            {
                Id = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                ApplicationTypeId = a.ApplicationTypeId,
                CreatedDate = a.CreatedDate,

                // Application Approval Status (EN/AR/FR)
                ApplicationApprovalStatus = a.ApplicationApprovalStatus == null ? null : new StatusDto
                {
                    Id = a.ApplicationApprovalStatus.ID,
                    DescriptionEn = a.ApplicationApprovalStatus.StatusDesc,
                    DescriptionAr = a.ApplicationApprovalStatus.StatusDescAr,
                    DescriptionFr = a.ApplicationApprovalStatus.StatusDescFr
                },

                // Application Status (EN/AR/FR)
                ApplicationStatus = a.Status == null ? null : new StatusDto
                {
                    Id = a.Status.ID,
                    DescriptionEn = a.Status.StatusDesc,
                    DescriptionAr = a.Status.StatusDescAr,
                    DescriptionFr = a.Status.StatusDescFr
                },

                ApplicationProcesses = a.ApplicationProcesses
                    .Where(p => p.IsDeleted == false)
                    .Select(proc => new ApplicationProcessDto
                    {
                        ApplicationId = proc.ApplicationId,
                        ProcessId = proc.ProcessId,
                        BPVarietyId = proc.BPVarietyId,

                        // Process & Variety multilingual via VarietyBusinessProcess
                        Process = proc.VarietyBusinessProcess != null && proc.VarietyBusinessProcess.Process != null
                            ? new ProcessNamesDto
                            {
                                NameEn = proc.VarietyBusinessProcess.Process.NameEn,
                                NameAr = proc.VarietyBusinessProcess.Process.NameAr,
                                NameFr = proc.VarietyBusinessProcess.Process.NameFr
                            }
                            : null,

                        ProcessVariety = proc.VarietyBusinessProcess != null && proc.VarietyBusinessProcess.BPVariety != null
                            ? new ProcessVarietyNamesDto
                            {
                                NameEn = proc.VarietyBusinessProcess.BPVariety.NameEn,
                                NameAr = proc.VarietyBusinessProcess.BPVariety.NameAr,
                                NameFr = proc.VarietyBusinessProcess.BPVariety.NameFr
                            }
                            : null,

                        // CheckList with EN/AR/FR (item + group)
                        CheckLists = proc.ApplicationProcessCheckLists
                            .Where(cl => cl.IsDeleted == false)
                            .Select(cl => new ApplicationProcessCheckListDto
                            {
                                ChekListId = cl.ChekListId,
                                IsDocRequired = cl.IsDocRequired,

                                CheckListEn = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null
                                    ? cl.ProcessCheckList.ChekList.DescriptionEn
                                    : null,
                                CheckListAr = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null
                                    ? cl.ProcessCheckList.ChekList.DescriptionAr
                                    : null,
                                CheckListFr = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null
                                    ? cl.ProcessCheckList.ChekList.DescriptionFr
                                    : null,

                                CheckListGroupEn = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null && cl.ProcessCheckList.ChekList.CheckListGroup != null
                                    ? cl.ProcessCheckList.ChekList.CheckListGroup.DescriptionEn
                                    : null,
                                CheckListGroupAr = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null && cl.ProcessCheckList.ChekList.CheckListGroup != null
                                    ? cl.ProcessCheckList.ChekList.CheckListGroup.DescriptionAr
                                    : null,
                                CheckListGroupFr = cl.ProcessCheckList != null && cl.ProcessCheckList.ChekList != null && cl.ProcessCheckList.ChekList.CheckListGroup != null
                                    ? cl.ProcessCheckList.ChekList.CheckListGroup.DescriptionFr
                                    : null
                            })
                            .ToList(),

                        // Documents with EN/AR/FR (document + group)
                        Documents = proc.ApplicationProcessDocuments
                            .Where(d => d.IsDeleted == false)
                            .Select(d => new ApplicationProcessDocumentDto
                            {
                                Id = d.Id,
                                DocumentId = d.DocumentId,
                                DocFilePath = d.DocFilePath,
                                DocFileExt = d.DocFileExt,

                                DocumentNameEn = d.Document != null ? d.Document.DocumentNameEn : null,
                                DocumentNameAr = d.Document != null ? d.Document.DocumentNameAr : null,
                                DocumentNameFr = d.Document != null ? d.Document.DocumentNameFr : null,

                                DocumentGroupEn = d.Document != null && d.Document.Group != null ? d.Document.Group.GroupNameEn : null,
                                DocumentGroupAr = d.Document != null && d.Document.Group != null ? d.Document.Group.GroupNameAr : null,
                                DocumentGroupFr = d.Document != null && d.Document.Group != null ? d.Document.Group.GroupNameFr : null
                            })
                            .ToList(),

                        // Fees with EN/AR/FR (from APP.ApplicationProcessFee)
                        Fees = proc.ApplicationProcessFees
                            .Select(f => new ApplicationProcessFeeDto
                            {
                                FeeId = f.FeeId,
                                FeeValue = f.FeeValue,
                                FeeTax = f.FeeTax,
                                IsPaid = f.IsPaid,

                                FeeNameEn = f.FeeNameEn,
                                FeeNameAr = f.FeeNameAr,
                                FeeNameFr = f.FeeNameFr
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(ct);
    }


    public async Task<bool> RejectApplication(string id, CancellationToken ct = default)
    {
        try
        {
            var applicationToUpdate = await _context.Applications
                .FirstOrDefaultAsync(
                    a => (a.Id == id || a.ApplicationNumber == id) && a.IsDeleted != true,
                    ct);

            applicationToUpdate.ApplicationApprovalStatusId = (int)ApplicationStatus.Rejected;
            applicationToUpdate.ApplicationApprovalStatusDate = DateTime.UtcNow;
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<bool> DocumentRequiredApplication(string id, CancellationToken ct = default)
    {
        try
        {
            var applicationToUpdate = await _context.Applications
                .FirstOrDefaultAsync(
                    a => (a.Id == id || a.ApplicationNumber == id) && a.IsDeleted != true,
                    ct);

            applicationToUpdate.ApplicationApprovalStatusId = (int)ApplicationStatus.PendingDoc;
            applicationToUpdate.ApplicationApprovalStatusDate = DateTime.UtcNow;
            //add to notification table
            await _context.SaveChangesAsync(ct);

            return true;
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public async Task<IActionResult> ApprovePendingApplication(string applicationId, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                var ex = new Exception($"ApplicationId is required");
                ex.HelpLink = "application_required";
                throw ex;
            }


            // 1: get application from mobile database
            // Use AsNoTracking() since this is only for DTO preparation, not for updates
            var application = await _context.Applications
                .AsNoTracking()
                .Include(a => a.Citizen)
                .Include(a => a.ApplicationProcesses)
                .Where(a => (a.Id == applicationId || a.ApplicationNumber == applicationId) && a.IsDeleted != true)
                .FirstOrDefaultAsync(ct);

            if (application == null)
            {
                var ex = new Exception($"Application {applicationId} not found");

                ex.HelpLink = "application_not_found";
                throw ex;
            }

            if (application.Citizen == null)
            {
                var ex = new Exception("Citizen is required.");
                ex.HelpLink = "citizen_required";
                throw ex;
            }

            var citizenLocalId = await GetCitizenLocalIdAsync(application.OwnerId, ct);
            if (citizenLocalId == null)
            {
                var ex = new Exception($"Citizen {application.OwnerId} is not linked to a local citizen or not approved yet");
                ex.HelpLink = "citizen_not_linked_or_not_approved";
                throw ex;
            }
            application.OwnerId = citizenLocalId.Value;
            // Load related process data sequentially (DbContext is not thread-safe)
            // Use AsNoTracking() to avoid tracking issues when ResetIds modifies entity keys
            var processes = await _context.ApplicationProcesses
                .AsNoTracking()
                .Where(p => p.ApplicationId == application.Id && !p.IsDeleted)
                .ToListAsync(ct);
            ResetIds(processes);

            var conditions = await _context.ApplicationProcessConditions
                .AsNoTracking()
                .Where(c => c.ApplicationId == application.Id && !c.IsDeleted)
                .ToListAsync(ct);
            ResetIds(conditions);


            var documents = await _context.ApplicationProcessDocuments
                .AsNoTracking()
                .Where(d => d.ApplicationId == application.Id && !d.IsDeleted)
                .ToListAsync(ct);
            ResetIds(documents);

            var checkLists = await _context.ApplicationProcessCheckLists
                .AsNoTracking()
                .Where(cl => cl.ApplicationId == application.Id && !cl.IsDeleted)
                .ToListAsync(ct);
            ResetIds(checkLists);

            var fees = await _context.ApplicationProcessFees
                .AsNoTracking()
                .Where(f => f.ApplicationId == application.Id && !f.IsDeleted)
                .ToListAsync(ct);
            ResetIds(fees);

            var pendingApplicationDTO = new ApprovePendingApplicationDTO
            {
                Application = application,
                ApplicationProcess = processes,
                ApplicationProcessCondition = conditions,
                ApplicationProcessDocument = documents,
                ApplicationProcessCheckList = checkLists,
                ApplicationProcessFees = fees
            };

            // Send DTO to external API
            var apiResponse = await _externalApiService.SendPendingApplicationAsync(pendingApplicationDTO, ct);

            // Update application status based on API response
            var applicationToUpdate = await _context.Applications
                .FirstOrDefaultAsync(
                    a => (a.Id == applicationId || a.ApplicationNumber == applicationId) && a.IsDeleted != true,
                    ct);

            if (applicationToUpdate != null)
            {
                if (apiResponse.StatusCode == 200)
                {
                    // API call successful - approve the application
                    applicationToUpdate.ApplicationApprovalStatusId = (int)ApplicationStatus.Approved;
                    applicationToUpdate.ApplicationApprovalStatusDate = DateTime.UtcNow;
                    await CreateReceiptAsync(applicationId, ct);
                    _logger.LogInformation(
                        "Application {ApplicationId} approved after successful external API call. Status: {Status}",
                        applicationId, apiResponse.StatusCode);
                }
                else
                {
                    // API call failed - reject the application
                    applicationToUpdate.ApplicationApprovalStatusId = (int)ApplicationStatus.Rejected;
                    applicationToUpdate.ApplicationApprovalStatusDate = DateTime.UtcNow;

                    _logger.LogWarning(
                        "Application {ApplicationId} rejected due to external API failure. Status: {Status}, Error: {Error}",
                        applicationId, apiResponse.StatusCode, apiResponse.ErrorMessage);
                }

                // No need for Update() - entity is already tracked since it was loaded from context
                await _context.SaveChangesAsync(ct);
            }
            else
            {
                _logger.LogError(
                    "Could not find application {ApplicationId} to update status after external API call",
                    applicationId);
            }

            // Return both the DTO and the API response
            return new OkObjectResult(new
            {
                ApplicationData = pendingApplicationDTO,
                ExternalApiResponse = apiResponse,
                ApplicationStatus = applicationToUpdate?.ApplicationApprovalStatusId
            });
        }
        catch (Exception ex)
        {

            throw;
        }
    }



    // Helper methods
    private async Task<int?> GetCitizenLocalIdAsync(int citizenOnlineId, CancellationToken ct)
    {
        // Look up CitizenLink to get local citizen ID
        var citizenLink = await _context.CitizenLinks
            .FirstOrDefaultAsync(cl => cl.CitizenOnlineId == citizenOnlineId, ct);

        if (citizenLink != null)
            return citizenLink.CitizenLocalId;

        // Fallback: try to find by matching identifiers (for backward compatibility)
        var onlineCitizen = await _context.Citizens
            .FirstOrDefaultAsync(c => c.Id == citizenOnlineId, ct);

        if (onlineCitizen == null) return null;

        if (onlineCitizen.IsValid == false) return null;

        // Try to find in DLS by NationalId or other identifiers
        var dlsCitizen = await _dlsdbContext.Citizens
            .FirstOrDefaultAsync(c =>
                (!string.IsNullOrEmpty(onlineCitizen.NationalId) && c.NationalId == onlineCitizen.NationalId) ||
                c.Phone == onlineCitizen.Phone, ct);

        return dlsCitizen?.Id;
    }


    public async Task<(IEnumerable<ApplicationListItemDto> items, PaginationMetaData metaData)> GetApplicationsAsync(Pagination pagination, string? keyword = null, string? status = null, string? filtration = "all", int? userId = null, int? branchId = null, CancellationToken ct = default)
    {
        // 0) Normalize inputs (avoid ToLower in SQL — SQL Server is case-insensitive by default)
        keyword = keyword?.Trim();
        status = status?.Trim();

        // 1) Build FILTER-ONLY query (no big projections)
        var filterQuery = _context.Applications
            .AsNoTracking()
            .Where(a => a.ApplicationApprovalStatusId == (int)ApplicationStatus.Pending && (a.IsDeleted == null || a.IsDeleted == false));


        // 1.c) Apply filtration based on the filtration parameter
        if (!string.IsNullOrWhiteSpace(filtration) && filtration.ToLower() != "all")
        {
            switch (filtration.ToLower())
            {
                case "user":
                    if (userId.HasValue)
                    {
                        filterQuery = filterQuery.Where(a => a.CreatedUserId == userId.Value);
                    }
                    else
                    {
                        // If no user ID provided, return empty result
                        filterQuery = filterQuery.Where(a => false);
                    }
                    break;
                case "branch": //it is perant of the user   
                    if (branchId.HasValue)
                    {
                        filterQuery = filterQuery.Where(a => a.BranchId == branchId.Value);
                    }
                    else
                    {
                        // If no branch ID provided, return empty result
                        filterQuery = filterQuery.Where(a => false);
                    }
                    break;
                default:
                    // Invalid filtration parameter, return empty result
                    filterQuery = filterQuery.Where(a => false);
                    break;
            }
        }

        // 1.a) Keyword filter: AND each token against any of the name/phone fields
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            var tokens = keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in tokens)
            {
                var token = t; // capture for EF
                filterQuery = filterQuery.Where(a =>
                       (a.ApplicationNumber != null && a.ApplicationNumber.Contains(token))
                    || (a.OwnerFullName != null && a.OwnerFullName.Contains(token))
                    || (a.Citizen.FirstName != null && a.Citizen.FirstName.Contains(token))
                    || (a.Citizen.LastName != null && a.Citizen.LastName.Contains(token))
                    || (a.Citizen.FathersName != null && a.Citizen.FathersName.Contains(token))
                    || (a.Citizen.FirstNameSecondLang != null && a.Citizen.FirstNameSecondLang.Contains(token))
                    || (a.Citizen.LastNameSecondLang != null && a.Citizen.LastNameSecondLang.Contains(token))
                    || (a.Citizen.FathersNameSecondLang != null && a.Citizen.FathersNameSecondLang.Contains(token))
                    || (a.Citizen.Phone != null && a.Citizen.Phone.Contains(token))
                );
            }
        }

        // 1.b) Status filter: resolve ID once, then filter by the id
        if (!string.IsNullOrWhiteSpace(status))
        {
            var statusId = await _context.Statuses
                .AsNoTracking()
                .Where(s => s.StatusDesc == status
                         || s.StatusDescFr == status
                         || s.StatusDescAr == status)
                .Select(s => s.ID)
                .FirstOrDefaultAsync(ct);

            if (statusId != 0)
                filterQuery = filterQuery.Where(a => a.ApplicationApprovalStatusId == statusId);
            else
                filterQuery = filterQuery.Where(a => false); // nothing matches this status
        }

        // 2) Count on the skinny filter
        var cacheKey = $"apps:count:kw={keyword ?? ""}:st={status ?? ""}";
        var totalCount = await _cache.GetOrCreateAsync(cacheKey, e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(5);
            return filterQuery.CountAsync(ct);
        });

        // 3) Page to a small shape (IDs + basic columns used in the grid)
        var pageSlice = await filterQuery
            .OrderByDescending(a => a.CreatedDate)
            .Select(a => new
            {
                a.Id,
                a.ApplicationNumber,
                a.OwnerFullName,
                a.OwnerId,
                a.CreatedDate,
                a.ModifiedDate,
                a.ApplicationApprovalStatusId
            })
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(ct);

        if (pageSlice.Count == 0)
            return (Enumerable.Empty<ApplicationListItemDto>(),
                    PageList<ApplicationListItemDto>.ToPageList(new List<ApplicationListItemDto>(), 0, pagination.PageNumber, pagination.PageSize).MetaData);

        var pageIds = pageSlice.Select(x => x.Id).ToList();

        // 4) Project the heavy details ONLY for the page (single SQL; no Includes)
        var detailed = await _context.Applications
            .AsNoTracking()
            .Where(a => pageIds.Contains(a.Id))
            .Select(a => new
            {
                a.Id,
                a.ApplicationNumber,
                a.OwnerFullName,
                a.OwnerId,
                a.CreatedDate,
                a.ModifiedDate,
                a.ApplicationApprovalStatusId,

                // Multilingual approval status via navigation
                ApprovalStatusEn = a.ApplicationApprovalStatus != null ? a.ApplicationApprovalStatus.StatusDesc : null,
                ApprovalStatusFr = a.ApplicationApprovalStatus != null ? a.ApplicationApprovalStatus.StatusDescFr : null,
                ApprovalStatusAr = a.ApplicationApprovalStatus != null ? a.ApplicationApprovalStatus.StatusDescAr : null,

                // Processes + fees (filtered)
                Processes = a.ApplicationProcesses
                    .Where(ap => ap.IsDeleted == false)
                    .Select(ap => new
                    {
                        ap.ProcessId,
                        ap.BPVarietyId,
                        ProcessNameEn = ap.VarietyBusinessProcess.Process.NameEn,
                        ProcessNameFr = ap.VarietyBusinessProcess.Process.NameFr,
                        ProcessNameAr = ap.VarietyBusinessProcess.Process.NameAr,
                        VarietyNameEn = ap.VarietyBusinessProcess.BPVariety.NameEn,
                        VarietyNameFr = ap.VarietyBusinessProcess.BPVariety.NameFr,
                        VarietyNameAr = ap.VarietyBusinessProcess.BPVariety.NameAr,
                        FeesPaidFlags = ap.ApplicationProcessFees
                            .Where(f => f.IsDeleted == false)
                            .Select(f => f.IsPaid)
                    }),
                ReceiptNumber = a.Receipts
                    .Where(r => r.IsDeleted == false)
                    .Select(r => new
                    {
                        r.ReceiptNumber
                    }).FirstOrDefault(),


            })
            .ToListAsync(ct);

        // 5) Join back to the page order & shape DTOs (in-memory only for the page)
        var detailedById = detailed.ToDictionary(x => x.Id);

        var resultItems = pageSlice.Select(a =>
        {
            var d = detailedById[a.Id];

            // Distinct sets for labels
            var pIds = d.Processes.Select(p => p.ProcessId).Distinct();
            var vIds = d.Processes.Select(p => p.BPVarietyId).Distinct();
            var procEn = string.Join(", ", d.Processes.Select(p => p.ProcessNameEn).Where(x => x != null).Distinct());
            var varEn = string.Join(", ", d.Processes.Select(p => p.VarietyNameEn).Where(x => x != null).Distinct());
            var procFr = string.Join(", ", d.Processes.Select(p => p.ProcessNameFr).Where(x => x != null).Distinct());
            var varFr = string.Join(", ", d.Processes.Select(p => p.VarietyNameFr).Where(x => x != null).Distinct());
            var procAr = string.Join(", ", d.Processes.Select(p => p.ProcessNameAr).Where(x => x != null).Distinct());
            var varAr = string.Join(", ", d.Processes.Select(p => p.VarietyNameAr).Where(x => x != null).Distinct());

            // Fee state in any process for this application
            var allFees = d.Processes.SelectMany(p => p.FeesPaidFlags).ToList();
            var hasPaid = allFees.Any(flag => flag == true);
            var hasUnpaid = allFees.Any(flag => flag == false);
            var hasNoPayment = allFees.Any(flag => flag == null);

            // Fill DrivingLicenseDTO for this applicant
            var dl = _context.DrivingLicenses
                .Where(dl => dl.IsDeleted == false && dl.CitizenId == a.OwnerId)
                .OrderByDescending(dl => dl.IssuanceDate) // get the most recent
                .FirstOrDefault();
            var drivingLicenseDto = new DrivingLicenseDTO();
            if (dl != null)
            {
                drivingLicenseDto.DrivingLicenseNumber = dl.IsInternational == false ? dl.DrivingLicenseNumber : null;
                drivingLicenseDto.DrivingLicenseInternationalNumber = dl.IsInternational == true ? dl.DrivingLicenseNumber : null;
                drivingLicenseDto.IssuanceDate = dl.IssuanceDate;
                drivingLicenseDto.ExpiryDate = dl.ExpiryDate;
                drivingLicenseDto.DrivingLicenseStatusId = dl.DrivingLicenseStatusId;
            }

            return new ApplicationListItemDto
            {
                ApplicationId = a.Id,
                ApplicationNumber = a.ApplicationNumber,
                ReceiptNumber = d.ReceiptNumber != null ? d.ReceiptNumber.ReceiptNumber : null,
                Applicant = a.OwnerFullName,
                ApplicantId = a.OwnerId,
                SubmissionDate = a.CreatedDate,
                LastUpdateDate = a.ModifiedDate ?? a.CreatedDate,
                ProcessId = string.Join(", ", pIds),
                BPVarietyId = string.Join(", ", vIds),
                ProcessTypeEn = procEn,
                ServiceTypeEn = varEn,
                ProcessTypeFr = procFr,
                ServiceTypeFr = varFr,
                ProcessTypeAr = procAr,
                ServiceTypeAr = varAr,
                ApprovalStatusId = a.ApplicationApprovalStatusId,
                ApprovalStatusEn = d.ApprovalStatusEn ?? "Unknown",
                ApprovalStatusFr = d.ApprovalStatusFr ?? "Unknown",
                ApprovalStatusAr = d.ApprovalStatusAr ?? "غير معروف",
                DrivingLicense = drivingLicenseDto,
                FeesEn = hasPaid ? "Paid" : (hasUnpaid ? "Pending Payment" : "No Payment Available"),
                FeesFr = hasPaid ? "Payé" : (hasUnpaid ? "Paiement en attente" : "Aucun paiement disponible"),
                FeesAr = hasPaid ? "تم الدفع" : (hasUnpaid ? "الدفع المعلق" : "لا يوجد دفع متاح"),
            };
        })
        .OrderByDescending(i => i.SubmissionDate) // keep same sort
        .ToList();

        var metaData = PageList<ApplicationListItemDto>
            .ToPageList(resultItems, totalCount, pagination.PageNumber, pagination.PageSize)
            .MetaData;

        return (resultItems, metaData);
    }

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

    private async Task CreateReceiptAsync(string applicationId, CancellationToken ct)
    {
        var request = await _context.Applications
            .FirstOrDefaultAsync(x => x.Id == applicationId, ct);
        if (request == null)
        {
            _logger.LogWarning("Unable to create receipt. Application {ApplicationId} not found.", applicationId);
            return;
        }

        var applicationProcessFee = await _context.ApplicationProcessFees
            .Where(x => x.ApplicationId == applicationId)
            .ToListAsync(ct);

        var receiptWithDetailRequest = new ReceiptWithDetailRequest();
        var receipt = new Receipt();
        var receiptDetail = new List<ReceiptDetail>();
        receipt.ApplicationId = request.Id;
        receipt.ReceiptStatusId = 3;
        receipt.StructureId = request.BranchId;
        receipt.IsPaid = false;
        receipt.IsPosted = false;
        receipt.CitizenFullName = request.OwnerFullName;
        receipt.CreatedUserId = 0;
        receipt.IsDeleted = false;


        foreach (var appProcFee in applicationProcessFee)
        {
            var receiptDetails = new ReceiptDetail();
            receiptDetails.ItemId = appProcFee.FeeId;
            receiptDetails.ItemDescriptionAR = appProcFee.FeeNameAr;
            receiptDetails.ItemDescriptionEN = appProcFee.FeeNameEn;
            receiptDetails.ItemCode = "0";
            receiptDetails.ItemTypeId = appProcFee.FeeTypeId;
            receiptDetails.ItemCategoryId = appProcFee.FeeCategoryId;
            receiptDetails.Amount = (float)appProcFee.FeeValue;
            receiptDetails.ProcessId = appProcFee.ProcessId;
            receiptDetails.BPVarietyId = appProcFee.BPVarietyId;
            receiptDetails.CreatedUserId = 0;
            receiptDetails.IsDeleted = false;
            receiptDetail.Add(receiptDetails);
        }
        receiptWithDetailRequest.Receipt = receipt;
        receiptWithDetailRequest.ReceiptDetails = receiptDetail;

        await _receiptService.CreateWithDeatailsAsync(receiptWithDetailRequest);
    }

}

