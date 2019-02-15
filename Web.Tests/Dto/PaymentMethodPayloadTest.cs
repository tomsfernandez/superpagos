using FluentAssertions;
using Web.Dto;
using Web.Model.Domain;
using Web.Tests.Helpers;
using Xunit;
using ProviderFactory = Web.Tests.Helpers.ProviderFactory;

namespace Web.Tests.Dto
{
    public class PaymentMethodPayloadTest
    {
        private AppDbContext Context { get; set; }
        
        private Provider Provider { get; set; }
        
        public PaymentMethodPayloadTest()
        {
            Context = TestHelper.MakeContext();
            Provider = ProviderFactory.GetVisa();
            Context.Providers.Add(Provider);
            Context.SaveChanges();
        }

        [Fact]
        public void test_01_payment_method_payload_is_valid()
        {
            var dto = new PaymentMethodPayload {ProviderCode = Provider.Code, OperationTokenFromProvider = "tokenFromProvider"};
            var noErrors = dto.Validate(Context);
            noErrors.Should().BeEmpty();
        }

        [Fact]
        public void test_02_payment_method_payload_without_provider_code_is_not_valid()
        {
            var dto = new PaymentMethodPayload {OperationTokenFromProvider = "tokenFromProvider"};
            var errors = dto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_03_payment_method_payload_with_nonmatching_provider_is_not_valid()
        {
            var dto = new PaymentMethodPayload {OperationTokenFromProvider = "tokenFromProvider"};
            dto.ProviderCode = "NON_MATCHING_CODE";
            var errors = dto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_04_payment_method_payload_without_operation_token_from_provider_is_not_valid()
        {
            var dto = new PaymentMethodPayload {ProviderCode = Provider.Code};
            var errors = dto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }
    }
}