using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Country")]
public partial class Country
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string CountryCode { get; set; } = null!;

    [StringLength(100)]
    public string CountryNameEN { get; set; } = null!;

    [StringLength(100)]
    public string? CountryNameAR { get; set; }

    [StringLength(100)]
    public string? CountryNameFR { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Country")]
    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    [InverseProperty("Country")]
    public virtual ICollection<CitizenAddress> CitizenAddresses { get; set; } = new List<CitizenAddress>();

    [InverseProperty("Country")]
    public virtual ICollection<Prefecture> Prefectures { get; set; } = new List<Prefecture>();

    [InverseProperty("Country")]
    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    [InverseProperty("Country")]
    public virtual ICollection<Village> Villages { get; set; } = new List<Village>();
}
