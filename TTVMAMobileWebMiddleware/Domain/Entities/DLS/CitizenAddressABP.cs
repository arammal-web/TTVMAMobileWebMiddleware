 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Entities.DLS
{
    [Table("CitizenAddress", Schema = "dbo")]
    public partial class CitizenAddressABP
    {
        [Key]
        public int Id { get; set; }

        public int CitizenId { get; set; }

        [StringLength(250)]
        public string? CountryNameAR { get; set; }

        [StringLength(250)]
        public string? CountryNameEN { get; set; }

        [StringLength(250)]
        public string? CountryNameFR { get; set; }

        [StringLength(250)]
        public string? PrefectureAR { get; set; }

        [StringLength(250)]
        public string? PrefectureEN { get; set; }

        [StringLength(250)]
        public string? PrefectureFR { get; set; }

        [StringLength(250)]
        public string? RegionAR { get; set; }

        [StringLength(250)]
        public string? RegionEN { get; set; }

        [StringLength(250)]
        public string? RegionFR { get; set; }

        [StringLength(250)]
        public string? CityAR { get; set; }

        [StringLength(250)]
        public string? CityEN { get; set; }

        [StringLength(250)]
        public string? CityFR { get; set; }

        [StringLength(250)]
        public string? VillageAR { get; set; }

        [StringLength(250)]
        public string? VillageEN { get; set; }

        [StringLength(250)]
        public string? VillageFR { get; set; }

        [StringLength(500)]
        public string? Address1 { get; set; }

        [StringLength(500)]
        public string? Address2 { get; set; }

        [StringLength(2000)]
        public string? Notes { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedUserId { get; set; }

        [ForeignKey("CitizenId")]
        [InverseProperty("CitizenAddresses")]
        public virtual CitizenABP Citizen { get; set; } = null!;

    }
}
