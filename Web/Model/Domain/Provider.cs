namespace Web.Model.Domain {
    public class Provider {

        public long Id { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Code { get; set; }
        public string RollbackEndPoint { get; set; }
        public string PaymentEndpoint { get; set; }
    }
}