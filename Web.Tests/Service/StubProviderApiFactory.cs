using System;
using Microsoft.AspNetCore.Mvc;
using Web.Dto;
using Web.Model;
using Web.Service;
using Web.Service.Provider;

namespace Web.Tests.Service {
    public class StubProviderApiFactory : ProviderApiFactory{

        public Func<PaymentMethodConfirmation, ObjectResult> OnAssociation { get; set; } = dto => throw new Exception();
        public Func<StartPaymentMessage, ObjectResult> OnPayment { get; set; } = dto => throw new Exception();
        public Func<RollbackMessage, ObjectResult> OnRollback { get; set; } = dto => throw new Exception();

        public ProviderApi Create(string endpoint) {
            return new StubProviderApi {
                OnAssociation = OnAssociation,
                OnPayment = OnPayment,
                OnRollback = OnRollback
            };
        }
    }
}