using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TTVMAMobileWebMiddleware.Application.Common;
using TTVMAMobileWebMiddleware.Application.DTOs;
using TTVMAMobileWebMiddleware.Application.Interfaces;
using TTVMAMobileWebMiddleware.Application.Interfaces.Mobile;
using TTVMAMobileWebMiddleware.Domain.Entities;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;
using TTVMAMobileWebMiddleware.Domain.Enums;
using ApplicationStatus = TTVMAMobileWebMiddleware.Domain.Enums.ApplicationStatus;
using AppointmentStatus = TTVMAMobileWebMiddleware.Domain.Enums.AppointmentStatus;
using AppointmentPurpose = TTVMAMobileWebMiddleware.Domain.Enums.AppointmentPurpose;
using RequestStatuses = TTVMAMobileWebMiddleware.Domain.Enums.RequestStatuses;

namespace TTVMAMobileWebMiddleware.Application.Services;

/// <summary>
/// Service implementation for appointment management operations using Agendum entity
/// </summary>
public class AppointmentService : IAppointmentService
{
    private readonly MOBDbContext _context;
    private readonly ILogger<AppointmentService> _logger;
    private readonly INotificationService _notificationService;
    private const int HoldDurationMinutes = 5;
    private readonly Dictionary<int, string> _processDescriptionCache = new();

    // Mapping AppointmentPurpose to AgendaTypeId (these should be configured in database)
    // IMPORTANT: Update these IDs to match your actual AgendaType table values
    // You can query: SELECT Id, DescriptionEN FROM AgendaType WHERE DescriptionEN IN ('Enrollment', 'Production', 'Pickup')
    private readonly Dictionary<AppointmentPurpose, int> _purposeToAgendaTypeMap = new()
    {
        { AppointmentPurpose.Enrollment, 1 }, // TODO: Configure actual AgendaType ID for Enrollment
        { AppointmentPurpose.Production, 2 },  // TODO: Configure actual AgendaType ID for Production
        { AppointmentPurpose.Pickup, 3 }      // TODO: Configure actual AgendaType ID for Pickup
    };

    // Mapping AppointmentStatus to AgendaStatus.StatusId (these should be configured in database)
    // IMPORTANT: Update these IDs to match your actual AgendaStatus table values
    // You can query: SELECT Id, DescriptionEN FROM AgendaStatus WHERE DescriptionEN IN ('Pending', 'Confirmed', 'Proposed', 'Cancelled', 'NoShow', 'Done')
    private readonly Dictionary<AppointmentStatus, int> _statusToAgendaStatusMap = new()
    {
        { AppointmentStatus.Pending, 1 },    // TODO: Configure actual AgendaStatus ID for Pending
        { AppointmentStatus.Confirmed, 2 },  // TODO: Configure actual AgendaStatus ID for Confirmed
        { AppointmentStatus.Rescheduled, 3 },   // TODO: Configure actual AgendaStatus ID for Proposed
        { AppointmentStatus.Cancelled, 4 },  // TODO: Configure actual AgendaStatus ID for Cancelled
        { AppointmentStatus.NoShow, 5 },     // TODO: Configure actual AgendaStatus ID for NoShow
        { AppointmentStatus.Completed, 6 }        // TODO: Configure actual AgendaStatus ID for Done
    };

    public AppointmentService(
        MOBDbContext context,
        INotificationService notificationService,
        ILogger<AppointmentService> logger)
    {
        _context = context;
        _notificationService = notificationService;
        _logger = logger;
    }

