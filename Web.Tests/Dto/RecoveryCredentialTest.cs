using FluentAssertions;
using Web.Dto;
using Xunit;

namespace Web.Tests.Dto
{
    public class RecoveryCredentialTest
    {
        [Fact]
        public void test_01_recovery_credential_is_valid()
        {
            var dto = new RecoveryCredential
                {Id = 1, Password = "password", Token = "token", ConfirmedPassword = "password"};
            var noErrors = dto.Validate();
            noErrors.Should().BeEmpty();
        }

        [Fact]
        public void test_02_recovery_credential_without_id_is_not_valid()
        {
            var dto = new RecoveryCredential {Password = "password", Token = "token", ConfirmedPassword = "password"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_03_recovery_credential_without_password_is_not_valid()
        {
            var dto = new RecoveryCredential {Id = 1, Token = "token", ConfirmedPassword = "password"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(2);
        }

        [Fact]
        public void test_04_recovery_credential_without_confirmed_password_is_not_valid()
        {
            var dto = new RecoveryCredential {Id = 1, Password = "password", Token = "token"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(2);
        }

        [Fact]
        public void test_05_recovery_credential_without_token_is_not_valid()
        {
            var dto = new RecoveryCredential {Id = 1, Password = "password", ConfirmedPassword = "password"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_06_recovery_credential_without_matching_passwords_is_not_valid()
        {
            var dto = new RecoveryCredential {Id = 1, Password = "password", Token = "token", ConfirmedPassword = "differentPassword"};
            var errors = dto.Validate();
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }
    }
}