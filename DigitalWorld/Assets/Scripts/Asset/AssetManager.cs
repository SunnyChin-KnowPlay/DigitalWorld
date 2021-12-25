using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DigitalWorld.Asset
{
    public static class AssetManager
    {
        #region Load Asset
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            var result = Addressables.LoadAssetAsync<T>(path);
            return result.WaitForCompletion();
        }

        public static async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var result = Addressables.LoadAssetAsync<T>(path);
            await result.Task;

            return result.Result;
        }
        #endregion
    }
}
