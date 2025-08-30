using Microsoft.AspNetCore.Mvc.Rendering;
using NewsWebsite.Core.Models;


namespace NewsWebsite.Areas.Admin.Controllers
{
    //[Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class NewsPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<NewsPostsController> _logger;

        private const long MaxFileSize = 4 * 1024 * 1024; // 4 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const string UploadFolder = "assets/img/gallery";

        public NewsPostsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<NewsPostsController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var newsPosts = _context.NewsPosts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(newsPosts);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var viewModel = new NewsPostViewModel
            {
                Categories = GetCategoryList(),
                Date = DateTime.Now
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                LogModelStateErrors(); // Debug log

                viewModel.Categories = GetCategoryList();
                return View(viewModel);
            }

            string? uploadedFileName = UploadImage(viewModel.File);

            if (string.IsNullOrEmpty(uploadedFileName))
            {
                ModelState.AddModelError("File", "Please select a valid image file (JPG, JPEG, PNG, GIF) under 4MB.");
                viewModel.Categories = GetCategoryList();
                return View(viewModel);
            }

            var newsPost = new NewsPost
            {
                Title = viewModel.Title,
                Date = viewModel.Date,
                Image = uploadedFileName,
                Topic = viewModel.Topic,
                CategoryId = viewModel.CategoryId
            };

            try
            {
                _context.NewsPosts.Add(newsPost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving NewsPost.");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                viewModel.Categories = GetCategoryList();
                return View(viewModel);
            }
        }



        public IActionResult Details(int id)
        {
            //var newsPost = new NewsPost
            //{
            //    Title = viewModel.Title,
            //    Date = viewModel.Date,
            //    Image = uploadedFileName,
            //    Topic = viewModel.Topic,
            //    CategoryId = viewModel.CategoryId
            //};
            var newsPost = _context.NewsPosts.FirstOrDefault(p => p.Id == id);

            //newsPost = new NewsPostViewModel
            //    {
            //    _context.NewsPosts.Include(p => p.Category)
                


            //};
            return View(newsPost);
        }


        [Authorize(Roles = "Admin")]
        // GET: Edit
        public IActionResult Edit(int id)
        {
            var newsPost = _context.NewsPosts.Find(id);

            if (newsPost == null)
            {
                return NotFound();
            }

            var viewModel = new NewsPostViewModel
            {
                Title = newsPost.Title,
                Date = newsPost.Date,
                Topic = newsPost.Topic,
                CategoryId = newsPost.CategoryId,
                Categories = GetCategoryList()
            };

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, NewsPostViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = GetCategoryList();
                return View(viewModel);
            }

            var newsPost = _context.NewsPosts.Find(id);
            if (newsPost == null)
            {
                return NotFound();
            }

            // Handle image upload
            var uploadedFileName = UploadImage(viewModel.File);
            if (!string.IsNullOrEmpty(uploadedFileName))
            {
                newsPost.Image = uploadedFileName;
            }

            // Update fields
            newsPost.Title = viewModel.Title;
            newsPost.Date = viewModel.Date;
            newsPost.Topic = viewModel.Topic;
            newsPost.CategoryId = viewModel.CategoryId;

            _context.NewsPosts.Update(newsPost);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var newsPost = _context.NewsPosts.Find(id);
            if (newsPost == null)
            {
                return NotFound();
            }
            _context.NewsPosts.Remove(newsPost);
            _context.SaveChanges();
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

        private List<SelectListItem> GetCategoryList()
        {
            return _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                })
                .ToList();
        }

        private string? UploadImage(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0 || file.Length > MaxFileSize)
                    return null;

                string extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(extension))
                    return null;

                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, UploadFolder);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                string uniqueFileName = Guid.NewGuid() + extension;
                string filePath = Path.Combine(uploadPath, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                return uniqueFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image.");
                return null;
            }
        }
    }
}
