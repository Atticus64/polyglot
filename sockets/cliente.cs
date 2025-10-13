using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ClienteSocket
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddr = ipHostInfo.AddressList[0];
                //IPAddress ipAddr = new IPAddress([192, 168, 100, 95]);
                IPEndPoint remoteEp = new IPEndPoint(ipAddr, 4545);

                Socket sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                sender.Connect(remoteEp);
                //Console.WriteLine("Ingresa el dato: ");
                string data = Console.ReadLine();
                data += "<EOF>";
                byte[] msg = Encoding.ASCII.GetBytes(data);
                int bytesSent = sender.Send(msg);
                Console.WriteLine("Mensaje enviado!");
                sender.Shutdown(SocketShutdown.Both);
                sender.Close();
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}


