using DigitalWorld.Table;
using DigitalWorld.UI;
using DigitalWorld.Login.UI;
using UnityEngine;

namespace DigitalWorld.Login
{
    public class Login : MonoBehaviour
    {
        private void Awake()
        {
        

            TableManager tm = TableManager.Instance;
            tm.Decode();

            _ = MainController.Instance;

            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<LoginPanel>(LoginPanel.path);

        }
    }
}
