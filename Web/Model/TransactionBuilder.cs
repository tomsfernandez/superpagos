using System.Collections.Generic;
using Web.Dto;
using Web.Model.Domain;

namespace Web.Model {
    public class TransactionBuilder {
        
        public AppDbContext Context { get; }

        public TransactionBuilder(AppDbContext context) {
            Context = context;
        }

        public Transaction Build(PaymentDto dto, double amount) {
            var payerMovement = GetPayerMovement(Context, dto, amount);
            var paidMovement = GetPaidMovement(Context, dto, amount);
            return new Transaction {
                Movements = new List<Movement>{payerMovement, paidMovement}
            };
        }

        private Movement GetPayerMovement(AppDbContext context, PaymentDto dto, double amount) {
            var method = context.PaymentMethods.Find(dto.PaymentMethodId);
            return new Movement {
                Account = method,
                Amount = amount,
                OperationType = Operation.WITHDRAWAL
            };
        }

        private Movement GetPaidMovement(AppDbContext context, PaymentDto dto, double amount) {
            var method = context.PaymentButtons.Find(dto.ButtonId).Method;
            return new Movement {
                Account = method,
                Amount = amount,
                OperationType = Operation.DEPOSIT
            };
        }
    }
}