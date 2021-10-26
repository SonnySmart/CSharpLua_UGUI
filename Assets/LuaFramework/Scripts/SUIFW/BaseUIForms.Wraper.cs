using System.Collections.Generic;
using LuaFramework;

namespace SUIFW
{
    partial class BaseUIForms
    {
        private object m_AppFacadeInstance;

        private BaseUIForms AppFacadeInstance
        {
            get
            {
                if (m_AppFacadeInstance == null)
                    m_AppFacadeInstance = Util.CallMethod("AppFacade", "getInstance");
                return this;
            }
        }

        private void OnInit_Wraper()
        {
            CallLuaFunction("OnInit");
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        private void OnOpen_Wraper()
        {
            CallLuaFunction("OnOpen");
        }

        /// <summary>
        /// 重新打开窗体
        /// </summary>
        private void OnReOpen_Wraper()
        {
            CallLuaFunction("OnReOpen");
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void OnClose_Wraper() 
        {
            CallLuaFunction("OnClose");
        }

        /// <summary>
        /// 冻结窗体
        /// </summary>
        private void OnFreeze_Wraper() 
        {
            CallLuaFunction("OnFreeze");
        }

        private BaseUIForms RegisterMessage_Wraper(LuaBehaviour behaviour, List<string> messages)
        {
#if USE_LUA
            if (m_AppFacadeInstance != null)
                Util.CallMethod("Facade", "RegisterMessage", m_AppFacadeInstance, behaviour.Table, messages);
#else
            AppFacade.Instance.RegisterMessage(behaviour, messages);
#endif
            return this;
        }

        private BaseUIForms RemoveMessage_Wraper(LuaBehaviour behaviour, List<string> messages)
        {
#if USE_LUA
            if (m_AppFacadeInstance != null)
                Util.CallMethod("Facade", "RemoveMessage", m_AppFacadeInstance, behaviour.Table, messages);
#else
            AppFacade.Instance.RemoveMessage(behaviour, messages);
#endif
            return this;
        }
    }
}