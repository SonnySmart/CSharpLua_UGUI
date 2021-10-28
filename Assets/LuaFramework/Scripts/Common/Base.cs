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
        /// C# Facade
        /// </summary>
        public AppFacade Facade {
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
                    m_LuaMgr = Facade.GetManager<LuaManager>(ManagerName.Lua);
                }
                return m_LuaMgr;
            }
        }

        public ResourceManager ResManager {
            get {
                if (m_ResMgr == null) {
                    m_ResMgr = Facade.GetManager<ResourceManager>(ManagerName.Resource);
                }
                return m_ResMgr;
            }
        }

        public NetworkManager NetManager {
            get {
                if (m_NetMgr == null) {
                    m_NetMgr = Facade.GetManager<NetworkManager>(ManagerName.Network);
                }
                return m_NetMgr;
            }
        }

        public SoundManager SoundManager {
            get {
                if (m_SoundMgr == null) {
                    m_SoundMgr = Facade.GetManager<SoundManager>(ManagerName.Sound);
                }
                return m_SoundMgr;
            }
        }

        public TimerManager TimerManager {
            get {
                if (m_TimerMgr == null) {
                    m_TimerMgr = Facade.GetManager<TimerManager>(ManagerName.Timer);
                }
                return m_TimerMgr;
            }
        }

        public ThreadManager ThreadManager {
            get {
                if (m_ThreadMgr == null) {
                    m_ThreadMgr = Facade.GetManager<ThreadManager>(ManagerName.Thread);
                }
                return m_ThreadMgr;
            }
        }

        public ObjectPoolManager ObjPoolManager {
            get {
                if (m_ObjectPoolMgr == null) {
                    m_ObjectPoolMgr = Facade.GetManager<ObjectPoolManager>(ManagerName.ObjectPool);
                }
                return m_ObjectPoolMgr;
            }
        }

        public FsmManager FsmManager {
            get {
                if (m_FsmMgr == null) {
                    m_FsmMgr = Facade.GetManager<FsmManager>(ManagerName.Fsm);
                }
                return m_FsmMgr;
            }
        }
    }
}