using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Web.Tests.Helpers;

namespace Web.Tests {
    public class DatabaseFixture : IDisposable{

        public AppDbContext DatabaseContext {
            get {
                if (UsePostgresDb) {
                    return PostgresContext;
                }
                return TestHelper.MakeContext();
            }
        }

        private AppDbContext PostgresContext { get; }
        private IConfiguration Configuration { get; } = Startup.Configuration;
        private bool UsePostgresDb { get; }

        public DatabaseFixture() {
            UsePostgresDb = bool.Parse(Configuration["UsePostgres"] ?? "false");
            if (UsePostgresDb) {
                PostgresContext = TestHelper.MakePostgresContext();
                //PostgresContext.Database.Migrate();
            }
        }

        public void Dispose() {
            DatabaseContext.Database.EnsureDeleted();
            DatabaseContext.Dispose();
        }
    }
}