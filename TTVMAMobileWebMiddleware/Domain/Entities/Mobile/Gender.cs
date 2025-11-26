using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Gender")]
public partial class Gender
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? DescriptionEN { get; set; }

    [StringLength(50)]
    public string? DescriptionAR { get; set; }

    [StringLength(50)]
    public string? DescriptionFR { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Gender")]
    public virtual ICollection<Citizen> Citizens { get; set; } = new List<Citizen>();
}
