using System.Collections.Generic;
using Web.Model.Domain;
using static System.String;

namespace Web.Dto {
    public class ProviderDto {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Code { get; set; }
        public string RollbackEndPoint { get; set; }
        public string PaymentEndpoint { get; set; }
        public string LinkEndpoint { get; set; }
        
        public List<string> Validate() {
            var errors = new List<string>();
            if (IsNullOrEmpty(Name)) errors.Add("Name es nulo o vacío");
            if (IsNullOrEmpty(Company)) errors.Add("Company es nulo o vacío");
            if (IsNullOrEmpty(Code)) errors.Add("Code es nulo o vacío");
            if (IsNullOrEmpty(RollbackEndPoint)) errors.Add("Endpoint es nulo o vacío");
            if (IsNullOrEmpty(PaymentEndpoint)) errors.Add("PaymentEndpoint es nulo o vacío");
            if (IsNullOrEmpty(LinkEndpoint)) errors.Add("LinkEndpoint es nulo o vacío");
            return errors;
        }
    }
}