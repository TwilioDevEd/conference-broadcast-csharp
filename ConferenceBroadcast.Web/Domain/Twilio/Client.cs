using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceBroadcast.Web.Domain.Twilio.Configuration;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ConferenceBroadcast.Web.Domain.Twilio
{
    public interface IClient
    {
        Task<CallResource> Call(string to, string from, string url);
        Task<List<RecordingResource>> Recordings();
    }

    public class Client : IClient
    {
        private readonly TwilioRestClient _client;

        public Client()
        {
            ICredentials credentials = new Credentials();
            _client = new TwilioRestClient(credentials.AccountSID, credentials.AuthToken);
        }

        public async Task<CallResource> Call(string to, string from, string url)
        {
            return await CallResource.CreateAsync(
                new PhoneNumber(to), new PhoneNumber(from), url: new Uri(url), client: _client);
        }

        public async Task<List<RecordingResource>> Recordings()
        {
            var recordings =  await RecordingResource.ReadAsync(client: _client);
            return recordings.ToList();
        }
    }
}
