using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data;

namespace NewsWebsite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        //public IActionResult Index()
        //{
        //    var Categories = _context.categories.ToList();
        //    return View(Categories);
        //}

        [Authorize]
        public IActionResult Details(int id)
        { 
        
            var Category = _context.Categories.Find(id);
            ViewBag.Category = Category.Name;
            return View(_context.NewsPosts.Where(newsPost => newsPost.CategoryId == id).ToList());
        }



     


    }
}
