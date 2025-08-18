using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Category
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Kategori adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla {1} karakter olabilir")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Açıklama en fazla {1} karakter olabilir")]
        public string? Description { get; set; }
        
        [StringLength(200, ErrorMessage = "Görsel URL'si en fazla {1} karakter olabilir")]
        public string? ImageUrl { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category>? SubCategories { get; set; }
        public virtual ICollection<Product>? Products { get; set; }
    }
}
