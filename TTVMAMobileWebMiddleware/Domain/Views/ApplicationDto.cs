using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public sealed class ApplicationDto
    {
        public string Id { get; set; } = null!;
        public string ApplicationNumber { get; set; } = null!;
        public string ApplicationTypeId { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public int OwnerId { get; set; }

        public StatusDto? ApplicationApprovalStatus { get; set; }
        public StatusDto? ApplicationStatus { get; set; }
        public DrivingLicenseDTO DrivingLicense { get; set; }

        public List<ApplicationProcessDto> ApplicationProcesses { get; set; } = new();
    }
    public class ApprovePendingApplicationDTO
    {
        public ApplicationMob? Application { get; set; }
        public List<ApplicationProcess> ApplicationProcess { get; set; } = new();
        public List<ApplicationProcessCondition> ApplicationProcessCondition { get; set; } = new();
        public List<ApplicationProcessDocument> ApplicationProcessDocument { get; set; } = new();
        public List<ApplicationProcessCheckList> ApplicationProcessCheckList { get; set; } = new();
        public List<ApplicationProcessFee> ApplicationProcessFees { get; set; } = new();
 
    }
}
