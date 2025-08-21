using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Core.Models;

namespace NewsWebsite.Core.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

       
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<NewsPost> NewsPosts { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }
      
    }
}
