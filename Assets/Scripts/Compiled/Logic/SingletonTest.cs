
using UnityEngine;

class SingletonTest : MonoSingleton<SingletonTest>
{
    public void TestPrint()
    {
        Debug.Log("SingletonTest TestPrint is call .");
    }
}