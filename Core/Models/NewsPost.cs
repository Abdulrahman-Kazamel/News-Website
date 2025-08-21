using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsWebsite.Core.Models
{
    public class NewsPost
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Date { get; set; }

        public string Image { get; set; } = string.Empty;
        public string? Topic { get; set; }

        public int CategoryId { get; set; } = default!;
        public Category? Category { get; set; }

    }
}
