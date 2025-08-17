using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Brand
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [StringLength(200)]
        public string? LogoUrl { get; set; }
        
        [StringLength(200)]
        public string? Website { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; } = 0;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Product>? Products { get; set; }
    }
}
