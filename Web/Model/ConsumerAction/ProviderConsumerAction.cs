using Newtonsoft.Json.Linq;
using Web.Dto;

namespace Web.Model.ConsumerAction {
    
    public class ProviderConsumerAction : ConsumerAction{
        
        public void Execute(string message) {
            var paymentInformation = JObject.Parse(message).ToObject<StartPaymentMessage>();
            // todo: save payment in redis
            // todo: emit both operations
        }
    }
}