
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Web.Dto {
    public class PaymentDto {

        public long ButtonId { get; set; }
        public long PaymentMethodId { get; set; }
        public string Description { get; set; }

        public List<string> Validate(AppDbContext context) {
            var errors = new List<string>();
            if(ButtonId == 0) errors.Add("ButtonId is invalid");
            if(string.IsNullOrEmpty(Description)) errors.Add("Description is null or empty");
            if(PaymentMethodId == 0) errors.Add("PayerId is invalid");
            var buttonExists = context.PaymentButtons.Find(ButtonId) != null;
            var paymentMethodExists = context.PaymentMethods.Find(PaymentMethodId) != null;
            if (!buttonExists) errors.Add("No existe Button para ese Button Id");
            if (!paymentMethodExists) errors.Add("No existe Medio de Pago para ese Id");
            if (!buttonExists || !paymentMethodExists) return errors;
            var payer = context.PaymentMethods
                    .Include(x => x.User)
                .Single(x => x.Id == PaymentMethodId).User;
            var payed = context.PaymentButtons
                .Include(x => x.Method)
                    .ThenInclude(x => x.User)
                .Single(x => x.Id == ButtonId).Method.User;
            if (payed.Id.Equals(payer.Id)) errors.Add("El pagador y el pagado son iguales");
            return errors;
        }
    }
}