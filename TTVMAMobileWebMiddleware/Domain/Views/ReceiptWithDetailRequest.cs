using System.ComponentModel.DataAnnotations;
using TTVMAMobileWebMiddleware.Domain.Entities.DLS;

namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public class ReceiptWithDetailRequest
    {
        /// <summary>
        /// Receipt
        /// </summary>
        [Required]
        public Receipt Receipt { get; set; }

        /// <summary>
        /// Receipt Details
        /// </summary>
        [Required]
        public List<ReceiptDetail> ReceiptDetails { get; set; }

    }
}
