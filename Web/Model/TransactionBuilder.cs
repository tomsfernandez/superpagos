using System.Collections.Generic;
using Web.Dto;
using Web.Model.Domain;

namespace Web.Model {
    public class TransactionBuilder {

        public Transaction Build(AppDbContext context, PaymentDto dto) {
            var payerMovement = GetPayerMovement(context, dto);
            var paidMovement = GetPaidMovement(context, dto);
            return new Transaction {
                Movements = new List<Movement>{payerMovement, paidMovement}
            };
        }

        private Movement GetPayerMovement(AppDbContext context, PaymentDto dto) {
            var method = context.PaymentMethods.Find(dto.PaymentMethodId);
            return new Movement {
                Account = method,
                Amount = dto.Amount,
                OperationType = Operation.WITHDRAWAL
            };
        }

        private Movement GetPaidMovement(AppDbContext context, PaymentDto dto) {
            var method = context.PaymentButtons.Find(dto.ButtonId).Method;
            return new Movement {
                Account = method,
                Amount = dto.Amount,
                OperationType = Operation.DEPOSIT
            };
        }
    }
}