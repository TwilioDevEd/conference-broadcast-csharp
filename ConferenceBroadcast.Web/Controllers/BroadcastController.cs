using Client = ConferenceBroadcast.Web.Domain.Twilio.Client;
using ConferenceBroadcast.Web.Domain.Twilio;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Twilio.Rest.Api.V2010.Account;
using Twilio.TwiML;
using Twilio.Types;
using WebGrease.Css.Extensions;


namespace ConferenceBroadcast.Web.Controllers
{
    public class BroadcastController : Controller
    {
        private readonly IPhoneNumbers _phoneNumbers;
        private ICustomRequest _customRequest;
        public BroadcastController() : this(new PhoneNumbers()) {}

        public BroadcastController(IPhoneNumbers phoneNumbers, ICustomRequest customRequest = null)
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

        // GET: Broadcast
        public ActionResult Index()
        {
            return View();
        }

        // GET: Broadcast/Send
        public ActionResult Send(string numbers, string recordingUrl)
        {
            var url = string.Format("{0}{1}", _customRequest.Url,
                Url.Action("Play", new {recordingUrl}));

            VolunteersNumbers(numbers).ForEach(number =>
                CallResource.Create(new PhoneNumber(number),
                new PhoneNumber(_phoneNumbers.Twilio), url: new System.Uri(url)));

            return View();
        }

        // POST: Broadcast/Play
        [HttpPost]
        public ActionResult Play(string recordingUrl)
        {
            var response = new VoiceResponse();
            response.Play(recordingUrl);

            return Content(response.ToString(), "text/xml");
        }

        private static IEnumerable<string> VolunteersNumbers(string numbers)
        {
            return numbers.Split(',').Select(n => n.Trim());
        }
    }
}
