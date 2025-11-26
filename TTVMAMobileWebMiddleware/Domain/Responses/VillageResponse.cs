using System.ComponentModel.DataAnnotations;

namespace TTVMAMobileWebMiddleware.Domain.Responses
{
    public class VillageResponse 
    {
        public int Id { get; set; } 
        public string NameEn { get; set; } = null!;
        public string? NameAr { get; set; }
        public string? NameFr { get; set; }
    }
}
