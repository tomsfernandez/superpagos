using System;

namespace Web.Model.Domain {
    public class PaymentMethod {

        public long Id { get; set; }
        public Provider Provider { get; set; }
        public User User { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public string Token { get; set; }
    }
}