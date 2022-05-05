using DigitalWorld.Table;
using DigitalWorld.UI;
using DigitalWorld.UI.Logic;
using UnityEngine;

namespace DigitalWorld.Login
{
    public class Login : MonoBehaviour
    {
        private void Awake()
        {
        

            TableManager tm = TableManager.instance;
            tm.Decode();

            _ = MainController.Instance;

            UIManager uiManager = UIManager.Instance;
            uiManager.ShowPanel<LoginControl>(LoginControl.path);

        }
    }
}
