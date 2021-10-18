using System.Collections;
using System.Collections.Generic;
using LuaFramework;
using LuaInterface;

public class AppFacade_LuaWrap
{
    private object mInstance;

    public AppFacade_LuaWrap Instance
    {
        get
        {
            if (mInstance == null)
                mInstance = Util.CallMethod("AppFacade", "getInstance");
            return this;
        }
    }

    public AppFacade_LuaWrap RegisterMessage(LuaBehaviour behaviour, List<string> messages)
    {
#if USE_LUA
        if (mInstance != null)
            Util.CallMethod("Facade", "RegisterMessage", mInstance, behaviour.Table, messages);
#else
        AppFacade.Instance.RegisterMessage(behaviour, messages);
#endif
        return this;
    }

    public AppFacade_LuaWrap RemoveMessage(LuaBehaviour behaviour, List<string> messages)
    {
#if USE_LUA
        if (mInstance != null)
            Util.CallMethod("Facade", "RemoveMessage", mInstance, behaviour.Table, messages);
#else
        AppFacade.Instance.RemoveMessage(behaviour, messages);
#endif
        return this;
    }
}
