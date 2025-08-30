using NewsWebsite.Core.Models;

namespace NewsWebsite.Core.Interfaces
{
    public interface INewsPostRepository : IRepository<NewsPost>
    {
        public Task<IEnumerable<NewsPost>> GetCategoryByIdAsync(int CategoryId);
    }
}