    /// <summary>
    /// Searches for available appointment slots
    /// </summary>
    public async Task<AppointmentAvailabilitySearchResponse> SearchAvailabilityAsync(
        AppointmentAvailabilitySearchRequest request,
        CancellationToken ct = default)
    {
        try
        {

            // Validate application and receipt
            var application = _context.Applications
                 .Include(a => a.Citizen)
                .Where(a => a.Id == request.ApplicationOnlineId && (!a.IsDeleted.HasValue || a.IsDeleted == false)).FirstOrDefault();

            if (application == null)
            {
                var ex = new Exception($"Application {request.ApplicationOnlineId} not found");
                ex.HelpLink = "application_not_found";
                throw ex;
            }

            // Check if receipt is paid
            var receipt = await _context.Set<ReceiptMOB>()
                .FirstOrDefaultAsync(r => r.ApplicationId == request.ApplicationOnlineId && r.IsDeleted == false, ct);

            if (receipt == null || !receipt.IsPaid)
            {
                var ex = new Exception("Receipt must be paid before booking appointments");
                ex.HelpLink = "receipt_payment_required";
                throw ex;
            }

            // Check application status
            if (application.ApplicationApprovalStatusId == (int)ApplicationStatus.Pending)
            {
                var ex = new Exception($"Application status must not  be Pending. Current status: {application.ApplicationApprovalStatusId}");
                ex.HelpLink = "application_status_not_pending";
                throw ex;
            }

            // Get AgendaTypeId for the purpose
            var agendaTypeId = GetAgendaTypeId(request.Purpose);

            // Check for existing active appointment for this purpose
            var existingAppointment = await _context.Agenda
                .FirstOrDefaultAsync(a =>
                    a.ApplicationId == request.ApplicationOnlineId &&
                    a.AgendaTypeId == agendaTypeId &&
                    (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                    (a.StatusId == GetAgendaStatusId(AppointmentStatus.Pending) || a.StatusId == GetAgendaStatusId(AppointmentStatus.NoShow) ||
                     a.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) ||
                     a.StatusId == GetAgendaStatusId(AppointmentStatus.Rescheduled)), ct);

            if (existingAppointment != null)
            {
                var ex = new Exception($"An active appointment already exists for this application and purpose");
                ex.HelpLink = "appointment_duplicate_for_purpose";
                throw ex;
            }

            // Get structure/branch information (StructureId maps to BranchId in Agendum)
            var structure = await _context.Structures
                .FirstOrDefaultAsync(s => s.Id == request.StructureId && !s.IsDeleted.HasValue || s.IsDeleted == false, ct);

            if (structure == null)
            {
                var ex = new Exception($"Structure {request.StructureId} not found");
                ex.HelpLink = "structure_not_found";
                throw ex;
            }

            // Load availability from calendars (placeholder - in real system, this would query a calendar service)
            var availableSlots = await LoadAvailabilityFromCalendarsAsync(
                request.StructureId,
                request.From,
                request.To,
                request.DurationMin,
                ct);

            return new AppointmentAvailabilitySearchResponse
            {
                ApplicationId = request.ApplicationOnlineId,
                Purpose = request.Purpose,
                StructureId = request.StructureId,
                StructureName = structure.Name,
                AvailableSlots = availableSlots
            };
        }
        catch (Exception ex)
        {
            var exception = new Exception($"Error searching appointment availability for application {request?.ApplicationOnlineId}");
            exception.HelpLink = "appointment_search_availability_error";
            throw exception;
        }
    }

    /// <summary>
    /// Holds an appointment slot temporarily (5 minutes) to prevent race conditions
    /// </summary>
    public async Task<AppointmentHoldResponse> HoldSlotAsync(
        AppointmentHoldRequest request,
        CancellationToken ct = default)
    {
        // Validate application and receipt
        await ValidateAppointmentPrerequisitesAsync(request.ApplicationOnlineId, request.Purpose, ct);

        // Check if slot is still available
        var requestDate = request.AppointmentDate.Date;
        var requestStartTime = request.StartTime;
        var conflictingAppointment = await _context.Agenda
            .FirstOrDefaultAsync(a =>
                a.BranchId == request.StructureId &&
                a.AppointmentDate.HasValue &&
                a.AppointmentDate.Value.Date == requestDate &&
                a.StartTime.HasValue &&
                a.StartTime.Value == requestStartTime &&
                (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                (a.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) ||
                 (a.StatusId == GetAgendaStatusId(AppointmentStatus.Pending) || a.StatusId == GetAgendaStatusId(AppointmentStatus.NoShow) &&
                  a.Comments != null && a.Comments.Contains("HOLD_TOKEN:"))), ct);

        if (conflictingAppointment != null)
        {
            // Check if hold is expired
            if (conflictingAppointment.Comments != null && conflictingAppointment.Comments.Contains("HOLD_TOKEN:"))
            {
                var holdExpiryStr = conflictingAppointment.Comments.Split("HOLD_EXPIRY:")[1]?.Split("|")[0];
                if (DateTime.TryParse(holdExpiryStr, out var expiry) && expiry > DateTime.UtcNow)
                {
                    return new AppointmentHoldResponse
                    {
                        Success = false,
                        Message = "Slot is no longer available"
                    };
                }
            }
            else
            {
                return new AppointmentHoldResponse
                {
                    Success = false,
                    Message = "Slot is no longer available"
                };
            }
        }

        // Generate hold token
        var holdToken = Guid.NewGuid().ToString();
        var holdExpiryTime = DateTime.UtcNow.AddMinutes(HoldDurationMinutes);

        // Get application to get CitizenId
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationOnlineId && !a.IsDeleted.HasValue || a.IsDeleted == false, ct);

        if (application == null)
        {
            var ex = new Exception($"Application {request.ApplicationOnlineId} not found");
            ex.HelpLink = "application_not_found";
            throw ex;
        }

        // Create temporary hold record using Agendum
        var holdAppointment = new Agendum
        {
            CitizenId = application.OwnerId,
            AgendaTypeId = GetAgendaTypeId(request.Purpose),
            ApplicationId = request.ApplicationOnlineId,
            AppointmentDate = request.AppointmentDate,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            BranchId = request.StructureId,
            StatusId = GetAgendaStatusId(AppointmentStatus.Pending),
            Subject = request.Room != null || request.Station != null
                ? $"Room: {request.Room ?? "N/A"}, Station: {request.Station ?? "N/A"}"
                : null,
            Comments = $"HOLD_TOKEN:{holdToken}|HOLD_EXPIRY:{holdExpiryTime:O}",
            CreatedDate = DateTime.UtcNow,
            IsDeleted = false
        };

        _context.Agenda.Add(holdAppointment);
        await _context.SaveChangesAsync(ct);

