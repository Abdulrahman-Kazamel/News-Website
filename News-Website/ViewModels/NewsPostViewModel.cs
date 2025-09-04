using Microsoft.AspNetCore.Mvc.Rendering;


namespace NewsWebsite.ViewModels
{
    public class NewsPostViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [StringLength(500, ErrorMessage = "Title cannot exceed 500 characters")]
        public string Title { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; } = default!;


        public string? Image { get; set; }
        [Required(ErrorMessage = "Topic is required")]
        [StringLength(2000, ErrorMessage = "Topic cannot exceed 2000 characters")]
        public string Topic { get; set; } = string.Empty;

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Please select a category")]
        public int CategoryId { get; set; }
       
        public   IEnumerable<SelectListItem>? Categories { get; set; } 

        //[Required(ErrorMessage = "Please select an image file")]
        [NotMapped]
        public IFormFile? File { get; set; } 

       
       


    }
}
