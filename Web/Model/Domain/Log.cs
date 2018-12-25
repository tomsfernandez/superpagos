using System;

namespace Web.Model.Domain {
    public class Log {

        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Description { get; set; }
    }
}