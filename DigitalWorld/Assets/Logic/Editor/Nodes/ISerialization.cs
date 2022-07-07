using System.Xml;

namespace DigitalWorld.Logic.Editor
{
    public interface ISerialization
    {
        void Decode(XmlElement node);
        void Encode(XmlElement node);
    }
}
