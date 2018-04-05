using System.Web.Configuration;

namespace ConferenceBroadcast.Web.Domain.Twilio.Configuration
{
    public interface ICredentials
    {
        string AccountSID { get; }
        string AuthToken { get; }
    }

    public class Credentials : ICredentials
    {
        public string AccountSID
        {
            get
            {
                return WebConfigurationManager.AppSettings["TwilioAccountSid"] ??
                    "ACXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            }
        }
        public string AuthToken
        {
            get
            {
                return WebConfigurationManager.AppSettings["TwilioAuthToken"] ??
                    "aXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            }
        }
    }
}
