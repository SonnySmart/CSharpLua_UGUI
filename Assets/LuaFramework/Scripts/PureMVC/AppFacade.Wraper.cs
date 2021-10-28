using System.Collections.Generic;
using LuaFramework;

public partial class AppFacade
{
    private static object m_LuaInstance;

    public static object LuaInstance
    {
        get
        {
            if (m_LuaInstance == null)
            {
                m_LuaInstance = LuaHelper.InvokeModule("AppFacade", "getInstance");
            }
            return m_LuaInstance;
        }
    }

    public void SendMessageCommand_Wraper(string message, object body = null)
    {
        LuaHelper.InvokeModule("Facade", "SendMessageCommand", LuaInstance, message, body);
    }

    public void RegisterMessage_Wraper(LuaBehaviour view, List<string> messages)
    {
        LuaHelper.InvokeModule("Facade", "RegisterMessage", LuaInstance, view.Table, messages);
    }

    public void RemoveMessage_Wraper(LuaBehaviour view, List<string> messages)
    {
        LuaHelper.InvokeModule("Facade", "RemoveMessage", LuaInstance, view.Table, messages);
    }
}