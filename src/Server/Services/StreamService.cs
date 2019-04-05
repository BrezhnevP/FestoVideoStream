using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace FestoVideoStream.Services
{
    public class StreamService
    {
        #region Fields

        private readonly PathService pathService;

        #endregion

        #region Ctor

        public StreamService(PathService pathService)
        {
            this.pathService = pathService;
        }

        #endregion

        #region Public methods

        public string GetFramesFilePattern(Guid id) => $"out_{id}_%03d.jpg";

        public string GetFramesFileUriPattern(Guid id) => $"out_{id}_" + "{0}.jpg";

        public IEnumerable<Uri> GetFilesUri(Guid id, int count)
        {
            var pattern = this.GetFramesFileUriPattern(id);
            var files = Enumerable.Range(0, count).Select(x => new Uri($"{this.pathService.FramesUrl}/{string.Format(pattern, count)}"));

            return files;
        }

        #endregion

    }
}