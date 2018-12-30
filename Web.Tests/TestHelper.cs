using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Web.Tests {
    public class TestHelper {
        public static AppDbContext MakeContext() {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("TestDb").Options;
            return new AppDbContext(options);
        }

        public static IMapper CreateAutoMapper() {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DomainProfile>());
            return new Mapper(config);
        }
    }
}