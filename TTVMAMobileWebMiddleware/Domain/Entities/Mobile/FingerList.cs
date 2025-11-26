using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("FingerList")]
public partial class FingerList
{
    [Key]
    public int Id { get; set; }

    [StringLength(250)]
    public string Description_En { get; set; } = null!;

    [StringLength(250)]
    public string? Description_Fr { get; set; }

    [StringLength(250)]
    public string? Description_Ar { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    [InverseProperty("Finger")]
    public virtual ICollection<CitizenFingerprint> CitizenFingerprints { get; set; } = new List<CitizenFingerprint>();
}
