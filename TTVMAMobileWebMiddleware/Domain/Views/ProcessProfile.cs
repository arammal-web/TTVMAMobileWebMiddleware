namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ProcessProfile
    {
        public int ProcessId { get; init; }
        public IReadOnlyList<int> CategoryIds { get; init; } = Array.Empty<int>();
        public int ApplicantId { get; init; }
        public int ApplicantAge { get; init; }

        // 1) MinAge
        public MinAgeProfile MinAge { get; init; } = new();
        public sealed class MinAgeProfile
        {
            public int? RequiredAge { get; set; }     // from Age1 (max across matched rules)
            public bool IsStopper { get; set; }       // any matched rule IsStopProcess==true
            public bool Passed { get; set; }          // ApplicantAge >= RequiredAge
        }

        // 2) RenewalValidity
        public RenewalValidityProfile Renewal { get; init; } = new();
        public sealed class RenewalValidityProfile
        {
            public int? Age1 { get; set; }            // threshold 1 (e.g., 50)
            public int? Age2 { get; set; }            // threshold 2 (e.g., 65)
            public int? Value1 { get; set; }          // years (e.g., 10 or 5)
            public int? Value2 { get; set; }          // years (e.g., 5 or 3)
            public int? Value3 { get; set; }          // years (e.g., 3 or 2)
            public int? ValidityYears { get; set; }   // computed validity for applicant
        }

        // 3) BiometricIsRequired
        public BiometricProfile Biometric { get; init; } = new();
        public sealed class BiometricProfile
        {
            public bool Required { get; set; }        // true if rule exists and no age exemption
            public bool RequiredEnrollment { get; set; } // true if rule Enrollment is required
            public int? AgeExemptMax { get; set; }    // Age2 in rules -> exempt >= this
        }

        // 4) ExamIsRequired
        public bool ExamRequired { get; set; }

        // 4.1) ExamIsRequired
        public bool UrgentExamRequired { get; set; }

        // 5) NationalityRestriction
        public NationalityProfile Nationality { get; init; } = new();
        public sealed class NationalityProfile
        {
            public List<int> AllowedNationalityIds { get; } = new();
            public bool Passed { get; set; }
        }

        // 6) RequiredToBeExpired
        public bool RequireExpiredLicense { get; set; }  // when a rule exists
        public bool? IsLicenseExpired { get; set; }      // actual check result if DL exists

        public bool IsOperationRequired { get; set; }      // actual check result if DL exists

        // 7) CategoryPrerequisites
        public List<MissingPrerequisite> MissingPrerequisites { get; } = new();
        public sealed class MissingPrerequisite
        {
            public int CategoryId { get; set; }                     // BPVarietyId
            public int RequiredCategoryId { get; set; }             // BPVarietyPrerequisiteId
            public ProcessVarietyNamesDto? RequiredCategoryName { get; set; }
        }

        // 8) WaitingPeriods (retake)
        public WaitingPeriodsProfile Waiting { get; init; } = new();
        public sealed class WaitingPeriodsProfile
        {
            public int? TheoryWeeks { get; set; }       // Value1 = weeks
            public int? PracticalMonths { get; set; }   // Value2 = months
            public DateTime? NextTheoryAllowedOn { get; set; }
            public DateTime? NextPracticalAllowedOn { get; set; }
            public bool Passed { get; set; } = true;
        }

        // 9) RequiredToBeSuspended
        public bool RequireSuspendedLicense { get; set; }   // when a rule exists
        public bool? IsLicenseValid { get; set; }           // computed from DL status/block

        // 10) IsAdministrative
        public bool IsAdministrative { get; set; }

        // 11) InitialStatus (Value1,Value2,Value3)
        public InitialStatusProfile InitialStatus { get; init; } = new();
        public sealed class InitialStatusProfile
        {
            public int? ApplicationApprovalStatusId { get; set; }  // Value1
            public int? ApplicationStatusId { get; set; }          // Value2
            public int? DrivingLicenseStatusId { get; set; }       // Value3
        }
        // 12) RenewalValidity
        public InternationalRenewalValidityProfile International { get; init; } = new();
        public sealed class InternationalRenewalValidityProfile
        {
            public int? Age1 { get; set; }            // threshold 1 (e.g., 0)
            public int? Age2 { get; set; }            // threshold 2 (e.g., 100)
            public int? Value1 { get; set; }          // years (e.g., 1)
            public int? Value2 { get; set; }          // years (e.g.,1)
            public int? Value3 { get; set; }          // years (e.g.,1)
            public int? ValidityYears { get; set; }   // computed validity for applicant
            public bool IsApplicable { get; set; }
        }
        // NEW: DL requirement + snapshot
        public bool? DLRequired { get; set; }
        public DrivingLicenseProfile? DrivingLicense { get; set; }
        public sealed class DrivingLicenseProfile
        {
            // Primary identifiers
            public int Id { get; set; }
            public string? DrivingLicenseNumber { get; set; }
            public bool IsInternational { get; set; }

            // Status (EN/AR/FR)
            public int StatusId { get; set; }
            public string? StatusEn { get; set; }
            public string? StatusAr { get; set; }
            public string? StatusFr { get; set; }
            public DateTime? StatusDate { get; set; }

            // Useful flags & stats
            public bool? IsBlocked { get; set; }
            public int? NumberOfPoints { get; set; }

            // High-level timing (across details)
            public DateTime? LastIssuingDate { get; set; }   //  IssuingDate
            public DateTime? ExpiryDate { get; set; }    //  ExpiryDate
        }


        /// <summary>
        /// Gets a list of errors as key-value pairs.
        /// </summary>
        public List<KeyValuePair<string, string>> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
        public bool IsValid => Errors.Count == 0;
    }

}
