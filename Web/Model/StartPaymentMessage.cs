using System;
using Web.Dto;
using Web.Model.Domain;

namespace Web.Model {
    public class StartPaymentMessage {

        public double Amount { get; set; }
        public string OperationId { get; set; }
        public Operation OperationType { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Token { get; set; }

        public static StartPaymentMessage Build(PaymentMethod method, PaymentDto dto) {
            var message = new StartPaymentMessage {
                Amount = dto.Amount,
                OperationType = Operation.DEPOSIT,
                OperationId = Guid.NewGuid().ToString(),
                TimeStamp = DateTime.Now,
                Token = method.Token
            };
            return message;
        }
    }
}