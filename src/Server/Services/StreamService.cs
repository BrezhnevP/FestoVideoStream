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

        private readonly UrlService urlService;

        #endregion

        #region Ctor

        public StreamService(UrlService urlService)
        {
            this.urlService = urlService;
        }

        #endregion

        #region Public methods

        public string GetFramesFilePattern(Guid id) => $"out_{id}_%03d.jpg";

        public string GetFramesFileUriPattern(Guid id) => $"out_{id}_" + "{0}.jpg";

        public IEnumerable<Uri> GetFilesUri(Guid id, int count)
        {
            var pattern = this.GetFramesFileUriPattern(id);
            var files = Enumerable.Range(0, count).Select(x => new Uri($"{urlService.FramesPath}/{string.Format(pattern, count)}"));

            return files;
        }

        #endregion

    }
}