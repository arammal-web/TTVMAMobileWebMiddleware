using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("OperationType" )]
public partial class OperationTypeABP
{
    /// <summary>
    /// Unique identifier for the operation type
    /// </summary>
    [Key]
    public int Id { get; set; } 

    /// <summary>
    /// Description of the operation type in English
    /// </summary>
    [StringLength(50)]
    public string DescriptionEN { get; set; } = null!;

    /// <summary>
    /// Description of the operation type in Arabic
    /// </summary>
    [StringLength(50)]
    public string DescriptionAR { get; set; } = null!;

    /// <summary>
    /// Description of the operation type in French
    /// </summary>
    [StringLength(50)]
    public string DescriptionFR { get; set; } = null!;

    /// <summary>
    /// Timestamp indicating when the record was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("OperationType")]
    public virtual ICollection<OperationRequestABP> OperationRequests { get; set; } = new List<OperationRequestABP>();
}
