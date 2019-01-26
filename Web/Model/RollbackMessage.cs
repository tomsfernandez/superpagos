using System;

namespace Web.Dto {
    public class RollbackMessage {

        public DateTime TimeStamp { get; set; }
        public string OperationId { get; set; }
    }
}