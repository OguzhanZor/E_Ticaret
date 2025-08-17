using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Ticaret.Repositories;
using E_Ticaret.Models;

namespace E_Ticaret.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUserRepository userRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBannerRepository bannerRepository,
            ILogger<AdminController> logger)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var dashboardViewModel = new AdminDashboardViewModel
                {
                    TotalUsers = await _userRepository.GetTotalCountAsync(),
                    TotalProducts = await _productRepository.GetTotalCountAsync(),
                    TotalCategories = 0, // ICategoryRepository'de GetTotalCountAsync yok
                    TotalBanners = await _bannerRepository.GetTotalCountAsync()
                };

                return View(dashboardViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard");
                TempData["Error"] = "Dashboard yüklenirken hata oluştu.";
                return View(new AdminDashboardViewModel());
            }
        }

        public IActionResult Users()
        {
            return RedirectToAction("Index", "User");
        }

        public IActionResult Products()
        {
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Categories()
        {
            return RedirectToAction("Index", "Category");
        }

        // GET: Banner Listesi
        public async Task<IActionResult> Banners()
        {
            try
            {
                var banners = await _bannerRepository.GetAllAsync();
                return View("Banners", banners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading banners");
                TempData["Error"] = "Banner'lar yüklenirken hata oluştu.";
                return View("Banners", new List<Banner>());
            }
        }

        // GET: Banner/Create
        public IActionResult CreateBanner()
        {
            return View("Create", new Banner());
        }

        // POST: Banner/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBanner([Bind("Title,Subtitle,ButtonText,ButtonUrl,ImageUrl,MobileImageUrl,DisplayOrder,IsActive,StartDate,EndDate")] Banner banner)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    banner.CreatedAt = DateTime.Now;
                    await _bannerRepository.CreateAsync(banner);
                    TempData["Success"] = "Banner başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Banners));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating banner");
                    TempData["Error"] = "Banner oluşturulurken hata oluştu.";
                }
            }
            return View(banner);
        }

        // GET: Banner/Edit/5
        public async Task<IActionResult> EditBanner(int id)
        {
            try
            {
                var banner = await _bannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    return NotFound();
                }
                return View("Edit", banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading banner for edit");
                TempData["Error"] = "Banner yüklenirken hata oluştu.";
                return RedirectToAction(nameof(Banners));
            }
        }

        // POST: Banner/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBanner(int id, [Bind("Id,Title,Subtitle,ButtonText,ButtonUrl,ImageUrl,MobileImageUrl,DisplayOrder,IsActive,StartDate,EndDate")] Banner banner)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    banner.UpdatedAt = DateTime.Now;
                    await _bannerRepository.UpdateAsync(banner);
                    TempData["Success"] = "Banner başarıyla güncellendi.";
                    return RedirectToAction(nameof(Banners));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating banner");
                    TempData["Error"] = "Banner güncellenirken hata oluştu.";
                }
            }
            return View(banner);
        }

        // GET: Banner/Delete/5
        public async Task<IActionResult> DeleteBanner(int id)
        {
            try
            {
                var banner = await _bannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    return NotFound();
                }
                return View("Delete", banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading banner for delete");
                TempData["Error"] = "Banner yüklenirken hata oluştu.";
                return RedirectToAction(nameof(Banners));
            }
        }

        // POST: Banner/Delete/5
        [HttpPost, ActionName("DeleteBanner")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBannerConfirmed(int id)
        {
            try
            {
                var result = await _bannerRepository.DeleteAsync(id);
                if (result)
                {
                    TempData["Success"] = "Banner başarıyla silindi.";
                }
                else
                {
                    TempData["Error"] = "Banner silinirken hata oluştu.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting banner");
                TempData["Error"] = "Banner silinirken hata oluştu.";
            }
            return RedirectToAction(nameof(Banners));
        }

        // POST: Banner/UpdateDisplayOrder
        [HttpPost]
        public async Task<IActionResult> UpdateBannerDisplayOrder(int id, int displayOrder)
        {
            try
            {
                var result = await _bannerRepository.UpdateDisplayOrderAsync(id, displayOrder);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating banner display order");
                return Json(new { success = false, message = "Hata oluştu." });
            }
        }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCategories { get; set; }
        public int TotalBanners { get; set; }
    }
}
