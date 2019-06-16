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

            Console.WriteLine( $"Cliente conectado: {sock.RemoteEndPoint.ToString()} protocolo {sock.ProtocolType.ToString()}" );
            Console.WriteLine( $"Bytes no canal: {sock.Available}");

            if(sock.Available > max_size) {
                Console.WriteLine( $"Payload muito grande!" );
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

            Console.WriteLine( $"Bytes enviados: {sent}" );
            sock.Shutdown(SocketShutdown.Receive);
        }

        public static void HandleHttpClient(object client)
        {
            Socket sock = client as Socket;
            bool too_big = false;
            int sent = 0;

            Console.WriteLine( $"[HTTP] Cliente conectado: {sock.RemoteEndPoint.ToString()} protocolo {sock.ProtocolType.ToString()}" );
            Console.WriteLine( $"[HTTP] Bytes no canal: {sock.Available}");

            if(sock.Available > max_size) {
                Console.WriteLine( $"[HTTP] Payload muito grande!" );
                too_big = true;
            }

            // Aloca os recursos
            byte[] data;

            if(!too_big)
                data = new byte[sock.Available];
            else
                data = new byte[max_size];

            // Recebe o dados e manda-o novamente no canal
            if(too_big) 
            {
                sock.Receive(data, 0, max_size, SocketFlags.Truncated);
                
                // Compila a resposta HTTP
                string response = BuildHttpResponse(data);
                
                Console.WriteLine($"[HTTP] Enviando resposta: {response}");

                // Converte para array de bytes
                data = Encoding.UTF8.GetBytes(response);
                sent = sock.Send(data, 0, max_size, SocketFlags.Truncated);
            } else {
                sock.Receive(data);

                // Compila a resposta HTTP
                string response = BuildHttpResponse(data);
                
                Console.WriteLine($"[HTTP] Enviando resposta: {response}");

                // Converte para array de bytes
                data = Encoding.UTF8.GetBytes(response);
                sent = sock.Send(data);
            }

            Console.WriteLine( $"[HTTP] Bytes enviados: {sent}" );
            sock.Shutdown(SocketShutdown.Receive);
        }

        public static void HandleClient(object client)
        {
            Socket sock = client as Socket;
            String protocol = sock.ProtocolType.ToString().ToLower();

            if(protocol == "tcp")
                HandleTcpClient(client);
            else if(protocol == "http")
                HandleHttpClient(client);
            else
                Console.WriteLine($"Protocolo não suportado");
        }

        private static string BuildHttpResponse(byte[] data)
        {
            string payload;
            string response = $"HTTP/1.1 200 OK\r\n";
            response += $"Content-Type: text/html\r\n";
            response += $"Content-Length: $SIZE\r\n";
            response += $"Server: Lucas-EchoServer\r\n";
            response += $"Connection: closed\r\n";
            response += $"<html><body><p>$PAYLOAD</p></body></html>\r\n";

            payload = payload = Encoding.UTF8.GetString(data);

            // Compila a resposta HTTP
            response = response.Replace("$SIZE", $"{payload.Length}");
            response = response.Replace("$PAYLOAD", payload);

            return response;
        }
    }
}