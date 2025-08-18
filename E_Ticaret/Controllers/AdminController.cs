using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using E_Ticaret.Repositories;
using E_Ticaret.Models;
using E_Ticaret.Services;

namespace E_Ticaret.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBannerRepository _bannerRepository;
        private readonly ISliderRepository _sliderRepository;
        private readonly IImageService _imageService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(
            IUserRepository userRepository,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IBannerRepository bannerRepository,
            ISliderRepository sliderRepository,
            IImageService imageService,
            ILogger<AdminController> logger)
        {
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _bannerRepository = bannerRepository;
            _sliderRepository = sliderRepository;
            _imageService = imageService;
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
        public async Task<IActionResult> CreateBanner([Bind("Title,Subtitle,ButtonText,ButtonUrl,DisplayOrder,IsActive,StartDate,EndDate")] Banner banner, IFormFile imageFile, IFormFile mobileImageFile)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Ana görsel yükleme
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        banner.ImageUrl = await _imageService.UploadImageAsync(imageFile, "banner");
                    }
                    else
                    {
                        ModelState.AddModelError("ImageUrl", "Ana görsel zorunludur");
                        return View("Create", banner);
                    }

                    // Mobil görsel yükleme (opsiyonel)
                    if (mobileImageFile != null && mobileImageFile.Length > 0)
                    {
                        banner.MobileImageUrl = await _imageService.UploadImageAsync(mobileImageFile, "banner");
                    }

                    banner.CreatedAt = DateTime.Now;
                    await _bannerRepository.CreateAsync(banner);
                    TempData["Success"] = "Banner başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Banners));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating banner");
                    TempData["Error"] = "Banner oluşturulurken hata oluştu: " + ex.Message;
                }
            }
            return View("Create", banner);
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
        public async Task<IActionResult> EditBanner(int id, [Bind("Id,Title,Subtitle,ButtonText,ButtonUrl,DisplayOrder,IsActive,StartDate,EndDate")] Banner banner, IFormFile imageFile, IFormFile mobileImageFile)
        {
            if (id != banner.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut banner'ı al
                    var existingBanner = await _bannerRepository.GetByIdAsync(id);
                    if (existingBanner == null) return NotFound();

                    // Ana görsel güncelleme
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Eski görseli sil
                        if (!string.IsNullOrEmpty(existingBanner.ImageUrl))
                        {
                            await _imageService.DeleteImageAsync(existingBanner.ImageUrl);
                        }
                        // Yeni görseli yükle
                        banner.ImageUrl = await _imageService.UploadImageAsync(imageFile, "banner");
                    }
                    else
                    {
                        // Mevcut görseli koru
                        banner.ImageUrl = existingBanner.ImageUrl;
                    }

                    // Mobil görsel güncelleme
                    if (mobileImageFile != null && mobileImageFile.Length > 0)
                    {
                        // Eski mobil görseli sil
                        if (!string.IsNullOrEmpty(existingBanner.MobileImageUrl))
                        {
                            await _imageService.DeleteImageAsync(existingBanner.MobileImageUrl);
                        }
                        // Yeni mobil görseli yükle
                        banner.MobileImageUrl = await _imageService.UploadImageAsync(mobileImageFile, "banner");
                    }
                    else
                    {
                        // Mevcut mobil görseli koru
                        banner.MobileImageUrl = existingBanner.MobileImageUrl;
                    }

                    banner.UpdatedAt = DateTime.Now;
                    await _bannerRepository.UpdateAsync(banner);
                    TempData["Success"] = "Banner başarıyla güncellendi.";
                    return RedirectToAction(nameof(Banners));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating banner");
                    TempData["Error"] = "Banner güncellenirken hata oluştu: " + ex.Message;
                }
            }
            return View("Edit", banner);
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
                // Banner'ı al
                var banner = await _bannerRepository.GetByIdAsync(id);
                if (banner == null)
                {
                    TempData["Error"] = "Banner bulunamadı.";
                    return RedirectToAction(nameof(Banners));
                }

                // Görselleri sil
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(banner.ImageUrl);
                }
                if (!string.IsNullOrEmpty(banner.MobileImageUrl))
                {
                    await _imageService.DeleteImageAsync(banner.MobileImageUrl);
                }

                // Banner'ı sil
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
                TempData["Error"] = "Banner silinirken hata oluştu: " + ex.Message;
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

        // SLIDER MANAGEMENT
        public async Task<IActionResult> Sliders()
        {
            try
            {
                var sliders = await _sliderRepository.GetAllAsync();
                return View("Sliders", sliders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading sliders");
                TempData["Error"] = "Slider'lar yüklenirken hata oluştu.";
                return View("Sliders", new List<Slider>());
            }
        }

        public IActionResult CreateSlider()
        {
            return View("CreateSlider", new Slider());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSlider([Bind("Title,Subtitle,ButtonText,ButtonUrl,DisplayOrder,IsActive,StartDate,EndDate,ImageUrl,MobileImageUrl")] Slider slider, IFormFile imageFile, IFormFile mobileImageFile)
        {
            // Debug: Gelen verileri logla
            _logger.LogInformation($"CreateSlider POST - Title: {slider.Title}");
            _logger.LogInformation($"imageFile: {(imageFile != null ? imageFile.FileName : "null")}");
            _logger.LogInformation($"mobileImageFile: {(mobileImageFile != null ? mobileImageFile.FileName : "null")}");
            
            // ModelState hatalarını logla
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState validation failed:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Ana görsel yükleme
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        slider.ImageUrl = await _imageService.UploadImageAsync(imageFile, "slider");
                    }
                    else
                    {
                        ModelState.AddModelError("ImageUrl", "Ana görsel zorunludur");
                        return View("CreateSlider", slider);
                    }

                    // Mobil görsel yükleme (opsiyonel)
                    if (mobileImageFile != null && mobileImageFile.Length > 0)
                    {
                        slider.MobileImageUrl = await _imageService.UploadImageAsync(mobileImageFile, "slider");
                    }

                    slider.CreatedAt = DateTime.Now;
                    var id = await _sliderRepository.CreateAsync(slider);
                    TempData["Success"] = "Slider başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Sliders));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating slider");
                    TempData["Error"] = "Slider oluşturulurken hata oluştu: " + ex.Message;
                }
            }
            else
            {
                // ModelState hatalarını TempData'ya ekle
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                TempData["Error"] = "Validation hataları: " + string.Join(", ", errors);
            }
            return View("CreateSlider", slider);
        }

        public async Task<IActionResult> EditSlider(int id)
        {
            try
            {
                var slider = await _sliderRepository.GetByIdAsync(id);
                if (slider == null) return NotFound();
                return View("EditSlider", slider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading slider for edit");
                TempData["Error"] = "Slider yüklenirken hata oluştu.";
                return RedirectToAction(nameof(Sliders));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSlider(int id, [Bind("Id,Title,Subtitle,ButtonText,ButtonUrl,DisplayOrder,IsActive,StartDate,EndDate,CreatedAt,ImageUrl,MobileImageUrl")] Slider slider, IFormFile imageFile, IFormFile mobileImageFile)
        {
            // Debug: Gelen verileri logla
            _logger.LogInformation($"EditSlider POST - ID: {id}, Slider ID: {slider.Id}");
            _logger.LogInformation($"Title: {slider.Title}, Subtitle: {slider.Subtitle}");
            _logger.LogInformation($"ImageUrl: {slider.ImageUrl}, MobileImageUrl: {slider.MobileImageUrl}");
            _logger.LogInformation($"imageFile: {(imageFile != null ? imageFile.FileName : "null")}");
            _logger.LogInformation($"mobileImageFile: {(mobileImageFile != null ? mobileImageFile.FileName : "null")}");
            
            // ModelState hatalarını logla
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState validation failed:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Validation Error: {error.ErrorMessage}");
                    }
                }
            }

            if (id != slider.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    // Mevcut slider'ı al
                    var existingSlider = await _sliderRepository.GetByIdAsync(id);
                    if (existingSlider == null) return NotFound();

                    // Ana görsel güncelleme
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Eski görseli sil
                        if (!string.IsNullOrEmpty(existingSlider.ImageUrl))
                        {
                            await _imageService.DeleteImageAsync(existingSlider.ImageUrl);
                        }
                        // Yeni görseli yükle
                        slider.ImageUrl = await _imageService.UploadImageAsync(imageFile, "slider");
                    }
                    else
                    {
                        // Mevcut görseli koru
                        slider.ImageUrl = existingSlider.ImageUrl;
                    }

                    // Mobil görsel güncelleme
                    if (mobileImageFile != null && mobileImageFile.Length > 0)
                    {
                        // Eski mobil görseli sil
                        if (!string.IsNullOrEmpty(existingSlider.MobileImageUrl))
                        {
                            await _imageService.DeleteImageAsync(existingSlider.MobileImageUrl);
                        }
                        // Yeni mobil görseli yükle
                        slider.MobileImageUrl = await _imageService.UploadImageAsync(mobileImageFile, "slider");
                    }
                    else
                    {
                        // Mevcut mobil görseli koru
                        slider.MobileImageUrl = existingSlider.MobileImageUrl;
                    }

                    slider.UpdatedAt = DateTime.Now;
                    await _sliderRepository.UpdateAsync(slider);
                    TempData["Success"] = "Slider başarıyla güncellendi.";
                    return RedirectToAction(nameof(Sliders));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating slider");
                    TempData["Error"] = "Slider güncellenirken hata oluştu: " + ex.Message;
                }
            }
            else
            {
                // ModelState hatalarını TempData'ya ekle
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                TempData["Error"] = "Validation hataları: " + string.Join(", ", errors);
            }
            return View("EditSlider", slider);
        }

        public async Task<IActionResult> DeleteSlider(int id)
        {
            try
            {
                var slider = await _sliderRepository.GetByIdAsync(id);
                if (slider == null) return NotFound();
                return View("DeleteSlider", slider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading slider for delete");
                TempData["Error"] = "Slider yüklenirken hata oluştu.";
                return RedirectToAction(nameof(Sliders));
            }
        }

        [HttpPost, ActionName("DeleteSlider")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSliderConfirmed(int id)
        {
            try
            {
                // Slider'ı al
                var slider = await _sliderRepository.GetByIdAsync(id);
                if (slider == null)
                {
                    TempData["Error"] = "Slider bulunamadı.";
                    return RedirectToAction(nameof(Sliders));
                }

                // Görselleri sil
                if (!string.IsNullOrEmpty(slider.ImageUrl))
                {
                    await _imageService.DeleteImageAsync(slider.ImageUrl);
                }
                if (!string.IsNullOrEmpty(slider.MobileImageUrl))
                {
                    await _imageService.DeleteImageAsync(slider.MobileImageUrl);
                }

                // Slider'ı sil
                var result = await _sliderRepository.DeleteAsync(id);
                TempData[result ? "Success" : "Error"] = result ? "Slider başarıyla silindi." : "Slider silinirken hata oluştu.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting slider");
                TempData["Error"] = "Slider silinirken hata oluştu: " + ex.Message;
            }
            return RedirectToAction(nameof(Sliders));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSliderDisplayOrder(int id, int displayOrder)
        {
            try
            {
                var result = await _sliderRepository.UpdateDisplayOrderAsync(id, displayOrder);
                return Json(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating slider display order");
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
