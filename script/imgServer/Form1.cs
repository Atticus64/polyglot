using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;


namespace imgServer
{
    public partial class Form1 : Form
    {

        public static string data;
        public static Thread? srvThread;
        public static Socket listener;
        bool connstat;

        public Form1()
        {
            InitializeComponent();
        }

        private void start_server()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddr = ipHostInfo.AddressList[0];
            IPAddress ipAddr = new IPAddress([0, 0, 0, 0]);
            byte[] bytes = new byte[1024];
            IPEndPoint localEndpoint = new IPEndPoint(ipAddr, 15000);

            listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                //Console.WriteLine("Servidor esperando");
                listener.Bind(localEndpoint);
                listener.Listen(10);

                statusLb.Invoke(new MethodInvoker(delegate () { 
                    statusLb.Text = "Encendido 🔛";
                }));
                while (true)
                {
                    Socket handler = listener.Accept();
                    data = null;

                    while (true)
                    {
                        string oldData = data;
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (oldData == data)
                        {
                            break;
                        }
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }
                    //Console.WriteLine(data);
                    //string extension = Server.DetectarExtension(imagenBytes);
                    //string path = "C:\\Users\\Jonathan\\Desktop\\testFile." + extension;
                    //File.Delete(path);
                    //File.WriteAllBytes(path, imagenBytes);

                    data = data.Replace("<EOF>", "");
                    byte[] imagenBytes = Convert.FromBase64String(data);
                    Image img = Image.FromStream(new MemoryStream(imagenBytes));
                    var bmp = new Bitmap(img);

                    imgBox.Invoke(new MethodInvoker(delegate ()
                    {
                        imgBox.Width = bmp.Width / 3;
                        imgBox.Height = bmp.Height / 3;
                        imgBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        imgBox.Image = bmp;
                        //imgBox.ImageLocation = data;
                        //imgBox.Image = img;
                    }));


                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


        private void startthread()
        {
            if (connstat)
            {
                return;
            }
            srvThread = new Thread(start_server); srvThread.IsBackground = true; srvThread.Start();
            connstat = true;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            startthread();
        }


        public class Server
        {

            public static string DetectarExtension(byte[] imagenBytes)
            {
                if (imagenBytes.Length < 4)
                    return "bin";

                // JPG: FF D8 FF
                if (imagenBytes[0] == 0xFF && imagenBytes[1] == 0xD8 && imagenBytes[2] == 0xFF)
                    return "jpg";

                // PNG: 89 50 4E 47
                if (imagenBytes[0] == 0x89 && imagenBytes[1] == 0x50 &&
                    imagenBytes[2] == 0x4E && imagenBytes[3] == 0x47)
                    return "png";

                return "bin";
            }

            public static Socket get_socket()
            {

                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddr = ipHostInfo.AddressList[0];
                IPAddress ipAddr = new IPAddress([0, 0, 0, 0]);
                IPEndPoint localEndpoint = new IPEndPoint(ipAddr, 15000);

                Socket listener = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    //Console.WriteLine("Servidor esperando");
                    listener.Bind(localEndpoint);
                    listener.Listen(10);

                    return listener;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    return null;
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (srvThread != null)
            {
               listener.Close();
               connstat = false;
               srvThread = null;
               statusLb.Text = "Apagado 📴";
            }
        }
    }
}
