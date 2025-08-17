using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Review
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        
        public int UserId { get; set; }
        
        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }
        
        [StringLength(1000)]
        public string? Comment { get; set; }
        
        public bool IsApproved { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
