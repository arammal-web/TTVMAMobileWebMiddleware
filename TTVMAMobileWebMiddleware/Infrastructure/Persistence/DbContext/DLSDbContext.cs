using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Domain.Entities;

public partial class DLSDbContext : DbContext
{
    public DLSDbContext()
    {
    }

    public DLSDbContext(DbContextOptions<DLSDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Provides access to application records.
    /// </summary>

    /// <summary>
    /// AgendaStatus
    /// </summary>
    public virtual DbSet<AgendaStatus> AgendaStatuses { get; set; }


    /// <summary>
    /// Provides access to agenda type definitions.
    /// </summary>
    public virtual DbSet<AgendaType> AgendaTypes { get; set; }

    /// <summary>
    /// Provides access to all agenda appointments.
    /// </summary>
    public virtual DbSet<Agenda> Agenda { get; set; }

    /// <summary>
    /// Provides access to application records.
    /// </summary>
    public virtual DbSet<ApplicationDLS> Applications { get; set; }

    /// <summary>
    /// Provides access to application process records.
    /// </summary>
    public virtual DbSet<ApplicationProcess> ApplicationProcesses { get; set; }

    /// <summary>
    /// Provides access to checklist records associated with application processes.
    /// </summary>
    public virtual DbSet<ApplicationProcessCheckList> ApplicationProcessCheckLists { get; set; }

    /// <summary>
    /// Provides access to checklist document files linked to application processes.
    /// </summary>
    public virtual DbSet<ApplicationProcessCheckListDocFile> ApplicationProcessCheckListDocFiles { get; set; }

    /// <summary>
    /// Provides access to conditions linked to application processes.
    /// </summary>
    public virtual DbSet<ApplicationProcessCondition> ApplicationProcessConditions { get; set; }

    /// <summary>
    /// Provides access to documents submitted for application processes.
    /// </summary>
    public virtual DbSet<ApplicationProcessDocument> ApplicationProcessDocuments { get; set; }

    /// <summary>
    /// Provides access to fee records for application processes.
    /// </summary>
    public virtual DbSet<ApplicationProcessFee> ApplicationProcessFees { get; set; }

    /// <summary>
    /// Provides access to requested plate information in applications.
    /// </summary>
    public virtual DbSet<ApplicationRequestedPlatesInfo> ApplicationRequestedPlatesInfos { get; set; }


    /// <summary>
    /// Provides access to predefined checklist templates.
    /// </summary>
    public virtual DbSet<CheckList> CheckLists { get; set; }

    /// <summary>
    /// Provides access to checklist group definitions.
    /// </summary>
    public virtual DbSet<CheckListGroup> CheckListGroups { get; set; }

    /// <summary>
    /// Provides access to condition definitions.
    /// </summary>
    public virtual DbSet<Condition> Conditions { get; set; }

    /// <summary>
    /// Provides access to condition group definitions.
    /// </summary>
    public virtual DbSet<ConditionGroup> ConditionGroups { get; set; }

    /// <summary>
    /// Provides access to document definitions.
    /// </summary>
    public virtual DbSet<Document> Documents { get; set; }

    /// <summary>
    /// Provides access to document group definitions.
    /// </summary>
    public virtual DbSet<DocumentGroup> DocumentGroups { get; set; }

    /// <summary>
    /// Provides access to document-process relationship definitions.
    /// </summary>
    public virtual DbSet<DocumentProcessRelationship> DocumentProcessRelationships { get; set; }

    /// <summary>
    /// Provides access to driving license definitions.
    /// </summary>
    public virtual DbSet<DrivingLicenseABP> DrivingLicenses { get; set; }
    /// <summary>
    /// Provides access to driving license details. 
    /// </summary>
    public virtual DbSet<DrivingLicenseDetailABP> DrivingLicenseDetails { get; set; }
    /// <summary>
    /// Provides access to exemption type definitions.
    /// </summary>
    public virtual DbSet<ExemptionType> ExemptionTypes { get; set; }

    /// <summary>
    /// Provides access to exemption type-document relationships.
    /// </summary>
    public virtual DbSet<ExemptionTypeDocRelationship> ExemptionTypeDocRelationships { get; set; }

    /// <summary>
    /// Provides access to fee definitions.
    /// </summary>
    public virtual DbSet<Fee> Fees { get; set; }

    /// <summary>
    /// Provides access to fee category definitions.
    /// </summary>
    public virtual DbSet<FeeCategory> FeeCategories { get; set; }

    /// <summary>
    /// Provides access to legacy process variety mappings.
    /// </summary>
    public virtual DbSet<LegacyProcessVariety> LegacyProcessVarieties { get; set; }

    /// <summary>
    /// Provides access to prerequisite process-variety mappings.
    /// </summary>
    public virtual DbSet<PrerequisiteProcessVariety> PrerequisiteProcessVarieties { get; set; }

    /// <summary>
    /// Provides access to business process definitions.
    /// </summary>
    public virtual DbSet<Process> Processes { get; set; }

    /// <summary>
    /// Provides access to checklists associated with processes.
    /// </summary>
    public virtual DbSet<ProcessCheckList> ProcessCheckLists { get; set; }

    /// <summary>
    /// Provides access to fees linked to exemptions per process.
    /// </summary>
    public virtual DbSet<ProcessExemptionFee> ProcessExemptionFees { get; set; }

    /// <summary>
    /// Provides access to fees defined per process.
    /// </summary>
    public virtual DbSet<ProcessFee> ProcessFees { get; set; }

    /// <summary>
    /// Provides access to process type definitions.
    /// </summary>
    public virtual DbSet<ProcessType> ProcessTypes { get; set; }

    /// <summary>
    /// Provides access to process variety definitions.
    /// </summary>
    public virtual DbSet<ProcessVariety> ProcessVarieties { get; set; }

    /// <summary>
    /// Provides access to mappings between process varieties and their types.
    /// </summary>
    public virtual DbSet<ProcessVarietyType> ProcessVarietyTypes { get; set; }


    /// <summary>
    /// Provides access to payment receipts.
    /// </summary>
    public virtual DbSet<Receipt> Receipts { get; set; }

    /// <summary>
    /// Provides access to detailed receipt records.
    /// </summary>
    public virtual DbSet<ReceiptDetail> ReceiptDetails { get; set; }

    /// <summary>
    /// Provides access to business processes associated with varieties.
    /// </summary>
    public virtual DbSet<VarietyBusinessProcess> VarietyBusinessProcesses { get; set; }

    /// <summary>
    /// Provides access to domains.
    /// </summary>
    public virtual DbSet<ApplicationDomain> Domains { get; set; }

    /// <summary>
    /// Provides access to status definitions.
    /// </summary>
    public virtual DbSet<Status> Status { get; set; }

    /// <summary>
    /// Provides access to citizen records.
    /// </summary>
    public virtual DbSet<CitizenABP> Citizens { get; set; }

    /// <summary>
    /// Provides access to citizen addresses.
    /// </summary>
    public virtual DbSet<CitizenAddressABP> CitizenAddresses { get; set; }

    /// <summary>
    /// Provides access to driving test requests.
    /// </summary>
    public virtual DbSet<DrivingTestRequestABP> DrivingTestRequests { get; set; }

    public virtual DbSet<StructureABP> Structures { get; set; }

    public virtual DbSet<OperationRequestABP> OperationRequests { get; set; }

    public virtual DbSet<OperationTypeABP> OperationTypes { get; set; }

    public virtual DbSet<RequestStatusABP> RequestStatuses { get; set; }
    public virtual DbSet<AppSetting> AppSettings { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=TTVMAConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Condition>().HasQueryFilter(c => c.IsDeleted == false || c.IsDeleted == null);
        modelBuilder.Entity<ProcessCheckList>().HasQueryFilter(c => c.IsDeleted == false || c.IsDeleted == null);
        modelBuilder.Entity<DocumentProcessRelationship>().HasQueryFilter(c => c.IsDeleted == false || c.IsDeleted == null);
        modelBuilder.Entity<CheckList>().HasQueryFilter(c => (c.IsDeleted == false || c.IsDeleted == null) && c.Domain == "DLS");
        modelBuilder.Entity<CheckListGroup>().HasQueryFilter(cg => (cg.IsDeleted == false || cg.IsDeleted == null) && cg.Domain == "DLS");
        modelBuilder.Entity<ProcessFee>().HasQueryFilter(f => (f.IsDeleted == false || f.IsDeleted == null) && f.Domain == "DLS");
        modelBuilder.Entity<Process>().HasQueryFilter(p => (p.IsDeleted == false || p.IsDeleted == null) && p.Domain == "DLS");
        modelBuilder.Entity<ProcessVariety>().HasQueryFilter(v => (v.IsDeleted == false || v.IsDeleted == null) && v.Domain == "DLS");
        modelBuilder.Entity<ProcessVarietyType>().HasQueryFilter(vt => vt.IsDeleted == false || vt.IsDeleted == null);
        modelBuilder.Entity<ProcessType>().HasQueryFilter(pt => pt.IsDeleted == false || pt.IsDeleted == null);
        modelBuilder.Entity<Fee>().HasQueryFilter(f => (f.IsDeleted == false || f.IsDeleted == null) && f.Domain == "DLS");
        modelBuilder.Entity<FeeCategory>().HasQueryFilter(fc => (fc.IsDeleted == false || fc.IsDeleted == null) && fc.Domain == "DLS");
        modelBuilder.Entity<ExemptionType>().HasQueryFilter(et => et.IsDeleted == false || et.IsDeleted == null);
        modelBuilder.Entity<ExemptionTypeDocRelationship>().HasQueryFilter(etdr => etdr.IsDeleted == false || etdr.IsDeleted == null);
        modelBuilder.Entity<LegacyProcessVariety>().HasQueryFilter(lpv => lpv.IsDeleted == false || lpv.IsDeleted == null);
        modelBuilder.Entity<PrerequisiteProcessVariety>().HasQueryFilter(ppv => ppv.IsDeleted == false || ppv.IsDeleted == null);
        modelBuilder.Entity<VarietyBusinessProcess>().HasQueryFilter(vbp => vbp.IsDeleted == false || vbp.IsDeleted == null);
        modelBuilder.Entity<ReceiptDetail>().HasQueryFilter(rd => rd.IsDeleted == false || rd.IsDeleted == null);
        modelBuilder.Entity<Document>().HasQueryFilter(d => (d.IsDeleted == false || d.IsDeleted == null) && d.Domain == "DLS");
        modelBuilder.Entity<DocumentGroup>().HasQueryFilter(dg => (dg.IsDeleted == false || dg.IsDeleted == null) && dg.Domain == "DLS");
        modelBuilder.Entity<Agenda>().HasQueryFilter(a => a.IsDeleted == false || a.IsDeleted == null);
        modelBuilder.Entity<ProcessExemptionFee>().HasQueryFilter(pef => pef.IsDeleted == false || pef.IsDeleted == null);
        modelBuilder.Entity<ApplicationDLS>().HasQueryFilter(a => a.IsDeleted == false || a.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcess>().HasQueryFilter(ap => ap.IsDeleted == false || ap.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcessCheckList>().HasQueryFilter(apcl => apcl.IsDeleted == false || apcl.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcessCheckListDocFile>().HasQueryFilter(apcdf => apcdf.IsDeleted == false || apcdf.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcessCondition>().HasQueryFilter(apc => apc.IsDeleted == false || apc.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcessDocument>().HasQueryFilter(apd => apd.IsDeleted == false || apd.IsDeleted == null);
        modelBuilder.Entity<ApplicationProcessFee>().HasQueryFilter(apf => apf.IsDeleted == false || apf.IsDeleted == null);
        modelBuilder.Entity<ApplicationRequestedPlatesInfo>().HasQueryFilter(arpi => arpi.IsDeleted == false || arpi.IsDeleted == null);
        modelBuilder.Entity<DrivingTestRequestABP>().HasQueryFilter(dtr => dtr.IsDeleted == false || dtr.IsDeleted == null);
        modelBuilder.Entity<DrivingLicenseABP>().HasQueryFilter(dtr => (dtr.IsDeleted == false || dtr.IsDeleted == null));
        modelBuilder.Entity<CitizenABP>().HasQueryFilter(c => c.IsDeleted == false || c.IsDeleted == null);
        modelBuilder.Entity<CitizenAddressABP>().HasQueryFilter(ca => ca.IsDeleted == false || ca.IsDeleted == null);
        modelBuilder.Entity<DrivingLicenseDetailABP>().HasQueryFilter(dld => dld.IsDeleted == false || dld.IsDeleted == null);
        modelBuilder.Entity<DrivingTestRequestABP>().HasQueryFilter(de => de.IsDeleted == false || de.IsDeleted == null);
        modelBuilder.Entity<OperationRequestABP>().HasQueryFilter(de => de.IsDeleted == false || de.IsDeleted == null);
        modelBuilder.Entity<ConditionGroup>().HasQueryFilter(de => de.IsDeleted == false || de.IsDeleted == null);

        modelBuilder.Entity<StructureABP>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Structur__3214EC07327F2D42");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("Unique identifier for each structure record");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("The date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedDate).HasComment("The date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
            entity.Property(e => e.Name).HasComment("Name of the structure");
            entity.Property(e => e.ParentId).HasComment("Recursive reference to parent structure");


        });

        modelBuilder.Entity<AppSetting>(entity =>
        {
            entity.HasKey(e => e.ID).HasFillFactor(70);
        });
        modelBuilder.Entity<CitizenAddressABP>(entity =>
        {
            entity.HasOne(a => a.Citizen)
                .WithMany(c => c.CitizenAddresses)
                .HasForeignKey(a => a.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);

        });
        modelBuilder.Entity<CitizenABP>(entity =>
        {
            entity.HasMany(c => c.Applications)
                .WithOne(a => a.Citizen)
                .HasForeignKey(a => a.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(c => c.CitizenAddresses)
                .WithOne(a => a.Citizen)
                .HasForeignKey(a => a.CitizenId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<AgendaStatus>(entity =>
        {
            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<AgendaType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Type");

            entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Agenda>(entity =>
        {
            entity.Property(e => e.HasReminder).HasDefaultValue(false);
            entity.Property(e => e.IsLate).HasDefaultValue(false);

            entity.HasOne(d => d.AgendaType).WithMany(p => p.Agenda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Agenda_Type");

            entity.HasOne(d => d.Application).WithMany(p => p.Agenda).HasConstraintName("FK_Agenda_Application");

            entity.HasOne(d => d.Status).WithMany(p => p.Agenda).HasConstraintName("FK_Agenda_Status");
        });

        modelBuilder.Entity<ApplicationDLS>(entity =>
        {
            entity.ToTable("Application", "APP", tb =>
            {
                tb.HasTrigger("trg_Application_Audit_Delete");
                tb.HasTrigger("trg_Application_Audit_InsertUpdate");
            });

            entity.Property(e => e.Id).HasComment("Unique identifier for the application");
            entity.Property(e => e.ApplicationApprovalStatusId).HasComment("Reference to the branch");
            entity.Property(e => e.ApplicationNumber).HasComment("Application number");
            entity.Property(e => e.ApplicationTypeId).HasComment("Reference to the branch");
            entity.Property(e => e.BranchId).HasComment("Reference to the branch");
            entity.Property(e => e.CarUniqueNumber).HasComment("Reference to the unique car identifier");
            entity.Property(e => e.Comments).HasComment("Comments related to the application");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("The date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.DepartmentId).HasComment("Reference to the department");
            entity.Property(e => e.HasActiveLicense).HasComment("Flag indicating active license status");
            entity.Property(e => e.HasActiveRegisteredVehicle).HasComment("Flag indicating active registered vehicle status");
            entity.Property(e => e.HasOwnership).HasComment("Flag indicating if ownership exists");
            entity.Property(e => e.InvoiceNumbers).HasComment("Invoice numbers associated with the application");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasComment("Logical deletion flag");
            entity.Property(e => e.IsExempted).HasComment("Flag indicating if the application is exempted");
            entity.Property(e => e.IsLoaded).HasComment("Flag indicating if the application is loaded");
            entity.Property(e => e.LicenseId).HasComment("Reference to the license");
            entity.Property(e => e.LoadedByUserId).HasComment("User ID of the loader");
            entity.Property(e => e.ModifiedDate).HasComment("The date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
            entity.Property(e => e.OwnerId).HasComment("Reference to the owner");
            entity.Property(e => e.OwnershipId).HasComment("Reference to the ownership record");
            entity.Property(e => e.OwnershipTypeId).HasComment("Reference to the ownership type");
            entity.Property(e => e.RegisteredVehicleId).HasComment("Reference to the registered vehicle");
            entity.Property(e => e.SectionId).HasComment("Reference to the section");
            entity.Property(e => e.StatusId).HasComment("Reference to the application status");

            entity.HasOne(d => d.ApplicationDomain).WithMany(p => p.Applications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Application_ApplicationType");

            entity.HasOne(d => d.Status).WithMany(p => p.Applications).HasConstraintName("FK_Application_Status");

            entity.HasOne(d => d.Citizen).WithMany(p => p.Applications)
             .HasForeignKey(d => d.OwnerId);

            entity.HasKey(x => x.Id);
            entity.HasMany(x => x.Receipts)
             .WithOne(r => r.Application)
             .HasForeignKey(r => r.ApplicationId)
             .HasPrincipalKey(x => x.Id)
             .OnDelete(DeleteBehavior.Restrict);
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
            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ApplicationProcesses)
             .OnDelete(DeleteBehavior.ClientSetNull)
             .HasConstraintName("FK_ApplicationProcess_VarietyBusinessProcess");
        });

        modelBuilder.Entity<ApplicationProcessCheckList>(entity =>
        {
            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
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

            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsSignedDocFile).HasDefaultValue(false);

            entity.HasOne(d => d.ApplicationProcessCheckList).WithMany(p => p.ApplicationProcessCheckListDocFiles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCheckListDocFile_ApplicationProcessCheckList");
        });

        modelBuilder.Entity<ApplicationProcessCondition>(entity =>
        {
            entity.HasKey(e => new { e.ApplicationId, e.ProcessId, e.BPVarietyId, e.ConditionId }).HasName("PK_Conditions");

            entity.HasOne(d => d.Condition).WithMany(p => p.ApplicationProcessConditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessCondition_Condition");

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

            entity.HasOne(d => d.DocumentProcessRelationship).WithMany(p => p.ApplicationProcessDocuments)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ApplicationProcessDocument_DocumentProcessRelationship");
        });

        modelBuilder.Entity<ApplicationProcessFee>(entity =>
        {
            entity.Property(e => e.ApplicationId).HasComment("Foreign key referencing the Application table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Record creation date");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.CurrentYearIncluded).HasComment("Indicates if the fee is applicable to the current year");
            entity.Property(e => e.DiscountAmount).HasComment("Amount of discount applied to the fee");
            entity.Property(e => e.DiscountPercentage).HasComment("Percentage of discount applied to the fee");
            entity.Property(e => e.FeeCategoryId).HasComment("Foreign key referencing the FeeCategory table");
            entity.Property(e => e.FeeNameAr).HasComment("Detailed description of the fee");
            entity.Property(e => e.FeeNameEn).HasComment("Detailed description of the fee");
            entity.Property(e => e.FeeNameFr).HasComment("Detailed description of the fee");
            entity.Property(e => e.FeeSP).HasComment("Service provider related to the fee");
            entity.Property(e => e.FeeTypeId).HasComment("Type of the fee applied");
            entity.Property(e => e.FeeValue).HasComment("Monetary value of the fee");
            entity.Property(e => e.InvoiceNumber).HasComment("Invoice number associated with the fee");
            entity.Property(e => e.IsExempted).HasComment("Indicates if the fee is exempted");
            entity.Property(e => e.IsMunicipalFee).HasComment("Indicates if the fee is a municipal fee");
            entity.Property(e => e.IsPaid).HasComment("Indicates if the fee has been paid");

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
            entity.Property(e => e.Id).HasComment("Unique identifier for the requested plates info record");
            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsIssuePlates).HasComment("Flag indicating whether plates are to be issued");
            entity.Property(e => e.Notes).HasComment("Additional notes for the plate request");
            entity.Property(e => e.NumberGeneration).HasComment("Reference to the number generation method or identifier");
            entity.Property(e => e.ReleasePlates).HasComment("Number of plates to be released");
            entity.Property(e => e.ReleasePlatesCode).HasComment("Code for the plates to be released");
            entity.Property(e => e.RequestedPlates).HasComment("Total number of requested plates");
            entity.Property(e => e.RequestedPlatesCode).HasComment("Code for the requested plates");
            entity.Property(e => e.StatusId).HasComment("Reference to the status of the plate request");

        });

        modelBuilder.Entity<CheckList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConditionCheckList");

            entity.Property(e => e.Id).HasComment("Primary key of the Conditions table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Timestamp when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates whether the record is deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Timestamp when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");

            entity.HasOne(d => d.CheckListGroup).WithMany(p => p.CheckLists)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Conditions_ConditionCheckListGroups");
        });

        modelBuilder.Entity<CheckListGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ConditionCheckListGroups");

            entity.Property(e => e.Id).HasComment("Unique identifier for the condition group");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.DescriptionAr).HasComment("Description of the condition group");
            entity.Property(e => e.DescriptionEn).HasComment("Description of the condition group");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.IsMandatory).HasComment("Indicates whether the group is mandatory");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.PrimaryProcessId).HasComment("Reference to the primary business process");
        });

        modelBuilder.Entity<Condition>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Conditions");

            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsStopProcess).HasDefaultValue(true);

            entity.HasOne(d => d.ConditionGroup).WithMany(p => p.Conditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Condition_ConditionGroups");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.Conditions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Condition_VarietyBusinessProcess");
        });

        modelBuilder.Entity<ConditionGroup>(entity =>
        {
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the document");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.DocumentNameAr).HasComment("Arabic name of the document");
            entity.Property(e => e.DocumentNameEn).HasComment("English name of the document");
            entity.Property(e => e.GroupId).HasComment("Reference to the DocumentGroups table");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
            entity.Property(e => e.BindingColumn).HasComment("Comma-separated string of binding columns for document validation");

            entity.HasOne(d => d.Group).WithMany(p => p.Documents).HasConstraintName("FK_Documents_DocumentGroups");
        });

        modelBuilder.Entity<DocumentGroup>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the document group");
            entity.Property(e => e.Choose)
                .IsFixedLength()
                .HasComment("Selection flag indicating choice (e.g., Yes/No)");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.GroupCode).HasComment("Code for the document group");
            entity.Property(e => e.GroupNameAr).HasComment("Arabic name of the document group");
            entity.Property(e => e.GroupNameEn).HasComment("English name of the document group");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
        });

