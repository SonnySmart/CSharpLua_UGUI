using LuaFramework;
using UnityEngine;

public class Game
{
    public void InitViewPanels()
    {

    }

    public void OnInitOK()
    {
        this.InitViewPanels();

        CtrlManager.Instance.Init();
        IController ctrl = CtrlManager.Instance.GetCtrl("Prompt");
        if (ctrl != null && AppConst.ExampleMode) {
            (ctrl as PromptCtrl).Awake();
        }
        
        Debug.Log("LuaFramework InitOK--->>>");
    }
}