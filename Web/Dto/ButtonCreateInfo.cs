using System.Collections.Generic;

namespace Web.Dto {
    public class ButtonCreateInfo {
        
        public string Code { get; set; }
        public long PaymentMethodId { get; set; }
        public double Amount { get; set; }

        public List<string> IsValid() {
            var errors = new List<string>();
            if (string.IsNullOrEmpty(Code)) errors.Add("El código no puede ser nulo o vacío");
            if (PaymentMethodId == 0) errors.Add("El código de medio de pago no puede ser 0");
            if (Amount <= 0) errors.Add("La cantidad no puede ser menor o igual a 0");
            return errors;
        }
    }
}