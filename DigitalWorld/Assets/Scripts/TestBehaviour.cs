using DigitalWorld.Network;
using Dream.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CA<T>
{
    private static int v = 0;
    protected static void Func()
    {
        v = v + 1;
    }
}

public class CB : CA<int>
{
    public static void FuncB()
    {
        Func();
    }
}

public class CC : CA<int>
{
    public static void FuncB()
    {
        Func();
    }
}

public class TestBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        var ag = AgentManager.CreateInstance();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
