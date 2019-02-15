using System.Net.Mime;
using FluentAssertions;
using Web.Dto;
using Web.Model.Domain;
using Web.Tests.Helpers;
using Xunit;

namespace Web.Tests.Dto
{
    public class PaymentDtoTest
    {
        private AppDbContext Context { get; }

        private PaymentMethod PayerPaymentMethod { get; set; }

        private PaymentMethod PaidPaymentMethod { get; set; }

        private PaymentButton PaidPaymentButton { get; set; }

        private PaymentDto PaymentDto { get; set; }

        public PaymentDtoTest()
        {
            PayerPaymentMethod = PaymentMethodFactory.GetVisaPaymentMethod();

            PaidPaymentMethod = PaymentMethodFactory.GetMasterCardPaymentMethod();
            PaidPaymentButton = PaymentButtonFactory.GetPaymentButton(PaidPaymentMethod);

            Context = TestHelper.MakeContext();
            Context.PaymentMethods.AddRange(PayerPaymentMethod, PaidPaymentMethod);
            Context.PaymentButtons.Add(PaidPaymentButton);
            Context.SaveChanges();
        }

        [Fact]
        public void test_01_payment_dto_is_valid()
        {
            PaymentDto = new PaymentDto
                {ButtonId = PaidPaymentButton.Id, PaymentMethodId = PayerPaymentMethod.Id, Description = "description"};
            var noErrors = PaymentDto.Validate(Context);
            noErrors.Should().BeEmpty();
        }

        [Fact]
        public void test_02_payment_dto_without_button_id_is_not_valid()
        {
            PaymentDto = new PaymentDto {PaymentMethodId = PayerPaymentMethod.Id, Description = "description"};
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(2);
        }

        [Fact]
        public void test_03_payment_dto_without_payment_method_id_is_not_valid()
        {
            PaymentDto = new PaymentDto {ButtonId = PaidPaymentButton.Id, Description = "description"};
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(2);
        }

        [Fact]
        public void test_04_payment_dto_without_description_is_not_valid()
        {
            PaymentDto = new PaymentDto {ButtonId = PaidPaymentButton.Id, PaymentMethodId = PayerPaymentMethod.Id};
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_05_payment_dto_with_nonexistent_payment_method_is_not_valid()
        {
            PaymentDto = new PaymentDto {ButtonId = PaidPaymentButton.Id, Description = "description"};
            PaymentDto.PaymentMethodId = 8888;
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_06_payment_dto_with_nonexistent_button_is_not_valid()
        {
            PaymentDto = new PaymentDto {PaymentMethodId = PayerPaymentMethod.Id, Description = "description"};
            PaymentDto.ButtonId = 8888;
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }

        [Fact]
        public void test_07_payment_dto_with_same_payer_as_paid_agent_is_not_valid()
        {
            PaymentDto = new PaymentDto {Description = "description"};
            PaymentDto.PaymentMethodId = PaidPaymentMethod.Id;
            PaymentDto.ButtonId = PaidPaymentButton.Id;
            var errors = PaymentDto.Validate(Context);
            errors.Should().NotBeEmpty();
            errors.Count.Should().Be(1);
        }
    }
}