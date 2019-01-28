using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Web.Tests.Helpers {
    public class TestHelper {
        public static AppDbContext MakeContext() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new AppDbContext(options);
        }

        public static IMapper CreateAutoMapper() {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>());
            return new Mapper(config);
        }
    }
}