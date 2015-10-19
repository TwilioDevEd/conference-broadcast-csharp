using ConferenceBroadcast.Web.Controllers;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Test.Controllers
{
    public class HomeControllerTest : ControllerTest
    {
        [Test]
        public void GivenAnIndexAction_ThenRenderTheDefaultView()
        {
            var controller = new HomeController();
            controller.WithCallTo(c => c.Index())
                .ShouldRenderDefaultView();
        }
    }
}
