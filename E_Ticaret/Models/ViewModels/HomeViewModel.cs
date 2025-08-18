using E_Ticaret.Models;

namespace E_Ticaret.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> FeaturedProducts { get; set; } = new();
        public List<Product> HotProducts { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Banner> Banners { get; set; } = new();
        public List<Slider> Sliders { get; set; } = new();
    }
}
