using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the requested plates information for an application
/// </summary>
[Table("ApplicationRequestedPlatesInfo", Schema = "APP")]
public partial class ApplicationRequestedPlatesInfo
{
    /// <summary>
    /// Unique identifier for the requested plates info record
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the application record
    /// </summary>
    /// <example>1</example>
    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    /// <summary>
    /// Reference to the status of the plate request
    /// </summary>
    /// <example>1</example>
    public int StatusId { get; set; }

    /// <summary>
    /// Flag indicating whether plates are to be issued
    /// </summary>
    /// <example>true</example>
    public bool IsIssuePlates { get; set; }

    /// <summary>
    /// Total number of requested plates
    /// </summary>
    /// <example>1</example>
    public int? RequestedPlates { get; set; }

    /// <summary>
    /// Code for the requested plates
    /// </summary>
    /// <example>1</example>
    public int? RequestedPlatesCode { get; set; }

    /// <summary>
    /// Number of plates to be released
    /// </summary>
    /// <example>1</example>
    public int? ReleasePlates { get; set; }

    /// <summary>
    /// Code for the plates to be released
    /// </summary>
    /// <example>1</example>
    public int? ReleasePlatesCode { get; set; }

    /// <summary>
    /// Reference to the number generation method or identifier
    /// </summary>
    /// <example>1</example>
    public int? NumberGeneration { get; set; }

    /// <summary>
    /// Additional notes for the plate request
    /// </summary>
    /// <example>Notes</example>
    [StringLength(250)]
    public string? Notes { get; set; }

    /// <summary>
    /// Flag indicating whether the record is deleted
    /// </summary>
    /// <example>false</example>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Reference to the user who deleted the record
    /// </summary>
    /// <example>1</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Timestamp when the record was deleted
    /// </summary>
    /// <example>2023-01-01T00:00:00.000Z</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// Timestamp when the record was created
    /// </summary>
    /// <example>2023-01-01T00:00:00.000Z</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Reference to the user who created the record
    /// </summary>
    /// <example>1</example>
    public int CreatedUserId { get; set; }

    /// <summary>
    /// Timestamp when the record was last modified
    /// </summary>
    /// <example>2023-01-01T00:00:00.000Z</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// Reference to the user who last modified the record
    /// </summary>
    /// <example>1</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Reference to the application
    /// </summary>
    [ForeignKey("ApplicationId")]
    [InverseProperty("ApplicationRequestedPlatesInfos")]
    public virtual ApplicationDLS? Application { get; set; } = null!;
}
