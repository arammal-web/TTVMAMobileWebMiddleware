using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[PrimaryKey("ApplicationId", "ProcessId", "BPVarietyId", "ChekListId")]
[Table("ApplicationProcessCheckList")]
public partial class ApplicationProcessCheckList
{
    [Key]
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    [Key]
    public int ProcessId { get; set; }

    [Key]
    public int BPVarietyId { get; set; }

    [Key]
    public int ChekListId { get; set; }

    public bool IsDocRequired { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }
     
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }
     
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }
     
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessCheckLists")]
    [JsonIgnore]
    public virtual ApplicationProcess ApplicationProcess { get; set; } = null!;

    [InverseProperty("ApplicationProcessCheckList")]
    [JsonIgnore]
    public virtual ICollection<ApplicationProcessCheckListDocFile> ApplicationProcessCheckListDocFiles { get; set; } = new List<ApplicationProcessCheckListDocFile>();

    [ForeignKey("ChekListId, ProcessId, BPVarietyId")]
    [InverseProperty("ApplicationProcessCheckLists")]
    [JsonIgnore]
    public virtual ProcessCheckList ProcessCheckList { get; set; } = null!;
}
