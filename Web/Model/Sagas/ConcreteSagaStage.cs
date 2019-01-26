using System;

namespace Web.Model.Sagas {
    public abstract class ConcreteSagaStage : SagaStage {

        public string OperationId { get; }
        public DateTime Timestamp { get; } = DateTime.Now;
        public bool Started { get; protected set; }
        public bool Finished { get; protected set; }
        public bool Success { get; protected set; }
        public bool Rollbacking { get; protected set; }
        public Transaction ParentTransaction { get; }

        public ConcreteSagaStage(string operationId, Transaction parentTransaction) {
            OperationId = operationId;
            ParentTransaction = parentTransaction;
        }
        
        public abstract void Start();
        public abstract void Rollback();

        public bool IsStarted() {
            return Started;
        }

        public bool IsFinished() {
            return Finished;
        }

        public bool IsSuccess() {
            return Success;
        }

        public bool IsRollback() {
            return Rollbacking;
        }
    }
}