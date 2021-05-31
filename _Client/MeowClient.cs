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
        public Socket.Io.Client.Core.SocketIoClient ss = new();
        /// <summary>
        /// SocketUrl
        /// </summary>
        protected string Url;
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
            Url = url;
            ss.On("OnGroupMsgs").Subscribe((f) => {
                var x = new ObjectEventArgs(JObject.Parse(f.Data[0].ToString()));
                OnServerAction.Invoke(new object(), x);
                OnGroupMsgs.Invoke(new object(), x);
            });
            ss.On("OnFriendMsgs").Subscribe((f) => {
                var x = new ObjectEventArgs(JObject.Parse(f.Data[0].ToString()));
                OnServerAction.Invoke(new object(), x);
                OnFriendMsgs.Invoke(new object(), x);
            });
            ss.On("OnEvents").Subscribe((f) => {
                var x = new ObjectEventArgs(JObject.Parse(f.Data[0].ToString()));
                OnServerAction.Invoke(new object(), x);
                OnEventMsgs.Invoke(new object(), x);
            });
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
