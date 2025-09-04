using NewsWebsite.Core.Models;

namespace NewsWebsite.Core.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        bool CategoryExists(int CategoryId);
       
    }
}
