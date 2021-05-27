using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[LuaAutoWrap]
public class TestBaseBehaviourScript3
{
    public TestBaseBehaviourScript3()
    {
        Debug.Log("TestBaseBehaviourScript3 Ctor is Call .");
    }

    public void ClassSayHello()
    {
        Debug.Log("TestBaseBehaviourScript3 ClassSayHello is Call .");
    }

    public static void SayHello()
    {
        Debug.Log("TestBaseBehaviourScript3 SayHello is Call .");
    }
}
