using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace DigitalWorld.Asset
{
    public class AssetUpdator : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(InitializeAddressables());
        }

        private IEnumerator InitializeAddressables()
        {
            var result = Addressables.InitializeAsync();
            yield return result;

            this.OnUpdateFinished();
        }

        private void OnUpdateFinished()
        {
            SceneManager.LoadScene("Login");
        }


    }
}
