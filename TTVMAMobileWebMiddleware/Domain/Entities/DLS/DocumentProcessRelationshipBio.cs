using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS
{
    /// <summary>
    /// Represents the relationship between a document and a process, including metadata like status, creation, and deletion details.
    /// </summary>
    /// <remarks>
    /// This entity is used to define mandatory document requirements per process and track their lifecycle.
    /// </remarks>
    /// <example>
    /// var relation = new DocumentProcessRelationship
    /// {
    ///     DocumentId = 1,
    ///     ProcessId = 10,
    ///     IsMandatory = true,
    ///     CreatedDate = DateTime.UtcNow,
    ///     CreatedUserId = 5
    /// };
    /// </example>
    /// 
    [Table("DocumentProcessRelationship", Schema = "dbo")]
    public partial class DocumentProcessRelationshipBio
    {
        /// <summary>
        /// Gets or sets the ID of the associated document.
        /// </summary>
        /// <example>1</example>
        public int DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the associated process.
        /// </summary>
        /// <example>10</example>
        public int ProcessId { get; set; }

        /// <summary>
        /// Indicates whether the document is mandatory for the process.
        /// </summary>
        /// <example>true</example>
        public bool IsMandatory { get; set; } = false;

        /// <summary>
        /// Optional migration reference ID.
        /// </summary>
        /// <example>1005</example>
        public int? MigrationId { get; set; }

        /// <summary>
        /// Indicates if this relationship has been soft-deleted.
        /// </summary>
        /// <example>false</example>
        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Date when the record was marked as deleted.
        /// </summary>
        /// <example>2024-01-01T00:00:00Z</example>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// ID of the user who marked the record as deleted.
        /// </summary>
        /// <example>42</example>
        public int? DeletedUserId { get; set; }

        /// <summary>
        /// Date when the record was created.
        /// </summary>
        /// <example>2023-11-15T10:30:00Z</example>
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// ID of the user who created the record.
        /// </summary>
        /// <example>5</example>
        public int CreatedUserId { get; set; } = -1;

        /// <summary>
        /// Date when the record was last modified.
        /// </summary>
        /// <example>2024-05-12T12:00:00Z</example>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// ID of the user who last modified the record.
        /// </summary>
        /// <example>7</example>
        public int? ModifiedUserId { get; set; }

        /// <summary>
        /// Optional status ID associated with the relationship.
        /// </summary>
        /// <example>3</example>
        public int? StatusId { get; set; } 

        /// <summary>
        /// Collection of citizen identity documents linked to this document-process relationship.
        /// </summary>
        public virtual ICollection<CitizenIdentityDocument?> CitizenIdentityDocuments { get; set; } = new List<CitizenIdentityDocument>();

        /// <summary>
        /// Gets or sets the associated document.
        /// </summary>
        public virtual Document? Document { get; set; } = null!;
    }
}
