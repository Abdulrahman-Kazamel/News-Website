using NewsWebsite.Core.Models;

namespace NewsWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")] 
    public class TeamMembersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<TeamMembersController> _logger;


        private const long MaxFileSize = 4 * 1024 * 1024; // 4 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const string UploadFolder = "assets/img/testimonials";
        public TeamMembersController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, ILogger<TeamMembersController> logger)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }


        // GET: TeamMemberes
        public  IActionResult Index()
        {
            var TeamMembers = _context.TeamMembers.ToList();
            return View(TeamMembers);
        }

        // GET: TeamMemberes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // GET: TeamMemberes/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: TeamMemberes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TeammembersViewModel teamMember)
        {

            if (!ModelState.IsValid)
            {
                LogModelStateErrors(); // Debug log
                return View(teamMember);
            }

            string? uploadedFileName = UploadImage(teamMember.File);

            if (string.IsNullOrEmpty(uploadedFileName))
            {
                ModelState.AddModelError("File", "Please select a valid image file (JPG, JPEG, PNG, GIF) under 4MB.");

                return View(teamMember);
            }

            var TeamMember = new TeamMember
            {
                Name = teamMember.Name,
                JobTitle = teamMember.JobTitle,
                Image = uploadedFileName


            };

            try
            {
                _context.Add(TeamMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving NewsPost.");
                ModelState.AddModelError("", "An error occurred while saving. Please try again.");

                return View(teamMember);
            }









        }

        private void LogModelStateErrors()
        {
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogError(error.ErrorMessage);
            }
        }







        // GET: TeamMemberes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember == null)
            {
                return NotFound();
            }
            return View(teamMember);
        }

        // POST: TeamMemberes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,JobTitle,Image")] TeamMember teamMember)
        {
            if (id != teamMember.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamMember);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamMemberExists(teamMember.Id))
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
            return View(teamMember);
        }

        // GET: TeamMemberes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teamMember = await _context.TeamMembers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (teamMember == null)
            {
                return NotFound();
            }

            return View(teamMember);
        }

        // POST: TeamMemberes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teamMember = await _context.TeamMembers.FindAsync(id);
            if (teamMember != null)
            {
                _context.TeamMembers.Remove(teamMember);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamMemberExists(int id)
        {
            return _context.TeamMembers.Any(e => e.Id == id);
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
