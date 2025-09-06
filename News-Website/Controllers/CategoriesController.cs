using NewsWebsite.Core.Models;

namespace NewsWebsite.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly INewsPostRepository _NewsPostsRepository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository CategoryRepository,
            INewsPostRepository NewsPostRepository,
            ILogger<CategoriesController> logger)
        {
            _CategoryRepository = CategoryRepository;
            _NewsPostsRepository = NewsPostRepository;
            _logger = logger;
        }
        /*
                                Index
        */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return View(await _CategoryRepository.GetAllAsync());
        }

        /*
                                Details

         */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            //old implentantion
            //ViewBag.Category = Category.Name;
            //return View(_context.NewsPosts.Where(newsPost => newsPost.CategoryId == id).ToList());

            var Category = await _CategoryRepository.GetByIdAsync(id);
            return View(Category);
        }



        /*
                            Create

        */


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,TitleIcon,Name,Description")] Category category)
        {
            if(ModelState.IsValid)
            {
                await _CategoryRepository.AddAsync(category);
                await _CategoryRepository.SaveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        /*
                                    Edit

        */


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = await _CategoryRepository.GetByIdAsync(id.Value);

            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TitleIcon,Name,Description")] Category category)
        {
            if(id != category.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _CategoryRepository.Update(category);
                    await _CategoryRepository.SaveAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!_CategoryRepository.CategoryExists(category.Id))
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

        /*

                                    Delete
        */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var category = await _CategoryRepository.GetByIdAsync(id.Value);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }



        // POST
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _CategoryRepository.GetByIdAsync(id);
            if(category != null)
            {
                _CategoryRepository.Delete(category);
            }

            await _CategoryRepository.SaveAsync();
            return RedirectToAction(nameof(Index));
        }


        /*
                    Category News is the handles  all news posts for users
                    which I think to move it later inside news posts

        */


        [Authorize]
        public async Task<IActionResult> CategoryNews(int id)
        {


            //if id is not found return all news posts from all categories
            var Category = await _CategoryRepository.GetByIdAsync(id);
            if(Category == null)
            {
                _logger.LogWarning("⚠️ Category Called without id : #{id} on time : {time}", id,DateTime.Now);
                return View(await _NewsPostsRepository.GetAllAsync());
                //_context.NewsPosts.ToList());
            }
            _logger.LogInformation("Category Called without id : #{id}", id);
            ViewBag.Category = Category.Name;
            return View(await _NewsPostsRepository.GetCategoryByIdAsync(id));


        }





    }
}
