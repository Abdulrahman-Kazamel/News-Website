using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.Data;
using NewsWebsite.Models;

namespace NewsWebsite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
      
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {


            //var data = _context.categories.ToList();
            //return View(data);
            _logger.LogInformation("Home page requested.");
            var Categories = _context.Categories.ToList();
            return View(Categories);
        }

        public IActionResult About()
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
