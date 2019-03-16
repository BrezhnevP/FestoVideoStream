using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FestoVideoStream.Services
{
    public class ConnectionService
    {
        public bool RtmpAvailable(string url)
        {
            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            var tcpClient = new TcpClient();
            tcpClient.Connect(uri.Host, uri.Port);

            return tcpClient.Connected;
        }

        public async Task<bool> UrlExists(string url)
        {
            if (url != null)
            {
                var webRequest = WebRequest.Create(url);
                webRequest.Timeout = 1200;
                webRequest.Method = "HEAD";

                try
                {
                    await webRequest.GetResponseAsync();
                }
                catch
                {
                    return false;
                }

                return true;
            }

            return false;
        }
    }
}