using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FestoVideoStream.Services
{
    public class StreamService
    {
        #region Fields

        private readonly IConfiguration configuration;

        private readonly string _rtmpServerPath;
        private readonly string _httpServerPath;

        #endregion

        #region Ctor

        public StreamService(IConfiguration configuration)
        {
            this.configuration = configuration;

            this._rtmpServerPath = this.configuration.GetValue<string>("RtmpServerPath");
            this._httpServerPath = this.configuration.GetValue<string>("HttpServerPath");
        }

        #endregion

        #region Properties

        public string RtmpPath => $"{_rtmpServerPath}/dash/";

        public string DashPath => $"{_httpServerPath}/dash/";

        public string FramesPath => $"{_httpServerPath}/frames/";

        #endregion

        #region Public methods

        public string GetDeviceRtmpPath(int id) => ReturnPathIfExists($"{RtmpPath}{id}");

        public string GetDeviceDashManifest(int id) => ReturnPathIfExists($"{DashPath}{id}.mpd");

        public string GetFramesFilePattern(int id) => $"out_{id}_%03d.jpg";

        public string GetFramesFileUriPattern(int id) => $"out_{id}_" + "{0}.jpg";

        public IEnumerable<Uri> GetFilesUri(int id, int count)
        {
            var pattern = this.GetFramesFileUriPattern(id);
            var files = Enumerable.Range(0, count).Select(x => new Uri(FramesPath + string.Format(pattern, count)));

            return files;
        }

        #endregion

        private bool UrlExists(string url)
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

        private string ReturnPathIfExists(string path)
        {
            return UrlExists(path) ? path : null;
        }
    }
}