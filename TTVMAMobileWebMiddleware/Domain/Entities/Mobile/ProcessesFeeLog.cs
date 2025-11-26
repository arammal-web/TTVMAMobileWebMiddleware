using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ProcessesFeeLog")]
public partial class ProcessesFeeLog
{
    [Key]
    public int LogID { get; set; }

    [StringLength(10)]
    public string OperationType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OperationTimestamp { get; set; }

    [StringLength(50)]
    public string ExecutedByUserID { get; set; } = null!;

    public int ProcessesFeeLogId { get; set; }

    public int FeeId { get; set; }

    public int BPId { get; set; }

    public int LicenseTypeId { get; set; }

    [Column(TypeName = "decimal(3, 2)")]
    public decimal? TaxPercentageApplicable { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }
}
