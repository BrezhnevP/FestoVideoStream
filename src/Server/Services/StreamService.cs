using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace FestoVideoStream.Services
{
    public class StreamService
    {
        private readonly IConfiguration _configuration;

        public StreamService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetRtmpPath(Guid id)
        {
            var serverPath = _configuration.GetValue<string>("RtmpServerPath");
            var manifestPath = $"{serverPath}/dash/{id}";
            return manifestPath;
        }

        public string GetDashManifest(Guid id)
        {
            var serverPath = _configuration.GetValue<string>("HttpServerPath");
            var manifestPath = $"{serverPath}/dash/{id}.mpd";
            return UrlExists(manifestPath) ? manifestPath : null;
        }

        public bool UrlExists(string url)
        {
            var result = true;

            var webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200;
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}