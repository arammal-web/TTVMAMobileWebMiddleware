using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Prefecture")]
public partial class Prefecture
{
    [Key]
    public int Id { get; set; }

    public int CountryId { get; set; }

    [StringLength(100)]
    public string NameEN { get; set; } = null!;

    [StringLength(100)]
    public string? NameAR { get; set; }

    [StringLength(100)]
    public string? NameFR { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Prefecture")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [InverseProperty("Prefecture")]
    public virtual ICollection<CitizenAddress> CitizenAddresses { get; set; } = new List<CitizenAddress>();

    [ForeignKey("CountryId")]
    [InverseProperty("Prefectures")]
    public virtual Country Country { get; set; } = null!;

    [InverseProperty("Prefecture")]
    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    [InverseProperty("Prefecture")]
    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();
}
