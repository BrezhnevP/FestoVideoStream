namespace FestoVideoStream.Models
{
    public class RtmpServerNotification
    {
        public string Addr { get; set; }

        /// <summary>
        ///     Nginx client id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        ///     Application name
        /// </summary>
        public string App { get; set; }

        /// <summary>
        ///     Stream name
        /// </summary>
        public string Name { get; set; }

        public string PageUrl { get; set; }

        public string Time { get; set; }

        public string Timestamp { get; set; }

    }
}