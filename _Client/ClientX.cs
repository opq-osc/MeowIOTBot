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
        /// <param name="url">
        /// IOT处理端地址(请详细参见项目wiki)
        /// <para>IOT backend Url:please visit Wiki to see more</para>
        /// </param>
        /// <param name="qq">
        /// 你要监听的qq
        /// <para>the QQ you want to listen to</para>
        /// </param>
        /// <param name="logFlag">
        /// 日志处理标
        /// <para>the log handler</para>
        /// </param>
        /// <returns>
        /// 完全代理的IOT端
        /// <para>Full stack delegated IOT Backend</para>
        /// <para>用法如下 (Usage as below)</para>
        /// <code>
        /// <para>using var recv = MeowIOTClient.Connect("url", "qq");</para>
        /// <para>recv._(delegate event) += (s, e) =>{};</para>
        /// </code>
        /// </returns>
        public static MeowServiceClient Connect(string url, string qq, bool logFlag = false)
        {
            socket = new MeowServiceClient(url, qq, logFlag);
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
