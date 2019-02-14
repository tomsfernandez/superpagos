using System;
using System.Collections.Generic;
using System.Security.Claims;
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
using Web.Model.JwtClaim;
using Web.Service.Email;
using Web.Tests.Helpers;
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
            UsersController = new UsersController(Context, Mapper, Config, 
                new SameClaimExtractorFactory(new List<Claim>()));
            EmailSender = new StubEmailSender(() => new List<object>());
            RecoveryController = new PasswordRecoveryController(Context, Config, EmailSender);
            AuthController = new AuthenticationController(Context, Config);
            Jaimito = UserFactory.GetJaimitoAsDto();
        }

        [Fact]
        public async void test_01_juancito_resets_password() {
            await RegisterJaimito();
            var resetToken = new ResetTokenDto {Email = Jaimito.Email};
            var result = await RecoveryController.SendResetToken(resetToken) as ObjectResult;
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
            var resetToken = new ResetTokenDto {Email = "aDifferentEmail@hotmail.com"};
            var result = await RecoveryController.SendResetToken(resetToken) as OkObjectResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            var url = result.Value as string;
            url.Should().BeNullOrEmpty();
        }

        [Fact]
        public async void test_03_email_is_not_sent_if_email_doesnt_exist() {
            await RegisterJaimito();
            var executedEmailSending = false;
            var emailSender = new StubEmailSender(() => {
                executedEmailSending = true;
            });
            var resetToken = new ResetTokenDto {Email = "aDifferentEmail"};
            var controller = new PasswordRecoveryController(Context, Config, emailSender);
            var result = await controller.SendResetToken(resetToken) as OkResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(200);
            executedEmailSending.Should().BeFalse();
        }

        private RecoveryCredential GetCredentialsFrom(string url, string password) {
            var urlParts = url.Split("/");
            var id = long.Parse(urlParts[1]);
            var token = urlParts[2];
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