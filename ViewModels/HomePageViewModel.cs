using NewsWebsite.Core.Models;

namespace NewsWebsite.ViewModels
{
    public class HomePageViewModel
    {
        public List<Category> Categories { get; set; } = new List<Category>();
        public List<Contact> Contacts { get; set; } = new List<Contact>();
        public Contact ContactForm { get; set; } = new Contact();
    }
}
