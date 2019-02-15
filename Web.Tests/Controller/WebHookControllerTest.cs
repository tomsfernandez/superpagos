using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;
using Web.Dto;
using Web.Service.Provider;
using Web.Tests.Helpers;
using Web.Tests.Service;
using Xunit;
using Transaction = Web.Model.Domain.Transaction;

namespace Web.Tests.Controller {
    public class WebHookControllerTest : IDisposable{
        
        private WebHookController Controller { get; set; }
        private AppDbContext Context { get; set; }
        private ProviderApiFactory Factory { get; set; }
        private Transaction Transaction { get; set; }

        public WebHookControllerTest() {
            Context = GetContext();
            Factory = new StubProviderApiFactory {
                OnRollback = dto => new OkObjectResult("OK")
            };
            Controller = new WebHookController(Context, Factory);
            Transaction = SaveTransaction();
        }

        [Fact]
        public void test_01_first_movement_is_succesfull_so_nothing_happens() {
            var operationId = Transaction.Movements[0].OperationId;
            var response = PaymentResponseFactory.GetSuccesfullResponse(operationId);
            CheckResponseIs(response, 200);
            Transaction.Movements[0].IsSuccesfull.Should().BeTrue();
            Transaction.Movements[0].InProcess.Should().BeFalse();
            Transaction.Movements[0].IsRollback.Should().BeFalse();
        }

        [Fact]
        public void test_02_first_movement_is_error_so_transaction_rollbacks() {
            var operationId = Transaction.Movements[0].OperationId;
            var response = PaymentResponseFactory.GetFailedResponse(operationId);
            CheckResponseIs(response, 200);
            Transaction.Movements.Select(x => x.InProcess).Should().AllBeEquivalentTo(false);
            Transaction.Movements.Select(x => x.IsRollback).Should().AllBeEquivalentTo(true);
        }

        [Fact]
        public void test_03_second_movement_is_error_so_transaction_rollbacks() {
            var operationId = Transaction.Movements[1].OperationId;
            var response = PaymentResponseFactory.GetFailedResponse(operationId);
            CheckResponseIs(response, 200);
            Transaction.Movements.Select(x => x.InProcess).Should().AllBeEquivalentTo(false);
            Transaction.Movements.Select(x => x.IsRollback).Should().AllBeEquivalentTo(true);
        }

        private async void CheckResponseIs(PaymentResponse response, int status) {
            var result = await Controller.Notify(response) as StatusCodeResult;
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(status);
        }

        public Transaction SaveTransaction() {
            var transaction = MovementFactory.GetTransaction();
            Context.Transactions.Add(transaction);
            Context.SaveChanges();
            return transaction;
        }

        public AppDbContext GetContext() {
            var context = TestHelper.MakeContext();
            context.Database.EnsureDeleted();
            return TestHelper.MakeContext();
        }

        public void Dispose() {
            Context.Database.EnsureDeleted();
        }
    }
}