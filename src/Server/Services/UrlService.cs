using Microsoft.Extensions.Configuration;
using System;
using System.Net;

namespace FestoVideoStream.Services
{
    public class UrlService
    {
        #region Fields

        private readonly IConfiguration configuration;

        private readonly Uri _rtmpServerPath;
        private readonly Uri _httpServerPath;

        private static readonly Uri Dash = new Uri("/dash", UriKind.Relative);
        private static readonly Uri Frames = new Uri("/frames", UriKind.Relative);

        #endregion

        #region Ctor

        public UrlService(IConfiguration configuration)
        {
            this.configuration = configuration;

            this._rtmpServerPath = new Uri(this.configuration.GetValue<string>("RtmpServerPath"), UriKind.Absolute);
            this._httpServerPath = new Uri(this.configuration.GetValue<string>("HttpServerPath"), UriKind.Absolute);
        }

        #endregion

        #region Properties

        public Uri RtmpPath => new Uri(_rtmpServerPath, Dash);

        public Uri DashPath => new Uri(_httpServerPath, Dash);

        public Uri FramesPath => new Uri(_httpServerPath, Frames);

        #endregion

        #region Public methods

        public string GetDeviceRtmpPath(Guid id) => ReturnPathIfExists($"{RtmpPath}/{id}");

        public string GetDeviceDashManifest(Guid id) => ReturnPathIfExists($"{DashPath}/{id}.mpd");

        public bool UrlExists(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200;
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private methods

        private string ReturnPathIfExists(string path)
        {
            return UrlExists(path) ? path : null;
        }

        #endregion
    }
}