using UnityEngine;
using LuaFramework;
using UnityEngine.UI;

public class PromptCtrl : Controller
{
    private GameObject gameObject;
    private Transform transform;
    private Component panel;
    private LuaBehaviour prompt;
    private PromptPanel mPromptPanel;


    public void Awake() {
        Debug.Log("PromptCtrl.Awake--->>");
	    LuaHelper.GetPanelManager().CreatePanel("Prompt", this.OnCreate);
    } 

    public void OnCreate(GameObject obj)
    {
        gameObject = obj;
        transform = obj.transform;

        panel = transform.GetComponent("UIPanel");
        prompt = transform.GetComponent<LuaBehaviour>();
        Debug.Log("Start lua--->>" + gameObject.name);

        //mPromptPanel = obj.AddComponent<PromptPanel>();

        mPromptPanel = obj.GetComponent<PromptPanel>();

        prompt.AddClick(mPromptPanel.btnOpen, this.OnClick);
        LuaHelper.GetResManager().LoadPrefab("prompt", "PromptItem", this.InitPanel);
    }

    public void InitPanel(Object[] objs)
    {
        int count = 100; 
        var parent = mPromptPanel.gridParent;
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.Instantiate(objs[0]) as GameObject;
            go.name = "Item" + i.ToString();
            go.transform.SetParent(parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            prompt.AddClick(go, this.OnItemClick);

            var label = go.transform.Find("Text");
            label.GetComponent<Text>().text = i.ToString();
        }
    }

    public void OnClick(GameObject go)
    {
        Debug.Log("OnClick---->>>" + go.name);
    }
    
    public void OnItemClick(GameObject go)
    {
        Debug.Log("OnItemClick---->>>" + go.name);
    }
}