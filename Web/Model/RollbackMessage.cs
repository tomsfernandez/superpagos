using System;

namespace Web.Model {
    public class RollbackMessage {

        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string OperationId { get; }

        public RollbackMessage(string operationId) {
            OperationId = operationId;
        }
    }
}