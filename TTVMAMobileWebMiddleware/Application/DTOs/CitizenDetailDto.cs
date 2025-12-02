namespace TTVMAMobileWebMiddleware.Application.DTOs;

/// <summary>
/// DTO for citizen details with all related entities
/// </summary>
public class CitizenDetailDto
{
    // Citizen properties
    public int Id { get; set; }
    public long UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? BirthPlace { get; set; }
    public string? MaidenName { get; set; }
    public string? FathersName { get; set; }
    public string? MothersName { get; set; }
    public string? MothersMaidenName { get; set; }
    public string? FirstNameSecondLang { get; set; }
    public string? LastNameSecondLang { get; set; }
    public string? FathersNameSecondLang { get; set; }
    public string? MothersNameSecondLang { get; set; }
    public string? MaidenNameSecondLang { get; set; }
    public string? MothersMaidenNameSecondLang { get; set; }
    public string? OtherNames { get; set; }
    public string? NationalId { get; set; }
    public int? GenderId { get; set; }
    public string? Address { get; set; }
    public bool? IsHandicap { get; set; }
    public string? HandicapCardNumber { get; set; }
    public string? HandicapNotes { get; set; }
    public bool? IsVIP { get; set; }
    public string? BioTemplate { get; set; }
    public bool? IsValid { get; set; }
    public DateTime? ValidationDate { get; set; }
    public int? ValidationUserId { get; set; }
    public int? StructureId { get; set; }
    public string? SecondLangCulture { get; set; }
    public string? FirstLangCulture { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; } = null!;
    public int? NationalityId { get; set; }
    public int? MaritalStatusId { get; set; }
    public int? ProfessionId { get; set; }
    public int? SpouseProfessionId { get; set; }
    public string? SpouseFirstName { get; set; }
    public string? SpouseLastName { get; set; }
    public string? SpouseFirstNameL { get; set; }
    public string? SpouseLastNameL { get; set; }
    public DateOnly? SpouseBirthDate { get; set; }
    public string? SpouseBirthPlace { get; set; }
    public int? SpouseNationalityId { get; set; }
    public string? Phone2 { get; set; }
    public string? PlaceOfBirthL { get; set; }
    public int? BloodId { get; set; }
    public string? RegisterId { get; set; }
    public string? Notes { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedUserId { get; set; }
    public int ApprovalStatusId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedUserId { get; set; }

    // Related entities
    public List<CitizenAddressDto> CitizenAddresses { get; set; } = new();
    public List<CitizenFaceImageDto> CitizenFaceImages { get; set; } = new();
    public List<CitizenIdentityDocumentDto> CitizenIdentityDocuments { get; set; } = new();
}

/// <summary>
/// DTO for citizen face image
/// </summary>
public class CitizenFaceImageDto
{
    public int Id { get; set; }
    public int CitizenId { get; set; }
    public byte[] FaceImage { get; set; } = null!;
    public byte[]? FaceTemplate { get; set; }
    public string? ImageFormat { get; set; }
    public bool? IsPrimaryPhoto { get; set; }
    public string? CaptureDevice { get; set; }
    public string? FaceEncodingVersion { get; set; }
    public string? Description { get; set; }
    public double? FaseImageScore { get; set; }
    public int? StructureId { get; set; }
    public string? Notes { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedUserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedUserId { get; set; }
}

/// <summary>
/// DTO for citizen identity document with document details
/// </summary>
public class CitizenIdentityDocumentDto
{
    public int Id { get; set; }
    public int CitizenId { get; set; }
    public int DocumentId { get; set; }
    public int ProcessId { get; set; }
    public byte[]? IdentityDocument { get; set; }
    public string? IdentityDocumentHash { get; set; }
    public string? IdentityDocumentData { get; set; }
    public string? Description { get; set; }
    public int? StructureId { get; set; }
    public string? Notes { get; set; }
    public string? AuthenticityResults { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedUserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedUserId { get; set; }
    public string? DocFileExt { get; set; }
    public DocumentDto? Document { get; set; }
}

/// <summary>
/// DTO for document
/// </summary>
public class DocumentDto
{
    public int Id { get; set; }
    public string DocumentNameEn { get; set; } = null!;
    public string DocumentNameAr { get; set; } = null!;
    public string? DocumentCode { get; set; }
    public int? GroupId { get; set; }
    public string? Domain { get; set; }
    public int? MigrationId { get; set; }
    public bool IsDeleted { get; set; }
    public int? DeletedUserId { get; set; }
    public DateTime? DeletedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedUserId { get; set; }
    public int? StatusId { get; set; }
    public string? DocumentNameFr { get; set; }
    public string? Icon { get; set; }
}

/// <summary>
/// DTO for citizen address with location details
/// </summary>
public class CitizenAddressDto
{
    public int Id { get; set; }
    public int CitizenId { get; set; }
    public int CityId { get; set; }
    public int CountryId { get; set; }
    public int? RegionId { get; set; }
    public int? PrefectureId { get; set; }
    public int? VillageId { get; set; }
    public string? Address1 { get; set; }
    public string? Address2 { get; set; }
    public string? Notes { get; set; }
    public bool? IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public int? DeletedUserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public int? CreatedUserId { get; set; }
    public DateTime? ModifiedDate { get; set; }
    public int? ModifiedUserId { get; set; }
    public CityDto? City { get; set; }
    public CountryDto? Country { get; set; }
    public RegionDto? Region { get; set; }
    public PrefectureDto? Prefecture { get; set; }
    public VillageDto? Village { get; set; }
}

/// <summary>
/// DTO for city
/// </summary>
public class CityDto
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int PrefectureId { get; set; }
    public int? RegionId { get; set; }
    public string NameEN { get; set; } = null!;
    public string? NameAR { get; set; }
    public string? NameFR { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// DTO for country
/// </summary>
public class CountryDto
{
    public int Id { get; set; }
    public string CountryCode { get; set; } = null!;
    public string CountryNameEN { get; set; } = null!;
    public string? CountryNameAR { get; set; }
    public string? CountryNameFR { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// DTO for region
/// </summary>
public class RegionDto
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int PrefectureId { get; set; }
    public string NameEN { get; set; } = null!;
    public string? NameAR { get; set; }
    public string? NameFR { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// DTO for prefecture
/// </summary>
public class PrefectureDto
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public string NameEN { get; set; } = null!;
    public string? NameAR { get; set; }
    public string? NameFR { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

/// <summary>
/// DTO for village
/// </summary>
public class VillageDto
{
    public int Id { get; set; }
    public int CountryId { get; set; }
    public int PrefectureId { get; set; }
    public int? RegionId { get; set; }
    public string NameEN { get; set; } = null!;
    public string? NameAR { get; set; }
    public string? NameFR { get; set; }
    public bool? IsCapital { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

