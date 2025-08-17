using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Product
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(1000)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal? OldPrice { get; set; }
        
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }
        
        [StringLength(200)]
        public string? ImageUrl { get; set; }
        
        [StringLength(200)]
        public string? ImageUrl2 { get; set; }
        
        [StringLength(200)]
        public string? ImageUrl3 { get; set; }
        
        public int CategoryId { get; set; }
        
        public int BrandId { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsFeatured { get; set; } = false;
        
        public bool IsHot { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Category? Category { get; set; }
        public virtual Brand? Brand { get; set; }
    }
}
