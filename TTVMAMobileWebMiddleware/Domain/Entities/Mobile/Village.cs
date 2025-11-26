using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Village")]
public partial class Village
{
    [Key]
    public int Id { get; set; }

    public int CountryId { get; set; }

    public int PrefectureId { get; set; }

    public int? RegionId { get; set; }

    [StringLength(100)]
    public string NameEN { get; set; } = null!;

    [StringLength(100)]
    public string? NameAR { get; set; }

    [StringLength(100)]
    public string? NameFR { get; set; }

    public bool? IsCapital { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Village")]
    public virtual ICollection<CitizenAddress> CitizenAddresses { get; set; } = new List<CitizenAddress>();

    [ForeignKey("CountryId")]
    [InverseProperty("Villages")]
    public virtual Country Country { get; set; } = null!;

    [ForeignKey("PrefectureId")]
    [InverseProperty("Villages")]
    public virtual Prefecture Prefecture { get; set; } = null!;
 
 
     public virtual Region? Region { get; set; } = null!;
}
