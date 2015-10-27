using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio;

namespace ConferenceBroadcast.Web.Domain.Twilio
{
    public interface IClient
    {
        void Call(CallOptions options);
        IEnumerable<Recording> Recordings();

    }

    public class Client : IClient
    {
        private readonly TwilioRestClient _client;

        public Client()
        {
            ICredentials credentials = new Credentials();
            _client = new TwilioRestClient(credentials.AccountSID, credentials.AuthToken);
        }

        public Client(ICredentials credentials)
        {
            _client = new TwilioRestClient(credentials.AccountSID, credentials.AuthToken);
        }

        public void Call(CallOptions options)
        {
            _client.InitiateOutboundCall(options);
        }

        public IEnumerable<Recording> Recordings()
        {
            return _client.ListRecordings().Recordings;
        }
    }
}
