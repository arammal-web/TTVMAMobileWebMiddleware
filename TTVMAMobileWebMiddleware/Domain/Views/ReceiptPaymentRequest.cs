using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TTVMAMobileWebMiddleware.Domain.Views
{
    public class ReceiptPaymentRequest
    { 
        /// <summary>
        /// Unique receipt number issued for the transaction.
        /// </summary>
        /// <example>RCPT-002345</example>
        [StringLength(50)]
        public string  ReceiptNumber { get; set; } = null!;

        /// <summary>
        /// Unique receipt category sequence number issued for the transaction.
        /// </summary>
        /// <example>RCPT-CAT-002345</example>
        [StringLength(50)]
        public string? ReceiptCategorySequenceNumber { get; set; } = null!;
         
        /// <summary>
        /// Indicates if the receipt has been paid.
        /// </summary>
        /// <example>true</example>
        public bool IsPaid { get; set; }
        /// <summary>
        /// Date when the receipt was paid.
        /// </summary>
        /// <example>2025-01-16T14:00:00</example>
        [Column(TypeName = "smalldatetime")]
        public DateTime? PaidDate { get; set; }
        /// <summary>
        /// Reference number provided by the payment provider.
        /// </summary>
        /// <example>PMT-998877</example>
        [StringLength(50)]
        public string? PaymentProviderNumber { get; set; }

        /// <summary>
        /// Date of transaction confirmation from the payment provider.
        /// </summary>
        /// <example>2025-01-16T14:01:00</example>
        [Column(TypeName = "smalldatetime")]
        public DateTime? PaymentProviderDate { get; set; }

        /// <summary>
        /// Additional metadata or payload returned from the payment provider.
        /// </summary>
        /// <example>{"transactionId": "abc123"}</example>
        public string? PaymentProviderData { get; set; } 

        /// <summary>
        /// Additional notes related to the receipt.
        /// </summary>
        /// <example>Paid via mobile app</example>
        [StringLength(2000)]
        public string? Notes { get; set; }

    }
}
