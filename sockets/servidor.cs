using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SocketServidor
{
    class Program
    {
        public static string data = null;
        public static void Main(string[] args)
        {
            byte[] bytes = new byte[1024];

            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHostInfo.AddressList[0];
            IPEndPoint localEndpoint = new IPEndPoint(ipAddr, 6000);
            
            Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Console.WriteLine("Servidor esperando");
                listener.Bind(localEndpoint);
                listener.Listen(10);

                while (true)
                {
                    Socket handler = listener.Accept();
                    data = null;

                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    Console.WriteLine(data);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Read();
        }
    }
}


