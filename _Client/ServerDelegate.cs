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
            meow.OnGroupMsgs += Meow_OnGroupMsgs;
            meow.OnEventMsgs += Meow_OnEventMsgs;
            meow.OnFriendMsgs += Meow_OnFriendMsgs;
            return this;
        }
        /// <summary>
        /// 事件解析
        /// <para>Event Interperter</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Meow_OnEventMsgs(object sender, ObjectEvent.ObjectEventArgs e)
        {
            
        }
        /// <summary>
        /// 私聊解析
        /// <para>Friend Conget</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Meow_OnFriendMsgs(object sender, ObjectEvent.ObjectEventArgs e)
        {
            #region 私聊报头 -- Private Info Head -- 
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
            //触发对应操作信息
            switch (e.Data["MsgType"].ToString())
            {
                case "TextMsg":
                    {
                        var msg = new TextMessage(content);
                        Log($"好友文本信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}] \n" +
                            $"内容:{msg.Content}", ConsoleColor.Magenta);
                        _FriendTextMsgRecieve.Invoke(prop, msg);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        Log($"好友图片信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}] \n" +
                            $"内容:{msg.Content} | 图片共 {msg.PicList.Length} 张", ConsoleColor.Yellow);
                        _FriendPicMsgRecieve.Invoke(prop, msg);
                    }
                    break;
            };
        }
        /// <summary>
        /// 群聊解析
        /// <para>Group Conget</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Meow_OnGroupMsgs(object sender, ObjectEvent.ObjectEventArgs e)
        {
            #region 群聊报头 -- Group Info Head -- 
            var prop = new QQRecieveMessage(
                MessageIOType.Friend,
                new IOBody(
                    e.Data["FromUserId"].ToObject<long>(),
                    0,
                    e.Data["FromNickName"].ToString(),
                    e.Data["FromGroupId"].ToObject<long>(),
                    e.Data["FromGroupName"].ToString()
                    ),
                new MsgProperty(
                    null,
                    e.Data["MsgSeq"].ToObject<long?>(),
                    e.Data["MsgRandom"].ToObject<long?>(),
                    null,
                    e.Data["RedBaginfo"].ToString(),
                    e.Data["MsgTime"].ToObject<long?>()
                    ),
                e.CurrentQQ
            );
            #endregion
            //文本
            var content = e.Data["Content"].ToString();
            //触发对应操作信息
            switch (e.Data["MsgType"].ToString())
            {
                case "TextMsg":
                    {
                        var msg = new TextMessage(content);
                        _GroupTextMsgRecieve.Invoke(prop, msg);
                        Log($"群文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", ConsoleColor.Magenta);
                    }
                    break;
                case "AtMsg":
                    {
                        var msg = new AtTextMessage(content);
                        _GroupAtTextMsgRecieve.Invoke(prop, msg);
                        Log($"群At文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", ConsoleColor.Magenta,ConsoleColor.Cyan);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        _GroupPicMsgRecieve.Invoke(prop, msg);
                        Log($"群图片信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content} | 图片共 {msg.PicList.Length} 张", ConsoleColor.Yellow);
                    }
                    break;
            };
        }

        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="s">字串</param>
        /// <param name="Fore">前景色</param>
        /// <param name="Back">背景色</param>
        private static void Log(string s, ConsoleColor Fore = ConsoleColor.White,ConsoleColor Back=ConsoleColor.Black)
        {
            if (logFlag)
            {
                Console.ForegroundColor = Fore;
                Console.BackgroundColor = Back;
                Console.WriteLine($"{DateTime.Now} : : {s}");
                Console.ResetColor();
            }
        }

        #region 触发事件区域 -- Event Trigger --
        /// <summary>
        /// 好友消息委托 : 文本
        /// <para>Serveric [OnFriend : Text] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendTextMessageEventHandler(QQRecieveMessage sender, TextMessage e);
        /// <summary>
        /// 好友消息事件 : 文本
        /// <para>Serveric [OnFriend : Text] Message Event</para>
        /// </summary>
        public event EventFriendTextMessageEventHandler _FriendTextMsgRecieve;
        /// <summary>
        /// 好友消息委托 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 好友消息事件 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Event</para>
        /// </summary>
        public event EventFriendPicMessageEventHandler _FriendPicMsgRecieve;

        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : AtText] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupAtTextMessageEventHandler(QQRecieveMessage sender, AtTextMessage e);
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : AtText] Message Event</para>
        /// </summary>
        public event EventGroupAtTextMessageEventHandler _GroupAtTextMsgRecieve;
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : Text] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupTextMessageEventHandler(QQRecieveMessage sender, TextMessage e);
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : Text] Message Event</para>
        /// </summary>
        public event EventGroupTextMessageEventHandler _GroupTextMsgRecieve;
        /// <summary>
        /// 群消息委托 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 群消息事件 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Event</para>
        /// </summary>
        public event EventGroupPicMessageEventHandler _GroupPicMsgRecieve;
        #endregion
    }
}
