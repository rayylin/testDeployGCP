using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using testDeployGCP.Models;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace testDeployGCP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IConfiguration configuration, ILogger<HomeController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }


        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> InsertCloudSql()
        {
            try
            {
                var connectionString = _configuration["ConnectionStringGCS"]
                      ?? Environment.GetEnvironmentVariable("ConnectionStringGCS");

                _logger.LogInformation("Using connection string: {conn}", connectionString);

                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("InsertGcpSql", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    await conn.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();
                }

                return Json(new { success = true, message = "Stored procedure executed." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SQL connection failed.");
                return Json(new { success = false, message = ex.Message });
            }
        }


        public IActionResult Index()
        {
            return View();
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
}
