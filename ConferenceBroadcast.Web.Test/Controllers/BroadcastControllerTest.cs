using ConferenceBroadcast.Web.Controllers;
using ConferenceBroadcast.Web.Domain.Twilio;
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
        public void GivenARecordAction_ThenTheResponseContainsRecord()
        {
            var controller = new BroadcastController();
            var result = controller.Record();

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Record").Attributes["finishOnKey"].Value,
                Is.EqualTo("*"));
        }

        [Test]
        public void GivenASendAction_When2PhoneNumbersAreProvided_ThenCallIsCalledTwice()
        {
            var mockClient = new Mock<IClient>();
            mockClient.Setup(c => c.Call(It.IsAny<CallOptions>()));
            var mockPhoneNumbers = new Mock<IPhoneNumbers>();
            mockPhoneNumbers.Setup(p => p.Twilio).Returns("twilio-number");

            var controller = new BroadcastController(mockClient.Object, mockPhoneNumbers.Object) {Url = Url};
            var result = controller.Send("phone-one, phone-two", "recording-url");

            result.ExecuteResult(MockControllerContext.Object);

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
