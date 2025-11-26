using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class Agendum
{
    [Key]
    public int Id { get; set; }

    public int CitizenId { get; set; }

    public int AgendaTypeId { get; set; }

    [StringLength(50)]
    public string? ApplicationId { get; set; }

    [StringLength(50)]
    public string? AppointmentSeqNumber { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? AppointmentDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? StartTime { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? EndTime { get; set; }

    [StringLength(250)]
    public string? Subject { get; set; }

    [StringLength(500)]
    public string? Note { get; set; }

    public int? BranchId { get; set; }

    public int? DepartmentId { get; set; }

    public int? SectionId { get; set; }

    public int? StatusId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? StatusDate { get; set; }

    public int? StatusModifyUser { get; set; }

    public bool? IsLate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? IsLateDate { get; set; }

    public bool? IsUrgent { get; set; }

    [StringLength(500)]
    public string? UrgentReason { get; set; }

    public bool? IsForWalkInAppointment { get; set; }

    public bool? HasReminder { get; set; }

    public int? RemiderInterval { get; set; }

    public int? RemiderUnits { get; set; }

    [StringLength(1000)]
    public string? CitizenFullName { get; set; }

    [StringLength(500)]
    public string? Comments { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("AgendaTypeId")]
    [InverseProperty("Agenda")]
    public virtual AgendaType AgendaType { get; set; } = null!;

    [ForeignKey("ApplicationId")]
    [InverseProperty("Agenda")]
    public virtual ApplicationMob? Application { get; set; }

    [ForeignKey("StatusId")]
    [InverseProperty("Agenda")]
    public virtual AgendaStatus? Status { get; set; }
}
