using System.Collections.Generic;
using UnityEngine;

public class CtrlManager
{
    private static CtrlManager _instance;
    private Dictionary<string, Controller> ctrlList = new Dictionary<string, Controller>();

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

        ctrlList["Prompt"] = new PromptCtrl();
    }

    public void AddCtrl(string ctrlName, Controller ctrlObj)
    {
        ctrlList[ctrlName] = ctrlObj;
    }

    public Controller GetCtrl(string ctrlName)
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