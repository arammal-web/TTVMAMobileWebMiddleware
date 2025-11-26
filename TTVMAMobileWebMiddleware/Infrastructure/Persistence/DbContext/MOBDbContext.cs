using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Domain.Entities;

public partial class MOBDbContext : DbContext
{
    public MOBDbContext()
    {
    }

    public MOBDbContext(DbContextOptions<MOBDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgendaStatus> AgendaStatuses { get; set; }

    public virtual DbSet<AgendaType> AgendaTypes { get; set; }

    public virtual DbSet<Agendum> Agenda { get; set; }

    public virtual DbSet<AppSetting> AppSettings { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<ApplicationMob> Applications { get; set; }

    //public virtual DbSet<ApplicationApprovalStatus> ApplicationApprovalStatuses { get; set; }

    public virtual DbSet<ApplicationFeeLog> ApplicationFeeLogs { get; set; }

    public virtual DbSet<ApplicationLog> ApplicationLogs { get; set; }

    public virtual DbSet<ApplicationProcess> ApplicationProcesses { get; set; }

    public virtual DbSet<ApplicationProcessCheckList> ApplicationProcessCheckLists { get; set; }

    public virtual DbSet<ApplicationProcessCheckListDocFile> ApplicationProcessCheckListDocFiles { get; set; }

    public virtual DbSet<ApplicationProcessCondition> ApplicationProcessConditions { get; set; }

    public virtual DbSet<ApplicationProcessDocument> ApplicationProcessDocuments { get; set; }

    public virtual DbSet<ApplicationProcessFee> ApplicationProcessFees { get; set; }

    public virtual DbSet<ApplicationRequestedPlatesInfo> ApplicationRequestedPlatesInfos { get; set; }

    public virtual DbSet<ApplicationStatus> ApplicationStatuses { get; set; }

    public virtual DbSet<ApplicationType> ApplicationTypes { get; set; }

    public virtual DbSet<BloodType> BloodTypes { get; set; }

    public virtual DbSet<CheckList> CheckLists { get; set; }

    public virtual DbSet<CheckListGroup> CheckListGroups { get; set; }

    public virtual DbSet<Citizen> Citizens { get; set; }

    public virtual DbSet<CitizenAddress> CitizenAddresses { get; set; }

    public virtual DbSet<CitizenFaceImage> CitizenFaceImages { get; set; }

    public virtual DbSet<CitizenFingerprint> CitizenFingerprints { get; set; }

    public virtual DbSet<CitizenIdentityDocument> CitizenIdentityDocuments { get; set; }

    public virtual DbSet<CitizenSignature> CitizenSignatures { get; set; }

    public virtual DbSet<CitizenLink> CitizenLinks { get; set; }

    public virtual DbSet<DocumentReview> DocumentReviews { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Condition> Conditions { get; set; }

    public virtual DbSet<ConditionGroup> ConditionGroups { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentGroup> DocumentGroups { get; set; }

    public virtual DbSet<DocumentProcessRelationship> DocumentProcessRelationships { get; set; }

    public virtual DbSet<DocumentRequestLog> DocumentRequestLogs { get; set; }

    public virtual DbSet<DomainMob> Domains { get; set; }

    public virtual DbSet<DrivingTestRequest> DrivingTestRequests { get; set; }

    public virtual DbSet<ExemptionType> ExemptionTypes { get; set; }

    public virtual DbSet<ExemptionTypeDocRelationship> ExemptionTypeDocRelationships { get; set; }

    public virtual DbSet<Fee> Fees { get; set; }

    public virtual DbSet<FeeCategory> FeeCategories { get; set; }

    public virtual DbSet<FingerList> FingerLists { get; set; }

    public virtual DbSet<FingerPrintStatus> FingerPrintStatuses { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<LegacyProcessVariety> LegacyProcessVarieties { get; set; }

    public virtual DbSet<LoginAudit> LoginAudits { get; set; }

    public virtual DbSet<MaritalStatus> MaritalStatuses { get; set; }

    public virtual DbSet<Nationality> Nationalities { get; set; }

    public virtual DbSet<OperationRequest> OperationRequests { get; set; }

    public virtual DbSet<OperationType> OperationTypes { get; set; }

    public virtual DbSet<OtpRequest> OtpRequests { get; set; }

    public virtual DbSet<PasswordReset> PasswordResets { get; set; }

    public virtual DbSet<Prefecture> Prefectures { get; set; }

    public virtual DbSet<PrerequisiteProcessVariety> PrerequisiteProcessVarieties { get; set; }

    public virtual DbSet<Process> Processes { get; set; }

    public virtual DbSet<ProcessCheckList> ProcessCheckLists { get; set; }

    public virtual DbSet<ProcessExemptionFee> ProcessExemptionFees { get; set; }

    public virtual DbSet<ProcessFee> ProcessFees { get; set; }

    public virtual DbSet<ProcessType> ProcessTypes { get; set; }

    public virtual DbSet<ProcessVariety> ProcessVarieties { get; set; }

    public virtual DbSet<ProcessVarietyType> ProcessVarietyTypes { get; set; }

    public virtual DbSet<ProcessesFeeLog> ProcessesFeeLogs { get; set; }

    public virtual DbSet<Profession> Professions { get; set; }

    public virtual DbSet<ReceiptMOB> Receipts { get; set; }

    public virtual DbSet<ReceiptDetailMOB> ReceiptDetails { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<RequestStatus> RequestStatuses { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<VarietyBusinessProcess> VarietyBusinessProcesses { get; set; }

    public virtual DbSet<Village> Villages { get; set; }
    public virtual DbSet<Structure> Structures { get; set; }

    public virtual DbSet<DrivingExam> DrivingExams { get; set; }
    public virtual DbSet<DrivingLicense> DrivingLicenses { get; set; }
    public virtual DbSet<DrivingLicenseDetail> DrivingLicenseDetails   { get; set; } 
    public virtual DbSet<SequenceNumber> SequenceNumber { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=TTVMAMobileConnection");


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgendaStatus>(entity =>
        {
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<AgendaType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Type");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Agendum>(entity =>
        {
            entity.Property(e => e.HasReminder).HasDefaultValue(false);
            entity.Property(e => e.IsLate).HasDefaultValue(false);

            entity.HasOne(d => d.AgendaType).WithMany(p => p.Agenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agenda_Type");

            entity.HasOne(d => d.Application).WithMany(p => p.Agenda).HasConstraintName("FK_Agenda_Application");

            entity.HasOne(d => d.Status).WithMany(p => p.Agenda).HasConstraintName("FK_Agenda_Status");
        });

        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.HasKey(e => e.ID).HasFillFactor(70);
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AppUser__1788CC4CE5107094");

            entity.HasIndex(e => e.EmailNormalized, "UX_AppUser_Email")
                .IsUnique()
                .HasFilter("([EmailNormalized] IS NOT NULL)");

            entity.HasIndex(e => e.PhoneNormalized, "UX_AppUser_Phone")
                .IsUnique()
                .HasFilter("([PhoneNormalized] IS NOT NULL)");

            entity.Property(e => e.Id).HasComment("Primary key for the application user account.");
            entity.Property(e => e.CreatedAtUtc).HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.EmailNormalized).HasComment("Normalized email used for uniqueness checks (uppercased, trimmed).");
            entity.Property(e => e.PhoneNormalized).HasComment("Normalized phone number used for uniqueness checks (E.164 format).");
        });

        modelBuilder.Entity<ApplicationMob>(entity =>
        {
            entity.ToTable("Application", tb =>
            {
                tb.HasTrigger("trg_Application_Audit_Delete");
                tb.HasTrigger("trg_Application_Audit_InsertUpdate");
            });

            entity.HasKey(x => x.Id);

            entity.Property(e => e.Id).HasComment("Unique identifier for the application");
            entity.Property(e => e.ApplicationApprovalStatusId).HasComment("Reference to the application approval status");
            entity.Property(e => e.ApplicationNumber).HasComment("Application number");
            entity.Property(e => e.ApplicationTypeId).HasComment("Reference to the application type");
            entity.Property(e => e.BranchId).HasComment("Reference to the branch");
            entity.Property(e => e.CarUniqueNumber).HasComment("Reference to the unique car identifier");
            entity.Property(e => e.Comments).HasComment("Comments related to the application");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())").HasComment("The date when the record was created");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false).HasComment("Logical deletion flag");

            // Relationships


            entity.HasOne(d => d.Citizen)
                .WithMany(p => p.Applications)
                .HasForeignKey(d => d.OwnerId);

            entity.HasOne(d => d.ApplicationApprovalStatus)
                .WithMany(p => p.Applications)
                .HasForeignKey(d => d.ApplicationApprovalStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_ApplicationApprovalStatus");

            entity.HasOne(d => d.Status)
                .WithMany(p => p.ApplicationsWithStatus)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_Application_Status");
        });

        modelBuilder.Entity<ApplicationLog>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.OperationTimestamp).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ApplicationProcess>(entity =>
        {
            entity.HasKey(e => new { e.ApplicationId, e.ProcessId, e.BPVarietyId }).HasName("PK_ApplicationSecondaryProcess");

            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
            entity.Property(e => e.ProcessId).HasComment("Reference to the business process related to the application");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
            entity.Property(e => e.DrivingLicenseId).HasComment("Reference to the associated Driving Licenses table in case of driving license renewal process");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Record creation date");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted).HasComment("Logical deletion flag for soft delete");
            entity.Property(e => e.ModifiedDate).HasComment("Last modification date of the record");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.Notes).HasComment("Additional notes for the secondary process of the application");

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcess_Application");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ApplicationProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcess_VarietyBusinessProcess");
        });
        modelBuilder.Entity<DrivingLicense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DrivingL__3214EC07AE9AF2AD");

            entity.Property(e => e.Id).HasComment("Primary key of the DrivingLicense table");
            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
            entity.Property(e => e.BlockingAuthority).HasComment("Authority responsible for blocking the license");
            entity.Property(e => e.BlockingReason).HasComment("Reason why the license was blocked");
            entity.Property(e => e.CardSerialNumber).HasComment("Updated after the card is printed");
            entity.Property(e => e.CitizenId).HasComment("Citizen ID associated with the driving license");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DataHash).HasComment("The hash of the printed data");
            entity.Property(e => e.Description).HasComment("Optional description of the driving license");
            entity.Property(e => e.DrivingLicenseNumber).HasComment("Unique driving license number");
            entity.Property(e => e.DrivingLicenseStatusDate).HasComment("Date of the driving license status");
            entity.Property(e => e.DrivingLicenseStatusId).HasComment("Status ID of the driving license");
            entity.Property(e => e.ExpiryDate).HasComment("Date the driving license will expire");
            entity.Property(e => e.IsBlocked)
                .HasDefaultValue(false)
                .HasComment("Indicates if the driving license is currently blocked");
            entity.Property(e => e.IsByPasssTest).HasDefaultValue(false);
            entity.Property(e => e.IsOldDrivingLicense)
                .HasDefaultValue(false)
                .HasComment("Indicates if the license is an older version");
            entity.Property(e => e.IsPrinted).HasDefaultValue(false);
            entity.Property(e => e.IssuanceDate).HasComment("Date the driving license was issued");
            entity.Property(e => e.IssueAuthorityId).HasComment("Authority that issued the driving license");
            entity.Property(e => e.LicenseCodes).HasComment("License codes associated with the driving license");
            entity.Property(e => e.LicensePrintedData).HasComment("JSON Data printed on the card");
            entity.Property(e => e.NumberOfPoints).HasComment("Number of points remaining on the driving license");
            entity.Property(e => e.OldDrivingLicenseImage).HasComment("Image of the old driving license");
            entity.Property(e => e.SAI).HasComment("SAI code for the driving license");

            //entity.HasOne(d => d.Application).WithMany(p => p.DrivingLicenses)
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("FK_DrivingLicense_Application");
        });
        modelBuilder.Entity<ApplicationProcessCheckList>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.ApplicationProcessCheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCheckList_ApplicationProcess");

            entity.HasOne(d => d.ProcessCheckList).WithMany(p => p.ApplicationProcessCheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCheckList_ProcessCheckList");
        });

