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
        public string ResponseEndpoint { get; set; }

        public static StartPaymentMessage Build(PaymentMethod method, 
            Operation operationType, double amount,
            string responseEndpoint, string operationId) {
            var message = new StartPaymentMessage {
                Amount = amount,
                OperationType = operationType,
                OperationId = operationId,
                TimeStamp = DateTime.Now,
                Token = method.Token,
                ResponseEndpoint = responseEndpoint
            };
            return message;
        }
    }
}