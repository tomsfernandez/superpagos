using System;
using Web.Dto;

namespace Web.Tests.Helpers {
    public class PaymentResponseFactory {

        public static PaymentResponse GetSuccesfullResponse(string operationId) {
            return new PaymentResponse {
                Code = 200,
                Message = "",
                Timestamp = DateTime.Now,
                OperationId = operationId
            };
        }

        public static PaymentResponse GetFailedResponse(string operationId) {
            return new PaymentResponse {
                Code = 400,
                Message = "",
                Timestamp = DateTime.Now,
                OperationId = operationId
            };
        }
    }
}