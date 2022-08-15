using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETest
{
    Empty = 0,
}

public class NewBehaviourScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        System.Type type = typeof(ETest).BaseType;
        int x = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
