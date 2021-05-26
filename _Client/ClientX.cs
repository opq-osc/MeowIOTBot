using System;

namespace MeowIOTBot
{
    /// <summary>
    /// 完全代理的IOT端
    /// <para>Full stack delegated IOT Backend</para>
    /// <para>用法如下 (Usage as below)</para>
    /// <code>
    /// <para>using var recv = MeowIOTClient.Connect("url", "qq");</para>
    /// <para>recv._(delegate event) += (s, e) =>{};</para>
    /// </code>
    /// </summary>
    public class MeowIOTClient : IDisposable
    {
        /// <summary>
        /// Socket标志
        /// <para>Socket Sign</para>
        /// </summary>
        private static MeowServiceClient socket = null;
        /// <summary>
        /// 连接IOT后端
        /// <para>connect IOT Backend</para>
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
        public static MeowServiceClient Connect(string url, LogType logFlag,
            double ReconnectInterval = 30, bool enableForceReconnection = false,
            long connectionTimedOutTick = 10000, int reconnectionDelay = 10000,
            int reconnectionDelayMax = 100000, int eIO = 3,
            bool reconnection = true, bool allowedRetryFirstConnection = false)
        {
            socket = new MeowServiceClient(url, logFlag, ReconnectInterval, enableForceReconnection,
             connectionTimedOutTick, reconnectionDelay, reconnectionDelayMax, eIO,
             reconnection, allowedRetryFirstConnection);
            socket.CreateClient();
            //防止空指针异常
            socket._FriendTextMsgRecieve += SocketNullDelegate; //好友私聊
            socket._FriendPicMsgRecieve += SocketNullDelegate; //好友图片
            socket._FriendVocMsgRecieve += SocketNullDelegate; //好友语音
            socket._FriendVidMsgRecieve += SocketNullDelegate; //好友视频
            socket._GroupAtTextMsgRecieve += SocketNullDelegate; //群聊At文本
            socket._GroupAtPicMsgRecieve += SocketNullDelegate; //群聊At图片
            socket._GroupPicMsgRecieve += SocketNullDelegate; //群聊图片
            socket._GroupTextMsgRecieve += SocketNullDelegate; //群聊文本
            socket._GroupVocMsgRecieve += SocketNullDelegate; //群聊语音
            socket._GroupVidMsgRecieve += SocketNullDelegate; //群聊视频

            socket.__ON_EVENT_GROUP_ADMIN += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_ADMINSYSNOTIFY += SocketNullDelegate; 
            socket.__ON_EVENT_GROUP_EXIT += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_EXIT_SUCC += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_INVITE += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_JOIN += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_REVOKE += SocketNullDelegate;
            socket.__ON_EVENT_GROUP_SHUT += SocketNullDelegate;
            socket.__ON_EVENT_FRIEND_ADD += SocketNullDelegate; 
            socket.__ON_EVENT_FRIEND_ADD_STATUS += SocketNullDelegate;
            socket.__ON_EVENT_FRIEND_DELETE += SocketNullDelegate;
            socket.__ON_EVENT_FRIEND_PUSHADDFRD += SocketNullDelegate;
            socket.__ON_UNMOUNT_EVENT += SocketNullDelegate;
            return socket;
        }
        private static void SocketNullDelegate(object sender, object e){ }
        /// <summary>
        /// 默认的关闭操作 (为了使用using)
        /// <para>default close opearion [as can use 'using']</para>
        /// </summary>
        public void Dispose() => socket.Dispose();
    }
}
