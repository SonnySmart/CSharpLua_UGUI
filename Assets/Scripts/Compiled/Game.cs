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
        SimpleCommand ctrl = CtrlManager.Instance.GetCtrl("Prompt");
        if (ctrl != null && AppConst.ExampleMode) {
            (ctrl as PromptCtrl).Awake();
        }

        AppFacade.Instance.RegisterProxy(new TestModel());
        
        Debug.Log("LuaFramework InitOK--->>>");
    }
}