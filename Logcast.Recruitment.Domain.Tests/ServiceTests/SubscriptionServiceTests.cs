using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Logcast.Recruitment.DataAccess.Exceptions;
using Logcast.Recruitment.DataAccess.Repositories;
using Logcast.Recruitment.Domain.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Logcast.Recruitment.Domain.Tests.ServiceTests
{
    [TestClass]
    public class SubscriptionServiceTests
    {
        private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
        private readonly ISubscriptionService _subscriptionService;

        public SubscriptionServiceTests()
        {
            _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
            _subscriptionService = new SubscriptionService(_subscriptionRepositoryMock.Object);
        }

        [TestMethod]
        public async Task RegisterSubscriptionAsync_NoErrors_VerifyCalls()
        {
            var email = "email@email.email";
            var firstName = "firstName";
            await _subscriptionService.RegisterSubscriptionAsync(firstName, email);

            _subscriptionRepositoryMock.Verify(a => a.RegisterSubscriptionAsync(firstName, email));
        }

        [TestMethod]
        public async Task RegisterSubscriptionAsync_AlreadyRegistered_NoEmailClientCalls()
        {
            var email = "taken@email.email";
            var firstName = "firstName";
            _subscriptionRepositoryMock.Setup(e => e.RegisterSubscriptionAsync(It.IsAny<string>(), "taken@email.email")).Throws(new EmailAlreadyRegisteredException());

            await Assert.ThrowsExceptionAsync<EmailAlreadyRegisteredException>(() => _subscriptionService.RegisterSubscriptionAsync(firstName, email));

            _subscriptionRepositoryMock.Verify(a => a.RegisterSubscriptionAsync(firstName, email));
        }
    }
}