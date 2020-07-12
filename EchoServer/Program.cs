using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace EchoServer
{
    class Program
    {
        static TcpListener listen;
        static Thread serverthread;
 
        static void Main(string[] args)
        {
            listen = new TcpListener(IPAddress.Parse("127.0.0.1"), 1234);
            serverthread = new Thread(new ThreadStart(DoListen));
            serverthread.Start();
        }
 
        private static void DoListen()
        {
            // Listen
            listen.Start();
            Console.WriteLine("Server: Started server");
 
            while (true)
            {
                Console.WriteLine("Server: Waiting...");
                TcpClient client = listen.AcceptTcpClient();
                Console.WriteLine("Server: Waited");
 
                // New thread with client
                Thread clientThread = new Thread(new ParameterizedThreadStart(DoClient));
                clientThread.Start(client);
            }
        }
 
        private static void DoClient(object client)
        {
            // Read data
            TcpClient tClient = (TcpClient)client;
 
            Console.WriteLine("Client (Thread: {0}): Connected!", Thread.CurrentThread.ManagedThreadId);
            do
            {
                if (!tClient.Connected)
                { 
                    tClient.Close();
                    Thread.CurrentThread.Abort();       // Kill thread.
                }
 
                if (tClient.Available > 0)
                {
                    // Resend
                    var buffer = new byte[1000];
                    tClient.GetStream().Read(buffer);
                    var s= Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(s);
                    
                    tClient.GetStream().WriteByte(new byte());
                }
 
                // Pause
                Thread.Sleep(100);
            } while (true);
        }
    }
}