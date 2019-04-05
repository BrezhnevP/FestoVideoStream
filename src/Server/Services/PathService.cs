using Microsoft.Extensions.Configuration;
using System;

namespace FestoVideoStream.Services
{
    public class PathService
    {
        #region Fields

        private static readonly Uri DashPath = new Uri("/dash", UriKind.Relative);
        private static readonly Uri FramesPath = new Uri("/frames", UriKind.Relative);

        private readonly Uri _rtmpServerPath;
        private readonly Uri _httpServerPath;

        #endregion

        #region Ctor

        public PathService(IConfiguration configuration)
        {
            this._rtmpServerPath = new Uri(configuration.GetValue<string>("RtmpServerPath"), UriKind.Absolute);
            this._httpServerPath = new Uri(configuration.GetValue<string>("HttpServerPath"), UriKind.Absolute);
            this.FramesDirectory = configuration.GetValue<string>("FramesDirectory");
        }

        #endregion

        #region Properties

        public Uri RtmpUrl => new Uri(this._rtmpServerPath, DashPath);

        public Uri DashUrl => new Uri(this._httpServerPath, DashPath);

        public Uri FramesUrl => new Uri(this._httpServerPath, FramesPath);

        public string FramesDirectory { get; }

        #endregion

        #region Public methods

        public string GetDeviceRtmpPath(Guid id) => $"{this.RtmpUrl}/{id}";

        public string GetDeviceDashManifest(Guid id) => $"{this.DashUrl}/{id}.mpd";

        #endregion
    }
}