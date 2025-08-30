using NewsWebsite.Core.Models;
using System.Diagnostics;

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

        public async Task<IActionResult> Index()
        {
            var ViewModel = new HomePageViewModel
            {
                Categories = await _context.Categories.ToListAsync(),
                Contacts = await _context.Contacts.Where(c => !string.IsNullOrEmpty(c.Message)).ToListAsync(),
                ContactForm = new Contact()  
            };

            return View(ViewModel);
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


        public IActionResult ContactUs()
        {
            return PartialView("partial/_Contactus", new Contact());

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public IActionResult ContactUs(Contact contact)
        {

            if (!ModelState.IsValid)
            {
                return PartialView("partial/_Contactus", contact);
            }


            _context.Contacts.Add(contact);
            _context.SaveChanges();
            return RedirectToAction("Index");



        }




    }
}
