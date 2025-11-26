using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId")]
[Table("ApplicationProcess")]
public partial class ApplicationProcess
{
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    public int? StatusId { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalFee { get; set; }

    [Column(TypeName = "decimal(18, 0)")]
    public decimal? TotalTax { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }
   
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? DrivingLicenseId { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("ApplicationProcesses")]
    [JsonIgnore]
    public virtual ApplicationMob Application { get; set; } = null!;

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcessCheckList> ApplicationProcessCheckLists { get; set; } = new List<ApplicationProcessCheckList>();

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcessCondition> ApplicationProcessConditions { get; set; } = new List<ApplicationProcessCondition>();

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcessDocument> ApplicationProcessDocuments { get; set; } = new List<ApplicationProcessDocument>();

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcessFee> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<DrivingTestRequest> DrivingTestRequests { get; set; } = new List<DrivingTestRequest>();

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<OperationRequest> OperationRequests { get; set; } = new List<OperationRequest>();

    [ForeignKey("ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcesses")]
    [JsonIgnore]
    public virtual VarietyBusinessProcess VarietyBusinessProcess { get; set; } = null!;

    [InverseProperty("ApplicationProcess")]
    [JsonIgnore]
    public virtual ICollection<DrivingLicenseDetail> DrivingLicenseDetails { get; set; } = new List<DrivingLicenseDetail>();

}
