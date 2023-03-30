using Dream.FixMath;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

using System.Xml.Serialization;
using UnityEngine;

[Serializable]
public struct VVec3
{
    public int x;
    public int y;
    public int z;

    public VVec3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override string ToString()
    {
        return "( " + (float)x * 0.001f + ", " + (float)y * 0.001f + ", " + (float)z * 0.001f + ")";
    }
}

public enum ETestE
{
    Zero = 0,
    Normal = 1,
}

[Serializable]
public struct TA
{
    public List<VVec3> list;
}



public class NewBehaviourScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        TA a = new TA();
        a.list = new List<VVec3>();
        for (int i = 0; i < 99; i++)
        {
            a.list.Add(new VVec3(i, i, i));
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,

        };
        string vv = JsonConvert.SerializeObject(a, settings);

        using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(vv);
            }
        }



        using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Open))
        {
            using (StreamReader sr = new StreamReader(fs))
            {
                TA b = JsonConvert.DeserializeObject<TA>(sr.ReadToEnd(), settings);

                int xx = 1;
            }
        }



        //}

        //using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Open))
        //{
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    TA b = binaryFormatter.Deserialize(fs) as TA;

        //    int x = 1;
        //}

    }



    // Update is called once per frame
    void Update()
    {

    }
}
