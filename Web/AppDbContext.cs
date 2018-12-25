using Microsoft.EntityFrameworkCore;
using Web.Dto;
using Web.Model.Domain;

namespace Web {
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
    }
}