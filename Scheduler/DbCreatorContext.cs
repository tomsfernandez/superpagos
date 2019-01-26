using Microsoft.EntityFrameworkCore;

namespace Scheduler {
    public sealed class DbCreatorContext : DbContext{
        public DbCreatorContext(DbContextOptions options) : base(options) {
            Database.Migrate();
        }
    }
}