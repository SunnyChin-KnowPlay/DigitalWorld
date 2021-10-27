using DigitalWorld.Proto.Common;
using DreamEngine;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace DigitalWorld.Net
{
    public class AgentManager : Singleton<AgentManager>
    {
        private Socket _connectSocket = null;
        private Dictionary<string, Agent> _agents = new Dictionary<string, Agent>();

        protected override void Awake()
        {
            base.Awake();

            this._connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            var ins = AgentManager.Instance;

            if (null != _connectSocket)
            {
                if (_connectSocket.Connected)
                {
                    _connectSocket.Shutdown(SocketShutdown.Both);
                }
                _connectSocket.Close();
                _connectSocket = null;
            }

            foreach (var kvp in _agents)
            {
                kvp.Value.Recycle();
            }
            this._agents.Clear();
        }

        private void Update()
        {

        }

        #region Connect
        public void Connect(string token, EndPoint ep)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = token;
            args.RemoteEndPoint = ep;
            args.Completed += this.OnConnect;
            _connectSocket.ConnectAsync(args);
        }

        protected void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            NotiConnectResult noti = NotiConnectResult.Alloc();


            //
            if (e.SocketError == SocketError.Success)
            {
                noti.result = EnumConnectResult.Success;
            }
            else
            {
                noti.result = EnumConnectResult.Failed;
                //OnInterruption(sender, e);
            }


        }
        #endregion


    }
}
