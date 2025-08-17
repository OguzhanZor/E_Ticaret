using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(20)]
        public string? Phone { get; set; }
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(100)]
        public string? City { get; set; }
        
        [StringLength(100)]
        public string? Country { get; set; }
        
        [StringLength(20)]
        public string? PostalCode { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public bool IsAdmin { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? LastLoginAt { get; set; }
        
        // Navigation properties
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual ICollection<Review>? Reviews { get; set; }
    }
}
