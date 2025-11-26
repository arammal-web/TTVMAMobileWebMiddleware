using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("FeeCategory")]
public partial class FeeCategory
{
    [Key]
    public int Id { get; set; }

    [StringLength(30)]
    public string NameEn { get; set; } = null!;

    [StringLength(30)]
    public string NameAr { get; set; } = null!;

    public int Sequence { get; set; }

    [StringLength(20)]
    public string? ExpiryDate { get; set; }

    [StringLength(20)]
    public string? AdditionalExpiryDate { get; set; }

    public int? StatusId { get; set; }

    [StringLength(50)]
    public string? Code { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

    public int? MigrationID { get; set; }

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

    [StringLength(250)]
    public string? NameFr { get; set; }

    [InverseProperty("FeeCategory")]
    public virtual ICollection<ApplicationProcessFee> ApplicationProcessFees { get; set; } = new List<ApplicationProcessFee>();

    [InverseProperty("FeeCategory")]
    public virtual ICollection<Fee> Fees { get; set; } = new List<Fee>();

    [InverseProperty("FeeCategory")]
    public virtual ICollection<ReceiptDetailMOB> ReceiptDetails { get; set; } = new List<ReceiptDetailMOB>();

    [ForeignKey("StatusId")]
    [InverseProperty("FeeCategories")]
    public virtual Status? Status { get; set; }
}
