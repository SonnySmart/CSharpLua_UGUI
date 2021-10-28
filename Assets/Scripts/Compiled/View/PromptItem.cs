using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;

public partial class PromptItem : LuaBehaviour
{
    public string text
    {
        set
        {
            m_Txt_Text.text = value;
        } 
    }
    // Start is called before the first frame update
    void Start()
    {
        AddClickEventListener(gameObject, this.OnClick);
    }

    void OnClick(GameObject go)
    {
        Debug.Log("OnClick---->>>" + go.name);
    }

    public override void OnMessage(IMessage message)
    {
        
    }
}
