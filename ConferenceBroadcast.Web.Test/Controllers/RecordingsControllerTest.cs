using System;
using System.Collections.Generic;
using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
using Moq;
using NUnit.Framework;
using Twilio;

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class RecordingsControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_ItReturnsAllTheRecordings()
        {
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Recordings()).Returns(new List<Recording>
            {
                new Recording {Uri = new Uri("/recording", UriKind.Relative), DateCreated = new DateTime(2015, 01, 01)},
                new Recording {Uri = new Uri("/recording", UriKind.Relative), DateCreated = new DateTime(2015, 01, 01)}
            });

            var controller = new RecordingsController(mockClient.Object) {Url = Url};
            var result = controller.Create("phone-number");

            result.ExecuteResult(MockControllerContext.Object);

            var recordings = Result.ToString();
            Assert.That(recordings, Is.Not.Null);
        }

        [Test]
        public void GivenASendAction_When2PhoneNumbersAreProvided_ThenCallIsCalledTwice()
        {
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Call(It.IsAny<CallOptions>()));

            var controller = new RecordingsController(mockClient.Object) {Url = Url};
            var result = controller.Create("phone-number");

            result.ExecuteResult(MockControllerContext.Object);

            mockClient.Verify(c => c.Call(It.IsAny<CallOptions>()), Times.Once);
        }

    }
}
