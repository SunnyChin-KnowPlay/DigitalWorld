using UnityEngine.UI;

namespace DigitalWorld.UI.Logic
{
    public class LoginControl : PanelControl
    {
        public const string path = "UI/Login/LoginPanel";

        #region Param
        private InputField accountInputField;
        private InputField passwordInputField;
        private Button loginButton;
        #endregion

        protected override void Awake()
        {
            base.Awake();


        }

        //protected override void BindWidgets()
        //{
        //    base.BindWidgets();

        //    this.accountInputField = this.GetWidget<InputField>("account");
        //    this.passwordInputField = this.GetWidget<InputField>("password");
        //    this.loginButton = this.GetWidget<Button>("login");

        //    if (null != this.loginButton)
        //    {
        //        this.loginButton.onClick.AddListener(OnClickLogin);
        //    }
        //}

        #region UI Event
        private void OnClickLogin()
        {

        }
        #endregion
    }
}
