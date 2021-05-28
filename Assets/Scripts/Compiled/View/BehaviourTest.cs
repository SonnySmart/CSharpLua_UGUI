using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;

public class BehaviourTest : LuaBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BehaviourTest Start is Call .");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init()
    {
        Debug.Log("BehaviourTest Init is Call .");
    }
}
