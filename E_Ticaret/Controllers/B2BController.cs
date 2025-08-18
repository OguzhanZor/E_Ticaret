using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Ticaret.Repositories;
using E_Ticaret.Models;
using E_Ticaret.Models.ViewModels;

namespace E_Ticaret.Controllers
{
    [Authorize]
    public class B2BController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly ILogger<B2BController> _logger;

        public B2BController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            IBannerRepository bannerRepository,
            ISliderRepository sliderRepository,
            ILogger<B2BController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _bannerRepository = bannerRepository;
            _sliderRepository = sliderRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var allBanners = await _bannerRepository.GetAllAsync();
                var allSliders = await _sliderRepository.GetActiveAsync();
                
                // Debug için tüm banner'ları logla
                _logger.LogInformation($"Toplam banner sayısı: {allBanners.Count()}");
                foreach (var banner in allBanners)
                {
                    _logger.LogInformation($"Tüm Banner: {banner.Title}, IsActive: {banner.IsActive}, StartDate: {banner.StartDate}, EndDate: {banner.EndDate}, DisplayOrder: {banner.DisplayOrder}");
                }

                // Sadece aktif banner'ları al ve tarih kontrolünü geçici olarak kaldır
                var activeBanners = allBanners.Where(b => b.IsActive).ToList();
                _logger.LogInformation($"Aktif banner sayısı: {activeBanners.Count()}");

                // Tarih kontrolünü geçici olarak devre dışı bırak - sadece aktif olanları al
                var filteredBanners = activeBanners
                    .OrderBy(b => b.DisplayOrder)
                    .Take(3)
                    .ToList();
                
                _logger.LogInformation($"Final banner sayısı (tarih kontrolü olmadan): {filteredBanners.Count}");

                var homeViewModel = new HomeViewModel
                {
                    Categories = (await _categoryRepository.GetAllAsync()).ToList(),
                    FeaturedProducts = (await _productRepository.GetAllAsync()).ToList(),
                    HotProducts = (await _productRepository.GetAllAsync()).ToList(),
                    Banners = filteredBanners,
                    Sliders = allSliders.OrderBy(s => s.DisplayOrder).ToList()
                };

                // Final debug bilgisi
                _logger.LogInformation($"Final banner sayısı: {filteredBanners.Count}");
                foreach (var banner in filteredBanners)
                {
                    _logger.LogInformation($"Final Banner: {banner.Title}, IsActive: {banner.IsActive}, StartDate: {banner.StartDate}, EndDate: {banner.EndDate}, DisplayOrder: {banner.DisplayOrder}");
                }

                return View(homeViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading B2B dashboard");
                TempData["Error"] = "B2B dashboard yüklenirken hata oluştu.";
                return View(new HomeViewModel());
            }
        }

        public async Task<IActionResult> Products()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading B2B products");
                TempData["Error"] = "Ürünler yüklenirken hata oluştu.";
                return View(new List<Product>());
            }
        }

        public async Task<IActionResult> Categories()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading B2B categories");
                TempData["Error"] = "Kategoriler yüklenirken hata oluştu.";
                return View(new List<Category>());
            }
        }

        public IActionResult Orders()
        {
            // B2B sipariş yönetimi
            return View();
        }

        public IActionResult Reports()
        {
            // B2B raporları
            return View();
        }
    }

    public class B2BViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
    }
}
