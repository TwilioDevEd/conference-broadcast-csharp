using System.Collections.Generic;
using System.Web.Helpers;
using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using Twilio;
using Twilio.Clients;
using Twilio.Http;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class RecordingsControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_WhenClientHasRecordings_ThenShowsTheRecordings()
        {
            var twilioClientMock = new Mock<ITwilioRestClient>();

            twilioClientMock.Setup(c => c.AccountSid).Returns("ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            twilioClientMock.Setup(c => c.RequestAsync(It.IsAny<Request>()))
                            .ReturnsAsync(new Response(
                                         System.Net.HttpStatusCode.OK,
                                         "{\"account_sid\": \"ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"api_version\": \"2010-04-01\",\"call_sid\": \"CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"date_created\": \"Wed, 01 Sep 2010 15:15:41 +0000\",\"date_updated\": \"Wed, 01 Sep 2010 15:15:41 +0000\",\"duration\": \"6\",\"sid\": \"REaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"price\": \"0.04\",\"price_unit\": \"USD\",\"status\": \"completed\",\"channels\": 1,\"source\": \"Trunking\",\"uri\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Recordings/REaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.json\"}"
                                     ));

            var stubPhoneNumbers = Mock.Of<IPhoneNumbers>();
             
            var controller = new RecordingsController(stubPhoneNumbers);

            TwilioClient.SetRestClient(twilioClientMock.Object);

            var result = controller.Index().Result;

            var recordings = Json.Decode<IList<IDictionary<string, string>>>(result.ToString());
            Assert.That(recordings.Count, Is.EqualTo(1));
        }

        [Test]
        public void GivenACreateAction_ThenCallIsCalledOnce()
        {
            var twilioClientMock = new Mock<ITwilioRestClient>();

            twilioClientMock.Setup(c => c.AccountSid).Returns("ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            twilioClientMock.Setup(c => c.RequestAsync(It.IsAny<Request>()))
                            .ReturnsAsync(new Response(
                                         System.Net.HttpStatusCode.OK,
                                         "{\"account_sid\": \"ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"api_version\": \"2010-04-01\",\"call_sid\": \"CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"date_created\": \"Wed, 01 Sep 2010 15:15:41 +0000\",\"date_updated\": \"Wed, 01 Sep 2010 15:15:41 +0000\",\"duration\": \"6\",\"sid\": \"REaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"price\": \"0.04\",\"price_unit\": \"USD\",\"status\": \"completed\",\"channels\": 1,\"source\": \"Trunking\",\"uri\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Recordings/REaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.json\"}"
                                     ));

            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-phone-number");

            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns("http://example.com");

            var controller = new RecordingsController(
                mockPhoneNumbers.Object, mockCustomRequest.Object) {Url = Url};

            TwilioClient.SetRestClient(twilioClientMock.Object);

            var result = controller.Create("phone-number");

            result.ExecuteResult(MockControllerContext.Object);

            twilioClientMock.Verify(
               c => c.RequestAsync(It.IsAny<Request>()), Times.AtMost(1));
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
