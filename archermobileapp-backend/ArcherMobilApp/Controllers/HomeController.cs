using System.Diagnostics;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using ArcherMobilApp.Models;

namespace ArcherMobilApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Users()
        {
            return View();
        }

        [HttpPost("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Documents()
        {
            return View();
        }

        [HttpPost("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Rooms()
        {
            return View();
        }

        [HttpPost("")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Announcements()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
