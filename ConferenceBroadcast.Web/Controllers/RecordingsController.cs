using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;
using Client = ConferenceBroadcast.Web.Domain.Twilio.Client;

namespace ConferenceBroadcast.Web.Controllers
{
    public class RecordingsController : TwilioController
    {
        private readonly IClient _client;
        private readonly IPhoneNumbers _phoneNumbers;
        private ICustomRequest _customRequest;

        public RecordingsController() : this(new Client(), new PhoneNumbers()) {}

        public RecordingsController(IClient client, IPhoneNumbers phoneNumbers, ICustomRequest customRequest = null)
        {
            _client = client;
            _phoneNumbers = phoneNumbers;
            _customRequest = customRequest;
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            _customRequest = new CustomRequest(requestContext.HttpContext.Request);
        }

        // GET: Recordings
        public ActionResult Index()
        {
            var recordings = _client.Recordings()
                .Select(r => new
                {
                    url = ResolveUrl(r.Uri.ToString()),
                    date = r.DateCreated.ToString("ddd, dd MMM yyyy HH:mm:ss")
                });

            return Json(recordings, JsonRequestBehavior.AllowGet);
        }

        // POST: Recordings/Create
        [HttpPost]
        public ActionResult Create(string phoneNumber)
        {
            var url = string.Format("{0}{1}", _customRequest.Url, Url.Action("Record"));
            _client.Call(new CallOptions
            {
                From = _phoneNumbers.Twilio,
                To = phoneNumber,
                // If there is no ngrok dependency we can use:
                // Url.Action("Action", "Controller", null, Request.Url.Scheme);
                Url = string.Format("{0}{1}", _customRequest.Url, Url.Action("Record"))
            });

            return new EmptyResult();
        }

        // POST: Recording/Record
        [HttpPost]
        public ActionResult Record()
        {
            var response = new TwilioResponse();
            response.Say(
                "Please record your message after the beep. Press star to end your recording");
            response.Record(new
            {
                finishOnKey = "*",
                action = Url.Action("Hangup")
            });

            return TwiML(response);
        }

        // POST: Recording/Hangup
        [HttpPost]
        public ActionResult Hangup()
        {
            var response = new TwilioResponse();
            response.Say(
                "Your recording has been saved. Good bye");
            response.Hangup();

            return TwiML(response);
        }

        private static string ResolveUrl(string uri)
        {
            return string.Format(
                "https://api.twilio.com{0}", uri.Replace(".json", ".mp3"));
        }
    }
}