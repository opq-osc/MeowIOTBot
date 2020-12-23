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
        public static Basex.MeowServiceClient Connect(string url, string qq, bool logFlag = false)
        {
            socket = new Basex.MeowServiceClient(url, qq, logFlag);
            socket.CreateClient();

            socket._FriendTextMsgRecieve += (s, e) => { }; //好友私聊
            socket._FriendPicMsgRecieve += (s, e) => { }; //好友图片
            socket._FriendVocMsgRecieve += (s, e) => { }; //好友语音
            socket._FriendVidMsgRecieve += (s, e) => { }; //好友视频
            socket._GroupAtTextMsgRecieve += (s, e) => { }; //群聊At文本
            socket._GroupAtPicMsgRecieve += (s, e) => { }; //群聊At图片
            socket._GroupPicMsgRecieve += (s, e) => { }; //群聊图片
            socket._GroupTextMsgRecieve += (s, e) => { }; //群聊文本
            socket._GroupVocMsgRecieve += (s, e) => { }; //群聊语音
            socket._GroupVidMsgRecieve += (s, e) => { }; //群聊视频

            return socket;
        }
        /// <summary>
        /// 默认的关闭操作 (为了使用using)
        /// <para>default close opearion [as can use 'using']</para>
        /// </summary>
        public void Dispose() => socket.Dispose();
    }
}
