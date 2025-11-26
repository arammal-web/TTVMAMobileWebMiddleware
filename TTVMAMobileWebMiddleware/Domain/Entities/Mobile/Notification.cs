using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Notification")]
public partial class Notification
{
    [Key]
    public int Id { get; set; }

    public long UserId { get; set; }

    [StringLength(200)]
    public string Title { get; set; } = null!;

    [StringLength(2000)]
    public string Message { get; set; } = null!;

    [StringLength(50)]
    public string? Type { get; set; }

    public bool IsRead { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ReadDate { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual AppUser? User { get; set; }
}

