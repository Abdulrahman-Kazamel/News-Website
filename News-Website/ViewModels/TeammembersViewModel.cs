using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebsite.ViewModels
{
    public class TeammembersViewModel
    {

        public string? Name { get; set; } = string.Empty;
        public string? JobTitle { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;

        [NotMapped]
        public IFormFile File { get; set; } = default!;
    }
}
