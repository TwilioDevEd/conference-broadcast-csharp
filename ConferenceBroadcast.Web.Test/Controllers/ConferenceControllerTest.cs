using ConferenceBroadcast.Web.Controllers;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class ConferenceControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_ThenRenderTheDefaultView()
        {
            var controller = new ConferenceController();
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView();
        }

        [Test]
        public void GivenAJoinAction_ThenTheResponseContainsAGatherVerb()
        {
            var controller = new ConferenceController {Url = Url};
            var result = controller.Join();

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Gather").Attributes["action"].Value,
                Is.EqualTo("/Conference/Connect"));
        }

        [TestCase("1", "true", "false", "false")]
        [TestCase("2", "false", "false", "false")]
        [TestCase("3", "false", "true", "true")]
        public void GivenAConnectAction_ThenGeneratesTheAppropriateResponse(
            string selectedOption,
            string expectedMute,
            string expectedStartConferenceOnEnter,
            string expectedEndConferenceOnEnter)
        {
            var controller = new ConferenceController();
            var result = controller.Connect(selectedOption);

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            var conferenceAttributes = document
                .SelectSingleNode("Response/Dial/Conference").Attributes;
            Assert.That(conferenceAttributes["muted"].Value,
                Is.EqualTo(expectedMute));
            Assert.That(conferenceAttributes["startConferenceOnEnter"].Value,
                Is.EqualTo(expectedStartConferenceOnEnter));
            Assert.That(conferenceAttributes["endConferenceOnExit"].Value,
                Is.EqualTo(expectedEndConferenceOnEnter));
        }
    }
}
