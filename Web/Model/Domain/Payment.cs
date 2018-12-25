using System;
using System.Collections.Generic;

namespace Web.Model.Domain {
    public class Payment {

        public PaymentMethod Payer { get; set; }
        public PaymentMethod Paid { get; set; }
        public double Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public List<Log> Logs { get; set; }
    }
}