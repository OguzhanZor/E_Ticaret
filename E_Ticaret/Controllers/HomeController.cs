using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_Ticaret.Models;
using E_Ticaret.Models.ViewModels;
using E_Ticaret.Repositories;

namespace E_Ticaret.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public HomeController(
        ILogger<HomeController> logger,
        IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _logger = logger;
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var featuredProducts = await _productRepository.GetFeaturedAsync();
            var hotProducts = await _productRepository.GetHotAsync();
            var categories = await _categoryRepository.GetActiveAsync();

            var viewModel = new HomeViewModel
            {
                FeaturedProducts = featuredProducts.ToList(),
                HotProducts = hotProducts.ToList(),
                Categories = categories.ToList()
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading home page data");
            // Return empty data if database is not available
            return View(new HomeViewModel
            {
                FeaturedProducts = new List<Product>(),
                HotProducts = new List<Product>(),
                Categories = new List<Category>()
            });
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
