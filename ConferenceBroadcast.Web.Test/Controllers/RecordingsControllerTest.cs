using System;
using System.Collections.Generic;
using System.Web.Helpers;
using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using Twilio;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class RecordingsControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_WhenClientHasRecordings_ThenShowsTheRecordings()
        {
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Recordings()).Returns(new List<Recording>
            {
                new Recording {Uri = new Uri("/recording", UriKind.Relative), DateCreated = new DateTime(2015, 01, 01)},
                new Recording {Uri = new Uri("/recording", UriKind.Relative), DateCreated = new DateTime(2015, 01, 01)}
            });

            var stubPhoneNumbers = Mock.Of<IPhoneNumbers>();

            var controller = new RecordingsController(mockClient.Object, stubPhoneNumbers);
            var result = controller.Index();

            result.ExecuteResult(MockControllerContext.Object);

            var recordings = Json.Decode<IList<IDictionary<string, string>>>(Result.ToString());
            Assert.That(recordings.Count, Is.EqualTo(2));
        }

        [Test]
        public void GivenACreateAction_ThenCallIsCalledOnce()
        {
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Call(It.IsAny<CallOptions>()));

            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-phone-number");

            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns("http://example.com");

            var controller = new RecordingsController(
                mockClient.Object, mockPhoneNumbers.Object, mockCustomRequest.Object) {Url = Url};
            var result = controller.Create("phone-number");

            result.ExecuteResult(MockControllerContext.Object);

            mockClient.Verify(c => c.Call(It.IsAny<CallOptions>()), Times.Once);
        }

        [Test]
        public void GivenARecordAction_ThenTheResponseContainsRecord()
        {
            var controller = new RecordingsController {Url = Url};
            var result = controller.Record();

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Record").Attributes["finishOnKey"].Value,
                Is.EqualTo("*"));
        }

        [Test]
        public void GivenAHangupAction_ThenTheResponseContainsHangup()
        {
            var controller = new RecordingsController();
            var result = controller.Hangup();

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Hangup"), Is.Not.Null);
        }
    }
}
