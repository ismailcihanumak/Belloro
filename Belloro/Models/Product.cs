using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Belloro.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Ürün adı gereklidir")]
        [StringLength(200, ErrorMessage = "Ürün adı en fazla 200 karakter olabilir")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fiyat gereklidir")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat 0'dan büyük olmalıdır")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Foreign Key - nullable
        public int? CategoryId { get; set; }

        // Navigation property
        public virtual Category? Category { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Satış adedi negatif olamaz")]
        public int SalesCount { get; set; }

        [StringLength(500, ErrorMessage = "Resim URL'si en fazla 500 karakter olabilir")]
        public string? ImageUrl { get; set; }

        [StringLength(1000, ErrorMessage = "Açıklama en fazla 1000 karakter olabilir")]
        public string? Description { get; set; }

        public bool IsFeatured { get; set; }
        public bool IsNew { get; set; }
    }
}