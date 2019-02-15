using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Web.Tests.Helpers {
    public class TestHelper {

        public static IConfiguration Config { get; } = Startup.Configuration;


        public static AppDbContext MakeContext() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new AppDbContext(options);
        }

        public static AppDbContext MakePostgresContext() {
            var connectionString = Config.GetConnectionString ("DefaultConnection");
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(connectionString).Options;
            return new AppDbContext(options);
        }

        public static IMapper CreateAutoMapper() {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>());
            return new Mapper(config);
        }
    }
}