        modelBuilder.Entity<ApplicationProcessCheckListDocFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_BPConditionCheckListDocFile");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsSignedDocFile).HasDefaultValue(false);

            entity.HasOne(d => d.ApplicationProcessCheckList).WithMany(p => p.ApplicationProcessCheckListDocFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCheckListDocFile_ApplicationProcessCheckList");
        });

        modelBuilder.Entity<ApplicationProcessCondition>(entity =>
        {
            entity.HasKey(e => new { e.ApplicationId, e.ProcessId, e.BPVarietyId, e.ConditionId }).HasName("PK_Conditions");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ApplicationProcessConditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCondition_VarietyBusinessProcess");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.ApplicationProcessConditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCondition_ApplicationProcess");
        });

        modelBuilder.Entity<ApplicationProcessDocument>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Document).WithMany(p => p.ApplicationProcessDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessDocument_Documents");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.ApplicationProcessDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessDocument_ApplicationProcess");
        });

        modelBuilder.Entity<ApplicationProcessFee>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DeletedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.FeeCategory).WithMany(p => p.ApplicationProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessFee_FeeCategory");

            entity.HasOne(d => d.Fee).WithMany(p => p.ApplicationProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessFee_Fees");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.ApplicationProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessFee_ApplicationProcess");

            entity.HasOne(d => d.ProcessFee).WithMany(p => p.ApplicationProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessFee_ProcessFee");
        });

        modelBuilder.Entity<ApplicationRequestedPlatesInfo>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationRequestedPlatesInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationRequestedPlatesInfo_Application");

            entity.HasOne(d => d.Status).WithMany(p => p.ApplicationRequestedPlatesInfos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationRequestedPlatesInfo_Status");
        });

        modelBuilder.Entity<ApplicationStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC076863C771");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<ApplicationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07DEB519BC");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<BloodType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BloodTyp__3214EC07B9E798C3");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<CheckList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConditionCheckList");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.CheckListGroup).WithMany(p => p.CheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conditions_ConditionCheckListGroups");
        });

        modelBuilder.Entity<CheckListGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConditionCheckListGroups");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
        });

        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasOne(d => d.Blood).WithMany(p => p.Citizens).HasConstraintName("FK_Citizen_BloodType");

            entity.HasOne(d => d.Gender).WithMany(p => p.Citizens).HasConstraintName("FK_Citizen_Gender");

            entity.HasOne(d => d.MaritalStatus).WithMany(p => p.Citizens).HasConstraintName("FK_Citizen_MaritalStatus");

            entity.HasOne(d => d.Nationality).WithMany(p => p.CitizenNationalities).HasConstraintName("FK_Citizen_Nationality");

            entity.HasOne(d => d.Profession).WithMany(p => p.CitizenProfessions).HasConstraintName("FK_Citizen_Profession");

            entity.HasOne(d => d.SpouseNationality).WithMany(p => p.CitizenSpouseNationalities).HasConstraintName("FK_Citizen_Nationality1");

            entity.HasOne(d => d.SpouseProfession).WithMany(p => p.CitizenSpouseProfessions).HasConstraintName("FK_Citizen_Profession1");
        });

        modelBuilder.Entity<CitizenAddress>(entity =>
        {
            entity.HasOne(d => d.Citizen).WithMany(p => p.CitizenAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenAddress_Citizen");

            entity.HasOne(d => d.City).WithMany(p => p.CitizenAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenAddress_City");

            entity.HasOne(d => d.Country).WithMany(p => p.CitizenAddresses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenAddress_Country");

            entity.HasOne(d => d.Prefecture).WithMany(p => p.CitizenAddresses).HasConstraintName("FK_CitizenAddress_Prefecture");

            entity.HasOne(d => d.Region).WithMany(p => p.CitizenAddresses).HasConstraintName("FK_CitizenAddress_Region");

            entity.HasOne(d => d.Village).WithMany(p => p.CitizenAddresses).HasConstraintName("FK_CitizenAddress_Village");
        });

        modelBuilder.Entity<CitizenFaceImage>(entity =>
        {
            entity.HasOne(d => d.Citizen).WithMany(p => p.CitizenFaceImages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenFaceImage_Citizen");
        });

        modelBuilder.Entity<CitizenFingerprint>(entity =>
        {
            entity.HasOne(d => d.Citizen).WithMany(p => p.CitizenFingerprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenFingerprints_Citizen");

            entity.HasOne(d => d.Finger).WithMany(p => p.CitizenFingerprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenFingerprints_FingerList");

            entity.HasOne(d => d.FingerprintStatus).WithMany(p => p.CitizenFingerprints)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenFingerprints_FingerPrintStatus");
        });

        modelBuilder.Entity<CitizenIdentityDocument>(entity =>
        {
            entity.HasOne(d => d.Citizen).WithMany(p => p.CitizenIdentityDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenIdentityDocument_Citizen");

            entity.HasOne(d => d.Document).WithMany(p => p.CitizenIdentityDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenIdentityDocument_Documents");
        });

        modelBuilder.Entity<CitizenSignature>(entity =>
        {
            entity.HasOne(d => d.Citizen).WithMany(p => p.CitizenSignatures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CitizenSignature_Citizen");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__City");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Country).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Country");

            entity.HasOne(d => d.Prefecture).WithMany(p => p.Cities)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_City_Prefecture");
        });

        modelBuilder.Entity<Condition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Conditions_01");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsStopProcess).HasDefaultValue(true);

            entity.HasOne(d => d.ConditionGroup).WithMany(p => p.Conditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Condition_ConditionGroups");
        });

        modelBuilder.Entity<ConditionGroup>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country__3214EC0733E651BA");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.Group).WithMany(p => p.Documents).HasConstraintName("FK_Documents_DocumentGroups");

            entity.HasOne(d => d.Status).WithMany(p => p.Documents).HasConstraintName("FK_Documents_Status");
        });

        modelBuilder.Entity<DocumentGroup>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Choose).IsFixedLength();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.Status).WithMany(p => p.DocumentGroups).HasConstraintName("FK_DocumentGroups_Status");
        });

        modelBuilder.Entity<DocumentProcessRelationship>(entity =>
        {
            entity.HasKey(e => new { e.DocumentId, e.ProcessId, e.BPVarietyId }).HasName("PK_DocumentProcessRelationship_1");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
            entity.Property(e => e.IsRequired).HasDefaultValue(false);

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentProcessRelationships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProcessRelationship_Documents");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.DocumentProcessRelationships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProcessRelationship_VarietyBusinessProcess");
        });


        modelBuilder.Entity<DrivingTestRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DrivingT__3214EC07F7588E2D");

            entity.Property(e => e.CitizenId).HasDefaultValue(-1);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Application).WithMany(p => p.DrivingTestRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingTestRequest_Application");

            entity.HasOne(d => d.OperationType).WithMany(p => p.DrivingTestRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingTestRequest_OperationType");

            entity.HasOne(d => d.RequestStatus).WithMany(p => p.DrivingTestRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingTestRequest_RequestStatus");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.DrivingTestRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingTestRequest_ApplicationProcess");
        });

        modelBuilder.Entity<ExemptionType>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.Status).WithMany(p => p.ExemptionTypes).HasConstraintName("FK_ExemptionType_Status");
        });

        modelBuilder.Entity<ExemptionTypeDocRelationship>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.Document).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_Documents");

            entity.HasOne(d => d.ExemptionType).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_ExemptionType");

            entity.HasOne(d => d.Process).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_Process");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.FeeCategory).WithMany(p => p.Fees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_FeeCategory");

            entity.HasOne(d => d.Status).WithMany(p => p.Fees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_Status");
        });

        modelBuilder.Entity<FeeCategory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.Status).WithMany(p => p.FeeCategories).HasConstraintName("FK_FeeCategory_Status");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Gender__3214EC07B2304811");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<LegacyProcessVariety>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusId).HasDefaultValue(1);

            entity.HasOne(d => d.BPVariety).WithMany(p => p.LegacyProcessVarietyBPVarieties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegacyProcessVariety_ProcessVariety");

            entity.HasOne(d => d.BPVarietyLegacy).WithMany(p => p.LegacyProcessVarietyBPVarietyLegacies)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LegacyProcessVariety_ProcessVariety1");
        });

        modelBuilder.Entity<LoginAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoginAud__A17F23985FFC8833");

            entity.Property(e => e.Id).HasComment("Primary key for the login audit record.");
            entity.Property(e => e.ClientAgent).HasComment("Client agent string (browser, OS, app version).");
            entity.Property(e => e.ClientIp).HasComment("Client IP address from which the login was attempted.");
            entity.Property(e => e.Detail).HasComment("Optional details for the login attempt, such as error messages or location info.");
            entity.Property(e => e.EventAtUtc)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasComment("UTC date/time when the event occurred.");
            entity.Property(e => e.EventType).HasComment("Type of login event (e.g., login_success, login_failed, locked_out).");
            entity.Property(e => e.UserId).HasComment("Foreign key to AppUser.Id representing the user who attempted login.");

            entity.HasOne(d => d.User).WithMany(p => p.LoginAudits)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_LoginAudit_AppUser");
        });

        modelBuilder.Entity<MaritalStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MaritalS__3214EC07DFBDC104");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<Nationality>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__National__3214EC07A2BDB25A");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<OperationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operatio__3214EC07705BB187");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Application).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_Application");

            entity.HasOne(d => d.OperationType).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_OperationType");

            entity.HasOne(d => d.RequestStatus).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_RequestStatus");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_ApplicationProcess");
        });

        modelBuilder.Entity<OperationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operatio__3214EC07348A5FBE");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<OtpRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OtpReque__3143C4A3C2EDCE4C");

            entity.Property(e => e.Id).HasComment("Primary key for the OTP request record.");
            entity.Property(e => e.AttemptCount).HasComment("Number of verification attempts already made.");
            entity.Property(e => e.Channel).HasComment("Email or phone channel used to deliver the OTP.");
            entity.Property(e => e.CodeHash).HasComment("Hash of the one-time password code. Plain code is never stored.");
            entity.Property(e => e.ConsumedAtUtc).HasComment("Date/time the OTP was successfully used (null if unused).");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasComment("Timestamp when the OTP record was created.");
            entity.Property(e => e.ExpiresAtUtc).HasComment("Server-side expiry date/time for the OTP.");
            entity.Property(e => e.MaxAttempts)
                .HasDefaultValue((byte)5)
                .HasComment("Maximum number of allowed verification attempts.");
            entity.Property(e => e.Purpose).HasComment("Purpose of the OTP request (e.g., register, login).");
            entity.Property(e => e.RequestIp).HasComment("IP address of the request originator.");
            entity.Property(e => e.Target).HasComment("Target recipient (email or phone) for the OTP.");
            entity.Property(e => e.UserId).HasComment("Foreign key to AppUser.Id. NULL for OTPs issued before account creation.");

            entity.HasOne(d => d.User).WithMany(p => p.OtpRequests).HasConstraintName("FK_OtpRequest_AppUser");
        });

        modelBuilder.Entity<PasswordReset>(entity =>
        {
            entity.HasKey(e => e.ResetId).HasName("PK__Password__783CF04DA8FA9C59");

            entity.Property(e => e.ResetId).HasComment("Primary key for the password reset request record.");
            entity.Property(e => e.ConsumedAtUtc).HasComment("UTC datetime when the reset token was successfully used.");
            entity.Property(e => e.CreatedAtUtc)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasComment("UTC datetime when this password reset request was created.");
            entity.Property(e => e.ExpiresAtUtc).HasComment("UTC datetime when the reset token expires.");
            entity.Property(e => e.TokenHash).HasComment("Hash of the password reset token. Plaintext is never stored.");
            entity.Property(e => e.UserId).HasComment("Foreign key to AppUser.Id requesting the password reset.");

            entity.HasOne(d => d.User).WithMany(p => p.PasswordResets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PasswordReset_AppUser");
        });

        modelBuilder.Entity<Prefecture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Prefecture");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Country).WithMany(p => p.Prefectures)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Prefecture_Country");
        });

        modelBuilder.Entity<PrerequisiteProcessVariety>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.StatusId).HasDefaultValue(1);

            entity.HasOne(d => d.BPVariety).WithMany(p => p.PrerequisiteProcessVarietyBPVarieties)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrerequisiteProcessVariety_ProcessVariety");

            entity.HasOne(d => d.BPVarietyPrerequisite).WithMany(p => p.PrerequisiteProcessVarietyBPVarietyPrerequisites)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PrerequisiteProcessVariety_ProcessVariety1");
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Processes");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.EnableVariety).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
            entity.Property(e => e.Domain).HasDefaultValue("DLS");

            entity.HasOne(d => d.Status).WithMany(p => p.Processes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Processes_Status");

            entity.HasOne(d => d.Type).WithMany(p => p.Processes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Process_Type");
        });

        modelBuilder.Entity<ProcessCheckList>(entity =>
        {
            entity.HasKey(e => new { e.ChekListId, e.ProcessId, e.BPVarietyId }).HasName("PK_BPConditions");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ChekList).WithMany(p => p.ProcessCheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessCheckList_CheckList1");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ProcessCheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessCheckList_VarietyBusinessProcess");
        });

        modelBuilder.Entity<ProcessExemptionFee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ExemptionFee");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.BP).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExemptionFee_Process");

            entity.HasOne(d => d.BPVariety).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessExemptionFee_ProcessVariety");

            entity.HasOne(d => d.ExemptionType).WithMany(p => p.ProcessExemptionFees).HasConstraintName("FK_ExemptionFee_ExemptionTypes");

            entity.HasOne(d => d.Fee).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessExemptionFee_Fees");

            entity.HasOne(d => d.Status).WithMany(p => p.ProcessExemptionFees).HasConstraintName("FK_ExemptionFee_Status");
        });

        modelBuilder.Entity<ProcessFee>(entity =>
        {
            entity.HasKey(e => new { e.FeeId, e.ProcessId, e.BPVarietyId }).HasName("PK_ProcessesFee");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
            entity.Property(e => e.IsMandatory).HasDefaultValue(false);

            entity.HasOne(d => d.Fee).WithMany(p => p.ProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessesFee_Fees");

            entity.HasOne(d => d.Status).WithMany(p => p.ProcessFees).HasConstraintName("FK_ProcessesFee_Status");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessFee_VarietyBusinessProcess");
        });

        modelBuilder.Entity<ProcessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Type_01");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProcessVariety>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.ProcessVarietyType).WithMany(p => p.ProcessVarieties).HasConstraintName("FK_ProcessVariety_ProcessVarietyType");
        });

        modelBuilder.Entity<ProcessVarietyType>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);
        });

        modelBuilder.Entity<ProcessesFeeLog>(entity =>
        {
            entity.Property(e => e.OperationTimestamp).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Profession>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Professi__3214EC07EC88CD15");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        modelBuilder.Entity<ReceiptMOB>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Receipt__3214EC076B0C224B");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        });

        modelBuilder.Entity<ReceiptDetailMOB>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptD__3214EC070D404C4D");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");

        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RefreshT__658FEEEA1218E2FC");

            entity.Property(e => e.Id).HasComment("Primary key for the refresh token record.");
            entity.Property(e => e.DeviceFingerprint).HasComment("Unique fingerprint/identifier for the device.");
            entity.Property(e => e.DeviceName).HasComment("User-friendly device label from which this token was issued.");
            entity.Property(e => e.ExpiresAtUtc).HasComment("UTC date/time when the refresh token will expire.");
            entity.Property(e => e.IssuedAtUtc)
                .HasDefaultValueSql("(sysutcdatetime())")
                .HasComment("UTC date/time when the refresh token was created.");
            entity.Property(e => e.IssuedIp).HasComment("IP address from which the token was issued.");
            entity.Property(e => e.RevokedAtUtc).HasComment("UTC date/time when the refresh token was revoked.");
            entity.Property(e => e.RevokedReason).HasComment("Reason for refresh token revocation.");
            entity.Property(e => e.TokenHash).HasComment("Hashed refresh token (never store plaintext).");
            entity.Property(e => e.UserId).HasComment("Foreign key to AppUser.Id that owns this refresh token.");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefreshToken_AppUser");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Region");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Country).WithMany(p => p.Regions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Region_Country");

            entity.HasOne(d => d.Prefecture).WithMany(p => p.Regions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Region_Prefecture");
        });

        modelBuilder.Entity<RequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestS__3214EC076B56801C");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.Property(e => e.ID)
                .ValueGeneratedNever()
                .HasComment("Unique identifier for the status");
            entity.Property(e => e.StatusDesc).HasComment("Short description of the status in English");
            entity.Property(e => e.StatusDescAr).HasComment("Short description of the status in Arabic");
        });

        modelBuilder.Entity<VarietyBusinessProcess>(entity =>
        {
            entity.HasKey(e => new { e.ProcessId, e.BPVarietyId }).HasName("PK_Relationships");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(true);

            entity.HasOne(d => d.BPVariety).WithMany(p => p.VarietyBusinessProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VarietyBusinessProcess_ProcessVariety");

            entity.HasOne(d => d.Process).WithMany(p => p.VarietyBusinessProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VarietyBusinessProcess_Process");

            entity.HasOne(d => d.Status).WithMany(p => p.VarietyBusinessProcesses).HasConstraintName("FK_Relationships_Status");
        });

        modelBuilder.Entity<Village>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Village");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Country).WithMany(p => p.Villages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Village_Country");

            entity.HasOne(d => d.Prefecture).WithMany(p => p.Villages)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Village_Prefecture");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Notification");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notification_AppUser");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
