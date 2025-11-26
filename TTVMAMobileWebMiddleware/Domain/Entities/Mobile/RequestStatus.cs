using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("RequestStatus")]
public partial class RequestStatus
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string DescriptionEN { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionAR { get; set; } = null!;

    [StringLength(50)]
    public string DescriptionFR { get; set; } = null!;

    [Column(TypeName = "smalldatetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("RequestStatus")]
    public virtual ICollection<DrivingTestRequest> DrivingTestRequests { get; set; } = new List<DrivingTestRequest>();

    [InverseProperty("RequestStatus")]
    public virtual ICollection<OperationRequest> OperationRequests { get; set; } = new List<OperationRequest>();
}
