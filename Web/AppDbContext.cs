using Microsoft.EntityFrameworkCore;
using Web.Model.Domain;

namespace Web {
    public class AppDbContext : DbContext{
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentButton> PaymentButtons { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Movement> Movements { get; set; }
    }
}