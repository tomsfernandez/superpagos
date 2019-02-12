using System;

namespace Web.Model.Domain {
    public class PaymentButton {
        public long Id { get; set; }
        public DateTime CreationDate { get; set; }
        public string Code { get; set; }
        public PaymentMethod Method { get; set; }
        public double Amount { get; set; }
    }
}