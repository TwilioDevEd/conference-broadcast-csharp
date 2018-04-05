using System;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using System.Web.Mvc;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;
using Twilio.TwiML.Voice;

namespace ConferenceBroadcast.Web.Controllers
{
    public class ConferenceController : TwilioController
    {
        private readonly IPhoneNumbers _phoneNumbers;

        public ConferenceController() : this(new PhoneNumbers()) {}

        public ConferenceController(IPhoneNumbers phoneNumbers)
        {
            _phoneNumbers = phoneNumbers;
        }

        // GET: Conference
        public ActionResult Index()
        {
            ViewBag.RapidResponseNumber = _phoneNumbers.RapidResponse;
            return View();
        }

        // POST: Conference/Join
        [HttpPost]
        public ActionResult Join()
        {
            var response = new VoiceResponse();
            response.Say("You are about to join the Rapid Response conference");
            response.Append(new Gather(action: Url.ActionUri("Connect", "Conference"))
                                    .Say("Press 1 to join as a listener")
                                    .Say("Press 2 to join as a speaker")
                                    .Say("Press 3 to join as the moderator"));

            return TwiML(response);
        }

        // POST: Conference/Connect
        [HttpPost]
        public ActionResult Connect(string digits)
        {
            var isMuted = digits.Equals("1"); // Listener
            var canControlConferenceOnEnter = digits.Equals("3"); // Moderator

            var response = new VoiceResponse();
            response.Say("You have joined the conference");

            var dial = new Dial();
            dial.Conference("RapidResponseRoom",
                waitUrl: new Uri("http://twimlets.com/holdmusic?Bucket=com.twilio.music.ambient"),
                muted: isMuted,
                startConferenceOnEnter: canControlConferenceOnEnter,
                endConferenceOnExit: canControlConferenceOnEnter);

            response.Append(dial);

            return TwiML(response);
        }
    }
}
