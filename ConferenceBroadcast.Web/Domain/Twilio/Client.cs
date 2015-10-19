using Twilio;

namespace ConferenceBroadcast.Web.Domain.Twilio
{
    public interface IClient
    {
        void Call(CallOptions options);
    }

    public class Client : IClient
    {
        private readonly ICredentials _credentials;

        public Client()
        {
            _credentials = new Credentials();
        }

        public Client(ICredentials credentials)
        {
            _credentials = credentials;
        }

        public void Call(CallOptions options)
        {
            var client = new TwilioRestClient(
                _credentials.AccountSID, _credentials.AuthToken);
            client.InitiateOutboundCall(options);
        }
    }
}