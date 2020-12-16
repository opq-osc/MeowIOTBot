using System;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using System.Collections.Generic;
using System.Text;

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
        private static Basex.MeowServiceClient socket = null;
        /// <summary>
        /// 连接IOT后端
        /// <para>Connect IOT Backend</para>
        /// </summary>
        /// <returns>
        /// 返回一个标准代理端
        /// <para>returns an IOT Standard Backend</para>
        /// </returns>
        public static Basex.MeowServiceClient Connect(string url, string qq, bool logFlag = false)
        {
            socket = new Basex.MeowServiceClient(url, qq, logFlag);
            socket.CreateClient();
            //好友私聊
            socket._FriendTextMsgRecieve += (s, e) => { };
            //好友图片
            socket._FriendPicMsgRecieve += (s, e) => { };

            //群聊文本
            socket._GroupAtTextMsgRecieve += (s, e) => { };
            //群聊图片
            socket._GroupPicMsgRecieve += (s, e) => { };
            return socket;
        }
        /// <summary>
        /// 默认的关闭操作 (为了使用using)
        /// <para>default close opearion [as can use 'using']</para>
        /// </summary>
        public void Dispose() => socket.Dispose();
    }
}
