using System;

namespace VisaApi.Models {
    public class RollbackMessage {

        public DateTime TimeStamp { get; set; } = DateTime.Now;
        public string OperationId { get; }

        public RollbackMessage(string operationId) {
            OperationId = operationId;
        }
    }
}