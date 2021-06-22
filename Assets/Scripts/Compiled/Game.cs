using LuaFramework;
using UnityEngine;

public class Game : LuaBehaviour
{
    private void Awake() {
        OnInitOK();
    }

    public void InitViewPanels()
    {

    }

    public void OnInitOK()
    {
        this.InitViewPanels();

        CtrlManager.Instance.Init();

        // 注册命令
        AppFacade.Instance.RegisterCommand(CmdConst.PromptCtrlCommandTest, typeof(PromptCtrl));
        // 注册模型
        AppFacade.Instance.RegisterProxy(new TestModel());

        TestModel model = AppFacade.Instance.RetrieveProxy(typeof(TestModel).Name) as TestModel;
        Debug.Log($" model bb is {model.bb}");

        // 执行命令
        AppFacade.Instance.SendMessageCommand(CmdConst.PromptCtrlCommandTest);
        
        Debug.Log("LuaFramework InitOK--->>>");
    }
}