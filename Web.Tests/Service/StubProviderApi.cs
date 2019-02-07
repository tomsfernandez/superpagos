using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Dto;
using Web.Model;
using Web.Service;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class StubProviderApi : ProviderApi {

        public Func<PaymentMethodConfirmation, ObjectResult> OnAssociation { get; set; } = dto => throw new Exception();
        public Func<StartPaymentMessage, ObjectResult> OnPayment { get; set; } = dto => throw new Exception();
        public Func<RollbackMessage, ObjectResult> OnRollback { get; set; } = dto => throw new Exception();

        public Task AssociateAccount(PaymentMethodConfirmation payload) {
            var task = new Task(() => OnAssociation(payload));
            task.Start();
            return task;
        }

        public Task<ObjectResult> Pay(StartPaymentMessage message) {
            var task = new Task<ObjectResult>(() => OnPayment(message));
            task.Start();
            return task;
        }

        public Task<ObjectResult> Rollback(RollbackMessage message) {
            var task = new Task<ObjectResult>(() => OnRollback(message));
            task.Start();
            return task;
        }
    }
}