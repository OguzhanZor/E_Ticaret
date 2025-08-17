using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Ticaret.Repositories;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    [Authorize]
    public class B2BController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<B2BController> _logger;

        public B2BController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IUserRepository userRepository,
            ILogger<B2BController> logger)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var b2bViewModel = new B2BViewModel
                {
                    Products = await _productRepository.GetAllAsync(),
                    Categories = await _categoryRepository.GetAllAsync()
                };

                return View(b2bViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading B2B dashboard");
                TempData["Error"] = "B2B dashboard yüklenirken hata oluştu.";
                return View(new B2BViewModel());
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
