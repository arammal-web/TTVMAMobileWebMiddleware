using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS
;
[Table("Agenda", Schema = "APP")]
public partial class Agenda
{
    /// <summary>
    /// Unique identifier for the agenda record.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Reference to the citizen associated with the agenda.
    /// </summary>
    /// <example>2025</example>
    public int CitizenId { get; set; }

    /// <summary>
    /// Reference to the agenda type.
    /// </summary>
    /// <example>1</example>
    public int AgendaTypeId { get; set; }

    /// <summary>
    /// Reference to the application record.
    /// </summary>
    /// <example>APP-2025-001</example>
    [StringLength(50)]
    public string? ApplicationId { get; set; }

    /// <summary>
    /// Appointment sequence number.
    /// </summary>
    /// <example>A-001</example>
    [StringLength(50)]
    public string? AppointmentSeqNumber { get; set; }

    /// <summary>
    /// Date of the appointment.
    /// </summary>
    /// <example>2025-06-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? AppointmentDate { get; set; }

    /// <summary>
    /// Appointment start time.
    /// </summary>
    /// <example>2025-06-01T09:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// Appointment end time.
    /// </summary>
    /// <example>2025-06-01T10:00:00</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// Subject of the agenda.
    /// </summary>
    /// <example>Driving License Interview</example>
    [StringLength(250)]
    public string? Subject { get; set; }

    /// <summary>
    /// Additional notes for the agenda.
    /// </summary>
    /// <example>Bring all required documents</example>
    [StringLength(500)]
    public string? Note { get; set; }

    /// <summary>
    /// Reference to the branch.
    /// </summary>
    /// <example>2</example>
    public int? BranchId { get; set; }

    /// <summary>
    /// Reference to the department.
    /// </summary>
    /// <example>5</example>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// Reference to the section.
    /// </summary>
    /// <example>3</example>
    public int? SectionId { get; set; }

    /// <summary>
    /// Status ID of the agenda.
    /// </summary>
    /// <example>1</example>
    public int? StatusId { get; set; }

    /// <summary>
    /// Status update date.
    /// </summary>
    /// <example>2025-06-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? StatusDate { get; set; }

    /// <summary>
    /// User who modified the status.
    /// </summary>
    /// <example>1001</example>
    public int? StatusModifyUser { get; set; }

    /// <summary>
    /// Flag indicating whether the appointment is late.
    /// </summary>
    /// <example>false</example>
    public bool? IsLate { get; set; }

    /// <summary>
    /// Date the appointment was marked late.
    /// </summary>
    /// <example>2025-06-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? IsLateDate { get; set; }

    /// <summary>
    /// Flag indicating whether the appointment is urgent.
    /// </summary>
    /// <example>true</example>
    public bool? IsUrgent { get; set; }

    /// <summary>
    /// Reason for urgency.
    /// </summary>
    /// <example>Medical urgency</example>
    [StringLength(500)]
    public string? UrgentReason { get; set; }

    /// <summary>
    /// Indicates if the appointment is for walk-in.
    /// </summary>
    /// <example>false</example>
    public bool? IsForWalkInAppointment { get; set; }

    /// <summary>
    /// Indicates whether the appointment has a reminder.
    /// </summary>
    /// <example>true</example>
    public bool? HasReminder { get; set; }

    /// <summary>
    /// Reminder interval.
    /// </summary>
    /// <example>15</example>
    public int? RemiderInterval { get; set; }

    /// <summary>
    /// Units for the reminder interval.
    /// </summary>
    /// <example>1</example>
    public int? RemiderUnits { get; set; }

    /// <summary>
    /// Full name of the citizen.
    /// </summary>
    /// <example>Ahmad Khoury</example>
    [StringLength(1000)]
    public string? CitizenFullName { get; set; }

    /// <summary>
    /// Additional comments.
    /// </summary>
    /// <example>Handle with care</example>
    [StringLength(500)]
    public string? Comments { get; set; }

    /// <summary>
    /// Logical deletion flag.
    /// </summary>
    /// <example>false</example>
    public bool  IsDeleted { get; set; }

    /// <summary>
    /// Deletion date.
    /// </summary>
    /// <example>2025-06-05</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who deleted the record.
    /// </summary>
    /// <example>15</example>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Record creation date.
    /// </summary>
    /// <example>2025-06-01</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the record.
    /// </summary>
    /// <example>1</example>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// Last modification date of the record.
    /// </summary>
    /// <example>2025-06-02</example>
    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the record.
    /// </summary>
    /// <example>2</example>
    public int? ModifiedUserId { get; set; }

    /// <summary>
    /// Navigation to AgendaType entity.
    /// </summary>
    [ForeignKey("AgendaTypeId")]
    [InverseProperty("Agenda")]
    public virtual AgendaType? AgendaType { get; set; } = null!;

    /// <summary>
    /// Navigation to Application entity.
    /// </summary>
    [ForeignKey("ApplicationId")]
    [InverseProperty("Agenda")]
    public virtual ApplicationDLS? Application { get; set; }

    /// <summary>
    /// Navigation to AgendaStatus entity.
    /// </summary>
    [ForeignKey("StatusId")]
    [InverseProperty("Agenda")]
    public virtual AgendaStatus? Status { get; set; }
}
