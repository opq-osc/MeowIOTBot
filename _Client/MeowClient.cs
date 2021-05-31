using MeowIOTBot.ObjectEvent;
using Newtonsoft.Json.Linq;
using SocketIOClient;
using System;
using System.Threading.Tasks;

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
    public class MeowClient : IDisposable
    {
        /// <summary>
        /// 日志记录标志
        /// <para>decided if you want to Console.Write(log)</para>
        /// <para>this can be dynamically</para>
        /// </summary>
        public static LogType logFlag { get; set; }
        /// <summary>
        /// socket标
        /// <para>socket Client Variable</para>
        /// </summary>
        public SocketIO ss = null;
        private static bool ping = false;
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
        /// <param name="eIO">Engine IO 版本</param>
        /// <param name="reconnection">是否使用官方推荐自动重连</param>
        /// <param name="allowedRetryFirstConnection">是否重试第一次失败连接</param>
        public MeowClient(string url, LogType logflag)
        {
            logFlag = logflag;
            ss = new(url);
            ss.Options.Reconnection = true;
            ss.Options.ReconnectionDelay = 1000;
            ss.Options.ReconnectionDelayMax = 1500;
            ss.Options.ConnectionTimeout = new(1, 0, 0, 0);
            ss.On("OnGroupMsgs", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnGroupMsgs.Invoke(new object(), x);
            });
            ss.On("OnFriendMsgs", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnFriendMsgs.Invoke(new object(), x);
            });
            ss.On("OnEvents", (fn) => {
                var x = new ObjectEventArgs(JObject.Parse(fn.GetValue(0).ToString()));
                OnServerAction.Invoke(new object(), x);
                OnEventMsgs.Invoke(new object(), x);
            });
            ss.OnError += (s, e) =>
            {
                ServerUtil.Log($"Server err {e}", LogType.None, ConsoleColor.Red, ConsoleColor.White);
            };
            ss.OnPing += (s, e) =>
            {
                ServerUtil.Log($"Client Ping", LogType.ServerMessage);
            };
            ss.OnPong += (s, e) =>
            {
                ServerUtil.Log($"Server Pong", LogType.ServerMessage);
            };
            ss.OnConnected += (s, e) =>
            {
                ServerUtil.Log($"{ss.ServerUri} is connected", LogType.None);
            };
            ss.OnDisconnected += (s, e) =>
            {
                ServerUtil.Log($"Disconnect : {e}", LogType.ServerMessage);
            };
            ss.OnReconnecting += (s, e) =>
            {
                ServerUtil.Log($"Reconnect : {e}", LogType.ServerMessage);
            };
        }
        /// <summary>
        /// 关闭连接
        /// <para>normally dispose</para>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            try
            {
                if (ss != null)
                {
                    ss = null;
                }
            }
            catch
            {
                ss = null;
                GC.Collect();
            }
        }
        private void ReConnect()
        {
            ss.DisconnectAsync();
            ss.ConnectAsync();
        }
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
