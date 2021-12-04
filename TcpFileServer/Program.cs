using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace TcpFileServer
{
    class Program
    {
        // my favorite prime number used as port number
        public const int Port = 14593;

        static void Main()
        {
            Console.WriteLine("TCP file server, port " + Port);
            TcpListener listener = new TcpListener(IPAddress.Any, Port);

            listener.Start();

            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Console.Write("Incoming file ...");
                Task.Run(() => HandleClient(socket));
            }
        }

        public static void HandleClient(TcpClient socket)
        {
            NetworkStream ns = socket.GetStream();
            string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fileName = "somefile" + Guid.NewGuid() + ".mp4";
            string fullName = Path.Combine(docPath, fileName);
            using (FileStream fileStream = new FileStream(fullName, FileMode.Create))
            {
                // https://docs.microsoft.com/en-us/dotnet/api/system.io.stream.copyto
                ns.CopyTo(fileStream);
            }
            socket.Close();
            Console.WriteLine(" ... done");
        }
    }
}
