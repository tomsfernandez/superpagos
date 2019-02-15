using System;

namespace VisaApi.Dto {
    public class PaymentResponse {
        public DateTime Timestamp { get; set; }
        public string OperationId { get; set; }
        public int Code { get; set; }
        public string Message { get; set; }

        public bool IsOk() {
            return Code == 200;
        }

        public bool IsBadRequest() {
            return Code == 400;
        }

        public bool IsError() {
            return Code == 500;
        }
    }
}