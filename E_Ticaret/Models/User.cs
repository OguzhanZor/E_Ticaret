using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Ad alanı zorunludur")]
        [StringLength(100, ErrorMessage = "Ad en fazla {1} karakter olabilir")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Soyad alanı zorunludur")]
        [StringLength(100, ErrorMessage = "Soyad en fazla {1} karakter olabilir")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [StringLength(150, ErrorMessage = "E-posta adresi en fazla {1} karakter olabilir")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Kullanıcı adı zorunludur")]
        [StringLength(100, ErrorMessage = "Kullanıcı adı en fazla {1} karakter olabilir")]
        public string Username { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, ErrorMessage = "Şifre en fazla {1} karakter olabilir")]
        public string Password { get; set; } = string.Empty;
        
        [StringLength(20, ErrorMessage = "Telefon numarası en fazla {1} karakter olabilir")]
        public string? Phone { get; set; }
        
        [StringLength(200, ErrorMessage = "Adres en fazla {1} karakter olabilir")]
        public string? Address { get; set; }
        
        [StringLength(100, ErrorMessage = "Şehir en fazla {1} karakter olabilir")]
        public string? City { get; set; }
        
        [StringLength(100, ErrorMessage = "Ülke en fazla {1} karakter olabilir")]
        public string? Country { get; set; }
        
        [StringLength(20, ErrorMessage = "Posta kodu en fazla {1} karakter olabilir")]
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
