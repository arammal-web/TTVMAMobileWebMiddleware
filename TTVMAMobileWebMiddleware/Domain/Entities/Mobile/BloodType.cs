using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("BloodType")]
public partial class BloodType
{
    [Key]
    public int Id { get; set; }

    [StringLength(10)]
    public string? BloodCode { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Blood")]
    public virtual ICollection<Citizen> Citizens { get; set; } = new List<Citizen>();
}
