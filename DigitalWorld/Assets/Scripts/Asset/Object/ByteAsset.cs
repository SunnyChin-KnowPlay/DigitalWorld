using UnityEngine;

namespace DigitalWorld.Asset
{
    public class ByteAsset : ScriptableObject
    {
        public byte[] bytes;

#if UNITY_EDITOR
        public static void CreateAsset(byte[] bytes, string path)
        {
            ByteAsset byteAsset = CreateInstance<ByteAsset>();
            byteAsset.bytes = bytes;
            UnityEditor.AssetDatabase.CreateAsset(byteAsset, path);
        }
#endif
    }
}
