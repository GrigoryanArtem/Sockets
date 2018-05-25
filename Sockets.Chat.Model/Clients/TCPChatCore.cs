// Copyright 2018 Grigoryan Artem
// Licensed under the Apache License, Version 2.0

using Sockets.Chat.Model.Data.Messages;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Sockets.Chat.Model.Clients
{
    public class TCPChatCore : ITCPChatClient
    {
        #region Members

        private TcpClient mClient;
        private NetworkStream mNetworkStream;
        private Thread mReceiveDataThread;

        private MessageHandlersService mMessageHandlers;

        #endregion

        public TCPChatCore(string ipAddress, int port, object handler = null)
        {
            IPAddress = IPAddress.Parse(ipAddress);
            Port = port;

            if(handler != null)
                mMessageHandlers = new MessageHandlersService(handler);
        }

        #region Properties

        public int Port { get; set; }
        public IPAddress IPAddress { get; set; }

        #endregion

        #region Public methods
        public void Connect()
        {
            mClient = new TcpClient();

            var connection = mClient.BeginConnect(IPAddress, Port, null, null);
            var success = connection.AsyncWaitHandle.WaitOne(
                TimeSpan.FromSeconds(Constants.ConnectionTimeout));

            if (!success)
                throw new ServerUnavailableException();

            mNetworkStream = mClient.GetStream();
            mReceiveDataThread = new Thread(client => ReceiveData((TcpClient)client));

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

        #region Private methods

        private void ReceiveData(TcpClient client)
        {
            NetworkStream ns = client.GetStream();
            byte[] receivedBytes = new byte[Constants.MessageSize];
            int byte_count;

            while ((byte_count = ns.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
            {
                var messageData = Encoding.UTF8.GetString(receivedBytes, 0, byte_count);
                var message = ChatMessage.Parse(messageData);

                mMessageHandlers?.Invoke(message);
            }
        }

        #endregion
    }
}
