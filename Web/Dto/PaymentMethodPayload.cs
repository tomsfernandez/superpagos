using System;
using Web.Model.Domain;

namespace Web.Dto {
    public class PaymentMethodPayload {

        public string ProviderCode { get; set; }
        public string OperationTokenFromProvider { get; set; }
    }
}