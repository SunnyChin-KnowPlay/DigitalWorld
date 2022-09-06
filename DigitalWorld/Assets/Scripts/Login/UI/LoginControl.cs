using DigitalWorld.Game;
using DigitalWorld.Net;
using DigitalWorld.Proto.Common;
using Dream.Proto;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DigitalWorld.UI.Logic
{
    public class LoginControl : PanelControl
    {
        public const string path = "Assets/Res/UI/Login/LoginPanel.prefab";

        #region Param

        private Button startButton;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

        }

        protected override void OnDisable()
        {
            base.OnDisable();

        }
        #endregion

        #region Bind
        protected override void BindWidgets()
        {
            base.BindWidgets();


            startButton = this.GetComponent<Button>("Root/StartButton");

            if (null != startButton)
            {
                startButton.onClick.AddListener(OnClickStartGame);
            }
        }
        #endregion


        #region UI Event
        private void OnClickStartGame()
        {
            DreamEngine.Logger.Info("Start Game");
            //UnityEngine.Debug.Log("Start Game");

            MainController mc = MainController.Instance;
            mc.worldInfo = new WorldInfo()
            {
                heroId = 1,
                mapId = 1
            };


            StartCoroutine(Func());

        }

        private IEnumerator Func()
        {
            SceneManager.LoadScene("World");
            yield return new WaitForEndOfFrame();

            this.Hide();

            WorldManager wm = WorldManager.Instance;
        }

        private void OnClickLogin()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55000);
            AgentManager.Instance.Connect(AgentManager.TokenLogin, ep);
        }
        #endregion

        #region Protocol
        //private void ProcessConnect(Protocol protocol)
        //{
        //    NotiConnectResult result = protocol as NotiConnectResult;
        //    if (null != result)
        //    {
        //        if (result.result == EnumConnectResult.Success)
        //        {
        //            Agent ag = AgentManager.Instance.LoginAgent;
        //            if (null != ag)
        //            {
        //                ag.AddProtocolListener(AckLogin.protocolId, ProcessAckLogin);
        //            }

        //            ReqLogin req = ReqLogin.Alloc();
        //            req.account = this.accountInputField.text;
        //            req.password = this.passwordInputField.text;

        //            AgentManager.Instance.LoginAgent.Send(req);
        //        }
        //        else
        //        {

        //        }
        //    }
        //}

        private void ProcessAckLogin(Protocol protocol)
        {
            AckLogin ack = protocol as AckLogin;
            if (null != ack)
            {
              
            }
        }
        #endregion
    }
}
