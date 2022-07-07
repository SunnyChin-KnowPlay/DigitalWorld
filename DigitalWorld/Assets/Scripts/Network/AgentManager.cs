using DigitalWorld.Proto.Common;
using Dream.Core;
using Dream.Proto;
using DreamEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DigitalWorld.Net
{
    public class AgentManager : DreamEngine.Singleton<AgentManager>
    {
        #region Event
        public delegate void OnProcessProtocolHandle(Protocol p);
        #endregion

        #region Tokens
        public const string TokenLogin = "Login";
        public const string TokenCenter = "Center";
        public const string TokenGame = "Game";
        #endregion

        #region Params
        private Socket connectSocket = null;

        /// <summary>
        /// 代理的词典 连接成功的或者准备进行UDP传输的代理都放在这里
        /// </summary>
        private readonly Dictionary<string, Agent> agents = new Dictionary<string, Agent>();

        /// <summary>
        /// 系统级协议队列 处理包括连接 断链等协议
        /// </summary>
        private readonly List<Protocol> protocols = new List<Protocol>();

        /// <summary>
        /// 针对不同协议的回调词典
        /// </summary>
        protected Dictionary<ushort, OnProcessProtocolHandle> protocolHandleDict = new Dictionary<ushort, OnProcessProtocolHandle>();
        #endregion

        #region Mono
        protected override void Awake()
        {
            base.Awake();

            this.connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (this.protocolHandleDict != null)
            {
                protocolHandleDict.Clear();
            }

            if (null != connectSocket)
            {
                if (connectSocket.Connected)
                {
                    connectSocket.Shutdown(SocketShutdown.Both);
                }
                connectSocket.Close();
                connectSocket = null;
            }

            lock (((ICollection)this.agents).SyncRoot)
            {
                foreach (var kvp in agents)
                {
                    kvp.Value.Recycle();
                }
                this.agents.Clear();
            }
        }

        private void Update()
        {
            if (Monitor.TryEnter(((ICollection)this.protocols).SyncRoot))
            {
                try
                {
                    foreach (Protocol proto in protocols)
                    {
                        this.ProcessProtocol(proto);
                        proto.Recycle();
                    }
                    protocols.Clear();
                }
                finally
                {
                    Monitor.Exit(((ICollection)this.protocols).SyncRoot);
                }
            }

            if (Monitor.TryEnter(((ICollection)this.agents).SyncRoot))
            {
                try
                {
                    foreach (var kvp in agents)
                    {
                        kvp.Value.Update();
                    }        
                }
                finally
                {
                    Monitor.Exit(((ICollection)this.agents).SyncRoot);
                }
            }
        }
        #endregion

        #region Listen
        public virtual void AddProtocolListener(ushort protocolID, OnProcessProtocolHandle handle)
        {
            ushort id = protocolID;

            if (this.protocolHandleDict.ContainsKey(id))
            {
                // 如果存在的话 直接替换
                this.protocolHandleDict[id] += handle;
            }
            else
            {
                this.protocolHandleDict.Add(id, handle);
            }
        }

        public virtual void RemoveProtocolListener(ushort protocolID, OnProcessProtocolHandle handle)
        {
            ushort id = protocolID;

            if (this.protocolHandleDict.ContainsKey(id))
            {
                //移除 不存在的话 就无视
                this.protocolHandleDict[id] -= handle;
            }
        }
        #endregion

        #region Process
        private void ProcessProtocol(Protocol proto)
        {
            ushort id = proto.Id;

            this.protocolHandleDict.TryGetValue(id, out OnProcessProtocolHandle handle);
            if (null != handle)
            {
                handle.Invoke(proto);
            }
        }
        #endregion

        #region Connect
        public void Connect(string token, EndPoint ep)
        {
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.UserToken = token;
            args.RemoteEndPoint = ep;
            args.Completed += this.OnConnect;
            connectSocket.ConnectAsync(args);
        }

        protected void OnConnect(object sender, SocketAsyncEventArgs e)
        {
            NotiConnectResult noti = NotiConnectResult.Alloc();

            //
            if (e.SocketError == SocketError.Success)
            {
                string token = e.UserToken as string;

                Agent agent = ObjectPool<Agent>.Instance.Allocate();
                agent.Start(e.ConnectSocket);

                lock (((ICollection)this.agents).SyncRoot)
                {
                    this.agents.Add(token, agent);
                }
                noti.token = token;
                noti.result = EnumConnectResult.Success;
            }
            else
            {
                noti.result = EnumConnectResult.Failed;
            }

            lock (((ICollection)this.protocols).SyncRoot)
            {
                this.protocols.Add(noti);
            }
        }
        #endregion

        #region Get
        public Agent GetAgent(string token)
        {
            this.agents.TryGetValue(token, out Agent ag);
            return ag;
        }

        public Agent LoginAgent
        {
            get
            {
                return this.GetAgent(TokenLogin);
            }
        }
        #endregion
    }
}
