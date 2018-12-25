using Microsoft.EntityFrameworkCore;
using Web.Model.Domain;

namespace Web {
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}