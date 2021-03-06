using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace LuaFramework {
    public class NetworkManager : Manager {
        private HiSocket socket;
        static readonly object m_lockObject = new object();
        static Queue<KeyValuePair<int, ByteBuffer>> mEvents = new Queue<KeyValuePair<int, ByteBuffer>>();
        /// <summary>
        /// 协议回调
        /// </summary>
        static Dictionary<int, Action<ByteBuffer>> m_proto_callbacks = new Dictionary<int, Action<ByteBuffer>>();
        /// <summary>
        /// 协议描述
        /// </summary>
        static Dictionary<int, string> m_proto_descs = new Dictionary<int, string>();

        HiSocket SocketClient {
            get { 
                if (socket == null)
                    socket = new HiSocket();
                return socket;                    
            }
        }

        void Awake() {
            Init();
        }

        void Init() {
            SocketClient.OnRegister();
        }

        public void OnInit() {
            //CallMethod("Start");
        }

        public void Unload() {
            //CallMethod("Unload");
        }

        ///------------------------------------------------------------------------------------
        public static void AddEvent(int _event, ByteBuffer data) {
            lock (m_lockObject) {
                mEvents.Enqueue(new KeyValuePair<int, ByteBuffer>(_event, data));
            }
        }

        /// <summary>
        /// ����Command�����ﲻ����ķ���˭��
        /// </summary>
        void Update() {
            if (mEvents.Count > 0) {
                while (mEvents.Count > 0) {
                    lock (m_lockObject) {
                        KeyValuePair<int, ByteBuffer> _event = mEvents.Dequeue();         
                        Facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);                       
                        Facade.SendMessageCommand_Wraper(NotiConst.DISPATCH_MESSAGE, _event);
                    }

                }
            }
        }

        public void DispatchMessage(int protocol, ByteBuffer buffer) {
            Action<ByteBuffer> callback;
            if (m_proto_callbacks.TryGetValue(protocol, out callback)) {
                callback(buffer);
                return;
            }
            Debug.LogWarning($"协议 {protocol} 未注册请检查");
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void SendConnect() {
            SocketClient.Connect();
        }

        /// <summary>
        /// 注册协议
        /// </summary>
        /// <param name="protocol">协议号</param>
        /// <param name="callback">回调</param>
        public void RegisterProtocol(int protocol, Action<ByteBuffer> callback) {
            RegisterProtocol(protocol, callback, "");
        }

        /// <summary>
        /// 注册协议
        /// </summary>
        /// <param name="protocol">协议号</param>
        /// <param name="callback">回调</param>
        /// <param name="desc">协议描述</param>
        public void RegisterProtocol(int protocol, Action<ByteBuffer> callback, string desc) {
            m_proto_callbacks[protocol] = callback;
            m_proto_descs[protocol] = desc;
        }

        /// <summary>
        /// 注册协议
        /// </summary>
        /// <param name="protocol">协议号</param>
        /// <param name="callback">回调</param>
        public void RegisterProtocol(int protocol, LuaFunction callback) {
            RegisterProtocol(protocol, callback, "");
        }

        /// <summary>
        /// 注册协议
        /// </summary>
        /// <param name="protocol">协议号</param>
        /// <param name="callback">回调</param>
        /// <param name="desc">协议描述</param>
        public void RegisterProtocol(int protocol, LuaFunction callback, string desc) {
            m_proto_callbacks[protocol] = delegate(ByteBuffer buffer) {
                callback.Call(buffer);
            };
            m_proto_descs[protocol] = desc;
        }

        /// <summary>
        /// 移除协议
        /// </summary>
        public void RemoveProtocol(int protocol) {
            if (m_proto_callbacks.ContainsKey(protocol))
                m_proto_callbacks.Remove(protocol);
            if (m_proto_descs.ContainsKey(protocol))
                m_proto_descs.Remove(protocol); 
        }

        /// <summary>
        /// ����SOCKET��Ϣ
        /// </summary>
        public void SendMessage(ByteBuffer buffer) {
            SocketClient.Send(buffer.ToBytes());
            buffer.Close();
        }

        public void SendMessage(int protocol, ByteBuffer buffer) {
            ByteBuffer bytes = new ByteBuffer();
            bytes.WriteShort((ushort)protocol);
            bytes.WriteBytes(buffer.ToBytes());
            SocketClient.Send(bytes.ToBytes());
            buffer.Close();
            bytes.Close();
        }

        /// <summary>
        /// ��������
        /// </summary>
        void OnDestroy() {
            SocketClient.Dispose();
            Debug.Log("~NetworkManager was destroy");
        }
    }
}