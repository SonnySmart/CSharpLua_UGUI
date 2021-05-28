using System.Collections.Generic;
using UnityEngine;

public class CtrlManager
{
    private static CtrlManager _instance;
    private Dictionary<string, SimpleCommand> ctrlList = new Dictionary<string, SimpleCommand>();

    public static CtrlManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new CtrlManager();
            return _instance;
        }
    }
    
    public void Init()
    {
        Debug.Log("CtrlManager.Init----->>>");

        var testCode = new TestCode();
        var testCtrl = new TestCtrl();

        ctrlList["Prompt"] = new PromptCtrl();
    }

    public void AddCtrl(string ctrlName, SimpleCommand ctrlObj)
    {
        ctrlList[ctrlName] = ctrlObj;
    }

    public SimpleCommand GetCtrl(string ctrlName)
    {
        return ctrlList[ctrlName];
    }

    public void RemoveCtrl(string ctrlName)
    {
        ctrlList.Remove(ctrlName);
    }

    public void Close()
    {
        Debug.Log("CtrlManager.Close---->>>");
    }
}