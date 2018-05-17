using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Sockets.Chat.Model.Clients
{
    public class TCPChatCore : ITCPChatClient
    {
        #region Members

        TcpClient mClient;
        NetworkStream mNetworkStream;
        Thread mReceiveDataThread;

        #endregion

        public TCPChatCore(string ipAddress, int port)
        {
            IPAddress = IPAddress.Parse(ipAddress);
            Port = port;
        }

        #region Properties

        public int Port { get; set; }
        public IPAddress IPAddress { get; set; }

        #endregion

        #region Public methods
        public void Connect(ParameterizedThreadStart receiveData)
        {
            mClient = new TcpClient();
            mClient.Connect(IPAddress, Port);

            mNetworkStream = mClient.GetStream();
            mReceiveDataThread = new Thread(receiveData);

            mReceiveDataThread.Start(mClient);
        }

        public void Disconnect()
        {
            if (!mClient.Client.Connected)
                return;

            mClient.Client.Shutdown(SocketShutdown.Send);
            mReceiveDataThread.Join();
            mNetworkStream.Close();
            mClient.Close();
        }

        public void SendMessage(ChatMessage message)
        {
            byte[] buffer = message.ToByteArray();
            mNetworkStream.Write(buffer, 0, buffer.Length);
        }

        #endregion
    }
}
