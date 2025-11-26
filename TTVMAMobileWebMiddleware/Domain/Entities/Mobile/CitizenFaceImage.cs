using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CitizenFaceImage")]
public partial class CitizenFaceImage
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public byte[] FaceImage { get; set; } = null!;

    public byte[]? FaceTemplate { get; set; }

    [StringLength(20)]
    public string? ImageFormat { get; set; }

    public bool? IsPrimaryPhoto { get; set; }

    [StringLength(100)]
    public string? CaptureDevice { get; set; }

    [StringLength(50)]
    public string? FaceEncodingVersion { get; set; }

    [StringLength(200)]
    public string? Description { get; set; }

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
    [InverseProperty("CitizenFaceImages")]
    public virtual Citizen? Citizen { get; set; } = null!;
}
