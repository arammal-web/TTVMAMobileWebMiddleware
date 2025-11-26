using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Status")]
public partial class Status
{
    [Key]
    public int ID { get; set; }

    [StringLength(25)]
    public string StatusDesc { get; set; } = null!;

    [StringLength(25)]
    public string? StatusDescAr { get; set; }

    [StringLength(25)]
    public string? StatusDescFr { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<ApplicationRequestedPlatesInfo> ApplicationRequestedPlatesInfos { get; set; } = new List<ApplicationRequestedPlatesInfo>();

    // Applications linked by Application.ApplicationApprovalStatusId
    [InverseProperty("ApplicationApprovalStatus")]
    public virtual ICollection<ApplicationMob> Applications { get; set; } = new List<ApplicationMob>();

    // Applications linked by Application.StatusId
    [InverseProperty("Status")]
    public virtual ICollection<ApplicationMob> ApplicationsWithStatus { get; set; } = new List<ApplicationMob>();

    [InverseProperty("Status")]
    public virtual ICollection<DocumentGroup> DocumentGroups { get; set; } = new List<DocumentGroup>();

    [InverseProperty("Status")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [InverseProperty("Status")]
    public virtual ICollection<ExemptionType> ExemptionTypes { get; set; } = new List<ExemptionType>();

    [InverseProperty("Status")]
    public virtual ICollection<FeeCategory> FeeCategories { get; set; } = new List<FeeCategory>();

    [InverseProperty("Status")]
    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    [InverseProperty("Status")]
    public virtual ICollection<ProcessExemptionFee> ProcessExemptionFees { get; set; } = new List<ProcessExemptionFee>();

    [InverseProperty("Status")]
    public virtual ICollection<ProcessFee> ProcessFees { get; set; } = new List<ProcessFee>();

    [InverseProperty("Status")]
    public virtual ICollection<Process> Processes { get; set; } = new List<Process>();

    [InverseProperty("Status")]
    public virtual ICollection<VarietyBusinessProcess> VarietyBusinessProcesses { get; set; } = new List<VarietyBusinessProcess>();
}
