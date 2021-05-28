using UnityEngine;
using LuaFramework;
using UnityEngine.UI;
using SUIFW;

public class PromptCtrl : SimpleCommand
{

    public void Awake() {
        Debug.Log("PromptCtrl.Awake--->>");
	    //LuaHelper.GetPanelManager().CreatePanel("Prompt", this.OnCreate);
        UIManager.GetInstance().ShowUIForms("PromptForm");
    } 
}