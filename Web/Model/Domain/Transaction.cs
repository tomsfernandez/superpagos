using System;
using System.Collections.Generic;

namespace Web.Model.Domain {
    public class Transaction {

        public long Id { get; set; }
        public DateTime Timestamp { get; } = DateTime.Now;
        public List<Movement> Movements { get; set; }
    }
}