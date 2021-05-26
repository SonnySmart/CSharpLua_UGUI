using UnityEngine;
using LuaFramework;

public class PromptPanel : LuaBehaviour
{
    public GameObject btnOpen;
    public Transform gridParent;

    public void Awake()
    {
        print("我是Cs被打印了 PromptPanel Awake");

        this.InitPanel();
    }

    public void Start()
    {
        print("我是Cs被打印了 PromptPanel Start");
    }

    public void InitPanel()
    {
        var behaviour = gameObject.AddComponent<BehaviourTest>();
        behaviour.Init();

        var tr = this.transform;

        this.btnOpen = tr.Find("Open").gameObject;
	    this.gridParent = tr.Find("ScrollView/Grid");
    }
}