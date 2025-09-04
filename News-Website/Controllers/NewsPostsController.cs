using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Models;



namespace NewsWebsite.Controllers
{

    public class NewsPostsController : Controller
    {
        private readonly INewsPostRepository _newsPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UploadImageService _uploadImageService;
        private readonly ILogger<NewsPostsController> _logger;


        public NewsPostsController(INewsPostRepository newsPostRepository, ICategoryRepository categoryRepository, UploadImageService uploadImageService, ILogger<NewsPostsController> logger)
        {
            _newsPostRepository = newsPostRepository;
            _categoryRepository = categoryRepository;
            _uploadImageService = uploadImageService;

            _logger = logger;
        }
        /*
                        Index
         
         */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {

            var newsPosts = await _newsPostRepository.GetAllNewsCategoryAsync();

            return View(newsPosts);
        }



        /*
                      Create

       */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {

            var Categories = await _categoryRepository.GetAllAsync();
            var viewModel = new NewsPostViewModel
            {
                Categories = new SelectList(Categories, "Id", "Name"),
                Date = DateTime.Now
            };

            return View("CreateEditPostViewForm", viewModel);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsPostViewModel viewModel)
        {
                var Categories = await _categoryRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                //Categories = await _categoryRepository.GetAllAsync();
                LogModelStateErrors(); // Debug log

                viewModel.Categories = new SelectList(Categories, "Id", "Name");
                return View("CreateEditPostViewForm", viewModel);
            }

            string? uploadedFileName = _uploadImageService.UploadImage(viewModel.File);

            if (string.IsNullOrEmpty(uploadedFileName))
            {
                ModelState.AddModelError("File", "Please select a valid image file (JPG, JPEG, PNG, GIF) under 4MB.");
                 //Categories = await _categoryRepository.GetAllAsync();
                viewModel.Categories = new SelectList(Categories, "Id", "Name");
                return View("CreateEditPostViewForm", viewModel);
            }
            //I need auto mapper here.

            var newsPost = new NewsPost
            {
                Id = viewModel.Id,
                Title = viewModel.Title,
                Date = viewModel.Date,
                Image = uploadedFileName,
                Topic = viewModel.Topic,
                CategoryId = viewModel.CategoryId
            };

            try
            {

                await _newsPostRepository.AddAsync(newsPost);
                await _newsPostRepository.SaveAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving NewsPost.");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                 //Categories = await _categoryRepository.GetAllAsync();
                viewModel.Categories = new SelectList(Categories, "Id", "Name");
                return View("CreateEditPostViewForm", viewModel);
            }
        }



        /*
                     Edit

      */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var newsPost = await _newsPostRepository.GetByIdAsync(id);

            if (newsPost == null)
            {
                return NotFound();
            }

            var Categories = await _categoryRepository.GetAllAsync();


            var viewModel = new NewsPostViewModel
            {
                Id = newsPost.Id,
                Title = newsPost.Title,
                Date = newsPost.Date,
                Topic = newsPost.Topic,
                Image = newsPost.Image,
               
                CategoryId = newsPost.CategoryId,
                Categories = new SelectList(Categories, "Id", "Name")

            };

            return View("CreateEditPostViewForm", viewModel);
        }


        // POST: Edit
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewsPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                
                var Categories = await _categoryRepository.GetAllAsync();
                viewModel.Categories = new SelectList(Categories, "Id", "Name");

                return View("CreateEditPostViewForm", viewModel);
            }

            var newsPost = await _newsPostRepository.GetByIdAsync(id);
            if (newsPost == null) return NotFound();
            
            if(viewModel.File != null)
            {
            string? uploadedFileName = _uploadImageService.UploadImage(viewModel.File);
                // Handle image upload
                if (!string.IsNullOrEmpty(uploadedFileName))
                {
                    newsPost.Image = uploadedFileName;
                }


            }


            // Update fields
            newsPost.Title = viewModel.Title;
            newsPost.Date = viewModel.Date;
            newsPost.Topic = viewModel.Topic;
            newsPost.CategoryId = viewModel.CategoryId;
            //newsPost.Image = viewModel.Image;

            _newsPostRepository.Update(newsPost);
            await _newsPostRepository.SaveAsync();

            return RedirectToAction("Index");
        }

        /*
                     Details

      */



        public async Task<IActionResult> Details(int id)
        {


            var newsPost = await _newsPostRepository.GetByIdAsync(id);



            return View(newsPost);
        }


        /*
                      Delete

       */

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var newsPost = await _newsPostRepository.GetByIdAsync(id);
            if (newsPost == null)
            {
                return NotFound();
            }

            _newsPostRepository.Delete(newsPost);

            await _newsPostRepository.SaveAsync();
            return RedirectToAction("Index");
        }






        private void LogModelStateErrors()
        {
            _logger.LogWarning("Model validation failed. Listing all errors:");

            foreach (var entry in ModelState)
            {
                var key = entry.Key;
                foreach (var error in entry.Value.Errors)
                {
                    _logger.LogWarning($"[ModelState] Field: {key} - Error: {error.ErrorMessage}");
                }
            }
        }









    }
}
