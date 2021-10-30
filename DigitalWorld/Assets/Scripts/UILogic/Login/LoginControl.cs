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

        #region Bind
        protected override void BindWidgets()
        {
            base.BindWidgets();

            accountInputField = this.GetComponent<InputField>("Root/AccountInputField");
            passwordInputField = this.GetComponent<InputField>("Root/PasswordInputField");
            loginButton = this.GetComponent<Button>("Root/LoginButton");

            if (null != loginButton)
            {
                loginButton.onClick.AddListener(OnClickLogin);
            }
        }
        #endregion
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
