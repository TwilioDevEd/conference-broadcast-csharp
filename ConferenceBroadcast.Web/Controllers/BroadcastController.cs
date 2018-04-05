using System;
using Client = ConferenceBroadcast.Web.Domain.Twilio.Client;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using Twilio.AspNet.Mvc;
using Twilio.TwiML;

namespace ConferenceBroadcast.Web.Controllers
{
    public class BroadcastController : TwilioController
    {
        private readonly IPhoneNumbers _phoneNumbers;
        private readonly IClient _client;
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
        public async Task<ActionResult> Send(string numbers, string recordingUrl)
        {
            var url = _customRequest.Url + Url.Action("Play", new {recordingUrl});

            var calls = VolunteersNumbers(numbers).Select(
                number => _client.Call(number, _phoneNumbers.Twilio, url));

            await Task.WhenAll(calls);

            return View();
        }

        // POST: Broadcast/Play
        [HttpPost]
        public ActionResult Play(string recordingUrl)
        {
            var response = new VoiceResponse();
            response.Play(new Uri(recordingUrl));

            return TwiML(response);
        }

        private static IEnumerable<string> VolunteersNumbers(string numbers)
        {
            return numbers.Split(',').Select(n => n.Trim());
        }
    }
}
