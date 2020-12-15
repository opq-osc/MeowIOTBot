using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeowIOTBot.Basex
{
    /// <summary>
    /// 带有详细回调的端
    /// <para>Client With a detailed delegate</para>
    /// </summary>
    public sealed class MeowServiceClient : MeowClient
    {
        /// <summary>
        /// 一个自枚举的多功能解释端
        /// <para>an Enumable multi-purpose explain backend</para>
        /// </summary>
        /// <param name="url">
        /// 你的IOT连接位置
        /// <para>IOT backend</para>
        /// </param>
        /// <param name="qq">
        /// 你的QQ号
        /// <para>qq number</para>
        /// </param>
        /// <param name="logflag">
        /// 一个是否记录日志的标志(默认关闭)
        /// <para>an entryset that if you want a log * (usually not)</para>
        /// </param>
        public MeowServiceClient(string url, string qq, bool logflag = false) : base(url, qq, logflag) { }
        /// <summary>
        /// 一个自枚举的多功能解释端
        /// <para>an Enumable multi-purpose explain backend</para>
        /// </summary>
        public MeowServiceClient CreateClient()
        {
            var meow = new MeowClient(url, qq, logFlag).Connect();
            meow.OnServerAction += (s, e) => { };//默认失去作用的
            meow.OnGroupMsgs += (s, e) => { };
            meow.OnEventMsgs += (s, e) => { };
            meow.OnFriendMsgs += (s, e) => {

                #region 报头 -- Info Head -- 
                var prop = new QQRecieveMessage(
                    MessageIOType.Friend,
                    new IOBody(
                        e.Data["FromUin"].ToObject<long>(),
                        e.Data["ToUin"].ToObject<long>(), 
                        null, 
                        null, 
                        null
                        ),
                    new MsgProperty(
                        null,
                        e.Data["MsgSeq"].ToObject<long?>(), 
                        null, 
                        null,
                        e.Data["RedBaginfo"].ToString(), 
                        null
                        ),
                    e.CurrentQQ
                );
                #endregion
                //文本
                var content = e.Data["Content"].ToString();

                switch (e.Data["MsgType"].ToString())
                {
                    case "TextMsg": 
                        {
                            var msg = new TextMessage(content);
                            _FriendTextMsgRecieve.Invoke(prop, msg);
                        } 
                        break;
                    case "PicMsg":
                        {
                            var msg = new PicMsg(content);
                        }
                        break;
                };
            };
            return this;
        }

        /// <summary>
        /// 好友消息委托
        /// <para>Serveric [OnFriend] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendTextMessageEventHandler(QQRecieveMessage sender, TextMessage e);
        /// <summary>
        /// 好友消息事件
        /// <para>Serveric [OnFriend] Message Event</para>
        /// </summary>
        public event EventFriendTextMessageEventHandler _FriendTextMsgRecieve;
    }
}
