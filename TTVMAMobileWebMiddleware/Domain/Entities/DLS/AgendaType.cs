using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

/// <summary>
/// Represents a type or category of agenda, such as Appointment, Meeting, or Task.
/// </summary>
[Table("AgendaType", Schema = "APP")]
public partial class AgendaType
{
    /// <summary>
    /// Unique identifier for the agenda type.
    /// </summary>
    /// <example>1</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Description of the agenda type in English.
    /// </summary>
    /// <example>Appointment</example>
    [StringLength(50)]
    public string? DescriptionEN { get; set; }

    /// <summary>
    /// Description of the agenda type in Arabic.
    /// </summary>
    /// <example>موعد</example>
    [StringLength(50)]
    public string? DescriptionAR { get; set; }

    /// <summary>
    /// Description of the agenda type in French.
    /// </summary>
    /// <example>Rendez-vous</example>
    [StringLength(50)]
    public string? DescriptionFR { get; set; }

    /// <summary>
    /// Date the agenda type record was created.
    /// </summary>
    /// <example>2025-01-01T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime DateCreated { get; set; }

    /// <summary>
    /// Collection of agenda items associated with this agenda type.
    /// </summary>
    [InverseProperty("AgendaType")]
    public virtual ICollection<Agenda?> Agenda { get; set; } = new List<Agenda>();
}
