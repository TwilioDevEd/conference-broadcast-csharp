using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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
        public BroadcastController() : this(new Client(), new PhoneNumbers()) {}

        public BroadcastController(IClient client, IPhoneNumbers phoneNumbers)
        {
            _client = client;
            _phoneNumbers = phoneNumbers;
        }

        // GET: Broadcast
        public ActionResult Index()
        {
            return View();
        }

        // GET: Broadcast/Send
        public ActionResult Send(string numbers, string recordingUrl)
        {
            VolunteersNumbers(numbers).ForEach(number =>
                _client.Call(new CallOptions
                {
                    From = _phoneNumbers.Twilio,
                    To = number,
                    Url = Url.Action("Play", new {recordingUrl})
                }));

            return new EmptyResult();
        }

        // GET: Broadcast/Play
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