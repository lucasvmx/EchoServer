/*
    Simples Echo Server TCP

    Autor: Lucas Vieira de Jesus
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

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
        public static int port = 1701;

        public static bool have_port = false;
        public static bool have_ip = false;

        public static TcpListener listener = null;

        public static void HandleCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine( "Parando ...");
            
             if(listener != null)
                listener.Stop();

            Process.GetCurrentProcess().Close();
        }

        static void Main(string[] args)
        {
            Server server;
            
            ParseArgs(args);

            Console.CancelKeyPress += new ConsoleCancelEventHandler(HandleCancelKeyPress);

            // Inicializa o servidor
            server = new Server();

            try {
                if(have_port)
                    listener = server.Initialize(ip, port);
                else
                    listener = server.Initialize(ip);
            } catch(Exception error) {
                Console.WriteLine( $"Falha ao inicializar o EchoServer: {error.Message}");
                Environment.Exit(1);
            }

            while(true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Socket sock = client.Client;

                Thread handler = new Thread(new ParameterizedThreadStart(ClientHandler.HandleClient));
                handler.Start(sock);
            }
        }

        static void ParseArgs(string[] args)
        {
            foreach(string s in args) 
            {
                // Analisa os parâmetros fornecidos
                if(s.StartsWith(cmdline_options.option_ip))
                {
                    ip = s.Substring(s.IndexOf("=") + 1);

                    // Valida o ip fornecido
                    try {
                        IPAddress.Parse(ip);
                    } catch(Exception error)
                    {
                        Console.WriteLine( $"Erro: {error.Message}");
                        Environment.Exit(1);
                    }

                    have_ip = true;
                } else if(s.StartsWith(cmdline_options.option_port)) 
                {
                    try {
                        port = Convert.ToInt16(s.Substring(s.IndexOf("=") + 1));
                    } catch(FormatException error)
                    {
                        Console.WriteLine($"Erro: {error.Message}");
                        Environment.Exit(1);
                    }

                    have_port = true;
                }
            }
        }
    }
}
