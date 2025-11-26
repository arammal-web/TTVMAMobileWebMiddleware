using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class ConditionGroup
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? Domain { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [InverseProperty("ConditionGroup")]
    public virtual ICollection<Condition> Conditions { get; set; } = new List<Condition>();
}
