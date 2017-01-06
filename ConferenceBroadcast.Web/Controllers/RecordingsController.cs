using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Client = ConferenceBroadcast.Web.Domain.Twilio.Client;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using System.Threading.Tasks;

namespace ConferenceBroadcast.Web.Controllers
{
    public class RecordingsController : Controller
    {
        private readonly IPhoneNumbers _phoneNumbers;
        private ICustomRequest _customRequest;

        public RecordingsController() : this(new PhoneNumbers()) {}

        public RecordingsController(IPhoneNumbers phoneNumbers, ICustomRequest customRequest = null)
        {
            new Client();
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
            var recordings = await RecordingResource.ReadAsync();

            var formattedRecordings = recordings.Select(r => new
            {
                url = ResolveUrl(r.Uri.ToString()),
                date = r.DateCreated?.ToString("ddd, dd MMM yyyy HH:mm:ss")
            });

            return Json(formattedRecordings, JsonRequestBehavior.AllowGet);
        }

        // POST: Recordings/Create
        [HttpPost]
        public ActionResult Create(string phoneNumber)
        {
            var url = string.Format("{0}{1}", _customRequest.Url, Url.Action("Record"));

            CallResource.Create(new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_phoneNumbers.Twilio), url: new System.Uri(url));

            return new EmptyResult();
        }

        // POST: Recording/Record
        [HttpPost]
        public ActionResult Record()
        {
            var response = new VoiceResponse();
            response.Say(
                "Please record your message after the beep. Press star to end your recording.");
            response.Record(finishOnKey: "*",
                            action: Url.Action("Hangup"));

            return Content(response.ToString(), "text/xml");
        }

        // POST: Recording/Hangup
        [HttpPost]
        public ActionResult Hangup()
        {
            var response = new VoiceResponse();
            response.Say(
                "Your recording has been saved. Good bye!");
            response.Hangup();

            return Content(response.ToString(), "text/xml");
        }

        private static string ResolveUrl(string uri)
        {
            return string.Format(
                "https://api.twilio.com{0}", uri.Replace(".json", ".mp3"));
        }
    }
}
