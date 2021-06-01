using MeowIOTBot.ObjectEvent;
using Newtonsoft.Json.Linq;
using Socket.Io.Client.Core.Model.SocketIo;
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
        /// Cascade timer monitor as the Socket disconnect
        /// </summary>
        public System.Timers.Timer MonitorTimer = new()
        {
            Interval = 10000,//10s
            AutoReset = true,
        };
        /// <summary>
        /// 构造代理的类
        /// </summary>
        /// <param name="url">ws的连接Client位置 例如 ws://localhost:10000</param>
        /// <param name="logflag">是否打印日志</param>
        /// <param name="MonitorInterval">底层监视器回报的间隔(底层监视器将在第一次连接成功后一直回报到进程结束)</param>
        public MeowClient(string url, LogType logflag, long MonitorInterval = 10000, bool _MonitorEnable = true)
        {
            logFlag = logflag;
            Url = url;
            MonitorTimer.Interval = MonitorInterval;
            MonitorTimer.Elapsed += (s, e) =>
            {
                switch( ss.State switch
                {
                    ReadyState.Open => ServerUtil.Log($"[Monitor : Serveric State Open]",LogType.Verbose, k: 0),
                    ReadyState.Opening => ServerUtil.Log($"[Monitor : Serveric State Opening]",LogType.Verbose, k: 1),
                    ReadyState.Paused => ServerUtil.Log($"[Monitor : Serveric State Paused]",LogType.Verbose, k: 2),
                    ReadyState.Closing => ServerUtil.Log($"[Monitor : Serveric State Closing]",LogType.Verbose, k: 3),
                    ReadyState.Closed => ServerUtil.Log($"[Monitor : Serveric State Closed]",LogType.Verbose, k: 4),
                    _ => ServerUtil.Log($"[Monitor : Serveric State Err]",LogType.Verbose, k: 5),
                })
                {
                    case 0: _ServericOpen.Invoke(new(), new());break;
                    case 2: _ServericPaused.Invoke(new(), new());break;
                    case 4: _ServericClosed.Invoke(new(), new());break;
                    default:break;
                }
            };
            ss.Events.OnConnect.Subscribe(d =>
            {
                ServerUtil.Log($"[Connected] {d}", LogType.None);
            });
            ss.Events.OnOpen.Subscribe(d =>
            {
                ServerUtil.Log($"[Open] {d}", LogType.ServerMessage);
            });
            ss.Events.OnHandshake.Subscribe(d =>
            {
                ServerUtil.Log($"[HandShake] SID:{d.Sid} PI:{d.PingInterval} PT:{d.PingTimeout}", LogType.Verbose);
            });
            ss.Events.OnPong.Subscribe(d =>
            {
                ServerUtil.Log($"[Pong] {d.Data}", LogType.Verbose);
            });
            ss.Events.OnError.Subscribe(d =>
            {
                ServerUtil.Log($"[Err] {d.Description}", LogType.ServerMessage);
            });
            ss.Events.OnProbeError.Subscribe(d => 
            {
                ServerUtil.Log($"[ProbeErr] {d.PingData} {d.PongData}", LogType.ServerMessage);
            });
            ss.Events.OnPacket.Subscribe(d =>
            {
                if (!string.IsNullOrEmpty(d.Data))
                {
                    ServerUtil.Log($"[Packet] {d.Data}", LogType.Verbose);
                    try
                    {
                        var ja = JArray.Parse(d.Data);
                        if ("OnGroupMsgs".Equals(ja[0].ToString()))
                        {
                            var x = new ObjectEventArgs(JObject.Parse(ja[1].ToString()));
                            OnServerAction.Invoke(new object(), x);
                            OnGroupMsgs.Invoke(new object(), x);
                        }
                        else if ("OnFriendMsgs".Equals(ja[0].ToString()))
                        {
                            var x = new ObjectEventArgs(JObject.Parse(ja[1].ToString()));
                            OnServerAction.Invoke(new object(), x);
                            OnFriendMsgs.Invoke(new object(), x);
                        }
                        else if ("OnEvents".Equals(ja[0].ToString()))
                        {
                            var x = new ObjectEventArgs(JObject.Parse(ja[1].ToString()));
                            OnServerAction.Invoke(new object(), x);
                            OnEventMsgs.Invoke(new object(), x);
                        }
                    }
                    catch
                    {

                    }
                }
                else
                {
                    ServerUtil.Log($"[Packet] [EmptyPacket / Maybe is serveric ping]", LogType.ServerMessage);
                }
                
            });
            if (_MonitorEnable)
            {
                MonitorTimer.Start();
            }
            _ServericOpen += (s, e) => { };
            _ServericPaused += (s, e) => { };
            _ServericClosed += (s, e) => { };
            OnServerAction += (s, e) => { };
            OnGroupMsgs += (s, e) => { };
            OnFriendMsgs += (s, e) => { };
            OnEventMsgs += (s, e) => { };
        }

        /// <summary>
        /// 关闭连接
        /// <para>normally dispose</para>
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            MonitorTimer.Stop();
            MonitorTimer.Dispose();
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
        /// SocketIO 服务器回报
        /// </summary>
        /// <param name="sender">$</param>
        /// <param name="e">*</param>
        public delegate void EventHandler(object sender, EventArgs e);
        /// <summary>
        /// 服务器当前状态开
        /// </summary>
        public event EventHandler _ServericOpen;
        /// <summary>
        /// 服务器当前状态暂停
        /// </summary>
        public event EventHandler _ServericPaused;
        /// <summary>
        /// 服务器当前状态关
        /// </summary>
        public event EventHandler _ServericClosed;

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
