using Web.Model.Domain;

namespace Web.Tests.Helpers {
    public class PaymentButtonFactory {

        public static PaymentButton GetPaymentButton(PaymentMethod method, string code = "aCode") {
            return new PaymentButton {Method = method, Code = code};
        }
    }
}