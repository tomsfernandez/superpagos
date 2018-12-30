using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Controllers;
using Web.Dto;
using Web.Model.Domain;
using Xunit;

namespace Web.Tests.UseCases {
    public class RegisterTest : IDisposable{

        private AppDbContext Context { get; set; }
        private UsersController Controller { get; set; }
        private IMapper Mapper { get; set; }
        private UserDto Jaimito { get; set; }
        private IConfiguration Config { get; } = Startup.Configuration;

        public RegisterTest() {
            Context = TestHelper.MakeContext();
            Mapper = TestHelper.CreateAutoMapper();
            Controller = new UsersController(Context, Mapper, Config);
            Jaimito = new UserDto {
                Name = "Jaimito Ramón Tercero",
                Email = "jaimito_ramon@superpagos.com",
                Password = "un_password_re_seguro",
                Role = Role.USER
            };
        }

        [Fact]
        public async void test_01_jaimito_can_register() {
            await RegisterJaimito();
            Context.Users.Any(x => x.Email.Equals(Jaimito.Email)).Should().BeTrue();
        }

        [Fact]
        public async void test_02_juancito_cant_register_because_email_is_taken() {
            await RegisterJaimito();
            var juancito = new UserDto { Name = "Juancito de la Vega", Email = Jaimito.Email, 
                Password = "el_zorro_zotudo", Role = Role.USER };
            var result = await RegisterJuancito(juancito);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Context.Users.Count().Should().Be(1);
        }

        [Fact]
        public async void test_03_juancito_cant_register_because_email_is_not_valid() {
            await RegisterJaimito();
            var juancito = new UserDto {
                Name = "Juancito de la Vega", Email = "turbio./com",
                Password = "el_zorro_zotudo", Role = Role.USER
            };
            var result = await RegisterJuancito(juancito);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Context.Users.Count().Should().Be(1);
        }

        [Fact]
        public async void test_04_juancito_cant_register_because_of_missing_info() {
            var juancito = new UserDto {
                Password = "el_zorro_zotudo", Role = Role.USER
            };
            var result = await RegisterJuancito(juancito);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Context.Users.Count().Should().Be(0);
        }

        private async Task<ObjectResult> RegisterJuancito(UserDto juancito) {
            var result = await Controller.Post(juancito) as ObjectResult;
            result.Should().NotBe(null);
            return result;
        }

        private async Task RegisterJaimito() {
            var result = await Controller.Post(Jaimito) as ObjectResult;
            result.Should().NotBe(null);
            if (result != null) result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
        }
    }
}