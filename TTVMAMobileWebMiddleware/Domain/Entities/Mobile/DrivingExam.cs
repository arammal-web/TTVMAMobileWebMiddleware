using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("DrivingExam")]
public partial class DrivingExam
{
    /// <summary>
    /// Primary key of the DrivingExam table
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Citizen ID for which the driving exam is conducted
    /// </summary>
    public int CitizenId { get; set; }

    /// <summary>
    /// Reference to the structure where the driving exam is conducted
    /// </summary>
    public int StructureId { get; set; }

    /// <summary>
    /// Reference to the driving test request record associated with this exam
    /// </summary>
    public int DrivingTestRequestId { get; set; }

    /// <summary>
    /// Date and time of the driving exam
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime ExamDateTime { get; set; }

    /// <summary>
    /// Driving license category ID being applied for
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Exam score achieved by the citizen
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// ID of the exam engine used for conducting the exam
    /// </summary>
    public int ExamEngineId { get; set; }

    /// <summary>
    /// Status of the driving exam
    /// </summary>
    public int ExamStatusId { get; set; }
    /// <summary>
    /// Image of the user during the driving exam
    /// </summary>
    public string? UserImage { get; set; }

    /// <summary>
    /// Any claims or additional information provided during the exam
    /// </summary>
    [StringLength(1000)]
    public string? Claim { get; set; }

    /// <summary>
    /// Notes or remarks regarding the driving exam
    /// </summary>
    [StringLength(2000)]
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates if the record is deleted
    /// </summary>
    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public TimeOnly? StartExamTime { get; set; }

    public TimeOnly? EndExamTime { get; set; }

    public bool? IsCompleted { get; set; }

    public bool? IsCanceled { get; set; }

    public int? CanceledUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CanceledDate { get; set; }

    [StringLength(250)]
    public string? CancelReason { get; set; }

    public string? QuestionGenerated { get; set; }

    public string? AnswerGenerated { get; set; }

    public bool IsPassed { get; set; }


}
