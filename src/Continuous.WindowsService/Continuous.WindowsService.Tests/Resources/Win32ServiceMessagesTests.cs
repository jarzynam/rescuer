﻿using Continuous.WindowsService.Resources;
using FluentAssertions;
using NUnit.Framework;

namespace Continuous.WindowsService.Tests.Resources
{
    [TestFixture]
    public class Win32ServiceMessagesTests
    {
        private IWin32ServiceMessages _messages;

        [SetUp]
        public void SetUp()
        {
            _messages = new Win32ServiceMessages();
        }

        [Test]
        public void Can_GetMessage_FromProperCode()
        {
            // arrange
            uint code = 6;
            string expectedMessage = "The service has not been started";

            // act
            var actualMessage = _messages.GetMessage(code);

            // assert
            actualMessage.Should().Be(expectedMessage);
        }

        [Test]
        public void Can_GetDefaultMessage_FromUnvalidCode()
        {
            // arrange
            uint code = 999;
            string expectedMessage = "unknown code 999";

            // act
            var actualMessage = _messages.GetMessage(code);

            // assert
            actualMessage.Should().Be(expectedMessage);        }
    }
}
