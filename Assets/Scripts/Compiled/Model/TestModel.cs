using UnityEngine;

public class TestModel : Proxy
{
    public int aa;
    public string bb;

    public override void OnRegister()
    {
        Debug.Log($"TestModel OnRegister is call . NAME => {NAME}");

        aa = 10;
        bb = "hahaha";
    }
}