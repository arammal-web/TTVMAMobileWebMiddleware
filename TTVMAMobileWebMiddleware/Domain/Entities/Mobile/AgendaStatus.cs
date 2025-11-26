using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("AgendaStatus")]
public partial class AgendaStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? DescriptionEN { get; set; }

    [StringLength(50)]
    public string? DescriptionAR { get; set; }

    [StringLength(50)]
    public string? DescriptionFR { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime DateCreated { get; set; }

    [InverseProperty("Status")]
    public virtual ICollection<Agendum> Agenda { get; set; } = new List<Agendum>();
}
