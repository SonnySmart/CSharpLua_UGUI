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

        this.DoTest();
    }

    public void Start()
    {
        print("我是Cs被打印了 PromptPanel Start");
    }

    public void InitPanel()
    {
        var tr = this.transform;

        this.btnOpen = tr.Find("Open").gameObject;
	    this.gridParent = tr.Find("ScrollView/Grid");
    }

    public void DoTest()
    {
        // 这是正确的
        var behaviour = gameObject.AddComponent<BehaviourTest>();
        behaviour.Init();

        // 这是错误的
        //gameObject.AddComponent<BehaviourTest1>();
        
        // 这是错误的
        //var test2 = new BehaviourTest2();
        //test2.SayHello();

        // 这是正确的
        //var test3 = new TestBaseBehaviourScript3();
        //test3.ClassSayHello();
        //TestBaseBehaviourScript3.SayHello();

        // 有问题...
        //var test3_ = new BehaviourTest3();
        //test3_.SayChild();
    }
}