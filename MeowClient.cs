using MeowIOTBot.ObjectEvent;
using Newtonsoft.Json.Linq;
using SocketLibrary;
using SocketLibrary.Messages;
using System;

namespace MeowIOTBot
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
        public bool logFlag { get; set; }
        /// <summary>
        /// Socket标
        /// <para>SocketClientModded</para>
        /// </summary>
        private Client socket = null;
        /// <summary>
        /// 要连接的QQ
        /// <para>the QQ you want to Connect</para>
        /// </summary>
        public string qq { get; }
        /// <summary>
        /// 构造方法
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="logFlag">
        /// 是否打印日志
        /// <para>if you want have an Log</para>
        /// </param>
        /// <param name="url">
        /// ws的连接Client位置 例如 ws://localhost:10000
        /// <para>Ws connection backend, like wise 'ws://localhost:10000'</para>
        /// </param>
        /// <param name="qq">
        /// 要监听的QQ
        /// <para>the QQ you want to Listento</para>
        /// </param>
        public MeowClient(string url, string qq, bool logFlag = false)
        {
            this.logFlag = logFlag;
            this.qq = qq;
            this.socket = new Client(url);
        }
        /// <summary>
        /// 服务器消息事件委托
        /// <para>Serveric Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ObjectEventHandler(object sender, ObjectEventArgs e);
        /// <summary>
        /// 服务器消息事件
        /// <para>Serveric Message Event</para>
        /// </summary>
        public event ObjectEventHandler OnServerAction;
        /// <summary>
        /// 连接并获取Client对象
        /// <para>Connect and get Object Client</para>
        /// </summary>
        public MeowClient Connect()
        {
            //开Socket
            socket.Connect();
            //服务端连接
            socket.On("connect", (fn) =>
            {
                Console.WriteLine(((ConnectMessage)fn).ConnectMsg);
                socket.Emit("GetWebConn", this.qq, null, (d) =>
                {
                    var jsonMsg = d as string;
                    Console.WriteLine($"返回状态 [{qq}].{jsonMsg}");
                });
            });
            //回调群消息事件源
            socket.On("OnGroupMsgs", (fn) => OnServerAction.Invoke(new object(), new ObjectEventArgs(JObject.Parse(((JSONMessage)fn).MessageText))));
            //回调好友消息事件源
            socket.On("OnFriendMsgs", (fn) => OnServerAction.Invoke(new object(), new ObjectEventArgs(JObject.Parse(((JSONMessage)fn).MessageText))));
            //回调事件源
            socket.On("OnEvents", (fn) => OnServerAction.Invoke(new object(), new ObjectEventArgs(JObject.Parse(((JSONMessage)fn).MessageText))));
            //支持连写
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
                    socket.Dispose(); // close & dispose of socket client
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
}
