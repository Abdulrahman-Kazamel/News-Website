using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Interfaces;
using NewsWebsite.Core.Models;

namespace NewsWebsite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly INewsPostRepository _NewsPostsRepository;

        public CategoriesController(
            ICategoryRepository CategoryRepository, INewsPostRepository NewsPostRepository)
        {
            _CategoryRepository = CategoryRepository;
            _NewsPostsRepository = NewsPostRepository;
        }


        public async Task<IActionResult> Index()
        {
            return View(await _CategoryRepository.GetAllAsync());
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {

            var Category = await _CategoryRepository.GetByIdAsync(id);


            //ViewBag.Category = Category.Name;
            //return View(_context.NewsPosts.Where(newsPost => newsPost.CategoryId == id).ToList());
            return View(Category);
        }

        [Authorize]
        public async Task<IActionResult> CategoryNews(int id)
        {

            //this need to think about bussiness logic later
            //for now if id is 0 or null return all news posts from all categories
            if (id == 0 || id == null)
            {

                return View(_NewsPostsRepository.GetAllAsync());

                //_context.NewsPosts.ToList());
            }
            var Category = await _CategoryRepository.GetByIdAsync(id);
            ViewBag.Category = Category.Name;
            return View(_NewsPostsRepository.GetCategoryByIdAsync(id));


        }





        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TitleIcon,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                _CategoryRepository.AddAsync(category);

                await _CategoryRepository.SaveAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _CategoryRepository.GetByIdAsync(id.Value);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleIcon,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _CategoryRepository.Update(category);
                    await _CategoryRepository.SaveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_CategoryRepository.CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _CategoryRepository.GetByIdAsync(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

       

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _CategoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                _CategoryRepository.Delete(category);
            }

            await _CategoryRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

       







    }
}
