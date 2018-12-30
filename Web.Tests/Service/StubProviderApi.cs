using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Dto;
using Web.Service;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class StubProviderApi : ProviderApi{
        
        private Func<PaymentMethodConfirmation, ObjectResult> ActionToPerform { get; set; }

        public StubProviderApi(Func<PaymentMethodConfirmation, ObjectResult> actionToPerform) {
            ActionToPerform = actionToPerform;
        }

        public Task<ObjectResult> AssociateAccount(PaymentMethodConfirmation payload) {
            var task = new Task<ObjectResult>(() => ActionToPerform(payload));
            task.Start();
            return task;
        }
    }
}