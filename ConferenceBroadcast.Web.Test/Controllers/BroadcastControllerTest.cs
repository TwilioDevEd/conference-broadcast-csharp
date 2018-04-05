using System.Web.Mvc;
using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

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
            const string baseUrl = "http://example.com";
            string broadcastPlayUrl = $"{baseUrl}/Broadcast/Play?recordingUrl=recording-url";
            const string twilioNumber = "twilio-number";

            var clientMock = new Mock<IClient>();

            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-number");
            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns(baseUrl);

            var controller = new BroadcastController(
                clientMock.Object,
                mockPhoneNumbers.Object,
                mockCustomRequest.Object
                ) {Url = Url};


            controller.WithCallTo(c => c.Send("phone-one, phone-two", "recording-url"))
                .ShouldRenderView("Send");

            clientMock.Verify(c => c.Call("phone-one", twilioNumber, broadcastPlayUrl));
            clientMock.Verify(c => c.Call("phone-two", twilioNumber, broadcastPlayUrl));
        }

        [Test]
        public void GivenAPlayAction_ThenTheResponseContainsPlay()
        {
            var controller = new BroadcastController();
            const string recordUrl = "http://record-url/some-url";
            var result = controller.Play(recordUrl);

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Play").InnerText,
                Is.EqualTo(recordUrl));
        }
    }
}
