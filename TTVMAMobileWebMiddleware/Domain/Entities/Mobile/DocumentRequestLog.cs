using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("DocumentRequestLog")]
public partial class DocumentRequestLog
{
    [Key]
    public int LogID { get; set; }

    [StringLength(10)]
    public string OperationType { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime OperationTimestamp { get; set; }

    [StringLength(50)]
    public string ExecutedByUserID { get; set; } = null!;

    public int DocumentRequestId { get; set; }

    public int DocumentRequestTypeId { get; set; }

    public int ApplicationId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ExpiryDate { get; set; }

    public int? NumberOfPrints { get; set; }

    public byte[]? DocumentFile { get; set; }

    [StringLength(50)]
    public string? DocumentTitle { get; set; }

    [StringLength(250)]
    public string? DocumentOriginalPath { get; set; }

    public int? StatusId { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }
}
