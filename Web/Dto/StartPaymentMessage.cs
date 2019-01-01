using Web.Model.Domain;

namespace Web.Dto {
    public class StartPaymentMessage {

        public double Amount { get; set; }
        public PayMessageInfo Payer { get; set; }
        public PayMessageInfo Paid { get; set; }
    }
}