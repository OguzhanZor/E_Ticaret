using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Banner
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Başlık alanı zorunludur")]
        [StringLength(200, ErrorMessage = "Başlık en fazla {1} karakter olabilir")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Alt başlık en fazla {1} karakter olabilir")]
        public string? Subtitle { get; set; }
        
        [StringLength(200, ErrorMessage = "Buton metni en fazla {1} karakter olabilir")]
        public string? ButtonText { get; set; }
        
        [StringLength(200, ErrorMessage = "Buton linki en fazla {1} karakter olabilir")]
        public string? ButtonUrl { get; set; }
        
        [Required(ErrorMessage = "Görsel URL'si zorunludur")]
        [StringLength(200, ErrorMessage = "Görsel URL'si en fazla {1} karakter olabilir")]
        public string ImageUrl { get; set; } = string.Empty;
        
        [StringLength(200, ErrorMessage = "Mobil görsel URL'si en fazla {1} karakter olabilir")]
        public string? MobileImageUrl { get; set; }
        
        public int DisplayOrder { get; set; } = 0;
        
        public bool IsActive { get; set; } = true;
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public DateTime? EndDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
