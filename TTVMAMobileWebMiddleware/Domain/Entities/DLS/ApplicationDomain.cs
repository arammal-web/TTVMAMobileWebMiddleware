
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("Domain", Schema = "STR")]
public partial class ApplicationDomain
{
    /// <summary>
    /// Primary key for the StructureType table
    /// </summary>
    [Key]
    [StringLength(3)]
    public string Id { get; set; } = null!;

    /// <summary>
    /// Description of the structure type
    /// </summary>
    [StringLength(50)]
    public string DescriptionEn { get; set; } = null!;

    /// <summary>
    /// Description of the structure type
    /// </summary>
    [StringLength(50)]
    public string DescriptionAr { get; set; } = null!;

    /// <summary>
    /// Description of the structure type
    /// </summary>
    [StringLength(50)]
    public string DescriptionFr { get; set; } = null!;

    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>
    /// Indicates if the structure type is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Date the record was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// User who created the record
    /// </summary>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Date the record was last modified
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// User who last modified the record
    /// </summary>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// User who created the application type record
    /// </summary>
    [InverseProperty("ApplicationDomain")]
    public virtual ICollection<ApplicationDLS?> Applications { get; set; } = new List<ApplicationDLS>();


}
