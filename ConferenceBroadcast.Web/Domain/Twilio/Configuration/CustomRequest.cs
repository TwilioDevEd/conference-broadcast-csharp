using System.Web;

// ReSharper disable PossibleNullReferenceException

namespace ConferenceBroadcast.Web.Domain.Twilio.Configuration
{
    public interface ICustomRequest
    {
        string Url { get; }
    }

    public class CustomRequest: ICustomRequest
    {
        private readonly HttpRequestBase _request;

        public CustomRequest(HttpRequestBase request)
        {
            _request = request;
        }

        /// <summary>
        /// Gets the Request URL
        /// </summary>
        /// <remarks>
        /// This is a patch used to get the request URL when using ngrok.
        /// To use ngrok with IIS we had to rewrite the host-header. See:
        /// https://www.twilio.com/docs/usage/tutorials/how-use-ngrok-windows-and-visual-studio-test-webhooks#get-ngrok-host-name
        /// </remarks>
        public string Url => GetProtocol() + "://" + GetDomainAndPort();

        private string GetDomainAndPort()
        {
            if (_request.Headers["X-Original-Host"] != null)
            {
                // Assume default port for protocol (http=80, https=443)
                return _request.Headers["X-Original-Host"];
            }

            // Leave off port if it's the default 80/443
            if (_request.Url.Port == 80 || _request.Url.Port == 443)
            {
                return _request.Url.Host;
            }
            return _request.Url.Host + ":" + _request.Url.Port;
        }

        private string GetProtocol()
        {
            if (_request.Headers["X-Forwarded-Proto"] != null)
            {
                return _request.Headers["X-Forwarded-Proto"];
            }

            return _request.IsSecureConnection ? "https" : "http";
        }
    }
}