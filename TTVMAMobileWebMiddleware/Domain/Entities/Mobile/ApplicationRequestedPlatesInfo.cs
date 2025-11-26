using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("ApplicationRequestedPlatesInfo")]
public partial class ApplicationRequestedPlatesInfo
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ApplicationId { get; set; } = null!;

    public int StatusId { get; set; }

    public bool IsIssuePlates { get; set; }

    public int? RequestedPlates { get; set; }

    public int? RequestedPlatesCode { get; set; }

    public int? ReleasePlates { get; set; }

    public int? ReleasePlatesCode { get; set; }

    public int? NumberGeneration { get; set; }

    [StringLength(250)]
    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("ApplicationId")]
    [InverseProperty("ApplicationRequestedPlatesInfos")]
    public virtual ApplicationMob Application { get; set; } = null!;

    [ForeignKey("StatusId")]
    [InverseProperty("ApplicationRequestedPlatesInfos")]
    public virtual Status Status { get; set; } = null!;
}
