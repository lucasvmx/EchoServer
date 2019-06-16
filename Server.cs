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

                Console.WriteLine($"Inicializando o cliente com o IP {ep.Address.ToString()} Porta {Program.port}");

                listener = new TcpListener(ep);
            } else {
                addr = IPAddress.Parse(ip);

                Console.WriteLine($"Inicializando o cliente com o IP {ip} Porta {Program.port}");

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

            Console.WriteLine($"Inicializando o cliente com o IP {ip} Porta {port}");

            // Inicia a escuta por conexões
            listener = new TcpListener(addr, port);
            listener.Start();

            return listener;
        }
    }
}