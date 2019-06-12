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

        public TcpListener Initialize(string ip = "")
        {
            IPAddress addr;
            TcpListener listener;

            // Verifica o endereço de IP
            if(string.IsNullOrEmpty(ip)) 
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Any, Program.port);

                Console.WriteLine($"Inicializando o cliente com o IP {ep.Address.ToString()}");

                listener = new TcpListener(ep);
            } else {
                addr = IPAddress.Parse(ip);

                Console.WriteLine($"Inicializando o cliente com o IP {ip}");

                // Inicia a escuta por conexões
                listener = new TcpListener(addr, 8085);
            }
            
            listener.Start();

            return listener;
        }

        public TcpListener Initialize(string ip = "", int port = 8085)
        {
            IPAddress addr;
            TcpListener listener;

            // Converte a string para uma instância da classe IPAddress
            addr = IPAddress.Parse(ip);

            Console.WriteLine($"Inicializando o cliente com o IP {ip}");

            // Inicia a escuta por conexões
            listener = new TcpListener(addr, port);
            listener.Start();

            return listener;
        }
    }
}