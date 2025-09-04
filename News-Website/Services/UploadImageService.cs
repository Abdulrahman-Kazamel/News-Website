using Microsoft.AspNetCore.Hosting;

namespace NewsWebsite.Services
{
    public class UploadImageService 
    {


        private readonly ILogger<UploadImageService> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private const string UploadFolder = "assets/img/gallery";


        public UploadImageService
            (ILogger<UploadImageService> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        /*
         the Idea behind file uploading,
        1- extenation {check your vaild extenations contained on the comming through path Method}
        2- check file lenght size
        3- get the file path and change it then convert the file bytes recived from the Iformfile 
        to a stream. 
        /// hint
        also the stream should be disposded and closed after saving proccess,
        so through "using" keyword do that on one step.


         
         */


        public string? UploadImage(IFormFile file)
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
