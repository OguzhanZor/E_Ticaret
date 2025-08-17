using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Banner
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Subtitle { get; set; }
        
        [StringLength(200)]
        public string? ButtonText { get; set; }
        
        [StringLength(200)]
        public string? ButtonUrl { get; set; }
        
        [Required]
        [StringLength(200)]
        public string ImageUrl { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? MobileImageUrl { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public DateTime? EndDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
