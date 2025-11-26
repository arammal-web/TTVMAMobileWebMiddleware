using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents the status of an agenda item, such as Scheduled, Completed, or Cancelled.
/// </summary>
[Table("AgendaStatus", Schema = "APP")]
public partial class AgendaStatus
{
    /// <summary>
    /// Unique identifier for the agenda status.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Description of the status in English.
    /// </summary>
    /// <example>Scheduled</example>
    [StringLength(50)]
    public string? DescriptionEN { get; set; }

    /// <summary>
    /// Description of the status in Arabic.
    /// </summary>
    /// <example>مجدول</example>
    [StringLength(50)]
    public string? DescriptionAR { get; set; }

    /// <summary>
    /// Description of the status in French.
    /// </summary>
    /// <example>Planifié</example>
    [StringLength(50)]
    public string? DescriptionFR { get; set; }

    /// <summary>
    /// Date the agenda status record was created.
    /// </summary>
    /// <example>2025-01-01T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Collection of agenda records associated with this status.
    /// </summary>
    [InverseProperty("Status")]
    public virtual ICollection<Agenda?> Agenda { get; set; } = new List<Agenda>();
}
