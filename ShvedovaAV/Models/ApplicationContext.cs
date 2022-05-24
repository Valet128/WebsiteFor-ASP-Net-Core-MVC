using Microsoft.EntityFrameworkCore;

namespace ShvedovaAV.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
 
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base (options)
        {
            Database.EnsureCreated();
        }
    }
}
