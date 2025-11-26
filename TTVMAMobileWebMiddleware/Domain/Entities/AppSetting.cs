using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ABP.Domain.Entities;

/// <summary>
/// Represents application-wide settings and user-specific configuration parameters.
/// </summary>
public partial class AppSetting
{
    /// <summary>
    /// Unique identifier for the application setting.
    /// </summary>
    /// <example>101</example>
    [Key]
    public int ID { get; set; }

    /// <summary>
    /// Optional reference to a user for user-specific settings.
    /// </summary>
    /// <example>3</example>
    public int? UserId { get; set; }

    /// <summary>
    /// Code identifying the setting parameter.
    /// </summary>
    /// <example>SMS_TEMPLATE</example>
    [StringLength(50)]
    public string ParamCode { get; set; } = null!;

    /// <summary>
    /// Primary value for the setting.
    /// </summary>
    /// <example>Your OTP code is {0}</example>
    [StringLength(500)]
    public string ParamValue { get; set; } = null!;

    /// <summary>
    /// Additional parameter value 1.
    /// </summary>
    /// <example>+961</example>
    [StringLength(500)]
    public string? ParamValue1 { get; set; }

    /// <summary>
    /// Additional parameter value 2.
    /// </summary>
    /// <example>Support</example>
    [StringLength(500)]
    public string? ParamValue2 { get; set; }

    /// <summary>
    /// Additional parameter value 3.
    /// </summary>
    /// <example>Active</example>
    [StringLength(500)]
    public string? ParamValue3 { get; set; }

    /// <summary>
    /// Additional parameter value 4.
    /// </summary>
    /// <example>10</example>
    [StringLength(500)]
    public string? ParamValue4 { get; set; }

    /// <summary>
    /// Description for the setting parameter.
    /// </summary>
    /// <example>SMS template for OTP</example>
    [StringLength(250)]
    public string? ParamDesc { get; set; }

    /// <summary>
    /// Optional access privilege key for secured settings.
    /// </summary>
    /// <example>AdminOnly</example>
    [StringLength(50)]
    [Unicode(false)]
    public string? PrivalgeAccess { get; set; }
}
