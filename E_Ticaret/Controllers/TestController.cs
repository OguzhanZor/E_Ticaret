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
    }
}
