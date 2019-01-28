using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Refit;
using Web.Service.Provider;

namespace Web.Model.Domain {
    public class Movement {

        public long Id { get; set; }
        public string OperationId { get; } = Guid.NewGuid().ToString();
        public PaymentMethod Account { get; set; }
        public double Amount { get; set; }
        public bool IsSuccesfull { get; private set; }
        public bool InProcess { get; private set; }
        public bool IsRollback { get; private set; }
        public bool Started { get; private set; }
        public Operation OperationType { get; set; }
        public Transaction Transaction { get; set; }

        // todo: hacer async
        public bool Start(ProviderApiFactory providerApiFactory, AppDbContext context) {
            InProcess = true;
            Started = true;
            var api = providerApiFactory.Create(Account.Provider.RollbackEndPoint);
            try {
                api.Pay(StartPaymentMessage.Build(Account, OperationType, Amount)).Wait();
                Success();
            }
            catch (Exception) {
                IsSuccesfull = false;
                InProcess = false;
            }
            context.Movements.Update(this);
            return IsSuccesfull;
        }
        
        public async Task Rollback(ProviderApiFactory providerApiFactory, AppDbContext context) {
            IsRollback = true;
            InProcess = true;
            var api = providerApiFactory.Create(Account.Provider.RollbackEndPoint);
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