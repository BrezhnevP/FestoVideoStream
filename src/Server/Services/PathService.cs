using Microsoft.Extensions.Configuration;
using System;

namespace FestoVideoStream.Services
{
    public class PathService
    {
        #region Fields

        private static readonly Uri DashPath = new Uri("/dash", UriKind.Relative);
        private static readonly Uri HlsPath = new Uri("/hls", UriKind.Relative);
        private static readonly Uri FramesPath = new Uri("/frames", UriKind.Relative);

        private readonly Uri rtmpServerPath;
        private readonly Uri httpServerPath;

        #endregion

        #region Ctor

        public PathService(IConfiguration configuration)
        {
            this.rtmpServerPath = new Uri(configuration.GetValue<string>("RtmpServerPath"), UriKind.Absolute);
            this.httpServerPath = new Uri(configuration.GetValue<string>("HttpServerPath"), UriKind.Absolute);
            this.FramesDirectory = configuration.GetValue<string>("FramesDirectory");
        }

        #endregion

        #region Properties

        public Uri RtmpUrl => new Uri(this.rtmpServerPath, DashPath);

        public Uri DashUrl => new Uri(this.httpServerPath, DashPath);

        public Uri HlsUrl => new Uri(this.httpServerPath, HlsPath);

        public Uri FramesUrl => new Uri(this.httpServerPath, FramesPath);

        public string FramesDirectory { get; }

        #endregion

        #region Public methods

        public string GetDeviceRtmpPath(Guid id) => $"{this.RtmpUrl}/{id}";

        public string GetDeviceDashManifest(Guid id) => $"{this.DashUrl}/{id}.mpd";

        public string GetDeviceHlsManifest(Guid id) => $"{this.DashUrl}/{id}.m3u8";

        #endregion
    }
}