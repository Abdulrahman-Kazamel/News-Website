using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data;
using NewsWebsite.Models;
using NewsWebsite.ViewModels;

namespace NewsWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        public IActionResult Index()
        {
            var newsPosts = _context.NewsPosts
                .Include(p => p.Category)
                .OrderByDescending(p => p.Id)
                .ToList();

            return View(newsPosts);
        }

        public IActionResult Create()
        {
            var viewModel = new NewsPostViewModel
            {
                Categories = GetCategoryList(),
                Date = DateTime.Now
            };

            return View(viewModel);
        }

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



        //public IActionResult Edit(int id)
        //{
        //    var newsPost = _context.NewsPosts
        //        .Include(p => p.Category)
        //        .FirstOrDefault(p => p.Id == id);
        //    return View();
        //}

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
