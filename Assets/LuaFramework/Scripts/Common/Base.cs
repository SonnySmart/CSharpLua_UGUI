using UnityEngine;
using System.Collections;
using LuaFramework;
using System.Collections.Generic;

namespace LuaFramework {
    public class Base : MonoBehaviour {
        private AppFacade m_Facade;
        private LuaManager m_LuaMgr;
        private ResourceManager m_ResMgr;
        private NetworkManager m_NetMgr;
        private SoundManager m_SoundMgr;
        private TimerManager m_TimerMgr;
        private ThreadManager m_ThreadMgr;
        private ObjectPoolManager m_ObjectPoolMgr;
        private FsmManager m_FsmMgr;

        /// <summary>
        /// 注册消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RegisterMessage(IView view, List<string> messages) {
            if (messages == null || messages.Count == 0) return;
            Controller.Instance.RegisterViewCommand(view, messages.ToArray());
        }

        /// <summary>
        /// 移除消息
        /// </summary>
        /// <param name="view"></param>
        /// <param name="messages"></param>
        protected void RemoveMessage(IView view, List<string> messages) {
            if (messages == null || messages.Count == 0) return;
            Controller.Instance.RemoveViewCommand(view, messages.ToArray());
        }

        protected AppFacade facade {
            get {
                if (m_Facade == null) {
                    m_Facade = AppFacade.Instance;
                }
                return m_Facade;
            }
        }

        public LuaManager LuaManager {
            get {
                if (m_LuaMgr == null) {
                    m_LuaMgr = facade.GetManager<LuaManager>(ManagerName.Lua);
                }
                return m_LuaMgr;
            }
        }

        public ResourceManager ResManager {
            get {
                if (m_ResMgr == null) {
                    m_ResMgr = facade.GetManager<ResourceManager>(ManagerName.Resource);
                }
                return m_ResMgr;
            }
        }

        public NetworkManager NetManager {
            get {
                if (m_NetMgr == null) {
                    m_NetMgr = facade.GetManager<NetworkManager>(ManagerName.Network);
                }
                return m_NetMgr;
            }
        }

        public SoundManager SoundManager {
            get {
                if (m_SoundMgr == null) {
                    m_SoundMgr = facade.GetManager<SoundManager>(ManagerName.Sound);
                }
                return m_SoundMgr;
            }
        }

        public TimerManager TimerManager {
            get {
                if (m_TimerMgr == null) {
                    m_TimerMgr = facade.GetManager<TimerManager>(ManagerName.Timer);
                }
                return m_TimerMgr;
            }
        }

        public ThreadManager ThreadManager {
            get {
                if (m_ThreadMgr == null) {
                    m_ThreadMgr = facade.GetManager<ThreadManager>(ManagerName.Thread);
                }
                return m_ThreadMgr;
            }
        }

        public ObjectPoolManager ObjPoolManager {
            get {
                if (m_ObjectPoolMgr == null) {
                    m_ObjectPoolMgr = facade.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
                }
                return m_ObjectPoolMgr;
            }
        }

        public FsmManager FsmManager {
            get {
                if (m_FsmMgr == null) {
                    m_FsmMgr = facade.GetManager<FsmManager>(ManagerName.Fsm);
                }
                return m_FsmMgr;
            }
        }
    }
}