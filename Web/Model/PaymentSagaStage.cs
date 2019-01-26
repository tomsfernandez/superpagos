using System;
using System.Threading.Tasks;
using Polly;
using Refit;
using Web.Dto;
using Web.Model.Domain;
using Web.Model.Sagas;
using Web.Service.Provider;
using Transaction = Web.Model.Sagas.Transaction;

namespace Web.Model {
    public class PaymentSagaStage : ConcreteSagaStage{
        private StartPaymentMessage Message { get; }
        private PaymentMethod Method { get; }
        private ProviderApi Api { get; }
        private int RetryAmount { get; }

        public PaymentSagaStage(Transaction transaction, ProviderApi api, StartPaymentMessage message,
            PaymentMethod method, int retryAmount = 0) : base(Guid.NewGuid().ToString(), transaction) {
            Message = message;
            Method = method;
            Api = api;
            RetryAmount = retryAmount;
        }

        public override async void Rollback() {
            if (!Rollbacking && Started) await Rollback(Api, GetRollbackMessage(Message));
        }

        public override async void Start() {
            var rollbackMessage = GetRollbackMessage(Message);
            await GetRetryAndFallbackPolicy(RetryAmount, async () => await Rollback(Api, rollbackMessage))
                .Execute(async () => {
                    await Api.Pay(Message);
                    Success = true;
                    Finished = true;
                });
        }

        private async Task Rollback(ProviderApi api, RollbackMessage message) {
            Rollbacking = true;
            await api.Rollback(message);
            Finished = true;
        }

        private Policy GetRetryAndFallbackPolicy(int retryAmount, Action fallbackAction) {
            var retry = Policy.Handle<ApiException>().Retry(retryAmount);
            var fallback = Policy.Handle<ApiException>().Fallback(fallbackAction);
            return Policy.Wrap(fallback, retry);
        }

        private RollbackMessage GetRollbackMessage(StartPaymentMessage message) {
            return new RollbackMessage {
                OperationId = message.OperationId,
                TimeStamp = DateTime.Now
            };
        }
    }
}