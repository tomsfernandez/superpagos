using Web.Model.Domain;

namespace Web.Tests.Helpers {
    public class PaymentMethodFactory {

        public static PaymentMethod GetVisaPaymentMethod(User user = null, string token = "aToken") {
            if (user == null) user = UserFactory.GetJaimito();
            var provider = ProviderFactory.GetVisa();
            return new PaymentMethod {Token = token, User = user, Provider = provider};
        }

        public static PaymentMethod GetMasterCardPaymentMethod(User user = null, string token = "aToken") {
            if (user == null) user = UserFactory.GetJuancito();
            var provider = ProviderFactory.GetMasterCard();
            return new PaymentMethod {Token = token, User = user, Provider = provider};
        }
    }
}