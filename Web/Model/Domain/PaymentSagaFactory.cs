
using System.Collections.Generic;
using Web.Dto;
using Web.Model.Sagas;
using Web.Service.Provider;

namespace Web.Model.Domain {
    public class PaymentSagaFactory : SagaFactory{

        public AppDbContext Context { get; }
        public PaymentDto Dto { get; }
        private int RetryAmount { get; } = 0;
        private ProviderApiFactory ApiFactory { get; }
        private Transaction Transaction { get; }

        public PaymentSagaFactory(AppDbContext context, PaymentDto dto, ProviderApiFactory apiFactory, Transaction transaction) {
            Context = context;
            Dto = dto;
            ApiFactory = apiFactory;
            Transaction = transaction;
        }

        public Saga Build() {
            var payerSaga = GetPayerSagaStage(Context, Dto);
            var paidSaga = GetPaidSagaStage(Context, Dto);
            var saga = new Saga(new List<SagaStage>{payerSaga, paidSaga});
            return saga;
        }

        private SagaStage GetPaidSagaStage(AppDbContext context, PaymentDto dto) {
            var paymentMethod = context.PaymentButtons.Find(dto.ButtonId).Method;
            var api = ApiFactory.Create(paymentMethod.Provider.PaymentEndpoint);
            var message = StartPaymentMessage.Build(paymentMethod, dto);
            var payerStage =  new PaymentSagaStage(Transaction,api,message, paymentMethod,RetryAmount);
            return new PersistentSagaStage(context, payerStage);
        }

        private SagaStage GetPayerSagaStage(AppDbContext context, PaymentDto dto) {
            var paymentMethod = context.PaymentMethods.Find(dto.PaymentMethodId);
            var api = ApiFactory.Create(paymentMethod.Provider.PaymentEndpoint);
            var message = StartPaymentMessage.Build(paymentMethod, dto);
            var stage = new PaymentSagaStage(Transaction, api, message, paymentMethod, RetryAmount);
            return new PersistentSagaStage(context, stage);
        }
    }
}