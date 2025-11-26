using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class CitizenFingerprint
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int FingerId { get; set; }

    public int FingerprintStatusId { get; set; }

    public byte[]? WSQ { get; set; }

    public int? StructureId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("CitizenId")]
    [InverseProperty("CitizenFingerprints")]
    public virtual Citizen? Citizen { get; set; } = null!;

    [ForeignKey("FingerId")]
    [InverseProperty("CitizenFingerprints")]
    public virtual FingerList? Finger { get; set; } = null!;

    [ForeignKey("FingerprintStatusId")]
    [InverseProperty("CitizenFingerprints")]
    public virtual FingerPrintStatus? FingerprintStatus { get; set; } = null!;
}
