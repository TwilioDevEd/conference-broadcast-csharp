using System.Collections.Generic;
using System.Linq;
using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;
using Twilio.Rest.Api.V2010.Account;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class RecordingsControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_WhenClientHasRecordings_ThenShowsTheRecordings()
        {
            var mockClient = new Mock<IClient>();

            var recordingsFromApi = new List<RecordingResource>
            {
                RecordingResource.FromJson("{\"date_created\": \"Mon, 22 Aug 2011 20:58:45 +0000\", \"uri\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Recordings/REaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.json\"}")
            };

            mockClient.Setup(c => c.Recordings()).ReturnsAsync(recordingsFromApi);

            var stubPhoneNumbers = Mock.Of<IPhoneNumbers>();
            var controller = new RecordingsController(mockClient.Object, stubPhoneNumbers);
            var result = controller.WithCallTo(c => c.Index()).ShouldReturnJson();
            var recordings = (result.Data as IEnumerable<object>).ToList();

            Assert.That(recordings.Count(), Is.EqualTo(1));
        }

        [Test]
        public void GivenACreateAction_ThenCallIsCalledOnce()
        {
            var mockClient = new Mock<IClient>();
            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-phone-number");

            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns("http://example.com");

            var controller = new RecordingsController(
                mockClient.Object, mockPhoneNumbers.Object, mockCustomRequest.Object) {Url = Url};

            controller.WithCallTo(c => c.Create("phone-number")).ShouldReturnEmptyResult();

            mockClient.Verify(
                c => c.Call("phone-number", "twilio-phone-number", "http://example.com/Recordings/Record"));
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
