using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS;

[Table("RequestStatus", Schema = "DLS")]
public partial class RequestStatusABP
{
    /// <summary>
    /// Unique identifier for each request status record
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Description of the request status in English
    /// </summary>
    [StringLength(50)]
    public string DescriptionEN { get; set; } = null!;

    /// <summary>
    /// Description of the request status in Arabic
    /// </summary>
    [StringLength(50)]
    public string DescriptionAR { get; set; } = null!;

    /// <summary>
    /// Description of the request status in French
    /// </summary>
    [StringLength(50)]
    public string DescriptionFR { get; set; } = null!;

    /// <summary>
    /// Timestamp when the request status record was created
    /// </summary>
    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("RequestStatus")]
    public virtual ICollection<DrivingTestRequestABP> DrivingTestRequests { get; set; } = new List<DrivingTestRequestABP>();

    [InverseProperty("RequestStatus")]
    public virtual ICollection<OperationRequestABP> OperationRequests { get; set; } = new List<OperationRequestABP>();

  
}
