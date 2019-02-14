using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Polly;
using Refit;
using Web.Service.Provider;

namespace Web.Model.Domain {
    public class Movement {

        public long Id { get; set; }
        public string OperationId { get; set; } = Guid.NewGuid().ToString();
        public PaymentMethod Account { get; set; }
        public double Amount { get; set; }
        public bool IsSuccesfull { get; private set; }
        public bool InProcess { get; private set; }
        public bool IsRollback { get; private set; }
        public bool Started { get; private set; }
        public Operation OperationType { get; set; }
        public Transaction Transaction { get; set; }

        // todo: hacer async
        public bool Start(ProviderApiFactory providerApiFactory, 
            AppDbContext context, 
            string responseEndpoint) {
            InProcess = true;
            Started = true;
            var couldTriggerPayment = false;
            var api = providerApiFactory.Create(Account.Provider.PaymentEndpoint);
            try {
                api.Pay(StartPaymentMessage.Build(Account, OperationType, 
                    Amount, responseEndpoint, OperationId)).Wait();
                couldTriggerPayment = true;
            }
            catch (Exception e) {
                Debug.Print(e.ToString());
                IsSuccesfull = false;
                InProcess = false;
            }
            context.Movements.Update(this);
            return couldTriggerPayment;
        }
        
        public async Task Rollback(ProviderApiFactory providerApiFactory, AppDbContext context) {
            IsRollback = true;
            InProcess = true;
            var api = providerApiFactory.Create(Account.Provider.RollbackEndpoint);
            await api.Rollback(new RollbackMessage(OperationId));
            InProcess = false;
            context.Movements.Update(this);
        }

        public void Success() {
            IsSuccesfull = true;
            InProcess = false;
        }
    }
}