using UnityEngine;

class SingletonNormalTest : NormalSingleton<SingletonNormalTest>
{
    public void TestPrint()
    {
        Debug.Log("SingletonNormalTest TestPrint is call .");
    }
}