        return new AppointmentHoldResponse
        {
            HoldToken = holdToken,
            HoldExpiryTime = holdExpiryTime,
            Success = true,
            Message = "Slot held successfully"
        };
    }

    /// <summary>
    /// Books an appointment (creates with Status=Pending, routes to officer for confirmation)
    /// </summary>
    public async Task<AppointmentBookResponse> BookAppointmentAsync(
        AppointmentBookRequest request,
        CancellationToken ct = default)
    {
        // Validate application and receipt
        await ValidateAppointmentPrerequisitesAsync(request.ApplicationOnlineId, request.Purpose, ct);

        // Get application to get CitizenId
        var application = await _context.Applications
            .Include(a => a.Citizen)
            .FirstOrDefaultAsync(a => a.Id == request.ApplicationOnlineId && !a.IsDeleted.HasValue || a.IsDeleted == false, ct);

        if (application == null)
        {
            var ex = new Exception($"Application {request.ApplicationOnlineId} not found");
            ex.HelpLink = "application_not_found";
            throw ex;
        }

        // If hold token provided, validate it
        Agendum? holdAppointment = null;
        if (!string.IsNullOrEmpty(request.HoldToken))
        {
            holdAppointment = await _context.Agenda
                .FirstOrDefaultAsync(a =>
                    a.Comments != null &&
                    a.Comments.Contains($"HOLD_TOKEN:{request.HoldToken}") &&
                    a.ApplicationId == request.ApplicationOnlineId &&
                    a.AgendaTypeId == GetAgendaTypeId(request.Purpose) &&
                    (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

            if (holdAppointment != null)
            {
                // Check expiry
                var holdExpiryStr = holdAppointment.Comments?.Split("HOLD_EXPIRY:")[1]?.Split("|")[0];
                if (DateTime.TryParse(holdExpiryStr, out var expiry) && expiry <= DateTime.UtcNow)
                {
                    holdAppointment = null; // Expired
                }
            }

            if (holdAppointment == null)
            {
                var ex = new Exception("Invalid or expired hold token");
                ex.HelpLink = "hold_token_invalid_or_expired";
                throw ex;
            }
        }

        // Check for existing active appointment
        var holdAppointmentId = holdAppointment?.Id ?? 0;
        var existingAppointment = await _context.Agenda
            .FirstOrDefaultAsync(a =>
                a.ApplicationId == request.ApplicationOnlineId &&
                a.AgendaTypeId == GetAgendaTypeId(request.Purpose) &&
                (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                a.Id != holdAppointmentId &&
                (a.StatusId == GetAgendaStatusId(AppointmentStatus.Pending) ||
                 a.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) ||
                 a.StatusId == GetAgendaStatusId(AppointmentStatus.Rescheduled)), ct);

        if (existingAppointment != null)
        {
            var ex = new Exception("An active appointment already exists for this application and purpose");
            ex.HelpLink = "appointment_duplicate_for_purpose";
            throw ex;
        }

        // Check for slot conflicts
        var requestDate = request.AppointmentDate.Date;
        var requestStartTime = request.StartTime;
        var conflictingAppointment = await _context.Agenda
            .FirstOrDefaultAsync(a =>
                a.BranchId == request.StructureId &&
                a.AppointmentDate.HasValue &&
                a.AppointmentDate.Value.Date == requestDate &&
                a.StartTime.HasValue &&
                a.StartTime.Value == requestStartTime &&
                (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                a.Id != holdAppointmentId &&
                (a.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) ||
                 (a.StatusId == GetAgendaStatusId(AppointmentStatus.Pending) &&
                  a.Comments != null && a.Comments.Contains("HOLD_TOKEN:"))), ct);

        if (conflictingAppointment != null)
        {
            var ex = new Exception("Selected slot is no longer available");
            ex.HelpLink = "appointment_slot_unavailable";
            throw ex;
        }

        Agendum appointment;
        if (holdAppointment != null)
        {
            // Update existing hold appointment
            holdAppointment.Comments = request.Notes; // Clear hold token, set notes
            holdAppointment.Note = request.Notes;
            holdAppointment.StatusDate = DateTime.UtcNow;
            appointment = holdAppointment;
        }
        else
        {
            // Create new appointment
            appointment = new Agendum
            {
                CitizenId = application.OwnerId,
                AgendaTypeId = GetAgendaTypeId(request.Purpose),
                ApplicationId = request.ApplicationOnlineId,
                AppointmentDate = request.AppointmentDate,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                BranchId = request.StructureId,
                StatusId = GetAgendaStatusId(AppointmentStatus.Pending),
                StatusDate = DateTime.UtcNow,
                Subject = request.Room != null || request.Station != null
                    ? $"Room: {request.Room ?? "N/A"}, Station: {request.Station ?? "N/A"}"
                    : $"Appointment for {request.Purpose}",
                Note = request.Notes,
                Comments = request.Notes,
                CitizenFullName = application.Citizen != null
                    ? $"{application.Citizen.FirstName} {application.Citizen.LastName}"
                    : null,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Agenda.Add(appointment);
        }

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Appointment {AppointmentId} booked for application {ApplicationId}, purpose {Purpose}",
            appointment.Id, request.ApplicationOnlineId, request.Purpose);

        return new AppointmentBookResponse
        {
            AppointmentId = appointment.Id,
            ApplicationId = appointment.ApplicationId ?? string.Empty,
            Purpose = request.Purpose,
            Status = AppointmentStatus.Pending,
            AppointmentDate = appointment.AppointmentDate ?? DateTime.MinValue,
            StartTime = appointment.StartTime ?? DateTime.MinValue,
            EndTime = appointment.EndTime ?? DateTime.MinValue,
            Room = ExtractRoomFromSubject(appointment.Subject),
            Station = ExtractStationFromSubject(appointment.Subject),
            Success = true,
            Message = "Appointment booked successfully. Pending officer confirmation."
        };
    }

    /// <summary>
    /// Confirms an appointment (officer action) - sets Status=Confirmed and marks requests Ready
    /// </summary>
    public async Task<AppointmentConfirmResponse> ConfirmAppointmentAsync(
        AppointmentConfirmRequest request,
        int officerUserId,
        CancellationToken ct = default)
    {
        var appointment = await _context.Agenda
            .Include(a => a.Application)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId && (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

        if (appointment == null)
        {
            var ex = new Exception($"Appointment {request.AppointmentId} not found");
            ex.HelpLink = "appointment_not_found";
            throw ex;
        }

        if (appointment.StatusId != GetAgendaStatusId(AppointmentStatus.Pending))
        {
            var ex = new Exception($"Appointment status must not be Pending. Current status ID: {appointment.StatusId}");
            ex.HelpLink = "appointment_status_invalid_for_confirm";
            throw ex;
        }

        // Update appointment status
        appointment.StatusId = GetAgendaStatusId(AppointmentStatus.Confirmed);
        appointment.StatusModifyUser = officerUserId;
        appointment.StatusDate = DateTime.UtcNow;
        appointment.Note = request.Notes;
        appointment.Comments = request.Notes;
        appointment.ModifiedDate = DateTime.UtcNow;
        appointment.ModifiedUserId = officerUserId;

        // Mark related OperationRequests as Ready/Approved based on purpose
        if (appointment.ApplicationId != null)
        {
            var purpose = GetPurposeFromAgendaType(appointment.AgendaTypeId);
            await MarkOperationRequestsReadyAsync(appointment.ApplicationId, purpose, ct);
        }

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Appointment {AppointmentId} confirmed by officer {OfficerUserId}",
            appointment.Id, officerUserId);

        // Create notification for status update (Confirmed)
        if (appointment.Application != null)
        {
            await _context.Entry(appointment.Application)
                .Reference(a => a.Citizen)
                .LoadAsync(ct);

            if (appointment.Application.Citizen != null && appointment.Application.Citizen.UserId > 0)
            {
                var appointmentDateStr = appointment.AppointmentDate?.ToString("yyyy-MM-dd") ?? "N/A";
                var startTimeStr = appointment.StartTime?.ToString("HH:mm") ?? "N/A";
                await CreateNotificationAsync(
                    appointment.Application.Citizen.UserId,
                    "Appointment Confirmed",
                    $"Your appointment for {GetPurposeFromAgendaType(appointment.AgendaTypeId)} has been confirmed. Date: {appointmentDateStr}, Time: {startTimeStr}.",
                    ct);
            }
        }

        return new AppointmentConfirmResponse
        {
            AppointmentId = appointment.Id,
            Success = true,
            Message = "Appointment confirmed successfully"
        };
    }

    /// <summary>
    /// Proposes alternative appointment slots (officer action) - sets Status=Proposed
    /// </summary>
    public async Task<AppointmentProposeResponse> ProposeAlternativesAsync(
        AppointmentProposeRequest request,
        int officerUserId,
        CancellationToken ct = default)
    {
        var appointment = await _context.Agenda
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId && (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

        if (appointment == null)
        {
            var ex = new Exception($"Appointment {request.AppointmentId} not found");
            ex.HelpLink = "appointment_not_found";
            throw ex;
        }

        if (appointment.StatusId != GetAgendaStatusId(AppointmentStatus.Pending))
        {
            var ex = new Exception($"Appointment status must be Pending. Current status ID: {appointment.StatusId}");
            ex.HelpLink = "appointment_status_must_be_pending";
            throw ex;
        }

        // Update original appointment status to Proposed
        appointment.StatusId = GetAgendaStatusId(AppointmentStatus.Cancelled);
        appointment.Comments = request.Message;
        appointment.Note = request.Message;
        appointment.ModifiedDate = DateTime.UtcNow;
        appointment.ModifiedUserId = officerUserId;

        // Create proposed alternative appointments
        foreach (var slot in request.ProposedSlots)
        {
            var proposedAppointment = new Agendum
            {
                CitizenId = appointment.CitizenId,
                AgendaTypeId = appointment.AgendaTypeId,
                ApplicationId = appointment.ApplicationId,
                AppointmentDate = slot.AppointmentDate,
                StartTime = slot.StartTime,
                EndTime = slot.StartTime.AddMinutes(20),
                BranchId = appointment.BranchId,
                StatusId = GetAgendaStatusId(AppointmentStatus.Confirmed),
                StatusDate = DateTime.UtcNow,
                Subject = appointment.Subject,
                Note = request.Message,
                Comments = request.Message,
                CitizenFullName = appointment.CitizenFullName,
                CreatedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Agenda.Add(proposedAppointment);
        }

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Appointment {AppointmentId} - {Count} alternatives proposed by officer {OfficerUserId}",
            appointment.Id, request.ProposedSlots.Count, officerUserId);

        // Create notification for status update (Alternatives Proposed)
        if (!string.IsNullOrEmpty(appointment.ApplicationId))
        {
            var application = await _context.Applications
                .Include(a => a.Citizen)
                .FirstOrDefaultAsync(a => a.Id == appointment.ApplicationId, ct);

            if (application?.Citizen != null && application.Citizen.UserId > 0)
            {
                var slotsInfo = string.Join(", ", request.ProposedSlots.Select(s => $"{s.AppointmentDate:yyyy-MM-dd} at {s.StartTime:HH:mm}"));
                await CreateNotificationAsync(
                    application.Citizen.UserId,
                    "Alternative Appointment Slots Proposed",
                    $"Alternative appointment slots have been proposed for your {GetPurposeFromAgendaType(appointment.AgendaTypeId)} appointment: {slotsInfo}.",
                    ct);
            }
        }

        return new AppointmentProposeResponse
        {
            AppointmentId = appointment.Id,
            Success = true,
            Message = "Alternative slots proposed successfully",
            ProposedSlotsCount = request.ProposedSlots.Count
        };
    }

    /// <summary>
    /// Gets appointment details by ID
    /// </summary>
    public async Task<AppointmentResponse?> GetAppointmentByIdAsync(
        int appointmentId,
        CancellationToken ct = default)
    {
        var appointment = await _context.Agenda
            .Include(a => a.Application)
                .ThenInclude(app => app.Citizen)
            .Include(a => a.Application)
                .ThenInclude(app => app.ApplicationType)
            .Include(a => a.AgendaType)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

        if (appointment == null)
        {
            return null;
        }

        return MapToResponse(appointment);
    }
    public async Task<List<AppointmentResponse>?> GetAppointments(int? status = null, CancellationToken ct = default)
    {
        // Normalize status input 

        var query = _context.Agenda
            .AsNoTracking()
            .Include(a => a.Application)
                .ThenInclude(app => app.Citizen)
            .Include(a => a.Application)
                .ThenInclude(app => app.ApplicationType)
            .Include(a => a.AgendaType)
            .Include(a => a.Status)
            .Where(x => ((x.StatusId == status) || status == null) && (x.IsDeleted == false || x.IsDeleted == null));


        var appointment = await query.ToListAsync(ct);

        if (appointment == null || !appointment.Any())
        {
            return new List<AppointmentResponse>();
        }

        List<AppointmentResponse> MappedAppointments = new List<AppointmentResponse>();
        foreach (var item in appointment)
        {
            MappedAppointments.Add(MapToResponse(item));
        }
        return MappedAppointments;
    }

    /// <summary>
    /// Gets all appointments for an application
    /// </summary>
    public async Task<List<AppointmentResponse>> GetAppointmentsByApplicationIdAsync(
        string applicationId,
        CancellationToken ct = default)
    {
        var appointments = await _context.Agenda
            .Include(a => a.Application)
                .ThenInclude(app => app.Citizen)
            .Include(a => a.Application)
                .ThenInclude(app => app.ApplicationType)
            .Include(a => a.AgendaType)
            .Where(a => a.ApplicationId == applicationId && (!a.IsDeleted.HasValue || a.IsDeleted == false))
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync(ct);

        return appointments.Select(MapToResponse).ToList();
    }

    /// <summary>
    /// Marks an appointment as completed (on-site operator action)
    /// </summary>
    public async Task<bool> MarkAppointmentDoneAsync(
        int appointmentId,
        int operatorUserId,
        CancellationToken ct = default)
    {
        var appointment = await _context.Agenda
            .Include(a => a.Application)
            .FirstOrDefaultAsync(a => a.Id == appointmentId && (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

        if (appointment == null)
        {
            var ex = new Exception($"Appointment {appointmentId} not found");
            ex.HelpLink = "appointment_not_found";
            throw ex;
        }

        if (appointment.StatusId != GetAgendaStatusId(AppointmentStatus.Confirmed))
        {
            var ex = new Exception($"Appointment must be Confirmed before marking as Done. Current status ID: {appointment.StatusId}");
            ex.HelpLink = "appointment_status_not_confirmed_for_done";
            throw ex;
        }

        appointment.StatusId = GetAgendaStatusId(AppointmentStatus.Completed);
        appointment.StatusModifyUser = operatorUserId;
        appointment.StatusDate = DateTime.UtcNow;
        appointment.ModifiedDate = DateTime.UtcNow;
        appointment.ModifiedUserId = operatorUserId;

        await _context.SaveChangesAsync(ct);

        // Create notification for status update (Completed)
        if (appointment.Application != null)
        {
            await _context.Entry(appointment.Application)
                .Reference(a => a.Citizen)
                .LoadAsync(ct);

            if (appointment.Application.Citizen != null && appointment.Application.Citizen.UserId > 0)
            {
                await CreateNotificationAsync(
                    appointment.Application.Citizen.UserId,
                    "Appointment Completed",
                    $"Your appointment for {GetPurposeFromAgendaType(appointment.AgendaTypeId)} has been marked as completed.",
                    ct);
            }
        }

        // Check if all required appointments for the application are done
        if (appointment.ApplicationId != null)
        {
            await CheckAndFinalizeApplicationAsync(appointment.ApplicationId, ct);
        }

        _logger.LogInformation("Appointment {AppointmentId} marked as Done by operator {OperatorUserId}",
            appointmentId, operatorUserId);

        return true;
    }

    /// <summary>
    /// Cancels an appointment
    /// </summary>
    public async Task<bool> CancelAppointmentAsync(
        int appointmentId,
        int userId,
        string? reason,
        CancellationToken ct = default)
    {
        var appointment = await _context.Agenda
            .FirstOrDefaultAsync(a => a.Id == appointmentId && (!a.IsDeleted.HasValue || a.IsDeleted == false), ct);

        if (appointment == null)
        {
            var ex = new Exception($"Appointment {appointmentId} not found");
            ex.HelpLink = "appointment_not_found";
            throw ex;
        }

        appointment.StatusId = GetAgendaStatusId(AppointmentStatus.Cancelled);
        appointment.Note = reason;
        appointment.Comments = reason;
        appointment.StatusModifyUser = userId;
        appointment.StatusDate = DateTime.UtcNow;
        appointment.ModifiedDate = DateTime.UtcNow;
        appointment.ModifiedUserId = userId;

        await _context.SaveChangesAsync(ct);

        // Create notification for status update (Cancelled)
        if (!string.IsNullOrEmpty(appointment.ApplicationId))
        {
            var application = await _context.Applications
                .Include(a => a.Citizen)
                .FirstOrDefaultAsync(a => a.Id == appointment.ApplicationId, ct);

            if (application?.Citizen != null && application.Citizen.UserId > 0)
            {
                var reasonText = !string.IsNullOrEmpty(reason) ? $" Reason: {reason}" : "";
                await CreateNotificationAsync(
                    application.Citizen.UserId,
                    "Appointment Cancelled",
                    $"Your appointment for {GetPurposeFromAgendaType(appointment.AgendaTypeId)} has been cancelled.{reasonText}",
                    ct);
            }
        }

        _logger.LogInformation("Appointment {AppointmentId} cancelled by user {UserId}", appointmentId, userId);

        return true;
    }

    #region Private Helper Methods

    private int GetAgendaTypeId(AppointmentPurpose purpose)
    {
        if (!_purposeToAgendaTypeMap.TryGetValue(purpose, out var agendaTypeId))
        {
            var ex = new Exception($"No AgendaType mapping found for purpose: {purpose}");
            ex.HelpLink = "agenda_type_mapping_missing";
            throw ex;
        }
        return agendaTypeId;
    }

    private AppointmentPurpose GetPurposeFromAgendaType(int agendaTypeId)
    {
        var mapping = _purposeToAgendaTypeMap.FirstOrDefault(x => x.Value == agendaTypeId);
        if (mapping.Key == default && mapping.Value == 0)
        {
            var ex = new Exception($"No AppointmentPurpose mapping found for AgendaTypeId: {agendaTypeId}");
            ex.HelpLink = "appointment_purpose_mapping_missing";
            throw ex;
        }
        return mapping.Key;
    }

    private int GetAgendaStatusId(AppointmentStatus status)
    {
        if (!_statusToAgendaStatusMap.TryGetValue(status, out var agendaStatusId))
        {
            var ex = new Exception($"No AgendaStatus mapping found for status: {status}");
            ex.HelpLink = "agenda_status_mapping_missing";
            throw ex;
        }
        return agendaStatusId;
    }

    private AppointmentStatus GetStatusFromAgendaStatus(int agendaStatusId)
    {
        var mapping = _statusToAgendaStatusMap.FirstOrDefault(x => x.Value == agendaStatusId);
        if (mapping.Key == default && mapping.Value == 0)
        {
            var ex = new Exception($"No AppointmentStatus mapping found for AgendaStatusId: {agendaStatusId}");
            ex.HelpLink = "appointment_status_mapping_missing";
            throw ex;
        }
        return mapping.Key;
    }

    private async Task ValidateAppointmentPrerequisitesAsync(
        string applicationId,
        AppointmentPurpose purpose,
        CancellationToken ct)
    {
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.Id == applicationId && !a.IsDeleted.HasValue || a.IsDeleted == false, ct);

        if (application == null)
        {
            var ex = new Exception($"Application {applicationId} not found");
            ex.HelpLink = "application_not_found";
            throw ex;
        }

        // Check if receipt is paid
        var receipt = await _context.Set<ReceiptMOB>()
            .FirstOrDefaultAsync(r => r.ApplicationId == applicationId && r.IsDeleted == false, ct);


        ///todo : add condition and condition groups for appointment 
        //if (receipt == null || !receipt.IsPaid)
        //{
        //   var ex = new Exception("Receipt must be paid before booking appointments");
        //}

        // Check application status
        if (application.StatusId != (int)ApplicationStatus.Pending)
        {
            var ex = new Exception($"Application status must be Pending. Current status: {application.StatusId}");
            ex.HelpLink = "application_status_must_be_pending";
            throw ex;
        }
    }

    private async Task<List<AppointmentAvailabilitySlotResponse>> LoadAvailabilityFromCalendarsAsync(
        int structureId,
        DateTime from,
        DateTime to,
        int durationMin,
        CancellationToken ct)
    {
        // TODO: Integrate with actual calendar service
        // For now, return mock availability slots
        // In production, this would query a calendar/availability service

        var slots = new List<AppointmentAvailabilitySlotResponse>();
        var currentDate = from.Date;

        while (currentDate <= to.Date)
        {
            // Generate sample slots (9 AM to 5 PM, every hour)
            for (int hour = 9; hour < 17; hour++)
            {
                var startTime = currentDate.AddHours(hour);
                var endTime = startTime.AddMinutes(durationMin);

                // Capture values for LINQ expression
                var checkDate = currentDate;
                var checkStartTime = startTime;

                // Check if slot conflicts with existing appointments
                var hasConflict = await _context.Agenda
                    .AnyAsync(a =>
                        a.BranchId == structureId &&
                        a.AppointmentDate.HasValue &&
                        a.AppointmentDate.Value.Date == checkDate &&
                        a.StartTime.HasValue &&
                        a.StartTime.Value == checkStartTime &&
                        (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                        (a.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) || a.StatusId == GetAgendaStatusId(AppointmentStatus.Pending) || a.StatusId == GetAgendaStatusId(AppointmentStatus.NoShow)), ct);

                if (!hasConflict)
                {
                    slots.Add(new AppointmentAvailabilitySlotResponse
                    {
                        AppointmentDate = currentDate,
                        StartTime = startTime,
                        EndTime = endTime,
                        AvailableCapacity = 1,
                        IsAvailable = true
                    });
                }
            }

            currentDate = currentDate.AddDays(1);
        }

        return slots;
    }

    private async Task MarkOperationRequestsReadyAsync(
        string applicationId,
        AppointmentPurpose purpose,
        CancellationToken ct)
    {
        // Find OperationRequests for this application based on purpose
        var operationRequests = await _context.Set<OperationRequest>()
            .Include(or => or.OperationType)
            .Where(or => or.ApplicationId == applicationId && !or.IsDeleted)
            .ToListAsync(ct);

        // Map purpose to operation type (this would need to be configured based on your system)
        // For now, we'll update all pending requests
        foreach (var request in operationRequests)
        {
            if (request.RequestStatusId == (int)RequestStatuses.Active ||
                request.RequestStatusId == (int)RequestStatuses.ReadyToSchedule)
            {
                // Mark as Ready (assuming ReadyToSchedule = 111 is the "Ready" status)
                request.RequestStatusId = (int)RequestStatuses.ReadyToSchedule;
                request.RequestStatusDate = DateTime.UtcNow;
                request.ModifiedDate = DateTime.UtcNow;
            }
        }

        await _context.SaveChangesAsync(ct);
    }

    private async Task CheckAndFinalizeApplicationAsync(
        string applicationId,
        CancellationToken ct)
    {
        var application = await _context.Applications
            .Include(a => a.Agenda)
            .FirstOrDefaultAsync(a => a.Id == applicationId && !a.IsDeleted.HasValue || a.IsDeleted == false, ct);

        if (application == null)
        {
            return;
        }

        // Get all required appointments for this application (Enrollment, Production, Pickup)
        var requiredAgendaTypes = _purposeToAgendaTypeMap.Values.ToList();
        var requiredAppointments = application.Agenda
            .Where(a => (!a.IsDeleted.HasValue || a.IsDeleted == false) &&
                   requiredAgendaTypes.Contains(a.AgendaTypeId))
            .ToList();

        // Check if all required appointments are Done
        var doneStatusId = GetAgendaStatusId(AppointmentStatus.Completed);
        var allDone = requiredAppointments.All(a => a.StatusId == doneStatusId);

        if (allDone && requiredAppointments.Any())
        {
            // Set ApplicationStatus = Completed
            application.StatusId = (int)ApplicationStatus.Completed;
            application.ApplicationStatusDate = DateTime.UtcNow;
            application.ModifiedDate = DateTime.UtcNow;

            // If not already, set ApplicationApprovalStatus = Committed
            if (application.ApplicationApprovalStatusId != (int)ApplicationStatus.Committed)
            {
                application.ApplicationApprovalStatusId = (int)ApplicationStatus.Committed;
                application.ApplicationApprovalStatusDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync(ct);

            // Create notification for application finalization
            await _context.Entry(application)
                .Reference(a => a.Citizen)
                .LoadAsync(ct);

            if (application.Citizen != null && application.Citizen.UserId > 0)
            {
                await CreateNotificationAsync(
                    application.Citizen.UserId,
                    "Application Completed",
                    $"Your application {application.ApplicationNumber ?? applicationId} has been completed. All required appointments have been finished.",
                    ct);
            }

            _logger.LogInformation("Application {ApplicationId} finalized - all appointments completed",
                applicationId);
        }
    }

    /// <summary>
    /// Creates a notification for a user and saves it to the mobile database
    /// </summary>
    private async Task CreateNotificationAsync(long userId, string title, string message, CancellationToken ct = default)
    {
        var notification = new Notification
        {
            UserId = userId,
            Title = title,
            Message = message,
            IsRead = false,
            CreatedDate = DateTime.UtcNow
        };
        await _notificationService.AddAsync(notification, ct);
    }

    private AppointmentResponse MapToResponse(Agendum appointment)
    {
        var structureName = string.Empty;
        var application = appointment.Application
            ?? (!string.IsNullOrEmpty(appointment.ApplicationId)
                ? _context.Applications
                    .AsNoTracking()
                    .Include(a => a.Citizen)
                    .Include(a => a.ApplicationType)
                    .FirstOrDefault(a => a.Id == appointment.ApplicationId)
                : null);

        var agendaType = appointment.AgendaType
            ?? _context.AgendaTypes.AsNoTracking().FirstOrDefault(a => a.Id == appointment.AgendaTypeId);

        var agendaStatus = appointment.Status
            ?? (appointment.StatusId.HasValue
                ? _context.AgendaStatuses.AsNoTracking().FirstOrDefault(s => s.Id == appointment.StatusId.Value)
                : null);

        if (appointment.BranchId.HasValue)
        {
            var structure = _context.Structures.AsNoTracking().FirstOrDefault(s => s.Id == appointment.BranchId.Value);
            structureName = structure?.Name ?? string.Empty;
        }

        var applicationNumber = application?.ApplicationNumber
            ?? appointment.ApplicationId
            ?? string.Empty;

        var citizenFullName = application?.Citizen != null
            ? $"{application.Citizen.FirstName} {application.Citizen.LastName}".Trim()
            : null;

        var applicantName = !string.IsNullOrWhiteSpace(citizenFullName)
            ? citizenFullName
            : !string.IsNullOrWhiteSpace(appointment.CitizenFullName)
                ? appointment.CitizenFullName
                : application?.OwnerFullName ?? string.Empty;

        var applicationType = application?.ProcessId is int processId
            ? GetProcessDescription(processId)
            : string.Empty;

        if (string.IsNullOrWhiteSpace(applicationType))
        {
            applicationType = application?.ApplicationType?.DescriptionEN
                ?? application?.ApplicationTypeId
                ?? string.Empty;
        }

        var appointmentType = agendaType?.DescriptionEN
            ?? GetPurposeFromAgendaType(appointment.AgendaTypeId).ToString();

        return new AppointmentResponse
        {
            Id = appointment.Id,
            ApplicationId = appointment.ApplicationId ?? string.Empty,
            ApplicationNumber = applicationNumber,
            ApplicantName = applicantName,
            ApplicationType = applicationType,
            AppointmentType = appointmentType,
            Purpose = GetPurposeFromAgendaType(appointment.AgendaTypeId),
            Status = appointment.StatusId.HasValue ? GetStatusFromAgendaStatus(appointment.StatusId.Value) : AppointmentStatus.Pending,
            StatusId = appointment.StatusId,
            StatusEn = agendaStatus?.DescriptionEN,
            StatusAr = agendaStatus?.DescriptionAR,
            StatusFr = agendaStatus?.DescriptionFR,
            StructureId = appointment.BranchId ?? 0,
            StructureName = structureName,
            AppointmentDate = appointment.AppointmentDate ?? DateTime.MinValue,
            StartTime = appointment.StartTime ?? DateTime.MinValue,
            EndTime = appointment.EndTime ?? DateTime.MinValue,
            Room = ExtractRoomFromSubject(appointment.Subject),
            Station = ExtractStationFromSubject(appointment.Subject),
            Notes = appointment.Note,
            ProposalMessage = appointment.StatusId == GetAgendaStatusId(AppointmentStatus.Rescheduled) ? appointment.Comments : null,
            CreatedDate = appointment.CreatedDate,
            ConfirmedDate = appointment.StatusId == GetAgendaStatusId(AppointmentStatus.Confirmed) ? appointment.StatusDate : null,
            CompletedDate = appointment.StatusId == GetAgendaStatusId(AppointmentStatus.Completed) ? appointment.StatusDate : null
        };
    }

    private string? ExtractRoomFromSubject(string? subject)
    {
        if (string.IsNullOrEmpty(subject)) return null;
        var roomMatch = System.Text.RegularExpressions.Regex.Match(subject, @"Room:\s*([^,]+)");
        return roomMatch.Success ? roomMatch.Groups[1].Value.Trim() : null;
    }

    private string? ExtractStationFromSubject(string? subject)
    {
        if (string.IsNullOrEmpty(subject)) return null;
        var stationMatch = System.Text.RegularExpressions.Regex.Match(subject, @"Station:\s*([^,]+)");
        return stationMatch.Success ? stationMatch.Groups[1].Value.Trim() : null;
    }

    private string GetProcessDescription(int processId)
    {
        if (_processDescriptionCache.TryGetValue(processId, out var cachedDescription))
        {
            return cachedDescription;
        }

        var description = _context.Processes
            .Where(p => p.Id == processId)
            .Select(p => p.NameEn ?? p.NameAr ?? p.NameFr ?? string.Empty)
            .FirstOrDefault() ?? string.Empty;

        _processDescriptionCache[processId] = description;
        return description;
    }

    #endregion
}
