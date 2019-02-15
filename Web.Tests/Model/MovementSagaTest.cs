using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Web.Model;
using Web.Model.Domain;
using Web.Service.Provider;
using Web.Tests.Helpers;
using Web.Tests.Service;
using Xunit;

namespace Web.Tests.Model {
    public class SagaTest : IDisposable, IClassFixture<DatabaseFixture> {

        public MovementSaga Saga { get; }
        public AppDbContext Context { get; }
        public ProviderApiFactory Factory { get; }
        public string FailedToken { get; } = "FailedToken";
        public string GoodToken { get; } = "GoodToken";

        public SagaTest(DatabaseFixture fixture) {
            Context = fixture.DatabaseContext;
            Factory = new StubProviderApiFactory {
                OnPayment = dto => dto.Token.Equals(FailedToken) ? throw new Exception() : new OkObjectResult("OK"),
                OnRollback = dto => new OkObjectResult("OK")
            };
            Saga = new MovementSaga {Factory = Factory, Context = Context};
        }

        [Fact]
        public void test_01_all_movements_succeed_in_first_setp() {
            var movements = new List<Movement>{GetSuccesfullMovement(), GetSuccesfullMovement()};
            Saga.Start(movements).Should().BeTrue();
            movements.Select(x => x.InProcess).Should().AllBeEquivalentTo(true);
            movements.Select(x => x.IsSuccesfull).Should().AllBeEquivalentTo(false);
        }

        [Fact]
        public void test_02_first_movements_fails_so_no_rollbacks() {
            var movements = new List<Movement>{GetFailedMovement(), GetSuccesfullMovement()};
            Saga.Start(movements);
            movements[0].Started.Should().BeTrue();
            movements[0].InProcess.Should().BeFalse();
            movements[0].IsSuccesfull.Should().BeFalse();
            movements[1].IsRollback.Should().BeFalse();
            movements[1].Started.Should().BeFalse();
        }

        [Fact]
        public void test_03_last_movement_fails_so_all_rollback() {
            var movements = new List<Movement>{GetSuccesfullMovement(), GetFailedMovement()};
            Saga.Start(movements);
            movements[0].IsRollback.Should().BeTrue();
            movements.Select(x => x.Started).Should().AllBeEquivalentTo(true);
            movements.Select(x => x.InProcess).Should().AllBeEquivalentTo(false);
        }

        public Movement GetSuccesfullMovement() {
            var method = PaymentMethodFactory.GetVisaPaymentMethod(token: GoodToken);
            return MovementFactory.GetWithdrawMovement(method, null);
        }

        public Movement GetFailedMovement() {
            var method = PaymentMethodFactory.GetVisaPaymentMethod(token: FailedToken);
            return MovementFactory.GetWithdrawMovement(method, null);
        }

        public void Dispose() {
            Context.Movements.DeleteFromQuery();
        }
    }
}