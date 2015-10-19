using System.Linq;
using System.Web.Mvc;
using ConferenceBroadcast.Web.Domain.Twilio;
using Twilio;

namespace ConferenceBroadcast.Web.Controllers
{
    public class RecordingsController : Controller
    {
        private readonly IClient _client;
        private readonly IPhoneNumbers _phoneNumbers;

        public RecordingsController() : this(new Client(), new PhoneNumbers()) {}

        public RecordingsController(IClient client, IPhoneNumbers phoneNumbers)
        {
            _client = client;
            _phoneNumbers = phoneNumbers;
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
                Url = Url.Action("Record", "Broadcast", null, Request.Url.Scheme)
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