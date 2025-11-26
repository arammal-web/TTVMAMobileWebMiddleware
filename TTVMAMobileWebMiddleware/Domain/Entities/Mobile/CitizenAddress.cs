using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CitizenAddress")]
public partial class CitizenAddress
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int   CityId { get; set; }

    public int CountryId { get; set; }

    public int? RegionId { get; set; }

    public int? PrefectureId { get; set; }

    [Column("VillageID")]
    public int? VillageId { get; set; }

    [StringLength(500)]
    public string? Address1 { get; set; }

    [StringLength(500)]
    public string? Address2 { get; set; }

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

   
    public virtual Citizen? Citizen { get; set; } = null!;

    [ForeignKey("CityId")]
    [InverseProperty("CitizenAddresses")]
    public virtual City? City { get; set; } = null!;

    [ForeignKey("CountryId")]
    [InverseProperty("CitizenAddresses")]
    public virtual Country? Country { get; set; } = null!;

    [ForeignKey("PrefectureId")]
    [InverseProperty("CitizenAddresses")]
    public virtual Prefecture? Prefecture { get; set; }

    [ForeignKey("RegionId")]
    [InverseProperty("CitizenAddresses")]
    public virtual Region? Region { get; set; }

    [ForeignKey("VillageId")]
    [InverseProperty("CitizenAddresses")]
    public virtual Village? Village { get; set; }
}
