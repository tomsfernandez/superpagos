namespace Web.Model.Sagas {
    public interface SagaStage {
        void Start();
        void Rollback();
        bool IsStarted();
        bool IsFinished();
        bool IsSuccess();
        bool IsRollback();
    }
}