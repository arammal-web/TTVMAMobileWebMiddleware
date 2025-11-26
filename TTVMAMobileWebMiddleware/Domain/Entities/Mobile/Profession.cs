using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Profession")]
public partial class Profession
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

    [InverseProperty("Profession")]
    public virtual ICollection<Citizen> CitizenProfessions { get; set; } = new List<Citizen>();

    [InverseProperty("SpouseProfession")]
    public virtual ICollection<Citizen> CitizenSpouseProfessions { get; set; } = new List<Citizen>();
}
