using UnityEngine;
using LuaFramework;
using UnityEngine.UI;
using SUIFW;

public class PromptCtrl : SimpleCommand
{
    public override void Execute(IMessage message)
    {
        Debug.Log("PromptCtrl.Execute--->>");
	    //LuaHelper.GetPanelManager().CreatePanel("Prompt", this.OnCreate);
        UIManager.GetInstance().ShowUIForms("PromptForm");
    }
}