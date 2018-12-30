using System;
using System.Threading.Tasks;
using Web.Service.Email;

namespace Web.Tests.Service {
    public class StubEmailSender : EmailSender{

        private Action ToPerform { get; }

        public StubEmailSender(Action perform) {
            ToPerform = perform;
        }

        public Task Send(string to, string @from, string subject, string htmlContent) {
            var task = new Task(() => ToPerform());
            task.Start();
            return task;
        }
    }
}