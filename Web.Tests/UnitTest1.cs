using System;
using FluentAssertions;
using Web.Service.Email;
using Xunit;

namespace Web.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1() {
            var sender =
                new SendGridEmailSender("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");
            await sender.Send("tomas.martinez@ing.austral.edu.ar", "tomas.martinez@ing.austral.edu.ar", "Sending with SendGrid is Fun",
                "<strong>and easy to do anywhere, even with C#</strong>");
        }
    }
}
