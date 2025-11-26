using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

[Table("Citizen")]
public partial class Citizen
{
    [Key]
    public int Id { get; set; }
    
    public long UserId { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    [StringLength(100)]
    public string LastName { get; set; } = null!;

    [StringLength(100)]
    public string? MiddleName { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime DateOfBirth { get; set; }

    [StringLength(500)]
    public string? BirthPlace { get; set; }

    [StringLength(100)]
    public string? MaidenName { get; set; }

    [StringLength(100)]
    public string? FathersName { get; set; }

    [StringLength(100)]
    public string? MothersName { get; set; }

    [StringLength(100)]
    public string? MothersMaidenName { get; set; }

    [StringLength(100)]
    public string? FirstNameSecondLang { get; set; }

    [StringLength(100)]
    public string? LastNameSecondLang { get; set; }

    [StringLength(100)]
    public string? FathersNameSecondLang { get; set; }

    [StringLength(100)]
    public string? MothersNameSecondLang { get; set; }

    [StringLength(100)]
    public string? MaidenNameSecondLang { get; set; }

    [StringLength(100)]
    public string? MothersMaidenNameSecondLang { get; set; }

    [StringLength(100)]
    public string? OtherNames { get; set; }

    [StringLength(100)]
    public string? NationalId { get; set; }

    public int? GenderId { get; set; }

    [StringLength(500)]
    public string? Address { get; set; }

    public bool? IsHandicap { get; set; }

    [StringLength(50)]
    public string? HandicapCardNumber { get; set; }

    [StringLength(1000)]
    public string? HandicapNotes { get; set; }

    public bool? IsVIP { get; set; }

    [Column(TypeName = "text")]
    public string? BioTemplate { get; set; }

    public bool? IsValid { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ValidationDate { get; set; }

    public int? ValidationUserId { get; set; }

    public int? StructureId { get; set; }

    [StringLength(10)]
    public string? SecondLangCulture { get; set; }

    [StringLength(10)]
    public string? FirstLangCulture { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string Phone { get; set; } = null!;

    public int? NationalityId { get; set; }

    public int? MaritalStatusId { get; set; }

    public int? ProfessionId { get; set; }

    public int? SpouseProfessionId { get; set; }

    [StringLength(50)]
    public string? SpouseFirstName { get; set; }

    [StringLength(50)]
    public string? SpouseLastName { get; set; }

    [StringLength(100)]
    public string? SpouseFirstNameL { get; set; }

    [StringLength(100)]
    public string? SpouseLastNameL { get; set; }

    public DateOnly? SpouseBirthDate { get; set; }

    [StringLength(50)]
    public string? SpouseBirthPlace { get; set; }

    public int? SpouseNationalityId { get; set; }

    [StringLength(50)]
    public string? Phone2 { get; set; }

    [StringLength(150)]
    public string? PlaceOfBirthL { get; set; }

    public int? BloodId { get; set; }

    [StringLength(100)]
    [Column("RegisterID")]
    public string? RegisterId { get; set; }

    [StringLength(2000)]
    public string? Notes { get; set; }

    public bool? IsDeleted { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? DeletedDate { get; set; }

    public int? DeletedUserId { get; set; }
    public int  ApprovalStatusId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? CreatedDate { get; set; }

    public int? CreatedUserId { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? ModifiedDate { get; set; }

    public int? ModifiedUserId { get; set; }

    [ForeignKey("BloodId")]
    [InverseProperty("Citizens")]
    public virtual BloodType? Blood { get; set; }
     
    public virtual ICollection<CitizenAddress?> CitizenAddresses { get; set; } = new List<CitizenAddress>();

    [InverseProperty("Citizen")]
    public virtual ICollection<CitizenFaceImage?> CitizenFaceImages { get; set; } = new List<CitizenFaceImage>();

    [InverseProperty("Citizen")]
    public virtual ICollection<CitizenFingerprint?> CitizenFingerprints { get; set; } = new List<CitizenFingerprint>();

    [InverseProperty("Citizen")]
    public virtual ICollection<CitizenIdentityDocument?> CitizenIdentityDocuments { get; set; } = new List<CitizenIdentityDocument>();

    [InverseProperty("Citizen")]
    public virtual ICollection<CitizenSignature?> CitizenSignatures { get; set; } = new List<CitizenSignature>();

    [InverseProperty("Citizen")]
    public virtual ICollection<CitizenLink> CitizenLinks { get; set; } = new List<CitizenLink>();

    [ForeignKey("GenderId")]
    [InverseProperty("Citizens")]
    public virtual Gender? Gender { get; set; }

    [ForeignKey("MaritalStatusId")]
    [InverseProperty("Citizens")]
    public virtual MaritalStatus? MaritalStatus { get; set; }

    [ForeignKey("NationalityId")]
    [InverseProperty("CitizenNationalities")]
    public virtual Nationality? Nationality { get; set; }

    [ForeignKey("ProfessionId")]
    [InverseProperty("CitizenProfessions")]
    public virtual Profession? Profession { get; set; }

    [ForeignKey("SpouseNationalityId")]
    [InverseProperty("CitizenSpouseNationalities")]
    public virtual Nationality? SpouseNationality { get; set; }

    [ForeignKey("SpouseProfessionId")]
    [InverseProperty("CitizenSpouseProfessions")]
    public virtual Profession? SpouseProfession { get; set; }

    //[ForeignKey("Id")]
    [InverseProperty("Citizen")]
    public virtual AppUser? User { get; set; } = null!;


    [InverseProperty("Citizen")]
    public virtual ICollection<ApplicationMob> Applications { get; set; } = new List<ApplicationMob>();
}
