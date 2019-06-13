using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace EchoServer
{
    class ClientHandler
    {
        private const int max_size = 1024;

        public static void HandleClient(object client)
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
            byte[] data = new byte[sock.Available];

            // Recebe o dados e manda-o novamente no canal
            if(too_big) {
                sock.Receive(data, 0, max_size, SocketFlags.Truncated);
                sent = sock.Send(data, 0, max_size, SocketFlags.Truncated);
            } else {
                sock.Receive(data);
                sent = sock.Send(data);
            }

            Console.WriteLine( $"Bytes enviados: {sent}" );
            sock.Close();
        }
    }
}