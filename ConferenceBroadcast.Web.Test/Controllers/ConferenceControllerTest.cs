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
            var controller = new ConferenceController();
            var result = controller.Join();

            result.ExecuteResult(MockControllerContext.Object);
            var document = BuildDocument();

            Assert.That(document.SelectSingleNode("Response/Say"), Is.Not.Null);
        }
    }
}
