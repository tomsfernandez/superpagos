using System;
using FluentAssertions;
using Xunit;

namespace Web.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1() {
            true.Should().BeTrue();
        }
    }
}
