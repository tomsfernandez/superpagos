using Web.Model.Domain;

namespace Web.Tests.Helpers {
    public class ProviderFactory {

        public static Provider GetVisa(string paymentEndpoint = "anotherEndpoint", string rollback = "anEndpoint", string linkEndpoint = "alinkEndpoint") {
            return new Provider {
                Code = "VISA",
                Company = "Visa",
                RollbackEndpoint = rollback,
                Name = "Visa",
                PaymentEndpoint = paymentEndpoint,
                LinkEndpoint = linkEndpoint
            };
        }

        public static Provider GetMasterCard(string paymentEndpoint = "anotherEndpoint", string rollback = "anEndpoint", string linkEndpoint = "alinkEndpoint") {
            return new Provider {
                Code = "MASTER_CARD",
                Company = "MasterCard",
                RollbackEndpoint = rollback,
                Name = "MasterCard",
                PaymentEndpoint = paymentEndpoint,
                LinkEndpoint = linkEndpoint
            };
        }
    }
}