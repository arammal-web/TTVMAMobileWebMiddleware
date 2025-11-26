using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Application.Validators;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Enums;
using TTVMAMobileWebMiddleware.Domain.Views;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Shared.Domain.Views;
using System;
using System.Data;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TTVMAMobileWebMiddleware.Application.Validators

{
    public class DLValidationEngineService : IDLValidationEngineService
    {
        private readonly MOBDbContext _context;
        private readonly IAppSettingService _appSettingService;

        public DLValidationEngineService(MOBDbContext context, IAppSettingService appSettingService)
        {
            _context = context;
            _appSettingService = appSettingService;
        }

        /// <summary>
        /// Validates a business process
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="categoryIds"></param>
        /// <param name="applicantId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<ProcessProfile> BusinessProcessValidationAsync(
        int processId, List<int> categoryIds, int applicantId, int ApplicationApprovalStatus = 0, CancellationToken ct = default)
        {
            var profile = new ProcessProfile
            {
                ProcessId = (int)processId,
                CategoryIds = categoryIds?.ToArray() ?? Array.Empty<int>(),
                ApplicantId = applicantId,
                ApplicantAge = await GetApplicantAgeAsync(applicantId, ct) ?? 0
            };

            // Pull rule groups (include -1 => applies to all)
            var ruleGroups = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ProcessId == processId
                         && (categoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1)
                         && (c.IsDeleted == false || c.IsDeleted == null))
                .Select(c => c.ConditionGroupId)
                .Distinct()
                .ToListAsync(ct);

            if (ruleGroups == null || ruleGroups.Count == 0)
            {
                var ex = new Exception("Business Process has no rules implemented. Please contact the software administrator.");
                ex.HelpLink = "business_process_has_no_rules_implemented";
                throw ex;
            }
            // 12) International Driving License Renewal Validity
            await FillInternationalDLRenewalValidityAsync(profile, ct);

            if (ApplicationApprovalStatus != (int)ApplicationStatus.Pending)
            {

                // 1) MinAge
                await FillMinAgeAsync(profile, ct);

                // 2) RenewalValidity
                await FillRenewalValidityAsync(profile, ct);

                // 3) BiometricIsRequired
                await FillBiometricAsync(profile, applicantId, ct);

                // 4) ExamIsRequired
                await FillExamRequiredAsync(profile, ct);

                // 4.1) UrgentExamIsRequired
                await FillUrgentExamRequiredAsync(profile, ct);

                // 5) NationalityRestriction
                await FillNationalityAsync(profile, ct);

                // 6) RequiredToBeExpired
                await FillExpiredRequiredAsync(profile, ct);

                // 7) Category Prerequisites
                await FillCategoryPrerequisitesAsync(profile, ct);

                // 8) Waiting Periods
                await FillWaitingPeriodsAsync(profile, ct);

                // 9) RequiredToBeSuspended
                await FillSuspendedRequiredAsync(profile, ct);

                // 10) IsAdministrative
                await FillAdministrativeAsync(profile, ct);

                // 11) Initial Status
                await FillInitialStatusAsync(profile, ct);
                // 13) DL required + snapshot (NEW)

                await FillDLIsRequiredAsync(profile, ct);
                // 13) DL required + snapshot (NEW)
                await FillDLIsRequiredAsync(profile, ct);

                // 14) DL Operation required 
                await CheckOperationRequired(profile, ct);
            }

            return profile;
        }

        //Helpers


        private async Task FillMinAgeAsync(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.MinAge /*1*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .OrderByDescending(c => c.Age1)
                .FirstOrDefaultAsync(ct);

            if (rule != null)
            {

                if (p.ApplicantId == -100)
                    return;//skip

                p.MinAge.RequiredAge = rule.Age1;
                p.MinAge.IsStopper = rule.IsStopProcess;
                p.MinAge.Passed = p.ApplicantAge >= (rule.Age1 ?? 0);

                if (!p.MinAge.Passed)
                {
                    var msg = $"Minimum age {rule.Age1} is required for the selected category(ies).";
                    if (p.MinAge.IsStopper) p.Errors.Add(new KeyValuePair<string, string>("Minimum_age", msg));
                }
            }
        }
        private async Task FillRenewalValidityAsync(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.RenewalValidity /*2*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .OrderBy(c => c.Value1)
                .FirstOrDefaultAsync(ct);

            if (rule == null) return;

            p.Renewal.Age1 = rule.Age1;
            p.Renewal.Age2 = rule.Age2;
            p.Renewal.Value1 = rule.Value1;
            p.Renewal.Value2 = rule.Value2;
            p.Renewal.Value3 = rule.Value3;

            int? validity;
            var age = p.ApplicantAge;
            if (age < rule.Age1)
            {
                validity = age < (rule.Age1 - rule.Value1) ? rule.Value1
                          : Math.Max((int)(rule.Age1 - age), (int)rule.Value2);
            }
            else if (age < rule.Age2)
            {
                validity = age < (rule.Age2 - rule.Value2) ? rule.Value2
                          : Math.Max((int)(rule.Age2 - age), (int)rule.Value3);
            }
            else
            {
                validity = rule.Value3;
            }

            p.Renewal.ValidityYears = validity;
        }

        private async Task FillBiometricAsync(ProcessProfile p, int applicantId, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.BiometricIsRequired /*3*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .OrderByDescending(c => c.Age2)
                .FirstOrDefaultAsync(ct);

            if (rule == null) return;

            int recordsCount = await ExecuteSqlAsync(applicantId);
            if (recordsCount == 0)
            {
                p.Biometric.RequiredEnrollment = true;
                p.Warnings.Add("Enrollment is required for this process.");
                //p.Errors.Add(new KeyValuePair<string, string>("enrollment_is_required",
                //    "Enrollment is required for this process."));
            }
            //get from the table , by citizenid if data count >0 no enrolment required 


            p.Biometric.AgeExemptMax = rule.Age2;
            p.Biometric.Required = !(rule.Age2.HasValue && p.ApplicantAge >= rule.Age2.Value);

            if (p.Biometric.Required && (rule.IsStopProcess))
                p.Warnings.Add("Biometric enrollment is required for this process.");
            //p.Errors.Add(new KeyValuePair<string, string>("biometric_enrollment_is_required", 
            //    "Biometric enrollment is required for this process."));
        }
        private async Task FillExamRequiredAsync(ProcessProfile p, CancellationToken ct)
        {
            var exists = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.ExamIsRequired /* 4 */&& c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .AnyAsync(ct);

            p.ExamRequired = exists;
        }
        private async Task FillUrgentExamRequiredAsync(ProcessProfile p, CancellationToken ct)
        {
            var exists = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.UrgentExamIsRequired /*13*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .AnyAsync(ct);

            p.UrgentExamRequired = exists;
        }
        private async Task FillNationalityAsync(ProcessProfile p, CancellationToken ct)
        {
            var rules = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.NationalityRestriction/* 5*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .ToListAsync(ct);

            if (rules.Count == 0) { p.Nationality.Passed = true; return; }

            // Aggregate allowed IDs from Value1/Value2/Value3 (non-null)
            foreach (var r in rules)
            {
                if (r.Value1.HasValue) p.Nationality.AllowedNationalityIds.Add(r.Value1.Value);
                if (r.Value2.HasValue) p.Nationality.AllowedNationalityIds.Add(r.Value2.Value);
                if (r.Value3.HasValue) p.Nationality.AllowedNationalityIds.Add(r.Value3.Value);
            }
            var distinctIds = p.Nationality.AllowedNationalityIds.Distinct().ToList();
            p.Nationality.AllowedNationalityIds.Clear();
            p.Nationality.AllowedNationalityIds.AddRange(distinctIds);

            if (p.ApplicantId == -100)
                return;//skip

            var citizenNat = await _context.Citizens
                .AsNoTracking()
                .Where(c => c.Id == p.ApplicantId)
                .Select(c => c.NationalityId)
                .FirstOrDefaultAsync(ct);


            if (citizenNat == null)
            {
                p.Errors.Add(new KeyValuePair<string, string>("citizen_nationality_not_found",
                    "Citizen nationality not found."));
                return;
            }

            p.Nationality.Passed = p.Nationality.AllowedNationalityIds.Count == 0
                                   || p.Nationality.AllowedNationalityIds.Contains(citizenNat.Value);

            if (!p.Nationality.Passed)
                p.Errors.Add(new KeyValuePair<string, string>("applicant_nationality_is_not_allowed",
                    "Applicant nationality is not allowed for this process."));
        }
        private async Task FillExpiredRequiredAsync(ProcessProfile p, CancellationToken ct)
        {
            bool inGrace;
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.RequiredToBeExpired /* 6*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .FirstOrDefaultAsync(ct);

            p.RequireExpiredLicense = rule != null;

            if (p.ApplicantId == -100)
                return;

            // Get grace period from app settings
            var gracePeriodDays = await _appSettingService.GetValueAsync<int?>("RenewGracePeriod_IndDyas", ct) ?? 30;

            var dl = await _context.DrivingLicenses
                .AsNoTracking()
                .Where(dl => (dl.CitizenId == p.ApplicantId)
                && dl.IsInternational == false
                && (dl.IsDeleted == false || dl.IsDeleted == null)
                && (dl.Islocked == false || dl.Islocked == null))
                .OrderByDescending(dl => dl.ExpiryDate)
                .Select(dl => dl.ExpiryDate)
                .FirstOrDefaultAsync(ct);

            if (dl != default)
            {
                p.IsLicenseExpired = dl < DateTime.UtcNow;

                var lastRequestDate = await _context.DrivingTestRequests
                    .AsNoTracking()
                    .Where(dtr => dtr.CitizenId == p.ApplicantId
                               && (dtr.IsDeleted == false || dtr.IsDeleted == null))
                    .OrderByDescending(dtr => dtr.CreatedDate)
                    .Select(dtr => dtr.CreatedDate)
                    .FirstOrDefaultAsync(ct);

                if (lastRequestDate.HasValue)
                {
                    var daysSinceLastRequest = (DateTime.UtcNow - lastRequestDate.Value).Days;
                    if (daysSinceLastRequest < gracePeriodDays)
                    {
                        p.Errors.Add(new KeyValuePair<string, string>("grace_period_days", $"You must wait {gracePeriodDays - daysSinceLastRequest} more days before submitting a new driving license request. Grace period: {gracePeriodDays} days."));

                    }
                }

                if (p.RequireExpiredLicense && p.IsLicenseExpired == false)
                    p.Errors.Add(new KeyValuePair<string, string>("expired_driving_license", "Driving License must be expired for this action."));
            }
            else
            {
                if (p.RequireExpiredLicense) p.Errors.Add(new KeyValuePair<string, string>("driving_license_not_found", "Driving License record not found."));
            }

            if (!p.RequireExpiredLicense && p.IsLicenseExpired == true)
                p.Errors.Add(new KeyValuePair<string, string>("driving_license_not_expired", "Driving License must not be expired for this action."));

            // to be updated later for international licenses where we need to
            // add a new business process "Renew International Driving License"

        }
        private async Task FillCategoryPrerequisitesAsync(ProcessProfile p, CancellationToken ct)
        {
            // For each selected category, check if a prerequisite exists and is present in the selection
            foreach (var catId in p.CategoryIds)
            {
                var pre = await _context.PrerequisiteProcessVarieties
                    .AsNoTracking()
                    .Where(x => x.BPVarietyId == catId && (x.IsDeleted == null || x.IsDeleted == false))
                    .Select(x => x.BPVarietyPrerequisiteId)
                    .FirstOrDefaultAsync(ct);

                if (pre > 0 && !p.CategoryIds.Contains(pre))
                {
                    var preName = await _context.ProcessVarieties
                        .AsNoTracking()
                        .Where(v => v.Id == pre)
                        .Select(v => new ProcessVarietyNamesDto { NameEn = v.NameEn, NameAr = v.NameAr, NameFr = v.NameFr })
                        .FirstOrDefaultAsync(ct);

                    p.MissingPrerequisites.Add(new ProcessProfile.MissingPrerequisite
                    {
                        CategoryId = catId,
                        RequiredCategoryId = pre,
                        RequiredCategoryName = preName
                    });

                    p.Errors.Add(new KeyValuePair<string, string>("missing_prerequisite", $"Missing prerequisite category ({pre}) for selected category ({catId})."));
                }
            }
        }
        private async Task FillWaitingPeriodsAsync(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.WaitingPeriods /*8*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .FirstOrDefaultAsync(ct);

            if (rule == null) { p.Waiting.Passed = true; return; }

            p.Waiting.TheoryWeeks = rule.Value1;
            p.Waiting.PracticalMonths = rule.Value2;

            if (p.ApplicantId == -100)
                return;//skip

            // last failed theory
            var lastTheo = await _context.DrivingExams
                .AsNoTracking()
                .Where(e => (e.CitizenId == p.ApplicantId)
                         && e.ExamStatusId == (int)DrivingExamStatus.TheoreticalExamFailed
                         && (e.IsCanceled == null || e.IsCanceled == false)
                         && (e.IsCompleted ?? false) == true
                         && (e.IsPassed == false))
                .OrderByDescending(e => e.ExamDateTime)
                .Select(e => (DateTime?)e.ExamDateTime)
                .FirstOrDefaultAsync(ct);

            if (lastTheo.HasValue && rule.Value1.HasValue)
            {
                var allow = lastTheo.Value.AddDays(rule.Value1.Value * 7);
                p.Waiting.NextTheoryAllowedOn = allow;
                if (DateTime.UtcNow < allow)
                {
                    p.Waiting.Passed = false;
                    p.Errors.Add(new KeyValuePair<string, string>("theory_retake_allowed", $"Theory retake allowed on {allow:yyyy-MM-dd}."));
                }
            }

            // last failed practical
            var lastPrac = await _context.DrivingExams
                .AsNoTracking()
                .Where(e => e.CitizenId == p.ApplicantId
                         && e.ExamStatusId == (int)DrivingExamStatus.PracticalExamFailed
                         && (e.IsCanceled == null || e.IsCanceled == false)
                         && (e.IsCompleted ?? false) == true
                         && (e.IsPassed == false))
                .OrderByDescending(e => e.ExamDateTime)
                .Select(e => (DateTime?)e.ExamDateTime)
                .FirstOrDefaultAsync(ct);

            if (lastPrac.HasValue && rule.Value2.HasValue)
            {
                var allow = lastPrac.Value.AddMonths(rule.Value2.Value);
                p.Waiting.NextPracticalAllowedOn = allow;
                if (DateTime.UtcNow < allow)
                {
                    p.Waiting.Passed = false;
                    p.Errors.Add(new KeyValuePair<string, string>("practical_retake_allowed", $"Practical retake allowed on {allow:yyyy-MM-dd}."));
                }
            }
        }
        private async Task FillSuspendedRequiredAsync(ProcessProfile p, CancellationToken ct)
        {
            var hasRule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.RequiredToBeSuspended /*9*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .AnyAsync(ct);

            p.RequireSuspendedLicense = hasRule;

            if (p.ApplicantId == -100)
                return;//skip

            var dl = await _context.DrivingLicenses
                .AsNoTracking()
                .Where(dl => dl.CitizenId == p.ApplicantId &&
                dl.IsInternational == false
                && (dl.IsDeleted == false || dl.IsDeleted == null)
                && (dl.Islocked == false || dl.Islocked == null))
                .Select(dl => new { dl.DrivingLicenseStatusId, dl.IsBlocked, dl.Islocked })
                .FirstOrDefaultAsync(ct);

            if (dl != null)
            {
                p.IsLicenseValid =
                    (dl.DrivingLicenseStatusId == (int)LicenseStatus.Active ||
                     dl.DrivingLicenseStatusId == (int)LicenseStatus.Pending)
                    && (dl.IsBlocked == false || dl.IsBlocked == null);

                if (p.RequireSuspendedLicense && p.IsLicenseValid == true)
                    p.Errors.Add(new KeyValuePair<string, string>("suspended_required", "This action requires the license to be suspended/invalid."));

                if (!p.RequireSuspendedLicense &&
                    dl.DrivingLicenseStatusId == (int)LicenseStatus.Suspended)
                    p.Errors.Add(new KeyValuePair<string, string>("suspended", "Driving License is Suspended."));

                if (!p.RequireSuspendedLicense &&
                   dl.IsBlocked == true)
                    p.Errors.Add(new KeyValuePair<string, string>("blocked", "Driving License is Blocked."));
            }
            else
            {
                if (p.RequireSuspendedLicense) p.Errors.Add(new KeyValuePair<string, string>("driving_license_not_found", "Driving License record not found."));
            }



            // to be updated later for international licenses where we need to
            // add a new business process "Suspend International Driving License"

        }
        private async Task FillAdministrativeAsync(ProcessProfile p, CancellationToken ct)
        {
            var hasAdmin = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.IsAdministrative /*10*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .AnyAsync(ct);

            p.IsAdministrative = hasAdmin;
        }
        private async Task FillInitialStatusAsync(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.InitialStatus/* 11 */&& c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .FirstOrDefaultAsync(ct);

            if (rule == null) return;

            // Value1 = initial app approval status, Value2 = initial app status, Value3 = initial DL status
            p.InitialStatus.ApplicationApprovalStatusId = rule.Value1;
            p.InitialStatus.ApplicationStatusId = rule.Value2;
            p.InitialStatus.DrivingLicenseStatusId = rule.Value3;
        }
        private async Task FillInternationalDLRenewalValidityAsync(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.InternationalDLRenewalValidity /*12*/ && c.ProcessId == p.ProcessId
                         && (p.CategoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .OrderBy(c => c.Value1)
                .FirstOrDefaultAsync(ct);

            if (rule == null) return;

            p.International.Age1 = rule.Age1;
            p.International.Age2 = rule.Age2;
            p.International.Value1 = rule.Value1;
            p.International.Value2 = rule.Value2;
            p.International.Value3 = rule.Value3;

            int? validity;
            var age = p.ApplicantAge;
            if (age < rule.Age1)
            {
                validity = age < (rule.Age1 - rule.Value1) ? rule.Value1
                          : Math.Max((int)(rule.Age1 - age), (int)rule.Value2);
            }
            else if (age < rule.Age2)
            {
                validity = age < (rule.Age2 - rule.Value2) ? rule.Value2
                          : Math.Max((int)(rule.Age2 - age), (int)rule.Value3);
            }
            else
            {
                validity = rule.Value3;
            }

            p.International.ValidityYears = validity;
            p.International.IsApplicable = true;
        }

        private async Task FillDLIsRequiredAsync(ProcessProfile profile, CancellationToken ct)
        {
            // Is the process defined to require an existing DL?
            var isRequired = await _context.Processes
                .AsNoTracking()
                .Where(p => p.Id == profile.ProcessId)
                .Select(p => p.IsDLRequired)
                .FirstOrDefaultAsync(ct);

            profile.DLRequired = isRequired;


            if (profile.ApplicantId == -100)
                return;//skip

            // Try to load that DL for the citizen
            var dl = await _context.DrivingLicenses
                .AsNoTracking()
                .Where(dl => dl.CitizenId == profile.ApplicantId
                          && dl.IsInternational == false
                          && (dl.IsDeleted == null || dl.IsDeleted == false)
                          && (dl.Islocked == null || dl.Islocked == false))
                .Select(dl => new
                {
                    dl.Id,
                    dl.DrivingLicenseNumber,
                    dl.IsInternational,
                    dl.DrivingLicenseStatusId,
                    StatusEn = dl.DrivingLicenseStatus != null ? dl.DrivingLicenseStatus.StatusDesc : null,
                    StatusAr = dl.DrivingLicenseStatus != null ? dl.DrivingLicenseStatus.StatusDescAr : null,
                    StatusFr = dl.DrivingLicenseStatus != null ? dl.DrivingLicenseStatus.StatusDescFr : null,
                    dl.DrivingLicenseStatusDate,
                    dl.IsBlocked,
                    dl.Islocked,
                    dl.NumberOfPoints,
                    LastIssuingDate = dl.IssuanceDate,
                    ExpiryDate = dl.ExpiryDate
                })
                .FirstOrDefaultAsync(ct);

            // If required and missing => block
            if (profile.DLRequired == true && dl == null)
            {
                profile.Errors.Add(new KeyValuePair<string, string>("drivingLicense_must_exist", "Driving license must exist before creating this application."));
                return;
            }

            // If present, hydrate profile snapshot
            if (dl != null)
            {
                profile.DrivingLicense = new ProcessProfile.DrivingLicenseProfile
                {
                    Id = dl.Id,
                    DrivingLicenseNumber = dl.DrivingLicenseNumber,
                    IsInternational = dl.IsInternational == true,
                    StatusId = (int)dl.DrivingLicenseStatusId,
                    StatusEn = dl.StatusEn,
                    StatusAr = dl.StatusAr,
                    StatusFr = dl.StatusFr,
                    StatusDate = dl.DrivingLicenseStatusDate,
                    IsBlocked = dl.IsBlocked,
                    NumberOfPoints = dl.NumberOfPoints,
                    LastIssuingDate = dl.LastIssuingDate,
                    ExpiryDate = dl.ExpiryDate
                };
                // If not required and dl exists => block
                if (profile.DLRequired == false)
                {
                    profile.Errors.Add(new KeyValuePair<string, string>("drivingLicense_must_not_exist",
                        "Driving license must not exist before creating this application."));
                    return;
                }
            }
        }


        // Action Menu generation


        /// <summary>
        /// Returns the action menu for the application
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<ActionMenu> GetApplicationActionMenuAsync(string applicationId, CancellationToken ct = default)
        {
            // Pull just what we need in a single trip
            var app = await _context.Applications
                .AsNoTracking()
                .Where(a => a.Id == applicationId && a.IsDeleted == false)
                .Select(a => new
                {
                    a.Id,
                    a.OwnerId,
                    AAS = a.ApplicationApprovalStatusId,
                    Processes = a.ApplicationProcesses
                        .Where(ap => !ap.IsDeleted)
                        .OrderByDescending(ap => ap.CreatedDate)
                        .Select(ap => new { ap.ProcessId, ap.BPVarietyId })
                        .Distinct()
                })
                .SingleOrDefaultAsync(ct);

            if (app == null)
            {
                var ex = new Exception("Application not found");
                ex.HelpLink = "application_not_found";
                throw ex;
            }


            var isApproved = app.AAS == (int)ApplicationStatus.Committed || app.AAS == (int)ApplicationStatus.Completed;
            var isRejected = app.AAS == (int)ApplicationStatus.Rejected;
            var isCancelled = app.AAS == (int)ApplicationStatus.Cancelled;

            // Consider the app "paid" if any receipt for this application is paid
            var isPaid = await _context.Receipts
                .AsNoTracking()
                .AnyAsync(r => r.ApplicationId == applicationId && r.IsPaid, ct);

            // Helper checks across *any* process/variety linked to the application.
            // If any process pair says "required/pass", we enable the related action.
            bool biometricPass = false;

            var applicantAge = await GetApplicantAgeAsync(app.OwnerId, ct) ?? 0;

            foreach (var pv in app.Processes)
            {
                // You already have these helpers in this service
                var bio = await CheckBiometricRequired(pv.ProcessId, new List<int> { pv.BPVarietyId }, applicantAge, ct); // pass real age if you store it
                biometricPass |= bio;

                if (biometricPass) break;
            }

            // Driving test is per-applicant (OwnerId maps to citizen here)
            var drivingTestPassed = await CheckDrivingLicenseTestAsync(app.Id, ct);

            // Build action menu
            var actions = new ActionMenu
            {
                SetApproved = !isApproved && !isPaid,
                SetRejected = !isPaid,
                SetCancelled = !isPaid,
                SetOnHold = !isRejected && !isCancelled,
                PrintReceipt = isApproved && isPaid,

                CreateRequestForEnrollment = biometricPass && isApproved && isPaid,
                CreateRequestForExam = false,
                CreateRequestForProduction = isApproved && isPaid && drivingTestPassed
            };

            return actions;
        }

        // ------- Helpers -------
        // Helpers region inside DLValidationEngineService

        private async Task<bool> CheckBiometricRequired(int processId, List<int> categoryIds, int age, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.BiometricIsRequired /*3*/ && c.ProcessId == processId
                         && (categoryIds.Contains(c.BPVarietyId) || c.BPVarietyId == -1))
                .OrderByDescending(c => c.Age2)
                .FirstOrDefaultAsync(ct);



            if (rule == null) return false;

            var ageExemptMax = rule.Age2;
            var required = !(rule.Age2.HasValue && age >= rule.Age2.Value);
            var isValid = !required || (required && (rule.IsStopProcess) == false);
            return isValid;
        }

        private async Task CheckOperationRequired(ProcessProfile p, CancellationToken ct)
        {
            var rule = await _context.Conditions
                .AsNoTracking()
                .Where(c => c.ConditionGroupId == (int)ConditionGroup.OperationRequestIsRequired /*14*/ && c.ProcessId == p.ProcessId)
                .FirstOrDefaultAsync(ct);
            p.IsOperationRequired = rule != null;

        }
        private async Task<int?> GetApplicantAgeAsync(int applicantId, CancellationToken ct)
        {
            // Get the citizen's DOB and compute age
            var dob = await _context.Citizens
                .Where(c => c.Id == applicantId)
                .Select(c => (DateTime?)c.DateOfBirth)
                .FirstOrDefaultAsync(ct);

            if (!dob.HasValue) return null;

            return CalculateAge(dob.Value, DateTime.UtcNow);
        }

        // pure function for age math
        private static int CalculateAge(DateTime dateOfBirth, DateTime nowUtc)
        {
            var age = nowUtc.Year - dateOfBirth.Year;
            if (nowUtc < dateOfBirth.AddYears(age)) age--;
            return age;
        }

        private async Task<bool> CheckDrivingLicenseTestAsync(string applicationId, CancellationToken ct)
        {
            var passed = await (
                from a in _context.Applications
                join r in _context.DrivingTestRequests on a.Id equals r.ApplicationId
                join e in _context.DrivingExams on r.Id equals e.DrivingTestRequestId
                where a.Id == applicationId
                      && (e.IsCanceled == null || e.IsCanceled == false)
                      && (e.IsCompleted ?? false) == true
                      && (e.IsPassed == true)
                select e
            ).AnyAsync(ct);

            return passed;
        }


        private async Task<int> ExecuteSqlAsync(int CitizenId)
        {
            int recordsCount;
            using (var conn = new SqlConnection(_context.Database.GetConnectionString()))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("GetCitizenEnrollments", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CitizenId", CitizenId);
                    var result = await cmd.ExecuteScalarAsync();
                    recordsCount = result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
                }
            }
            return recordsCount;

        }
    }
}

