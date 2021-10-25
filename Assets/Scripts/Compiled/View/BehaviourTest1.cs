#if true
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LuaFramework;

public class BehaviourTest1 : LuaBehaviour
{
    [SerializeField]
    GameObject testGO;
    [SerializeField]
    Transform testTr;

    [SerializeField]
    bool testBool = true;

    [SerializeField]
    int testInt32 = 10;

    [SerializeField]
    float testFloat = 20.20f;

    [SerializeField]
    int[] testInt32Array;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("BehaviourTest1 Start is Call .");

        //SayHello();

        print (testGO.name);
        print (testTr.name);
        print (testBool);
        print (testInt32);
        print (testFloat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
#endif