        modelBuilder.Entity<DocumentProcessRelationship>(entity =>
        {
            entity.HasKey(e => new { e.DocumentId, e.ProcessId, e.BPVarietyId }).HasName("PK_DocumentProcessRelationship_1");

            entity.Property(e => e.DocumentId).HasComment("Reference to the document");
            entity.Property(e => e.ProcessId).HasComment("Reference to the business process");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the license type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.IsMandatory).HasComment("Indicates if the relationship is mandatory");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the status");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentProcessRelationships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProcessRelationship_Documents");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.DocumentProcessRelationships)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProcessRelationship_VarietyBusinessProcess");
        });
        modelBuilder.Entity<DrivingLicenseABP>(entity =>
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
            entity.Property(e => e.Islocked)
                .HasDefaultValue(false)
                .HasComment("Indicates if the driving license is currently locked");
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

        });

        modelBuilder.Entity<DrivingLicenseDetailABP>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DrivingL__3214EC0789D0D9ED");

            entity.Property(e => e.Id).HasComment("Unique identifier for each driving license detail record");
            entity.Property(e => e.ApplicationId).HasComment("Reference to the application record");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.Description).HasComment("Optional description or additional details about the driving license");
            entity.Property(e => e.DrivingLicenseId).HasComment("Reference to the driving license associated with this detail");
            entity.Property(e => e.ExpiryDate).HasComment("The expiry date of the driving license");
            entity.Property(e => e.IssuingDate).HasComment("The date when the driving license was issued");
            entity.Property(e => e.ProcessId).HasComment("Reference to the business process related to the application");
            entity.Property(e => e.StructureId).HasComment("Reference to the structure associated with this driving license detail");

            entity.HasOne(d => d.BPVariety).WithMany(p => p.DrivingLicenseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingLicenseDetail_ProcessVariety");

            entity.HasOne(d => d.DrivingLicense).WithMany(p => p.DrivingLicenseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingLicenseDetail_DrivingLicense");

            entity.HasOne(d => d.Process).WithMany(p => p.DrivingLicenseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingLicenseDetail_Process");

            entity.HasOne(d => d.ApplicationProcess).WithMany(p => p.DrivingLicenseDetails)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DrivingLicenseDetail_ApplicationProcess");
        });
        modelBuilder.Entity<ExemptionType>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the exemption type");
            entity.Property(e => e.Code).HasComment("Unique code for the exemption type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.ExemptionTypeDescAr).HasComment("Description of the exemption type in Arabic");
            entity.Property(e => e.ExemptionTypeDescEn).HasComment("Description of the exemption type in English");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.OwnershipTypeId).HasComment("Reference to the ownership type");
            entity.Property(e => e.StatusId).HasComment("Reference to the status table");
            entity.Property(e => e.TotalNumberOfExemption).HasComment("Total number of exemptions allowed");
        });

        modelBuilder.Entity<ExemptionTypeDocRelationship>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the exemption type-document relationship");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.DocumentId).HasComment("Reference to the document");
            entity.Property(e => e.ExemptionTypeId).HasComment("Reference to the exemption type");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.IsMandatory).HasComment("Indicates if the document is mandatory");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.ProcessId).HasComment("Reference to the business process");
            entity.Property(e => e.StatusId).HasComment("Reference to the status");

            entity.HasOne(d => d.Document).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_Documents");

            entity.HasOne(d => d.ExemptionType).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_ExemptionType");

            entity.HasOne(d => d.Process).WithMany(p => p.ExemptionTypeDocRelationships).HasConstraintName("FK_ExemptionTypeDocRelationship_Process");
        });

        modelBuilder.Entity<Fee>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the fee record");
            entity.Property(e => e.Code).HasComment("Fee identifier code used within the system");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.FeeCategoryId).HasComment("Reference to the FeeCategory table");
            entity.Property(e => e.FeeCode).HasComment("External system fee code (if applicable)");
            entity.Property(e => e.FeeNameAr).HasComment("Name of the fee in Arabic");
            entity.Property(e => e.FeeNameEn).HasComment("Name of the fee in English");
            entity.Property(e => e.FeeNameFr).HasComment("Name of the fee in Arabic");
            entity.Property(e => e.FeeType).HasComment("Type of the fee (linked to FeeType table)");
            entity.Property(e => e.FeeValue).HasComment("Value of the fee");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");

            entity.HasOne(d => d.FeeCategory).WithMany(p => p.Fees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Fees_FeeCategory");
        });

        modelBuilder.Entity<FeeCategory>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the fee category");
            entity.Property(e => e.AdditionalExpiryDate).HasComment("Additional expiry date for the fee category");
            entity.Property(e => e.Code).HasComment("Code representing the fee category");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.ExpiryDate).HasComment("Primary expiry date for the fee category");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
            entity.Property(e => e.NameAr).HasComment("Fee category name in Arabic");
            entity.Property(e => e.NameEn).HasComment("Fee category name in English");
            entity.Property(e => e.Sequence).HasComment("Sequence number for fee category sorting");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
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

            entity.Property(e => e.Id).HasComment("Unique identifier for the business process");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.NameAr).HasComment("Business Process Name in Arabic");
            entity.Property(e => e.NameEn).HasComment("Business Process Name in English");
            entity.Property(e => e.StatusId).HasComment("Reference to Status table");
            entity.Property(e => e.TypeId).HasComment("Reference to BP Type");

            entity.HasOne(d => d.Type).WithMany(p => p.Processes)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Process_Type");
        });

        modelBuilder.Entity<ProcessCheckList>(entity =>
        {
            entity.HasKey(e => new { e.ChekListId, e.ProcessId, e.BPVarietyId }).HasName("PK_BPConditions");

            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
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

            entity.Property(e => e.Id).HasComment("Primary key of the ExemptionFee table");
            entity.Property(e => e.BPId).HasComment("Reference to the BusinessProcesses table");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the license type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User ID of the creator of the record");
            entity.Property(e => e.ExemptionPercentage).HasComment("Percentage of the exemption");
            entity.Property(e => e.ExemptionTypeId).HasComment("Reference to the ExemptionTypes table");
            entity.Property(e => e.FeeId).HasComment("Reference to the Fees table");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates whether the record is deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User ID of the last modifier of the record");
            entity.Property(e => e.StatusId).HasComment("Status of the exemption fee");
            entity.Property(e => e.TaxPercentageApplicable).HasComment("Tax percentage applicable for the exemption");

            entity.HasOne(d => d.BP).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExemptionFee_Process");

            entity.HasOne(d => d.BPVariety).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessExemptionFee_ProcessVariety");

            entity.HasOne(d => d.Fee).WithMany(p => p.ProcessExemptionFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessExemptionFee_Fees");

            entity.HasOne(d => d.ExemptionType).WithMany(p => p.ProcessExemptionFees).HasConstraintName("FK_ExemptionFee_ExemptionTypes");
        });

        modelBuilder.Entity<ProcessFee>(entity =>
        {
            entity.HasKey(e => new { e.FeeId, e.ProcessId, e.BPVarietyId }).HasName("PK_ProcessesFee");

            entity.Property(e => e.FeeId).HasComment("Reference to the Fees table");
            entity.Property(e => e.ProcessId).HasComment("Reference to the BusinessProcesses table");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the LicenseTypes table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasComment("Unique identifier for the process fee record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
            entity.Property(e => e.TaxPercentageApplicable).HasComment("Applicable tax percentage for the fee");

            entity.HasOne(d => d.Fee).WithMany(p => p.ProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessesFee_Fees");

            entity.HasOne(d => d.VarietyBusinessProcess).WithMany(p => p.ProcessFees)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProcessFee_VarietyBusinessProcess");
        });

        modelBuilder.Entity<ProcessType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Type");

            entity.Property(e => e.Id).HasComment("Unique identifier for the Business Process Type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.NameAr).HasComment("Name of the Business Process Type in English");
            entity.Property(e => e.NameEn).HasComment("Name of the Business Process Type in English");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
        });

        modelBuilder.Entity<ProcessVariety>(entity =>
        {
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Logical deletion flag (1 = deleted, 0 = active)");
            entity.Property(e => e.ModifiedDate).HasComment("Date the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
            entity.Property(e => e.NameAr).HasComment("BP category name in Arabic");
            entity.Property(e => e.NameEn).HasComment("BP category name in English");

            entity.HasOne(d => d.ProcessVarietyType).WithMany(p => p.ProcessVarieties).HasConstraintName("FK_ProcessVariety_ProcessVarietyType");
        });

        modelBuilder.Entity<ProcessVarietyType>(entity =>
        {
            entity.Property(e => e.Id).HasComment("Unique identifier for the Business Process Type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.NameAr).HasComment("Name of the Business Process Type in English");
            entity.Property(e => e.NameEn).HasComment("Name of the Business Process Type in English");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Receipt__3214EC076B0C224B");

            entity.Property(e => e.Id).HasComment("Unique identifier for each receipt record");
            entity.Property(e => e.ApplicationId).HasComment("Reference to the associated application for the receipt");
            entity.Property(e => e.ReceiptNumber).HasComment("Unique receipt number issued for the transaction");
            entity.Property(e => e.TotalAmount).HasComment("Total amount specified on the receipt");



            entity.Property(x => x.ApplicationId)
                .IsRequired(true)
                .HasMaxLength(50);

            entity.HasIndex(x => x.ApplicationId);

        });

        modelBuilder.Entity<ReceiptDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ReceiptD__3214EC073F6990F5");

            entity.Property(e => e.Id).HasComment("Unique identifier for each receipt detail record");
            entity.Property(e => e.Amount).HasComment("Amount charged for the item in the receipt");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the receipt detail record was created");
            entity.Property(e => e.CreatedUserId).HasComment("Identifier of the user who created the receipt detail record");
            entity.Property(e => e.DeletedDate).HasComment("Date when the receipt detail was marked as deleted");
            entity.Property(e => e.DeletedUserId).HasComment("Identifier of the user who marked the receipt detail as deleted");
            entity.Property(e => e.IsDeleted).HasComment("Indicates whether the receipt detail is marked as deleted");
            entity.Property(e => e.ItemCategoryId).HasComment("Reference to the item category being billed");
            entity.Property(e => e.ItemCode).HasComment("Code of the item being billed");
            entity.Property(e => e.ItemDescriptionAR).HasComment("Arabic description of the item being billed");
            entity.Property(e => e.ItemDescriptionEN).HasComment("English description of the item being billed");
            entity.Property(e => e.ItemId).HasComment("Reference to the item being billed in the receipt");
            entity.Property(e => e.ItemTypeId).HasComment("Reference to the item type (if applicable)");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the receipt detail record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("Identifier of the user who last modified the receipt detail record");
            entity.Property(e => e.Notes).HasComment("Additional notes or comments for the receipt detail");
            entity.Property(e => e.ReceiptId).HasComment("Reference to the associated receipt");


            entity.HasOne(x => x.FeeCategory)
                  .WithMany(c => c.ReceiptDetails)
                  .HasForeignKey(x => x.ItemCategoryId) // ReceiptDetail.ItemCategoryId
                  .HasPrincipalKey(c => c.Id)           // FeeCategory.Id
                  .OnDelete(DeleteBehavior.Restrict);   // or .Cascade / .SetNull if you prefer

        });

        modelBuilder.Entity<ApplicationDomain>(entity =>
        {

            entity.Property(e => e.Id).HasComment("Primary key for the StructureType table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date the record was created");
            entity.Property(e => e.CreatedUserId).HasComment("User who created the record");
            entity.Property(e => e.DescriptionAr).HasComment("Description of the structure type");
            entity.Property(e => e.DescriptionEn).HasComment("Description of the structure type");
            entity.Property(e => e.DescriptionFr).HasComment("Description of the structure type");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasComment("Indicates if the structure type is active");
            entity.Property(e => e.ModifiedDate).HasComment("Date the record was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("User who last modified the record");
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

            entity.Property(e => e.ProcessId).HasComment("Reference to the main BusinessProcesses table");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date of record creation");
            entity.Property(e => e.CreatedUserId).HasComment("ID of the user who created the record");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(true)
                .HasComment("Indicates if the record is logically deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date of last modification");
            entity.Property(e => e.ModifiedUserId).HasComment("ID of the user who last modified the record");
            entity.Property(e => e.StatusId).HasComment("Reference to the Status table");

            entity.HasOne(d => d.BPVariety).WithMany(p => p.VarietyBusinessProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VarietyBusinessProcess_ProcessVariety");

            entity.HasOne(d => d.Process).WithMany(p => p.VarietyBusinessProcesses)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_VarietyBusinessProcess_Process");
        });



        modelBuilder.Entity<OperationRequestABP>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operatio__3214EC07705BB187");
            entity.ToTable("OperationRequest", "DLS", tb => tb.HasTrigger("trg_OperationRequest_Test"));

            entity.Property(e => e.Id).HasComment("Unique identifier for each operation request record");
            entity.Property(e => e.ApplicationId).HasComment("Reference to the associated application for the operation request");
            entity.Property(e => e.BPVarietyId).HasComment("Reference to the associated BusinessProcesses table");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Date when the operation request was created");
            entity.Property(e => e.CreatedUserId).HasComment("Identifier of the user who created the operation request record");
            entity.Property(e => e.DeletedDate).HasComment("Date when the operation request was marked as deleted");
            entity.Property(e => e.DeletedUserId).HasComment("Identifier of the user who marked the operation request as deleted");
            entity.Property(e => e.Description).HasComment("Optional description or additional information for the operation request");
            entity.Property(e => e.IsDeleted).HasComment("Indicates whether the operation request is marked as deleted");
            entity.Property(e => e.ModifiedDate).HasComment("Date when the operation request was last modified");
            entity.Property(e => e.ModifiedUserId).HasComment("Identifier of the user who last modified the operation request record");
            entity.Property(e => e.Notes).HasComment("Additional notes or comments regarding the operation request");
            entity.Property(e => e.OperationTypeId).HasComment("Type of the operation being requested");
            entity.Property(e => e.ProcessId).HasComment("Reference to the business process related to the application");
            entity.Property(e => e.RequestStatusDate).HasComment("Date when the operation request status was last updated");
            entity.Property(e => e.RequestStatusId).HasComment("Status of the operation request");
            entity.Property(e => e.StructureId).HasComment("Reference to the structure associated with the operation request");

            entity.HasOne(d => d.OperationType).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_OperationType");

            entity.HasOne(d => d.RequestStatus).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_RequestStatus");

            entity.HasOne(d => d.Structure).WithMany(p => p.OperationRequests)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OperationRequest_Structure");

            entity.HasOne(d => d.Application).WithMany(p => p.OperationRequests)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK_DrivingLicense_Application");
        });

        modelBuilder.Entity<OperationTypeABP>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Operatio__3214EC072E78EDE3");

            entity.Property(e => e.Id).HasComment("Unique identifier for the operation type");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasComment("Timestamp indicating when the record was created");
            entity.Property(e => e.DescriptionAR).HasComment("Description of the operation type in Arabic");
            entity.Property(e => e.DescriptionEN).HasComment("Description of the operation type in English");
            entity.Property(e => e.DescriptionFR).HasComment("Description of the operation type in French");
        });

        modelBuilder.Entity<RequestStatusABP>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RequestS__3214EC07425314A0");

            entity.Property(e => e.Id).HasComment("Unique identifier for each request status record");
            entity.Property(e => e.CreatedDate).HasComment("Timestamp when the request status record was created");
            entity.Property(e => e.DescriptionAR).HasComment("Description of the request status in Arabic");
            entity.Property(e => e.DescriptionEN).HasComment("Description of the request status in English");
            entity.Property(e => e.DescriptionFR).HasComment("Description of the request status in French");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
