using MeowIOTBot.ObjectEvent;
using Newtonsoft.Json.Linq;
using System;

namespace MeowIOTBot.Basex
{
    /// <summary>
    /// 代理的类
    /// <para>DelegateLibrary</para>
    /// <code>
    /// <para>用法如下 (Usage)</para>
    /// <para>using var c = new MeowClient("url", "qq");</para>
    /// <para>c.Connect();</para>
    /// <para>c.OnServerAction += (s, e) =>{};</para>
    /// </code>
    /// </summary>
    public partial class MeowClient : IDisposable
    {
        /// <summary>
        /// 日志记录标志
        /// <para>decided if you want to Console.Write(log)</para>
        /// <para>this can be dynamically</para>
        /// </summary>
        public static LogType logFlag { get; set; }
        /// <summary>
        /// 后端的处理位置
        /// <para>Backends Url</para>
        /// </summary>
        public string url { get; }
        /// <summary>
        /// 自动重连计时器 Reconnection Timer
        /// </summary>
        public static System.Timers.Timer refreshTimer = new();
        /// <summary>
        /// 连接的超时Tick .Connection TimeOut Tick (ms)
        /// </summary>
        public long ConnectionTimedOutTick;
        /// <summary>
        /// 重连延迟 ReconnectionDelay
        /// </summary>
        public int ReconnectionDelay;
        /// <summary>
        /// 重连最大延迟 ReconnectionDelay
        /// </summary>
        public int ReconnectionDelayMax;
        /// <summary>
        /// ENGINEIO 版本 Engine.io version
        /// </summary>
        public int EIO;
        /// <summary>
        /// 是否能重连
        /// <para>Reconnection</para>
        /// </summary>
        public bool Reconnection;
        /// <summary>
        /// 是否重试第一次连接
        /// <para>AllowedRetryFirstConnection</para>
        /// </summary>
        public bool AllowedRetryFirstConnection;
        /// <summary>
        /// socket标
        /// <para>socket Client Variable</para>
        /// </summary>
        private SocketIOClient.SocketIO socket = null;
        /// <summary>
        /// 构造代理的类
        /// <code>
        /// <para>用法如下</para>
        /// <para>using var c = new MeowClient("url", "qq");</para>
        /// <para>c.Connect();</para>
        /// <para>c.OnServerAction += (s, e) =>{};</para>
        /// </code>
        /// </summary>
        /// <param name="url">ws的连接Client位置 例如 ws://localhost:10000</param>
        /// <param name="logflag">是否打印日志</param>
        /// <param name="ReconnectInterval">强制重连请求 *分钟</param>
        /// <param name="enableForceReconnection">是否强制使用计时器重连</param>
        /// <param name="connectionTimedOutTick">自动重连计时</param>
        /// <param name="reconnectionDelay">自动重连延迟</param>
        /// <param name="reconnectionDelayMax">自动重连最大计时</param>
        /// <param name="eIO">Engine IO 版本</param>
        /// <param name="reconnection">是否使用官方推荐自动重连</param>
        /// <param name="allowedRetryFirstConnection">是否重试第一次失败连接</param>
        public MeowClient(string url, LogType logflag = LogType.None,
            double ReconnectInterval = 30, bool enableForceReconnection = false,
            long connectionTimedOutTick = 10000, int reconnectionDelay = 1,
            int reconnectionDelayMax = 10, int eIO = 3,
            bool reconnection = true, bool allowedRetryFirstConnection = true)
        {
            logFlag = logflag;
            this.url = url;
            refreshTimer.Interval = ReconnectInterval * 1000 * 60;
            ConnectionTimedOutTick = connectionTimedOutTick;
            ReconnectionDelay = reconnectionDelay;
            ReconnectionDelayMax = reconnectionDelayMax;
            Reconnection = reconnection;
            AllowedRetryFirstConnection = allowedRetryFirstConnection;
            EIO = eIO;
            if (enableForceReconnection)
            {
                refreshTimer.Start();
            }
        }
        /// <summary>
        /// 连接并获取最原始的Client对象
        /// <para>Connect and get Object Client</para>
        /// </summary>
        public MeowClient Connect()
        {
            socket = new(url);
            socket.Options.AllowedRetryFirstConnection = AllowedRetryFirstConnection;
            socket.Options.Reconnection = Reconnection;
            socket.Options.ReconnectionDelay = ReconnectionDelay;
            socket.Options.ReconnectionDelayMax = ReconnectionDelayMax;
            socket.Options.EIO = EIO;
            socket.ConnectAsync();
            socket.OnConnected += (s, e) =>
            {
                ServerUtil.Log($"{socket.ServerUri} is connected",LogType.None);
            };
            socket.OnPing += (s, e) =>
            {
                ServerUtil.Log($"server - ping", LogType.ServerMessage);
            };
            socket.OnPong += (s, e) =>
            {
                ServerUtil.Log($"client - pong check {e}", LogType.ServerMessage);
            };
            socket.OnReconnecting += (s, e) =>
            {
                ServerUtil.Log($"{socket.ServerUri} reconnecting", LogType.ServerMessage);
            };
            socket.OnDisconnected += (s, e) =>
            {
                ServerUtil.Log($"{socket.ServerUri} closed", LogType.ServerMessage);
                socket.ConnectAsync();
            };
            refreshTimer.Elapsed += (s, e) =>
            {
                socket.DisconnectAsync();
            };
            socket.On("OnGroupMsgs", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnGroupMsgs.Invoke(new object(), x);
            });
            socket.On("OnFriendMsgs", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnFriendMsgs.Invoke(new object(), x);
            });
            socket.On("OnEvents", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnEventMsgs.Invoke(new object(), x);
            });
            return this;
        }
        /// <summary>
        /// 强制关闭Socket连接
        /// <para>force close Socket</para>
        /// </summary>
        public void Close()
        {
            try
            {
                if (socket != null)
                {
                    refreshTimer.Stop();
                    refreshTimer.Dispose();
                    socket = null; // close & dispose of socket client
                }
            }
            catch
            {
                socket = null;
                GC.Collect();
            }
        }
        /// <summary>
        /// 关闭连接
        /// <para>normally dispose</para>
        /// </summary>
        public void Dispose() => Close();
    }
    public partial class MeowClient : IDisposable
    {
        /// <summary>
        /// 服务器的总体事件集合委托
        /// <para>On Server Message delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);
        /// <summary>
        /// 服务器的总体事件集合 (如果你重写这个事件,那么服务端的解析将可以由您自己决定)
        /// <para>On Server Message (if you rewrite the Build Core will fail)</para>
        /// </summary>
        public event ObjectEventHandler OnServerAction;
        /// <summary>
        /// 服务器群聊消息事件委托
        /// <para>Serveric [OnGroup] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void GroupMessageEventHandler(object sender, ObjectEventArgs e);
        /// <summary>
        /// 服务器群聊消息事件 (如果你重写这个事件,那么服务端的解析将可以由您自己决定)
        /// <para>Serveric [OnGroup] Message Event (if you rewrite the Build Core will fail)</para>
        /// </summary>
        public event GroupMessageEventHandler OnGroupMsgs;
        /// <summary>
        /// 服务器好友消息事件委托
        /// <para>Serveric [OnFriend] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void FriendMessageEventHandler(object sender, ObjectEventArgs e);
        /// <summary>
        /// 服务器好友消息事件 (如果你重写这个事件,那么服务端的解析将可以由您自己决定)
        /// <para>Serveric [OnFriend] Message Event (if you rewrite the Build Core will fail)</para>
        /// </summary>
        public event FriendMessageEventHandler OnFriendMsgs;
        /// <summary>
        /// 服务器事件消息事件委托
        /// <para>Serveric [OnEvent] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventMessageEventHandler(object sender, ObjectEventArgs e);
        /// <summary>
        /// 服务器事件消息事件 (如果你重写这个事件,那么服务端的解析将可以由您自己决定)
        /// <para>Serveric [OnEvent] Message Event (if you rewrite the Build Core will fail)</para>
        /// </summary>
        public event EventMessageEventHandler OnEventMsgs;
    }
}
