using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belloro.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [Required]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // TotalPrice yerine Amount kullan

        [Required]
        public string Status { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }

        // Foreign Keys
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Product? Product { get; set; }
    }
}