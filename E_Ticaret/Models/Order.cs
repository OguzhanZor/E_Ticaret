using System.ComponentModel.DataAnnotations;

namespace E_Ticaret.Models
{
    public class Order
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;
        
        public int UserId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
        
        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal ShippingCost { get; set; } = 0;
        
        [Range(0, double.MaxValue)]
        public decimal TaxAmount { get; set; } = 0;
        
        [StringLength(200)]
        public string? ShippingAddress { get; set; }
        
        [StringLength(100)]
        public string? ShippingCity { get; set; }
        
        [StringLength(100)]
        public string? ShippingCountry { get; set; }
        
        [StringLength(20)]
        public string? ShippingPostalCode { get; set; }
        
        [StringLength(500)]
        public string? Notes { get; set; }
        
        public DateTime OrderDate { get; set; } = DateTime.Now;
        
        public DateTime? ShippedDate { get; set; }
        
        public DateTime? DeliveredDate { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public virtual User? User { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
    }
}
