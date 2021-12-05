using Dream.Core;
using Dream.Proto;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;

public class NavMeshTriangulationData : ByteBuffer
{
    public List<Vector3> Vertices
    {
        get { return vertices; }
        set { vertices = value; }
    }
    protected List<Vector3> vertices;

    public List<int> Indices
    {
        get { return indices; }
        set { indices = value; }
    }
    protected List<int> indices;

    public List<int> Areas
    {
        get { return areas; }
        set { areas = value; }
    }
    protected List<int> areas;

    public override void OnRecycle()
    {
        base.OnRecycle();

        vertices = default(List<Vector3>);
        indices = default(List<int>);
        areas = default(List<int>);
    }

    protected override void OnEncode(XmlElement element)
    {
        base.OnEncode(element);

        this.Encode(this.vertices, "vertices");
        this.Encode(this.indices, "indices");
        this.Encode(this.areas, "areas");
    }

    protected override void OnDecode(XmlElement element)
    {
        base.OnDecode(element);

        this.Decode(ref this.vertices, "vertices");
        this.Decode(ref this.indices, "indices");
        this.Decode(ref this.areas, "areas");
    }

    protected override void OnEncode(byte[] buffer, int pos)
    {
        base.OnEncode(buffer, pos);

        this.Encode(this.vertices);
        this.Encode(this.indices);
        this.Encode(this.areas);
    }

    protected override void OnDecode(byte[] buffer, int pos)
    {
        base.OnDecode(buffer, pos);

        this.Decode(ref this.vertices);
        this.Decode(ref this.indices);
        this.Decode(ref this.areas);
    }

    #region Encode & Decode
    protected void Encode(Vector3 v)
    {
        this.Encode(v.x);
        this.Encode(v.y);
        this.Encode(v.z);
    }

    protected void Decode(ref Vector3 v)
    {
        this.Decode(ref v.x);
        this.Decode(ref v.y);
        this.Decode(ref v.z);
    }

    protected void Encode(List<Vector3> v)
    {
        int length = v.Count;
        Encode(length);

        AssertOffsetAndLength(_pos, length);
        for (int i = 0; i < v.Count; ++i)
        {
            Encode(v[i]);
        }
    }

    protected void Decode(ref List<Vector3> v)
    {
        int length = 0;
        this.Decode(ref length);
        AssertOffsetAndLength(_pos, length);

        if (null == v)
            v = new List<Vector3>(length);
        else
        {
            v.Clear();
            v.Capacity = length;
        }

        for (int i = 0; i < length; ++i)
        {
            Vector3 v1 = Vector3.zero;
            Decode(ref v1);
            v[i] = v1;
        }
    }

    protected void Encode(List<Vector3> v, string paramName)
    {
        string t = string.Join(separatorStr, v);
        this.Element.SetAttribute(paramName, t);
    }

    protected void Decode(ref List<Vector3> v, string paramName)
    {
        string r = this.Element.GetAttribute(paramName);
        string[] list = r.Split(separatorStr);
        int length = list.Length;

        Regex regex = new Regex(@"\((?<v>.*?)\)");

        if (null == v)
            v = new List<Vector3>(length);
        else
        {
            v.Clear();
            v.Capacity = length;
        }

        for (int i = 0; i < length; ++i)
        {
            string v1 = list[i];

            Group matchGroup = regex.Match(v1).Groups["v"];

            string[] vectorStrSplit = matchGroup.Value.Split(",");
            if (null != vectorStrSplit && vectorStrSplit.Length == 3)
            {
                Vector3 vec = new Vector3(float.Parse(vectorStrSplit[0]), float.Parse(vectorStrSplit[1]), float.Parse(vectorStrSplit[2]));
                v.Add(vec);
            }

        }

    }
    #endregion
}
