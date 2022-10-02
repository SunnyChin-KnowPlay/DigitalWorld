using DigitalWorld.Events;
using DigitalWorld.Game;
using DigitalWorld.Net;
using DigitalWorld.Proto.Common;
using DigitalWorld.UI;
using Dream.Proto;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DigitalWorld.Login.UI
{
    public class LoginPanel : PanelControl
    {
        #region Enter
        public const string path = "Assets/Res/UI/Login/LoginPanel.prefab";
        #endregion

        #region Param

        private Button startButton;
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            startButton = this.GetControlComponent<Button>("Root/StartButton");

            if (null != startButton)
            {
                startButton.onClick.AddListener(OnClickStartGame);
            }


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

        #region UI Event
        private void OnClickStartGame()
        {
            UnityEngine.Debug.Log("Start Game");

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
            yield return SceneManager.LoadSceneAsync("World");
            yield return new WaitForEndOfFrame();

            UIManager.Instance.UnloadAllPanels();

            _ = WorldManager.Instance;
        }

        private void OnClickLogin()
        {
            EndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 55000);
            AgentManager.Instance.Connect(AgentManager.TokenLogin, ep);
        }
        #endregion

        #region Event
        /// <summary>
        /// Login收到退出时 直接忽略
        /// </summary>
        /// <param name="type"></param>
        /// <param name="args"></param>
        protected override void OnEscape(EEventType type, EventArgs args)
        {
            
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
