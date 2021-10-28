using UnityEngine;

namespace DigitalWorld.Asset
{
    public class AssetManager : DreamEngine.Singleton<AssetManager>
    {
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }
    }
}
