using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Web.Controllers;
using Web.Dto;
using Web.Model.JwtClaim;
using Web.Tests.Helpers;
using Xunit;

namespace Web.Tests.UseCases {
    public class RegisterTest : IDisposable, IClassFixture<DatabaseFixture> {

        private AppDbContext Context { get; }
        private UsersController Controller { get; }
        private IMapper Mapper { get; }
        private UserDto Jaimito { get; }
        private IConfiguration Config { get; } = Startup.Configuration;

        public RegisterTest(DatabaseFixture fixture) {
            Context = fixture.DatabaseContext;
            Mapper = TestHelper.CreateAutoMapper();
            Controller = new UsersController(Context, Mapper, Config, new SameClaimExtractorFactory(new List<Claim>()));
            Jaimito = UserFactory.GetJaimitoAsDto();
        }

        [Fact]
        public async void test_01_jaimito_can_register() {
            await RegisterJaimito();
            Context.Users.Any(x => x.Email.Equals(Jaimito.Email)).Should().BeTrue();
        }

        [Fact]
        public async void test_02_juancito_cant_register_because_email_is_taken() {
            await RegisterJaimito();
            var juancito = UserFactory.GetJuancitoAsDto(Jaimito.Email);
            var result = await RegisterJuancito(juancito);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Context.Users.Count().Should().Be(1);
        }

        [Fact]
        public async void test_03_juancito_cant_register_because_email_is_not_valid() {
            await RegisterJaimito();
            var juancito = UserFactory.GetJuancitoAsDto("turbio./com");
            var result = await RegisterJuancito(juancito);
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            Context.Users.Count().Should().Be(1);
        }

        [Fact]
        public async void test_04_juancito_cant_register_because_of_missing_info() {
            var juancito = UserFactory.GetJuancitoAsDto();
            juancito.Name = "";
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
            Context.Users.DeleteFromQuery();
        }
    }
}