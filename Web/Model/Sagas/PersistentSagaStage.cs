namespace Web.Model.Sagas {
    public class PersistentSagaStage : SagaStage{

        private AppDbContext Context { get; }
        private PaymentSagaStage SagaStage { get; }

        public PersistentSagaStage(AppDbContext context, PaymentSagaStage sagaStage) {
            Context = context;
            SagaStage = sagaStage;
        }
        
        public void Start() {
            Context.PaymentSagaStage.Add(SagaStage);
            Context.SaveChanges();
            SagaStage.Start();
            Context.PaymentSagaStage.Add(SagaStage);
            Context.SaveChanges();
        }

        public void Rollback() {
            
        }

        public bool IsStarted() {
            return SagaStage.IsStarted();
        }

        public bool IsFinished() {
            return SagaStage.Finished;
        }

        public bool IsSuccess() {
            return SagaStage.Success;
        }

        public bool IsRollback() {
            return SagaStage.Rollbacking;
        }
    }
}