using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationType")]
public partial class ApplicationType
{
    [Key]
    [StringLength(3)]
    [Unicode(false)]
    public string Id { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionEN { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionAR { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionFR { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("ApplicationType")]
    public virtual ICollection<ApplicationMob> Applications { get; set; } = new List<ApplicationMob>();
}
