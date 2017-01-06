using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;
using Twilio;
using Twilio.Clients;
using Twilio.Http;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class BroadcastControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_ThenRenderTheDefaultView()
        {
            var controller = new BroadcastController();
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenASendAction_When2PhoneNumbersAreProvided_ThenCallIsCalledTwice()
        {
            var twilioClientMock = new Mock<ITwilioRestClient>();

            twilioClientMock.Setup(c => c.AccountSid).Returns("ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            twilioClientMock.Setup(c => c.RequestAsync(It.IsAny<Request>()))
                            .ReturnsAsync(new Response(
                                         System.Net.HttpStatusCode.Created,
                                         "{\"account_sid\": \"ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"annotation\": null,\"answered_by\": null,\"api_version\": \"2010-04-01\",\"caller_name\": null,\"date_created\": \"Tue, 31 Aug 2010 20:36:28 +0000\",\"date_updated\": \"Tue, 31 Aug 2010 20:36:44 +0000\",\"direction\": \"inbound\",\"duration\": \"15\",\"end_time\": \"Tue, 31 Aug 2010 20:36:44 +0000\",\"forwarded_from\": \"+141586753093\",\"from\": \"+14158675308\",\"from_formatted\": \"(415) 867-5308\",\"group_sid\": null,\"parent_call_sid\": null,\"phone_number_sid\": \"PNaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"price\": \"-0.03000\",\"price_unit\": \"USD\",\"sid\": \"CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa\",\"start_time\": \"Tue, 31 Aug 2010 20:36:29 +0000\",\"status\": \"completed\",\"subresource_uris\": {\"notifications\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Calls/CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Notifications.json\",\"recordings\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Calls/CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Recordings.json\"},\"to\": \"+14158675309\",\"to_formatted\": \"(415) 867-5309\",\"uri\": \"/2010-04-01/Accounts/ACaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa/Calls/CAaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa.json\"}"
                                     ));


            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-number");
            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns("http://example.com");

            var controller = new BroadcastController(
                mockPhoneNumbers.Object, mockCustomRequest.Object) {Url = Url};

            TwilioClient.SetRestClient(twilioClientMock.Object);

            controller.Send("phone-one, phone-two", "recording-url");

            twilioClientMock.Verify(
                c => c.RequestAsync(It.IsAny<Request>()), Times.AtMost(2));
        }

        [Test]
        public void GivenAPlayAction_ThenTheResponseContainsPlay()
        {
            var controller = new BroadcastController();
            const string recordUrl = "record-url";
            var result = controller.Play(recordUrl);

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Play").InnerText,
                Is.EqualTo(recordUrl));
        }
    }
}
