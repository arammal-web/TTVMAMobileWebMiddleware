namespace TTVMAMobileWebMiddleware.Domain.Enums;

/// <summary>
/// Status of online citizen validation
/// </summary>
public enum CitizenStatus
{
    PendingValidation = 1,
    Approved = 2,
    Rejected = 3
}

/// <summary>
/// Method used to link online and local citizens
/// </summary>
public enum LinkMethod
{
    NationalId = 1,
    Passport = 2,
    Registration = 3,
    Composite = 4,
    Manual = 5
}

/// <summary>
/// Document review status
/// </summary>
public enum DocumentReviewStatus
{
    Valid = 1,
    Invalid = 2,
    Missing = 3
}

 

 

/// <summary>
/// Receipt status
/// </summary>
public enum ReceiptStatus
{
    Unpaid = 1,
    Paid = 2,
    Settled = 3,
    Error = 4
}

/// <summary>
/// Appointment purpose
/// </summary>
public enum AppointmentPurpose
{
    Enrollment = 1,
    Production = 2,
    Pickup = 3
}

/// <summary>
/// Appointment status
/// </summary>
public enum AppointmentStatus
{
    /// <summary>
    /// Appointment is pending confirmation.
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Appointment has been confirmed and scheduled.
    /// </summary>
    Confirmed = 2,

    /// <summary>
    /// Appointment has been rescheduled to a new date/time.
    /// </summary>
    Rescheduled = 3,

    /// <summary>
    /// Appointment has been completed successfully.
    /// </summary>
    Completed = 4,

    /// <summary>
    /// Appointment has been cancelled by the citizen or system.
    /// </summary>
    Cancelled = 5,

    /// <summary>
    /// Appointment is on hold pending further action.
    /// </summary>
    OnHold = 6,

    /// <summary>
    /// Appointment was not attended (no-show).
    /// </summary>
    NoShow = 7,

    /// <summary>
    /// Appointment is in progress.
    /// </summary>
    InProgress = 8,

    /// <summary>
    /// Appointment has been approved by the system.
    /// </summary>
    Approved = 9,

    /// <summary>
    /// Appointment has been rejected.
    /// </summary>
    Rejected = 10
}

/// <summary>
/// Audit event type
/// </summary>
public enum AuditEventType
{
    Search = 1,
    Link = 2,
    Create = 3,
    Approve = 4,
    Reject = 5,
    Payment = 6,
    Appointment = 7
}

