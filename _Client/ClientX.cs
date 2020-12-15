using System;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using System.Collections.Generic;
using System.Text;

namespace MeowIOTBot
{
    /// <summary>
    /// 完全代理的IOT端
    /// <para>Full stack delegated IOT Backend</para>
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
            socket._FriendTextMsgRecieve += (s, e) => { };
            return socket;
        }
        /// <summary>
        /// 默认的关闭操作 (为了使用using)
        /// <para>default close opearion [as can use 'using']</para>
        /// </summary>
        public void Dispose() => socket.Dispose();
    }
}
