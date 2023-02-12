using DigitalWorld.Table;
using DigitalWorld.UI;
using DigitalWorld.Login.UI;
using UnityEngine;
using DigitalWorld.Inputs;

namespace DigitalWorld.Login
{
    public class Login : MonoBehaviour
    {
        private void Awake()
        {


            TableManager tm = TableManager.Instance;
            tm.Decode();

            _ = MainController.Instance;
            _ = InputManager.Instance;

            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<LoginPanel>(LoginPanel.path);
        }
    }
}
