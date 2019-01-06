using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Controllers;
using Web.Dto;
using Web.Model.Domain;
using Xunit;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Web.Tests.UseCases {
    public class LoginTest : IDisposable{

        private AppDbContext Context { get; }
        private UsersController UsersController { get; }
        private AuthenticationController AuthController { get; }
        private IConfiguration Config = Startup.Configuration;
        private IMapper Mapper { get; }
        private UserDto Jaimito { get; }

        public LoginTest() {
            Context = TestHelper.MakeContext();
            Mapper = TestHelper.CreateAutoMapper();
            UsersController = new UsersController(Context, Mapper, Config);
            AuthController = new AuthenticationController(Context, Config);
            Jaimito = new UserDto {
                Name = "Jaimito Ramón Tercero",
                Email = "jaimito_ramon@superpagos.com",
                Password = "un_password_re_seguro",
                Role = Role.USER
            };
        }

        [Fact]
        public async void test_01_jaimito_can_login() {
            await RegisterJaimito();
            var loginCredentials = new LoginCredentials{Email = Jaimito.Email, Password = Jaimito.Password};
            var result = AuthController.Login(loginCredentials) as ObjectResult;
            result.Should().NotBeNull();
            if (result == null) return;
            result.StatusCode.Should().Be(Status200OK);
            var tokens = result.Value as LoginTokens;
            tokens.Should().NotBeNull();
            if (tokens == null) return;
            tokens.LongLivedToken.Should().NotBeNullOrEmpty();
            tokens.ShortLivedToken.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async void test_02_jaimito_cant_login_because_password_is_wrong() {
            await RegisterJaimito();
            var loginCredentials = new LoginCredentials {Email = Jaimito.Email, Password = "a_wrong_email"};
            var result = AuthController.Login(loginCredentials) as StatusCodeResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(Status401Unauthorized);
        }

        [Fact]
        public async void test_03_jaimito_cant_login_because_email_doesnt_exist() {
            await RegisterJaimito();
            var loginCredentials = new LoginCredentials {Email = "anotherEmail@hotmail.com", Password = Jaimito.Password};
            var result = AuthController.Login(loginCredentials) as StatusCodeResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(Status401Unauthorized);
        }

        private async Task RegisterJaimito() {
            var result = await UsersController.Post(Jaimito) as ObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(Status200OK);
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
        }
    }
}