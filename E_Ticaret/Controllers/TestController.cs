using Microsoft.AspNetCore.Mvc;
using E_Ticaret.Data;
using Dapper;

namespace E_Ticaret.Controllers
{
    public class TestController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<TestController> _logger;

        public TestController(DatabaseContext context, ILogger<TestController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                using var connection = _context.CreateConnection();
                
                // Test connection
                connection.Open();
                
                // Test simple query
                var result = connection.QueryFirstOrDefault<int>("SELECT 1");
                
                ViewBag.ConnectionStatus = "Başarılı";
                ViewBag.TestResult = result;
                ViewBag.ConnectionString = _context.ConnectionString;
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database connection failed");
                ViewBag.ConnectionStatus = "Başarısız";
                ViewBag.ErrorMessage = ex.Message;
                ViewBag.ConnectionString = _context.ConnectionString;
                
                return View();
            }
        }

        public IActionResult Banners()
        {
            try
            {
                using var connection = _context.CreateConnection();
                connection.Open();
                
                // Tüm banner'ları getir
                var banners = connection.Query<dynamic>(@"
                    SELECT Id, Title, Subtitle, ButtonText, ButtonUrl, ImageUrl, 
                           MobileImageUrl, DisplayOrder, IsActive, StartDate, EndDate, 
                           CreatedAt, UpdatedAt
                    FROM Banners 
                    ORDER BY DisplayOrder");
                
                ViewBag.Banners = banners;
                ViewBag.BannerCount = banners.Count();
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Banner query failed");
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }
    }
}
