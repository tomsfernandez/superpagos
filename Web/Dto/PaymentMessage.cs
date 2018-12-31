using Microsoft.AspNetCore.JsonPatch.Operations;

namespace Web.Dto {
    public class PaymentMessage {

        public string OperationId { get; set; }
        public double Amount { get; set; }
        public Operation OperationType { get; set; }
        public string EndPoint { get; set; }
        public string OperationToken { get; set; }
    }
}