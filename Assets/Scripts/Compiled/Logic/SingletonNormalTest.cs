using UnityEngine;

class SingletonNormalTest : Singleton<SingletonNormalTest>
{
    public void TestPrint()
    {
        Debug.Log("SingletonNormalTest TestPrint is call .");
    }
}