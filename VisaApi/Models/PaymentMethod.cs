using System;

namespace VisaApi.Models {
    public class PaymentMethod {

        public long Id { get; set; }
        public User User { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public string Token { get; set; }
    }
}