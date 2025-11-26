using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
 

[Table("Structure" )]
public partial class Structure 
{
    /// <summary>
    /// Unique identifier for each structure record
    /// </summary>
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Recursive reference to parent structure
    /// </summary>
    public int? ParentId { get; set; }


    /// <summary>
    /// Name of the structure
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Name of the structure
    /// </summary>
    [StringLength(100)]
    public string NameFr { get; set; } = null!;

    /// <summary>
    /// Name of the structure
    /// </summary>
    [StringLength(100)]
    public string NameAr { get; set; } = null!;

    [StringLength(255)]
    public string? DescriptionEn { get; set; }

    /// <summary>
    /// Description of the structure
    /// </summary>
    [StringLength(255)]
    public string? DescriptionAr { get; set; }

    /// <summary>
    /// Description of the structure
    /// </summary>
    [StringLength(255)]
    public string? DescriptionFr { get; set; }

    [StringLength(50)]
    public string? Abv { get; set; }

    [StringLength(15)]
    public string? Phone { get; set; }

    [StringLength(255)]
    public string? ReportHeader1 { get; set; }

    [StringLength(255)]
    public string? ReportHeader2 { get; set; }

    [StringLength(255)]
    public string? ReportFooter1 { get; set; }

    [StringLength(255)]
    public string? ReportFooter2 { get; set; }

    public bool IsHidden { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    /// <summary>
    /// The date when the record was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the record
    /// </summary>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// The date when the record was last modified
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who last modified the record
    /// </summary>
    public int? ModifiedUserId { get; set; }

}
