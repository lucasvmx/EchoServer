using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;

namespace EchoServer
{
    class ClientHandler
    {
        private const int max_size = 1024;

        public static void HandleTcpClient(object client)
        {
            Socket sock = client as Socket;
            
            bool too_big = false;
            int sent = 0;

            Console.WriteLine( $"[tcp] client connected: {sock.RemoteEndPoint.ToString()}" );
            Console.WriteLine( $"[tcp] bytes in channel: {sock.Available}");

            if(sock.Available > max_size) {
                Console.WriteLine( $"[tcp] payload too big" );
                too_big = true;
            }

            // Aloca os recursos
            byte[] data;

            if(!too_big)
                data = new byte[sock.Available];
            else
                data = new byte[max_size];

            // Recebe o dados e manda-o novamente no canal
            if(too_big) {
                sock.Receive(data, 0, max_size, SocketFlags.Truncated);
                sent = sock.Send(data, 0, max_size, SocketFlags.Truncated);
            } else {
                sock.Receive(data);
                sent = sock.Send(data);
            }

            Console.WriteLine( $"[tcp] {sent} bytes sent" );
            sock.Shutdown(SocketShutdown.Receive);
        }

        public static void HandleHttpClient(object context)
        {
            HttpListenerContext ctx = context as HttpListenerContext;
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse res = ctx.Response;
            byte[] buffer = new byte[8192];
            int len = 0;

            Console.WriteLine($"[http] client connected: {req.RemoteEndPoint.ToString()}");

            // Validate content-type
            if(!String.IsNullOrEmpty(req.ContentType)) {
                if(req.ContentType != "text/html") {
                    Console.WriteLine($"[http] we can't handle this type '{req.ContentType}'");
                    return;
                }
            }

            // Validate request type
            if(req.HttpMethod != "GET") {
                Console.WriteLine($"[http] unknown method '{req.HttpMethod}'");
                res.StatusCode = 405;
                res.AddHeader("Server", "Lucas's EchoServer");
                res.Close();

                return;
            }
            
            // Gets body data
            len = req.InputStream.Read(buffer, 0, buffer.Length);

            res.ContentType = "text/html";
            res.ContentEncoding = req.ContentEncoding;
            res.StatusCode = 200;
            res.AddHeader("Server", "Lucas's EchoServer");

            res.OutputStream.Write(buffer);
            res.Close();
        }
    }
}