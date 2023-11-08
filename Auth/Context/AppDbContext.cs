using Auth.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Context
{
   public class AppDbContext:DbContext
    {
        public DbSet<AppUser> AppUsers  { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> option):base(option)
        {
                
        }
    }
}
