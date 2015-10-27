using System.Web;
using System.Web.Configuration;

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
        /// To use ngrok with IIS we had to rewrite the host-header.
        /// </remarks>
        public string Url
        {
            get
            {
                return _request.Url.Authority.Contains("localhost")
                    ? WebConfigurationManager.AppSettings["NgrokRequestUrl"]
                    : string.Format("{0}://{1}", _request.Url.Scheme, _request.Url.Authority);
            }
        }
    }
}