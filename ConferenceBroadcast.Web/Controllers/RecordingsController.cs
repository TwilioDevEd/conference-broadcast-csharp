using System.Linq;
using System.Web.Mvc;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio;

namespace ConferenceBroadcast.Web.Controllers
{
    public class RecordingsController : Controller
    {
        private readonly IClient _client;
        private readonly IPhoneNumbers _phoneNumbers;
        private readonly ICustomRequest _customRequest;

        public RecordingsController() : this(new Client(), new PhoneNumbers()) {}

        public RecordingsController(IClient client, IPhoneNumbers phoneNumbers, ICustomRequest customRequest = null)
        {
            _client = client;
            _phoneNumbers = phoneNumbers;
            _customRequest = customRequest ?? new CustomRequest(Request);
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
            _client.Call(new CallOptions
            {
                From = _phoneNumbers.Twilio,
                To = phoneNumber,
                // If there is no ngrok dependency we can use:
                // Url.Action("Action", "Controller", null, Request.Url.Scheme);
                Url = string.Format("{0}/{1}", Url.Action("Record", "Broadcast"), _customRequest.Url)
            });

            return new EmptyResult();
        }

        private static string ResolveUrl(string uri)
        {
            return string.Format(
                "https://api.twilio.com{0}", uri.Replace(".json", ".mp3"));
        }
    }
}