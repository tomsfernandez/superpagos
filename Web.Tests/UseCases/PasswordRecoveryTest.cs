using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Configuration;
using Web.Controllers;
using Web.Dto;
using Web.Model;
using Web.Model.Domain;
using Web.Service.Email;
using Web.Tests.Service;
using Xunit;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace Web.Tests.UseCases {
    public class PasswordRecoveryTest : IDisposable{

        private UsersController UsersController { get; }
        private PasswordRecoveryController RecoveryController { get; }
        private AuthenticationController AuthController { get; }
        private UserDto Jaimito { get; }
        private AppDbContext Context { get; }
        private IMapper Mapper { get; }
        private IConfiguration Config { get; } = Startup.Configuration;
        private EmailSender EmailSender { get; }

        public PasswordRecoveryTest() {
            Context = TestHelper.MakeContext();
            Mapper = TestHelper.CreateAutoMapper();
            UsersController = new UsersController(Context, Mapper, Config);
            EmailSender = new StubEmailSender(() => new List<object>());
            RecoveryController = new PasswordRecoveryController(Context, Config, EmailSender);
            AuthController = new AuthenticationController(Context, Config);
            Jaimito = new UserDto {
                Name = "Jaimito Ramón Tercero",
                Email = "jaimito_ramon@superpagos.com",
                Password = "un_password_re_seguro",
                Role = Role.USER
            };
        }

        [Fact]
        public async void test_01_juancito_resets_password() {
            await RegisterJaimito();
            var result = await RecoveryController.SendResetToken(Jaimito.Email) as ObjectResult;
            var url = result.Value as string;
            url.Should().NotBeNullOrEmpty();
            var credentials = GetCredentialsFrom(url, "aDifferentPassword");
            var resultResult = await RecoveryController.ResetPassword(credentials) as OkResult;
            resultResult.Should().NotBeNull();
            resultResult.StatusCode.Should().Be(Status200OK);
            var loginResult = AuthController.Login(new LoginCredentials
                {Email = Jaimito.Email, Password = "aDifferentPassword"}) as OkObjectResult;
            loginResult.StatusCode.Should().Be(Status200OK);
        }

        [Fact]
        public async void test_02_that_non_existant_email_returns_ok_without_url() {
            await RegisterJaimito();
            var result = await RecoveryController.SendResetToken("aDifferentEmail@hotmail.com") as OkResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async void test_03_email_is_not_sent_if_email_doesnt_exist() {
            await RegisterJaimito();
            var executedEmailSending = false;
            var emailSender = new StubEmailSender(() => {
                executedEmailSending = true;
            });
            var controller = new PasswordRecoveryController(Context, Config, emailSender);
            var result = await controller.SendResetToken("aDifferentEmail") as OkResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            executedEmailSending.Should().BeFalse();
        }

        private RecoveryCredential GetCredentialsFrom(string url, string password) {
            var urlParts = url.Substring(1, url.Length-1).Split("/");
            var id = long.Parse(urlParts[0]);
            var token = urlParts[1];
            return new RecoveryCredential {
                Id = id,
                Token = token,
                Password = password,
                ConfirmedPassword = password
            };
        }

        private async Task RegisterJaimito() {
            var result = await UsersController.Post(Jaimito) as ObjectResult;
            result.Should().NotBe(null);
            if (result != null) result.StatusCode.Should().Be(Status200OK);
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
        }
    }
}