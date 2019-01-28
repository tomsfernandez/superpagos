using System.Buffers;
using System.Collections.Generic;
using Web.Model.Domain;

namespace Web.Tests.Helpers {
    public class MovementFactory {

        public static Transaction GetTransaction(PaymentMethod aMethod = null, PaymentMethod anotherMethod = null) {
            if (aMethod == null) aMethod = PaymentMethodFactory.GetVisaPaymentMethod();
            if (anotherMethod == null) anotherMethod = PaymentMethodFactory.GetVisaPaymentMethod();
            var transaction = new Transaction();
            var movements= new List<Movement>{
                GetDepositMovement(aMethod, transaction), 
                GetWithdrawMovement(anotherMethod, transaction)
            };
            transaction.Movements = movements;
            return transaction;
        }

        public static Movement GetDepositMovement(PaymentMethod method, Transaction transaction, int amount = 50) {
            return new Movement {
                Account = method,
                Amount = amount,
                OperationType = Operation.DEPOSIT,
                Transaction = transaction
            };
        }

        public static Movement GetWithdrawMovement(PaymentMethod method, Transaction transaction, int amount = 50) {
            return new Movement {
                Account = method,
                Amount = amount,
                OperationType = Operation.WITHDRAWAL,
                Transaction = transaction
            };
        }
    }
}