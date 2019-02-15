using FluentAssertions;
using Web.Dto;
using Xunit;

namespace Web.Tests.Dto {
    public class LoginCredentialsTest {
        [Fact]
        public void test_01_complete_login_credentials_dto_is_valid() {
            var dto = new LoginCredentials {Email = "email", Password = "password"};
            var noErrors = dto.Validate();
            noErrors.Should().BeEmpty();
        }

        [Fact]
        public void test_02_login_credentials_dto_without_email_is_not_valid() {
            var dto = new LoginCredentials {Password = "password"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_03_login_credentials_dto_without_password_is_not_valid() {
            var dto = new LoginCredentials {Email = "email"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_04_empty_login_credentials_dto_is_not_valid() {
            var dto = new LoginCredentials();
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(2);
        }
    }
}