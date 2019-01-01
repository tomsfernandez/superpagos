namespace Web.Model.ConsumerAction {
    public interface ConsumerAction {

        void Execute(string message);
    }
}