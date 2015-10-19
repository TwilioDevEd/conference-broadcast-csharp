using System.Web.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;

namespace ConferenceBroadcast.Web.Controllers
{
    public class ConferenceController : TwilioController
    {
        // GET: Conference
        public ActionResult Index()
        {
            // TODO: Get rapid response conference number.
            return View();
        }

        // GET: Conference/Join
        public ActionResult Join()
        {
            var response = new TwilioResponse();
            response.Say("You are about to join the Rapid Response conference");
            response.BeginGather()
                .Say("Press 1 to join as a listener")
                .Say("Press 2 to join as a speaker")
                .Say("Press 3 to join as the moderator")
                .EndGather();

            return TwiML(response);
        }
    }
}