using System;
using System.Collections.Generic;
using System.Linq;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio;

namespace ConferenceBroadcast.Web.Domain.Twilio
{
    public class Client
    {
        public Client()
        {
            ICredentials credentials = new Credentials();
            TwilioClient.Init(credentials.AccountSID, credentials.AuthToken);
        }

        public Client(ICredentials credentials)
        {
            TwilioClient.Init(credentials.AccountSID, credentials.AuthToken);
        }
    }
}
