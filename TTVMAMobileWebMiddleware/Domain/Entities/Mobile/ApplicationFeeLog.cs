using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationFeeLog")]
public partial class ApplicationFeeLog
{
    [Key]
    public int LogID { get; set; }

    [StringLength(10)]
    public string OperationType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OperationTimestamp { get; set; }

    [StringLength(50)]
    public string ExecutedByUserID { get; set; } = null!;

    public int ApplicationFeeId { get; set; }

    public int ApplicationId { get; set; }

    public int FeeId { get; set; }

    [StringLength(512)]
    public string? FeeCode { get; set; }

    public int FeeCategoryId { get; set; }

    public int FeeTypeId { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal FeeValue { get; set; }

    [StringLength(50)]
    public string? FeeSP { get; set; }

    public bool IsMunicipalFee { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal FeeTotal { get; set; }

    public bool CurrentYearIncluded { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountAmount { get; set; }

    [Column(TypeName = "decimal(19, 2)")]
    public decimal? DiscountPercentage { get; set; }

    public bool IsExempted { get; set; }

    [StringLength(250)]
    public string? Description { get; set; }

    [StringLength(512)]
    public string? InvoiceNumber { get; set; }

    public bool? IsPaid { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }
}
