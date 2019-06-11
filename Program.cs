/*
    Simples Echo Server TCP

    Autor: Lucas Vieira de Jesus
*/

using System;
using System.Net.Sockets;

namespace EchoServer
{
    class Program
    {
        class cmdline_options 
        {
            public static string option_ip = "--ip=";
            public static string option_port = "--port=";
        }

        public static string ip = "";
        public static int port = 8085;

        public static bool have_port = false;
        public static bool have_ip = false;

        public static TcpListener listener = null;

        public static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine( "Parando ...");
            
             if(listener != null)
                listener.Stop();
        }

        static void Main(string[] args)
        {
            Server server;
            
            ParseArgs(args);

            Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleCancelKeyPress);

            // Inicializa o servidor
            server = new Server();

            if(have_port)
                listener = server.Initialize(ip, port);
            else
                listener = server.Initialize(ip);

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Socket sock = client.Client;
                byte[] data = new byte[512];

                Console.WriteLine( "Cliente conectado" );
                sock.Receive(data);
                sock.Send(data);

                Console.WriteLine( "Fechando conexão...");
                sock.Close();
            }
        }

        static void ParseArgs(string[] args)
        {
            foreach(string s in args) 
            {
                if(s.StartsWith(cmdline_options.option_ip))
                {
                    ip = s.Substring(s.IndexOf("=") + 1);
                    have_ip = true;
                } else if(s.StartsWith(cmdline_options.option_port)) {
                    port = Convert.ToInt16(s.Substring(s.IndexOf("=") + 1));
                    have_port = true;
                }
            }
        }
    }
}
