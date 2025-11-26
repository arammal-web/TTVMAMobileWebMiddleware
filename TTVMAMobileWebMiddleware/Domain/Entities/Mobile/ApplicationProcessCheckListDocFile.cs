using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationProcessCheckListDocFile")]
public partial class ApplicationProcessCheckListDocFile
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    public int ProcessId { get; set; }

    public int BPVarietyId { get; set; }

    public int ChekListId { get; set; }

    [StringLength(250)]
    public string? DocFilePath { get; set; }

    [StringLength(250)]
    public string? DocTitle { get; set; }

    [StringLength(500)]
    public string? DocKeyWord { get; set; }

    [MaxLength(2000)]
    public byte[]? DocFileData { get; set; }

    [StringLength(10)]
    public string? DocFileExt { get; set; }

    [StringLength(150)]
    public string? DocFileHash { get; set; }

    public bool? IsSignedDocFile { get; set; }

    [StringLength(150)]
    public string? DocFilePublicKey { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public int? StatusId { get; set; }

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

    [ForeignKey("ApplicationId, ProcessId, BPVarietyId, ChekListId")]
    [InverseProperty("ApplicationProcessCheckListDocFiles")]
    public virtual ApplicationProcessCheckList ApplicationProcessCheckList { get; set; } = null!;
}
