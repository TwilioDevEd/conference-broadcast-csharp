using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Moq;
using NUnit.Framework;
using TestStack.FluentMVCTesting;
using Twilio;

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
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Call(It.IsAny<CallOptions>()));
            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-number");
            var mockCustomRequest = new Mock<ICustomRequest>();
            mockCustomRequest.Setup(r => r.Url).Returns("http://example.com");

            var controller = new BroadcastController(
                mockClient.Object, mockPhoneNumbers.Object, mockCustomRequest.Object) {Url = Url};
            controller.Send("phone-one, phone-two", "recording-url");

            mockClient.Verify(c => c.Call(It.IsAny<CallOptions>()), Times.Exactly(2));
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
