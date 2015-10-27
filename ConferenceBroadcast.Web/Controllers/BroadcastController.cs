using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio;
using Twilio.TwiML;
using Twilio.TwiML.Mvc;
using WebGrease.Css.Extensions;
using Client = ConferenceBroadcast.Web.Domain.Twilio.Client;

namespace ConferenceBroadcast.Web.Controllers
{
    public class BroadcastController : TwilioController
    {
        private readonly IClient _client;
        private readonly IPhoneNumbers _phoneNumbers;
        private ICustomRequest _customRequest;
        public BroadcastController() : this(new Client(), new PhoneNumbers()) {}

        public BroadcastController(IClient client, IPhoneNumbers phoneNumbers, ICustomRequest customRequest = null)
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

        // GET: Broadcast
        public ActionResult Index()
        {
            return View();
        }

        // GET: Broadcast/Send
        public ActionResult Send(string numbers, string recordingUrl)
        {
            var url = string.Format("{0}{1}", _customRequest.Url, Url.Action("Play", new {recordingUrl}));
            VolunteersNumbers(numbers).ForEach(number =>
                _client.Call(new CallOptions
                {
                    From = _phoneNumbers.Twilio,
                    To = number,
                    Url = string.Format("{0}{1}", _customRequest.Url, Url.Action("Play", new {recordingUrl}))
                }));

            return View();
        }

        // POST: Broadcast/Play
        [HttpPost]
        public ActionResult Play(string recordingUrl)
        {
            var response = new TwilioResponse();
            response.Play(recordingUrl);

            return TwiML(response);
        }

        private static IEnumerable<string> VolunteersNumbers(string numbers)
        {
            return numbers.Split(',').Select(n => n.Trim());
        }
    }
}