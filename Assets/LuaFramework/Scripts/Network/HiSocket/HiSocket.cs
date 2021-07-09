using System;
using System.Collections;
using System.Collections.Generic;
using HiSocket.Tcp;
using UnityEngine;

namespace LuaFramework
{
    public class HiSocket : IDisposable
    {
        static int SocketTimeout = 1000;
        static TcpConnection _connection;
        static TcpConnection Connection
        {
            get
            {
                if (_connection == null)
                    _connection = new TcpConnection(new HiPack());
                return _connection;
            }
        }

        void OnConnected()
        {
            var buffer = new ByteBuffer();
            buffer.WriteInt(0);
            buffer.WriteString("服务器链接成功");
            NetworkManager.AddEvent(Protocal.Connect, new ByteBuffer(buffer.ToBytes()));
            buffer.Close();
        }

        void OnDisconnected()
        {
            var buffer = new ByteBuffer();
            buffer.WriteInt(0);
            buffer.WriteString("服务器链接断开");
            NetworkManager.AddEvent(Protocal.Disconnect, new ByteBuffer(buffer.ToBytes()));
            buffer.Close();
        }

        void OnReceiveMessage(byte[] message)
        {
            ByteBuffer buffer = new ByteBuffer(message);
            int mainId = buffer.ReadShort();
            NetworkManager.AddEvent(mainId, buffer);
        }

        public void OnRegister()
        {
            Connection.OnConnected += OnConnected;
            Connection.OnDisconnected += OnDisconnected;
            Connection.OnReceiveMessage += OnReceiveMessage;
        }

        /// <summary>
        /// Connect Server
        /// </summary>
        public void Connect()
        {            
            Connection.Connect(AppConst.SocketAddress, AppConst.SocketPort);
            /*
            Connection.Socket.NoDelay = true;
            Connection.Socket.SendTimeout = SocketTimeout;
            Connection.Socket.ReceiveTimeout = SocketTimeout;
            */
        }

        public void Send(byte[] bytes)
        {
            Connection.Send(bytes);
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }
}