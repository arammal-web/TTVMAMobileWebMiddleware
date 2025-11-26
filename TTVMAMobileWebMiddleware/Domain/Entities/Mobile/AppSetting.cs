using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

public partial class AppSetting
{
    [Key]
    public int ID { get; set; }

    public int? UserId { get; set; }

    [StringLength(50)]
    public string ParamCode { get; set; } = null!;

    [StringLength(500)]
    public string ParamValue { get; set; } = null!;

    [StringLength(500)]
    public string? ParamValue1 { get; set; }

    [StringLength(500)]
    public string? ParamValue2 { get; set; }

    [StringLength(500)]
    public string? ParamValue3 { get; set; }

    [StringLength(500)]
    public string? ParamValue4 { get; set; }

    [StringLength(250)]
    public string? ParamDesc { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string? PrivalgeAccess { get; set; }
}
