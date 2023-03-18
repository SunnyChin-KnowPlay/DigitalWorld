using Dream.FixMath;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot]
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

[XmlRoot]
public struct VVec2
{

    public int x;
    public int y;

    public VVec2(int x, int y)
    {
        this.x = x;
        this.y = y;

    }
}

[Serializable]
[XmlRoot]
public class TA : ISerializable, IXmlSerializable
{
    [XmlAttribute]
    public int v1;
    [XmlAttribute]
    public int v2;
    [XmlAttribute]
    public VVec3 v3;
    [XmlAttribute]
    public ETestE eee;
    [XmlAttribute]
    public VVec2 v4;
    [XmlAttribute]
    public List<int> l1;

    public TA()
    {

    }

    protected TA(SerializationInfo info, StreamingContext context)
    {
        v1 = (int)info.GetValue("v1", typeof(int));
        v3 = (VVec3)info.GetValue("v3", typeof(VVec3));
        eee = (ETestE)info.GetValue("eee", typeof(ETestE));
        l1 = (List<int>)info.GetValue("l1", typeof(List<int>));
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue("v1", v1);
        info.AddValue("v3", v3);
        info.AddValue("eee", eee);
        info.AddValue("l1", l1);
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {

        while (reader.MoveToNextAttribute())
        {
            switch (reader.Name)
            {
                case "v1":
                {
                    v1 = (int)reader.ReadContentAs(typeof(int), null);
                    break;
                }
                case "v3":
                {
                    v3 = (VVec3)reader.ReadContentAs(typeof(VVec3), null);
                    break;
                }
                case "eee":
                {
                    eee = (ETestE)reader.ReadContentAs(typeof(ETestE), null);
                    break;
                }
                case "L1":
                {
                    l1 = (List<int>)reader.ReadContentAs(typeof(List<int>), null);
                    break;
                }
            }

        }
    }

    public void WriteXml(XmlWriter writer)
    {
        

        writer.WriteAttributeString("v1", v1.ToString());
        //writer.WriteValue(v3);
        writer.WriteAttributeString("eee", eee.ToString());
        XmlSerializer ser = new XmlSerializer(typeof(List<int>));
        ser.Serialize(writer, l1);

    }
}

public class NewBehaviourScript : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        TA a = new TA();
        a.v1 = 3;
        a.v3 = new VVec3(1, 2, 3);
        a.eee = ETestE.Normal;
        a.v4 = new VVec2(2, 3);
        a.l1 = new List<int>() { 8, 8, 8 };

        XmlSerializer serializer = new XmlSerializer(typeof(TA));
        using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Create))
        {
            serializer.Serialize(fs, a);
        }

        using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Open))
        {
            TA b = serializer.Deserialize(fs) as TA;
            int x = 1;
        }


        //using (FileStream fs = new FileStream("C:/Users/sunny/Documents/text.txt", FileMode.Create))
        //{
        //    BinaryFormatter binaryFormatter = new BinaryFormatter();
        //    binaryFormatter.Serialize(fs, a);




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
