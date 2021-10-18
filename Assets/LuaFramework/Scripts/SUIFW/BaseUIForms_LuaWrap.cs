using SUIFW;

public class BaseUIForms_LuaWrap
{
    public void OnInit(BaseUIForms self)
    {
        self.CallLuaFunction("OnInit");
    }

    /// <summary>
    /// 打开窗体
    /// </summary>
    public void OnOpen(BaseUIForms self)
    {
        self.CallLuaFunction("OnOpen");
    }

    /// <summary>
    /// 重新打开窗体
    /// </summary>
    public void OnReOpen(BaseUIForms self)
    {
        self.CallLuaFunction("OnReOpen");
    }

    /// <summary>
    /// 关闭窗体
    /// </summary>
    public void OnClose(BaseUIForms self) 
    {
        self.CallLuaFunction("OnClose");
    }

    /// <summary>
    /// 冻结窗体
    /// </summary>
    public void OnFreeze(BaseUIForms self) 
    {
        self.CallLuaFunction("OnFreeze");
    }
}
