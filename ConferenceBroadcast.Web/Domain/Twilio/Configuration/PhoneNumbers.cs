using System.Web.Configuration;

namespace ConferenceBroadcast.Web.Domain.Twilio.Configuration
{
    public interface IPhoneNumbers
    {
        string Twilio { get; }
        string RapidResponse { get; }
    }

    public class PhoneNumbers : IPhoneNumbers
    {
        public string Twilio {
            get
            {
                return WebConfigurationManager.AppSettings["TwilioPhoneNumber"];
            }
        }

        public string RapidResponse {
            get
            {
                return WebConfigurationManager.AppSettings["RapidResponsePhoneNumber"];
            }
        }
    }
}