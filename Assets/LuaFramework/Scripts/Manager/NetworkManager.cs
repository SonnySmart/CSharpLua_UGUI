using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;

namespace LuaFramework {
    public class NetworkManager : Manager {
        private SocketClient socket;
        static readonly object m_lockObject = new object();
        static Queue<KeyValuePair<int, ByteBuffer>> mEvents = new Queue<KeyValuePair<int, ByteBuffer>>();
        static Dictionary<int, Action<ByteBuffer>> mProtocols = new Dictionary<int, Action<ByteBuffer>>();

        SocketClient SocketClient {
            get { 
                if (socket == null)
                    socket = new SocketClient();
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

        /// <summary>
        /// ִ��Lua����
        /// </summary>
        public object CallMethod(string func, params object[] args) {
            return Util.CallMethod("Network", func, args);
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
                    KeyValuePair<int, ByteBuffer> _event = mEvents.Dequeue();         
#if USE_LUA
                    var Instance = Util.CallMethod("AppFacade", "getInstance");
                    if (Instance != null)
                        Util.CallMethod("Facade", "SendMessageCommand", Instance, NotiConst.DISPATCH_MESSAGE, _event);
#else
                    facade.SendMessageCommand(NotiConst.DISPATCH_MESSAGE, _event);
#endif
                }
            }
        }

        public void DispatchMessage(int protocol, ByteBuffer buffer) {
            Action<ByteBuffer> callback;
            if (mProtocols.TryGetValue(protocol, out callback)) {
                callback(buffer);
                return;
            }
            Debug.LogWarning($"协议 {protocol} 未注册请检查");
        }

        /// <summary>
        /// ������������
        /// </summary>
        public void SendConnect() {
            SocketClient.SendConnect();
        }

        public void RegisterProtocol(int protocol, Action<ByteBuffer> callback) {
            mProtocols[protocol] = callback;
        }

        public void RegisterProtocol(int protocol, LuaFunction callback) {
            mProtocols[protocol] = delegate(ByteBuffer buffer) {
                callback.Call(buffer);
            };
        }

        public void RemoveProtocol(int protocol) {
            if (mProtocols.ContainsKey(protocol))
                mProtocols.Remove(protocol);
        }

        /// <summary>
        /// ����SOCKET��Ϣ
        /// </summary>
        public void SendMessage(ByteBuffer buffer) {
            SocketClient.SendMessage(buffer);
        }

        public void SendMessage(int protocol, ByteBuffer buffer) {
            ByteBuffer byteBuffer = new ByteBuffer();
            byteBuffer.WriteShort((ushort)protocol);
            byteBuffer.WriteBytes(buffer.ToBytes());
            SocketClient.SendMessage(byteBuffer);
            buffer.Close();
        }

        /// <summary>
        /// ��������
        /// </summary>
        void OnDestroy() {
            SocketClient.OnRemove();
            Debug.Log("~NetworkManager was destroy");
        }
    }
}