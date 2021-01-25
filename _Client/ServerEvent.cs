using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using MeowIOTBot.QQ.QQEvent;
using Newtonsoft.Json.Linq;

namespace MeowIOTBot.Basex
{
    public sealed partial class MeowServiceClient : MeowClient
    {
        #region 触发好友消息事件区域 -- Event Trigger --
        /// <summary>
        /// 好友消息委托 : 文本
        /// <para>Serveric [OnFriend : Text] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        /// <summary>
        /// 好友消息委托 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 好友消息委托 : 语音大类
        /// <para>Serveric [OnFriend : Voc] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        /// <summary>
        /// 好友消息委托 : 视频大类
        /// <para>Serveric [OnFriend : Vid] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventFriendVidMessageEventHandler(QQRecieveMessage sender, VideoMsg e);

        /// <summary>
        /// 好友消息事件 : 文本
        /// <para>Serveric [OnFriend : Text] Message Event</para>
        /// </summary>
        public event EventFriendTextMessageEventHandler _FriendTextMsgRecieve;
        /// <summary>
        /// 好友消息事件 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Event</para>
        /// </summary>
        public event EventFriendPicMessageEventHandler _FriendPicMsgRecieve;
        /// <summary>
        /// 好友消息事件 : 语音大类
        /// <para>Serveric [OnFriend : Voc] Message Event</para>
        /// </summary>
        public event EventFriendVocMessageEventHandler _FriendVocMsgRecieve;
        /// <summary>
        /// 好友消息事件 : 视频大类
        /// <para>Serveric [OnFriend : Vid] Message Event</para>
        /// </summary>
        public event EventFriendVidMessageEventHandler _FriendVidMsgRecieve;
        #endregion
        #region 触发群消息事件区域 -- Event Trigger --
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : AtText] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupAtTextMessageEventHandler(QQRecieveMessage sender, AtTextMsg e);
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : Text] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        /// <summary>
        /// 群消息委托 : 图片大类
        /// <para>Serveric [OnGroup : Pic] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 群消息委托 : 图片大类
        /// <para>Serveric [OnGroup : AtPic] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupAtPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 群消息委托 : 语音大类
        /// <para>Serveric [OnGroup : Voc] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        /// <summary>
        /// 群消息委托 : 视频大类
        /// <para>Serveric [OnGroup : Vid] Message Delegate</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void EventGroupVidMessageEventHandler(QQRecieveMessage sender, VideoMsg e);

        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : AtText] Message Event</para>
        /// </summary>
        public event EventGroupAtTextMessageEventHandler _GroupAtTextMsgRecieve;
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : Text] Message Event</para>
        /// </summary>
        public event EventGroupTextMessageEventHandler _GroupTextMsgRecieve;
        /// <summary>
        /// 群消息事件 : 图片大类
        /// <para>Serveric [OnGroup : Pic] Message Event</para>
        /// </summary>
        public event EventGroupPicMessageEventHandler _GroupPicMsgRecieve;
        /// <summary>
        /// 群消息事件 : 图片大类
        /// <para>Serveric [OnGroup : AtPic] Message Event</para>
        /// </summary>
        public event EventGroupAtPicMessageEventHandler _GroupAtPicMsgRecieve;
        /// <summary>
        /// 群消息事件 : 语音大类
        /// <para>Serveric [OnGroup : Voc] Message Event</para>
        /// </summary>
        public event EventGroupVocMessageEventHandler _GroupVocMsgRecieve;
        /// <summary>
        /// 群消息事件 : 视频大类
        /// <para>Serveric [OnGroup : Vid] Message Event</para>
        /// </summary>
        public event EventGroupVidMessageEventHandler _GroupVidMsgRecieve;
        #endregion
        #region 触发事件代理区域 -- Event Trigger -- 
        public delegate void Event_ON_EVENT_GROUP_ADMIN_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMIN e);
        public delegate void Event_ON_EVENT_GROUP_SHUT_EventHandler(EventMsg sender, ON_EVENT_GROUP_SHUT e);
        public delegate void Event_ON_EVENT_GROUP_ADMIN_SYSNOTIFY_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMINSYSNOTIFY e);
        public delegate void Event_ON_EVENT_GROUP_EXIT_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT e);
        public delegate void Event_ON_EVENT_GROUP_EXIT_SUCC_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT_SUCC e);
        public delegate void Event_ON_EVENT_GROUP_JOIN_EventHandler(EventMsg sender, ON_EVENT_GROUP_JOIN e);
        public delegate void Event_ON_EVENT_FRIEND_ADD_EventHandler(EventMsg sender, ON_EVENT_FRIEND_ADD e);
        public delegate void Event_ON_EVENT_EventHandler(EventMsg sender, JObject e);

        public event Event_ON_EVENT_GROUP_ADMIN_EventHandler __ON_EVENT_GROUP_ADMIN;
        public event Event_ON_EVENT_GROUP_SHUT_EventHandler __ON_EVENT_GROUP_SHUT;
        public event Event_ON_EVENT_GROUP_ADMIN_SYSNOTIFY_EventHandler __ON_EVENT_GROUP_ADMINSYSNOTIFY;
        public event Event_ON_EVENT_GROUP_EXIT_EventHandler __ON_EVENT_GROUP_EXIT;
        public event Event_ON_EVENT_GROUP_EXIT_SUCC_EventHandler __ON_EVENT_GROUP_EXIT_SUCC;
        public event Event_ON_EVENT_GROUP_JOIN_EventHandler __ON_EVENT_GROUP_JOIN;
        public event Event_ON_EVENT_FRIEND_ADD_EventHandler __ON_EVENT_FRIEND_ADD;
        public event Event_ON_EVENT_EventHandler __ON_UNMOUNT_EVENT;
        #endregion
    }
}
