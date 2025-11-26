using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("CheckList")]
public partial class CheckList
{
    [Key]
    public int Id { get; set; }

    public int CheckListGroupId { get; set; }

    public int? CheckListCode { get; set; }

    [StringLength(150)]
    public string? DescriptionEn { get; set; }

    [StringLength(150)]
    public string? DescriptionAr { get; set; }

    [StringLength(50)]
    public string? DescriptionFr { get; set; }

    public int? MigrationId { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [StringLength(20)]
    public string? Icon { get; set; }

    [ForeignKey("CheckListGroupId")]
    [InverseProperty("CheckLists")]
    public virtual CheckListGroup CheckListGroup { get; set; } = null!;

    [InverseProperty("ChekList")]
    public virtual ICollection<ProcessCheckList> ProcessCheckLists { get; set; } = new List<ProcessCheckList>();
}
