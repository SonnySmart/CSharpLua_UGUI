﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Compiled继承他的话就是错误的
/// </summary>
[LuaAutoWrap]
public class TestBaseBehaviourScript2
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("TestBaseBehaviourScript2 Start is Call .");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SayHello()
    {
        Debug.Log("TestBaseBehaviourScript2 SayHello is Call .");
    }
}
