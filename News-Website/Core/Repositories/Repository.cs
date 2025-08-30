using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Context;
using NewsWebsite.Core.Interfaces;

namespace NewsWebsite.Core.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbset;


        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbset = context.Set<T>();
            
        }


        public async Task<IEnumerable<T>> GetAllAsync() => await _dbset.ToListAsync();

        public async Task<T> GetByIdAsync(int id) => await _dbset.FindAsync(id);

        public async Task AddAsync(T entity) => await _dbset.AddAsync(entity);


        public void Update(T entity) => _dbset.Update(entity);


        public void Delete(T entity) => _dbset.Remove(entity);


        public async Task SaveAsync() => await _context.SaveChangesAsync();
    }
}
