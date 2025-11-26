 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TTVMAMobileWebMiddleware.Domain.Entities.Mobile;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS
{
    [Table("Citizen", Schema = "dbo")]
    public partial class CitizenABP
    {
        /// <summary>
        /// Table identity
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// First name of the citizen
        /// </summary>
        public string FirstName { get; set; } = null!;

        /// <summary>
        /// Last name of the citizen
        /// </summary>
        public string LastName { get; set; } = null!;

        /// <summary>
        /// Middle name of the citizen
        /// </summary>
        public string? MiddleName { get; set; }

        /// <summary>
        /// Date of birth of the citizen
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Place of birth of the citizen
        /// </summary>
        public string? BirthPlace { get; set; }

        /// <summary>
        /// Maiden name of the citizen
        /// </summary>
        public string? MaidenName { get; set; }

        /// <summary>
        /// Father&apos;s name of the citizen
        /// </summary>
        public string? FathersName { get; set; }

        /// <summary>
        /// Mother&apos;s name of the citizen
        /// </summary>
        public string? MothersName { get; set; }

        /// <summary>
        /// Mother&apos;s maiden name of the citizen
        /// </summary>
        public string? MothersMaidenName { get; set; }

        /// <summary>
        /// First name of the citizen in the second language
        /// </summary>
        public string? FirstNameSecondLang { get; set; }

        /// <summary>
        /// Last name of the citizen in the second language
        /// </summary>
        public string? LastNameSecondLang { get; set; }

        /// <summary>
        /// Father&apos;s name of the citizen in the second language
        /// </summary>
        public string? FathersNameSecondLang { get; set; }

        /// <summary>
        /// Mother&apos;s name of the citizen in the second language
        /// </summary>
        public string? MothersNameSecondLang { get; set; }

        /// <summary>
        /// Maiden name of the citizen in the second language
        /// </summary>
        public string? MaidenNameSecondLang { get; set; }

        /// <summary>
        /// Mother&apos;s maiden name of the citizen in the second language
        /// </summary>
        public string? MothersMaidenNameSecondLang { get; set; }

        /// <summary>
        /// Other names of the citizen, if any
        /// </summary>
        public string? OtherNames { get; set; }

        /// <summary>
        /// National ID of the citizen
        /// </summary>
        public string? NationalId { get; set; } = null!;

        /// <summary>
        /// Gender of the citizen
        /// </summary>
        public int? GenderId { get; set; }

        /// <summary>
        /// Address of the citizen
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Indicates if the citizen is handicapped
        /// </summary>
        public bool? IsHandicap { get; set; }

        /// <summary>
        /// Handicap card number of the citizen, if applicable
        /// </summary>
        public string? HandicapCardNumber { get; set; }

        /// <summary>
        /// Additional notes related to the citizen&apos;s handicap
        /// </summary>
        public string? HandicapNotes { get; set; }

        /// <summary>
        /// Indicates if the citizen is marked as VIP
        /// </summary>
        public bool? IsVip { get; set; }

        /// <summary>
        /// Biometric template data of the citizen
        /// </summary>
        public string? BioTemplate { get; set; }

        /// <summary>
        /// the citizen is valid after the biometric data are valid and the BioTemplate is not null
        /// </summary>
        public bool? IsValid { get; set; }

        /// <summary>
        /// Date when the citizen was validated
        /// </summary>
        public DateTime? ValidationDate { get; set; }

        /// <summary>
        /// User who validated the citizen
        /// </summary>
        public int? ValidationUserId { get; set; }

        /// <summary>
        /// Structure identifier linked to the citizen
        /// </summary>
        public int? StructureId { get; set; }

        /// <summary>
        /// Culture code for the second language
        /// </summary>
        public string? SecondLangCulture { get; set; }

        /// <summary>
        /// Culture code for the first language
        /// </summary>
        public string? FirstLangCulture { get; set; }

        /// <summary>
        /// Email address of the citizen
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Primary phone number of the citizen
        /// </summary>
        public string? Phone { get; set; } = null!;

        /// <summary>
        /// Nationality identifier linked to the citizen
        /// </summary>
        public int? NationalityId { get; set; }

        /// <summary>
        /// Marital status of the citizen
        /// </summary>
        public int? MaritalStatusId { get; set; }

        /// <summary>
        /// Profession identifier for the citizen
        /// </summary>
        public int? ProfessionId { get; set; }

        /// <summary>
        /// Profession identifier for the citizen&apos;s spouse, if applicable
        /// </summary>
        public int? SpouseProfessionId { get; set; }

        /// <summary>
        /// Spouse&apos;s first name
        /// </summary>
        public string? SpouseFirstName { get; set; }

        /// <summary>
        /// Spouse&apos;s last name
        /// </summary>
        public string? SpouseLastName { get; set; }

        /// <summary>
        /// Spouse&apos;s first name in second language
        /// </summary>
        public string? SpouseFirstNameL { get; set; }

        /// <summary>
        /// Spouse&apos;s last name in second language
        /// </summary>
        public string? SpouseLastNameL { get; set; }

        /// <summary>
        /// Spouse&apos;s date of birth
        /// </summary>
        public DateOnly? SpouseBirthDate { get; set; }

        /// <summary>
        /// Spouse&apos;s place of birth
        /// </summary>
        public string? SpouseBirthPlace { get; set; }

        /// <summary>
        /// Spouse&apos;s nationality identifier
        /// </summary>
        public int? SpouseNationalityId { get; set; }

        /// <summary>
        /// Secondary phone number of the citizen
        /// </summary>
        public string? Phone2 { get; set; }

        /// <summary>
        /// Place of birth of the citizen in second language
        /// </summary>
        public string? PlaceOfBirthL { get; set; }

        /// <summary>
        /// Blood type identifier linked to the citizen
        /// </summary>
        public int? BloodId { get; set; }

        /// <summary>
        /// CitizeDLn&apos;s register identifier number
        /// </summary>
        public string? RegisterId { get; set; }

        /// <summary>
        /// Notes or comments related to the citizen
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Passport Nationality Id
        /// </summary>
        public int? PassportNationalityId { get; set; }

        /// <summary>
        /// CitizenDL&apos;s Passport Number/Number
        /// </summary>
        public string? PassportNumber { get; set; }

        /// <summary>
        ///Passport Expiry Date 
        /// </summary>
        public DateTime? PassportExpiryDate { get; set; }
        
        /// <summary>
        /// Indicates if the citizen record is deleted
        /// </summary>
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Date when the record was deleted
        /// </summary>
        public DateTime? DeletedDate { get; set; }

        /// <summary>
        /// User who deleted the record
        /// </summary>
        public int? DeletedUserId { get; set; }

        /// <summary>
        /// Date when the record was created
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// User who created the record
        /// </summary>
        public int? CreatedUserId { get; set; }

        /// <summary>
        /// Date when the record was last modified
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        ///  User who last modified the record
        /// </summary>
        public int? ModifiedUserId { get; set; }

        [InverseProperty("Citizen")]
        public virtual ICollection<CitizenAddressABP> CitizenAddresses { get; set; } = new List<CitizenAddressABP>();
       
        [InverseProperty("Citizen")]
        public virtual ICollection<ApplicationDLS> Applications { get; set; } = new List<ApplicationDLS>();


        [InverseProperty("Citizen")]
        public virtual ICollection<CitizenIdentityDocument?> CitizenIdentityDocuments { get; set; } = new List<CitizenIdentityDocument>();

      
    }

}
