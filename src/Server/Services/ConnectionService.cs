using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FestoVideoStream.Services
{
    public class ConnectionService
    {
        public static async Task<bool> UrlExistsAsync(string url)
        {
            if (url == null)
            {
                return false;
            }
            try
            {
                await GetHeadRequest(url).GetResponseAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static async Task<bool> CheckConnectionByTcp(IPEndPoint endPoint)
        {
            using var tcpClient = new TcpClient();
            await tcpClient.ConnectAsync(endPoint.Address, endPoint.Port);

            return tcpClient.Connected;
        }

        public static async Task<bool> CheckConnectionByPing(IPEndPoint endPoint)
        {
            var ping = new Ping();
            var response = await ping.SendPingAsync(endPoint.Address, 2000);

            return response.Status == IPStatus.Success;

        }

        public bool RtmpAvailable(string url)
        {
            Uri.TryCreate(url, UriKind.Absolute, out var uri);
            var tcpClient = new TcpClient();
            tcpClient.Connect(uri.Host, uri.Port);

            return tcpClient.Connected;
        }

        private static WebRequest GetHeadRequest(string url)
        {
            var webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200;
            webRequest.Method = WebRequestMethods.Http.Head;

            return webRequest;
        }
    }
}