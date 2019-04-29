using System;
using System.Collections.Generic;
using System.Linq;

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

        public static string GetFramesFilePattern(Guid id) => $"out_{id}_%03d.jpg";

        public static string GetFramesFileUriPattern(Guid id) => $"out_{id}_" + "{0}.jpg";

        #region Public methods

        public IEnumerable<Uri> GetFilesUri(Guid id, int count)
        {
            var pattern = GetFramesFileUriPattern(id);
            var files = Enumerable.Range(1, count).Select(x => new Uri($"{this.pathService.FramesUrl}/{string.Format(pattern, x)}"));

            return files;
        }

        #endregion

    }
}