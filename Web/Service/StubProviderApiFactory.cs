using System;
using Microsoft.AspNetCore.Mvc;
using Web.Dto;

namespace Web.Service {
    public class StubProviderApiFactory : ProviderApiFactory{

        private Func<PaymentMethodConfirmation, ObjectResult> ActionToPerform { get; }

        public StubProviderApiFactory(Func<PaymentMethodConfirmation, ObjectResult> actionToPerform) {
            ActionToPerform = actionToPerform;
        }

        public ProviderApi Create(string endpoint) {
            return new StubProviderApi(ActionToPerform);
        }
    }
}