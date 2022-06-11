
using ShvedovaAV.Services;

namespace ShvedovaAV.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Slider> Sliders { get; set; } = null!;
        public DbSet<Feedback> Feedbacks { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Content> Contents { get; set; } = null!;
        public DbSet<ContentAccess> ContentAccesses { get; set; } = null!;
 
        public ApplicationContext(DbContextOptions<ApplicationContext> options, CountService countService)
        : base (options)
        {
            if (countService.Count == 0)
            {
                Database.EnsureDeleted();
                string pathVideos = Directory.GetCurrentDirectory() + "/wwwroot/Files/Videos/Content";
                string pathFeedback = Directory.GetCurrentDirectory() + "/wwwroot/Files/Images/Feedback";
                string pathProduct = Directory.GetCurrentDirectory() + "/wwwroot/Files/Images/Product";
                string pathSlider = Directory.GetCurrentDirectory() + "/wwwroot/Files/Images/Slider";
                string[] paths = { pathVideos, pathFeedback, pathProduct, pathSlider };
                foreach (var item in paths)
                {
                    if (Directory.Exists(item))
                    {
                        Directory.Delete(item, true);
                    }
                    Directory.CreateDirectory(item);
                }
            }
            Database.EnsureCreated();
            countService.Counting();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin"},
                new Role { Id = 2, Name = "User"}
                );
            modelBuilder.Entity<User>().HasData(
                 new User
                 {
                     Id = 1,
                     Name = "asdasd",
                     Email = "krskagent@mail.ru",
                     Phone = "89131743699",
                     Password = HashService.GetHash("1"),
                     RoleId = 1,
                 },
                 new User
                 {
                     Id = 2,
                     Name = "wefwewef",
                     Email = "lexus747@mail.ru",
                     Phone = "8923423433499",
                     Password = HashService.GetHash("1"),
                     RoleId = 2,
                 }
                );
        }
    }
}
