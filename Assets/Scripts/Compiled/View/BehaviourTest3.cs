using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTest3 : MonoBehaviour
{
    public BehaviourTest3() : base()
    {
        Debug.Log("BehaviourTest3 Ctor is Call .");
    }

    public void SayChild()
    {
        Debug.Log("BehaviourTest3 SayChild is Call .");

        //this.ClassSayHello();
    }
}
