using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace BioEnrollment.Domain.Entities;

[Table("Documents", Schema = "dbo")]
public partial class DocumentBio
{
    public int Id { get; set; }

    public string DocumentNameEn { get; set; } = null!;

    public string DocumentNameAr { get; set; } = null!;

    public string? DocumentCode { get; set; }

    public int? GroupId { get; set; }

    public string? Domain { get; set; }

    public int? MigrationId { get; set; }

    public bool IsDeleted { get; set; }

    public int? DeletedUserId { get; set; }

    public DateTime? DeletedDate { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    public int? StatusId { get; set; }
    public string? BindingColumn { get; set; }


 }
