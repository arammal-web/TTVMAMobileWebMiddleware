using System;
using System.Collections.Generic;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

public partial class CitizenIdentityDocument
{
    /// <summary>
    /// Primary key of the CitizenIdentityDocument table
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Reference to the citizen associated with this identity document
    /// </summary>
    public int CitizenId { get; set; }

    /// <summary>
    /// Reference to the document type
    /// </summary>
    public int DocumentId { get; set; }

    /// <summary>
    /// Reference to the process
    /// </summary>
    public int ProcessId { get; set; }

    /// <summary>
    /// The binary data for the citizen identity document
    /// </summary>
    public byte[]? IdentityDocument { get; set; }

    /// <summary>
    /// The hash value for the identity document, used for data integrity
    /// </summary>
    public string? IdentityDocumentHash { get; set; }

    /// <summary>
    /// Text data or metadata related to the identity document
    /// </summary>
    public string? IdentityDocumentData { get; set; }

    /// <summary>
    /// Description or title for the identity document
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Reference to the structure related to the identity document
    /// </summary>
    public int? StructureId { get; set; }

    /// <summary>
    /// Additional notes or comments about the identity document
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Indicates if the identity document is marked as deleted
    /// </summary>
    public bool? IsDeleted { get; set; }

    /// <summary>
    /// Date the identity document was marked as deleted
    /// </summary>
    public DateTime? DeletedDate { get; set; }

    /// <summary>
    /// ID of the user who marked the identity document as deleted
    /// </summary>
    public int? DeletedUserId { get; set; }

    /// <summary>
    /// Date the identity document was created
    /// </summary>
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// ID of the user who created the identity document record
    /// </summary>
    public int? CreatedUserId { get; set; }

    /// <summary>
    /// Date the identity document was last modified
    /// </summary>
    public DateTime? ModifiedDate { get; set; }

    /// <summary>
    /// ID of the user who last modified the identity document record
    /// </summary>
    public int? ModifiedUserId { get; set; }

   /// <summary>
   /// The file extension of the identity document
   /// </summary>
    public virtual string? DocFileExt { get; set; } = null!;

    /// <summary>
    /// The citizen associated with this identity document
    /// </summary>
    public virtual CitizenABP? Citizen { get; set; } = null!;

    /// <summary>
    /// The document type associated with this identity document
    /// </summary>
}
