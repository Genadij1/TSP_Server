using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Server
{
    static async Task Main()
    {
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        int port = 8888;
        TcpListener listener = new TcpListener(ip, port);
        listener.Start();

        Console.WriteLine("Server started. Waiting for a connection...");

        while (true)
        {
            TcpClient client = await listener.AcceptTcpClientAsync();
            _ = HandleClientAsync(client);
        }
    }

    private static async Task HandleClientAsync(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        IPEndPoint clientEndPoint = (IPEndPoint)client.Client.RemoteEndPoint;
        string clientIP = clientEndPoint.Address.ToString();

        byte[] buffer = new byte[1024];
        int receivedLength = await stream.ReadAsync(buffer, 0, buffer.Length);
        string clientMessage = Encoding.ASCII.GetString(buffer, 0, receivedLength);

        string serverResponse = "Hello client!";
        byte[] responseBytes = Encoding.ASCII.GetBytes(serverResponse);
        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

        Console.WriteLine($"о {DateTime.Now:HH:mm} від [{clientIP}] отримано рядок: {clientMessage}");

        client.Close();
    }
}

