/*
    Simples Echo Server TCP

    Autor: Lucas Vieira de Jesus
*/

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EchoServer
{
    class Server
    {
        public Server()
        {
            
        }

        public HttpListener Initialize_Http()
        {
            HttpListener listener;
            listener = new HttpListener();
            listener.Prefixes.Add("http://127.0.0.1:1702/");
            listener.Start();

            Console.WriteLine($"[http] started http server");

            return listener;
        }

        public TcpListener Initialize(string ip = "")
        {
            IPAddress addr;
            TcpListener listener;

            // Verifica o endereço de IP
            if(string.IsNullOrEmpty(ip)) 
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Program.port);

                Console.WriteLine($"starting tcp server on {ep.Address.ToString()}:{Program.port}");

                listener = new TcpListener(ep);
            } else {
                addr = IPAddress.Parse(ip);

                Console.WriteLine($"starting tcp server on {ip}:{Program.port}");

                // Inicia a escuta por conexões
                listener = new TcpListener(addr, Program.port);
            }
            
            listener.Start();

            return listener;
        }

        public TcpListener Initialize(string ip = "", int port = 1701)
        {
            IPAddress addr;
            TcpListener listener;
            
            if(String.IsNullOrEmpty(ip)) ip = "0.0.0.0";
            
            // Converte a string para uma instância da classe IPAddress
            addr = IPAddress.Parse(ip);

            Console.WriteLine($"starting tcp server on {ip}:{port}");

            // Inicia a escuta por conexões
            listener = new TcpListener(addr, port);
            listener.Start();

            return listener;
        }
    }
}