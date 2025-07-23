using System.ComponentModel.DataAnnotations;

namespace Belloro.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Kategori adı gereklidir")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir")]
        public string Name { get; set; } = string.Empty; // Kategori adı (Lüks Saat, Klasik Saat vb.)

        [StringLength(255, ErrorMessage = "Açıklama en fazla 255 karakter olabilir")]
        public string? Description { get; set; } // Açıklama (örn: Bu kategoriye lüks saatler girer)

        // Navigation property
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}