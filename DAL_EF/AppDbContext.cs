using DAL_EF.Entity;
using Microsoft.EntityFrameworkCore;

namespace DAL_EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {}

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                FirstName = "John",
                LastName = "Smith",
                GoogleId = "asdf",
                Email = "asdf@asdf.com",
                PictureUrl = "no"
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
