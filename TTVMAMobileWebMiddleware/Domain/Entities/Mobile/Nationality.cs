using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Nationality")]
public partial class Nationality
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string? NameEN { get; set; }

    [StringLength(100)]
    public string? NameAR { get; set; }

    [StringLength(100)]
    public string? NameFR { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Nationality")]
    public virtual ICollection<Citizen> CitizenNationalities { get; set; } = new List<Citizen>();

    [InverseProperty("SpouseNationality")]
    public virtual ICollection<Citizen> CitizenSpouseNationalities { get; set; } = new List<Citizen>();
}
