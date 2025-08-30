using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Context;
using NewsWebsite.Core.Interfaces;
using NewsWebsite.Core.Models;

namespace NewsWebsite.Core.Repositories
{
    public class NewsPostRepository : Repository<NewsPost>, INewsPostRepository
    {
        private readonly ApplicationDbContext _context;
        public NewsPostRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }



        public async Task<IEnumerable<NewsPost>> GetCategoryByIdAsync(int CategoryId)
        {
            return await _context.NewsPosts.Where(newsPost => newsPost.CategoryId == CategoryId).ToListAsync();
        }








    }
}
