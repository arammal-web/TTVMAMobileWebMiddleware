using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class DocumentGroup
{
    [Key]
    public int Id { get; set; }

    public int? GroupCode { get; set; }

    [StringLength(100)]
    public string GroupNameEn { get; set; } = null!;

    [StringLength(100)]
    public string? GroupNameAr { get; set; }

    [StringLength(100)]
    public string? GroupNameFr { get; set; }

    [StringLength(1)]
    public string? Choose { get; set; }

    [StringLength(50)]
    public string? Icon { get; set; }

    [StringLength(50)]
    public string? Domain { get; set; }

    public int? MigrationId { get; set; }

    public bool IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }

    [InverseProperty("Group")]
    public virtual ICollection<Document> Documents { get; set; } = new List<Document>();

    [ForeignKey("StatusId")]
    [InverseProperty("DocumentGroups")]
    public virtual Status? Status { get; set; }
}
