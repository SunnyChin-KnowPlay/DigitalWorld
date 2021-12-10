using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DigitalWorld.Asset
{
    public class AssetManager : DreamEngine.Singleton<AssetManager>
    {
        #region Initialize
        protected override void Awake()
        {
            base.Awake();

        }

        #endregion

        #region Load Asset
        public T LoadAsset<T>(string path) where T : UnityEngine.Object
        {
            var result = Addressables.LoadAssetAsync<T>(path);
            return result.WaitForCompletion();
        }

        public async Task<T> LoadAssetAsync<T>(string path) where T : UnityEngine.Object
        {
            var result = Addressables.LoadAssetAsync<T>(path);
            await result.Task;

            return result.Result;
        }
        #endregion
    }
}
