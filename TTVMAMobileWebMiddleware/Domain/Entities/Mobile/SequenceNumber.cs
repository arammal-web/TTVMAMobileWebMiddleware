using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Entities.Mobile
{
    [PrimaryKey("YearValue", "TableName", "StructureId")]
    [Table("SequenceNumber" )]
    public partial class SequenceNumber
    {
        [Key]
        public int MaxValue { get; set; }

        [Key]
        public int YearValue { get; set; }

        [Key]
        [StringLength(50)]
        public string TableName { get; set; } = null!;

        [StringLength(50)]
        public string? Prefix { get; set; }

        [Key]
        public int StructureId { get; set; }
    }
}
