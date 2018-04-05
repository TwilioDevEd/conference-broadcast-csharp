using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio.TwiML;
using System.Threading.Tasks;
using ConferenceBroadcast.Web.Domain.Twilio;

namespace ConferenceBroadcast.Web.Controllers
{
    public class RecordingsController : Controller
    {
        private readonly IPhoneNumbers _phoneNumbers;
        private readonly IClient _client;
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
        public async Task<ActionResult> Index()
        {
            var recordings = await _client.Recordings();

            var formattedRecordings = recordings.Select(r => new
            {
                url = ResolveUrl(r.Uri.ToString()),
                date = r.DateCreated?.ToString("ddd, dd MMM yyyy HH:mm:ss")
            });

            return Json(formattedRecordings, JsonRequestBehavior.AllowGet);
        }

        // POST: Recordings/Create
        [HttpPost]
        public async Task<ActionResult> Create(string phoneNumber)
        {
            var url = $"{_customRequest.Url}{Url.Action("Record")}";

            await _client.Call(phoneNumber, _phoneNumbers.Twilio, url);

            return new EmptyResult();
        }

        // POST: Recording/Record
        [HttpPost]
        public ActionResult Record()
        {
            var response = new VoiceResponse();
            response
                .Say("Please record your message after the beep. Press star to end your recording.")
                .Record(finishOnKey: "*", action: Url.Action("Hangup"));

            return Content(response.ToString(), "text/xml");
        }

        // POST: Recording/Hangup
        [HttpPost]
        public ActionResult Hangup()
        {
            var response = new VoiceResponse();
            response
                .Say("Your recording has been saved. Good bye!")
                .Hangup();

            return Content(response.ToString(), "text/xml");
        }

        private static string ResolveUrl(string uri)
        {
            return $"https://api.twilio.com{uri.Replace(".json", ".mp3")}";
        }
    }
}
