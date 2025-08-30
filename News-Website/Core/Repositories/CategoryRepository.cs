using NewsWebsite.Core.Context;
using NewsWebsite.Core.Interfaces;
using NewsWebsite.Core.Models;

namespace NewsWebsite.Core.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context; 
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool CategoryExists(int CategoryId)
        {
            return _context.Categories.Any(C => C.Id == CategoryId);
        }
    }
}
