using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using MeowIOTBot.QQ.QQEvent;
using Newtonsoft.Json.Linq;
using MeowIOTBot.Basex;

namespace MeowIOTBot
{
    public sealed partial class MeowServiceClient : MeowClient
    {
        #region 触发好友消息事件区域 -- Event Trigger --
        public delegate void EventFriendTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        public delegate void EventFriendPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        public delegate void EventFriendVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        public delegate void EventFriendVidMessageEventHandler(QQRecieveMessage sender, VideoMsg e);
        /*-----------------------------------------------------------------------*/
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
        public delegate void EventGroupAtTextMessageEventHandler(QQRecieveMessage sender, AtTextMsg e);
        public delegate void EventGroupTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        public delegate void EventGroupPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        public delegate void EventGroupAtPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        public delegate void EventGroupVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        public delegate void EventGroupVidMessageEventHandler(QQRecieveMessage sender, VideoMsg e);
        /*-----------------------------------------------------------------------*/
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
        public delegate void Event_ON_EVENT_GROUP_EXIT_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT e);
        public delegate void Event_ON_EVENT_GROUP_EXIT_SUCC_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT_SUCC e);
        public delegate void Event_ON_EVENT_GROUP_JOIN_EventHandler(EventMsg sender, ON_EVENT_GROUP_JOIN e);

        public delegate void Event_ON_EVENT_GROUP_ADMIN_SYSNOTIFY_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMINSYSNOTIFY e);
        public delegate void Event_ON_EVENT_GROUP_INVITE_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMINSYSNOTIFY_INVITE_GROUP e);

        public delegate void Event_ON_EVENT_FRIEND_ADD_EventHandler(EventMsg sender, ON_EVENT_FRIEND_ADD e);
        public delegate void Event_ON_EVENT_FRIEND_DELETE_EventHandler(EventMsg sender, ON_EVENT_FRIEND_DELETE e);
        public delegate void Event_ON_EVENT_NOTIFY_PUSHADDFRD_EventHandler(EventMsg sender, ON_EVENT_NOTIFY_PUSHADDFRD e);
        public delegate void Event_ON_EVENT_FRIEND_ADD_STATUS_EventHandler(EventMsg sender, ON_EVENT_FRIEND_ADD_STATUS e);

        public delegate void Event_ON_EVENT_EventHandler(EventMsg sender, JObject e);
        /*-----------------------------------------------------------------------*/
        /// <summary>
        /// 管理员变更事件
        /// <para>GroupAdminChangeEvent</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_ADMIN_EventHandler __ON_EVENT_GROUP_ADMIN;
        /// <summary>
        /// 群禁言事件
        /// <para>GroupShut-upEvent</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_SHUT_EventHandler __ON_EVENT_GROUP_SHUT;
        /// <summary>
        /// 群操作相关事件 (一层解析)
        /// <para>*FirstLayerOf*GroupAdministratorEvent</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_ADMIN_SYSNOTIFY_EventHandler __ON_EVENT_GROUP_ADMINSYSNOTIFY;
        /// <summary>
        /// 退群相关事件
        /// <para>Someone Exit the Group</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_EXIT_EventHandler __ON_EVENT_GROUP_EXIT;
        /// <summary>
        /// 主动退群成功事件
        /// <para>SelfExitEvent</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_EXIT_SUCC_EventHandler __ON_EVENT_GROUP_EXIT_SUCC;
        /// <summary>
        /// 加群事件
        /// <para>Join Group Event</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_JOIN_EventHandler __ON_EVENT_GROUP_JOIN;
        /// <summary>
        /// 邀请加群事件
        /// <para>Invite into group Event</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_INVITE_EventHandler __ON_EVENT_GROUP_INVITE;

        /// <summary>
        /// 加好友事件
        /// <para>Add Friend Event</para>
        /// </summary>
        public event Event_ON_EVENT_FRIEND_ADD_EventHandler __ON_EVENT_FRIEND_ADD;
        /// <summary>
        /// 删除好友事件
        /// <para>Friend Delete Event</para>
        /// </summary>
        public event Event_ON_EVENT_FRIEND_DELETE_EventHandler __ON_EVENT_FRIEND_DELETE;
        /// <summary>
        /// 成为好友事件
        /// <para>Become Friend Event</para>
        /// </summary>
        public event Event_ON_EVENT_NOTIFY_PUSHADDFRD_EventHandler __ON_EVENT_FRIEND_PUSHADDFRD;
        /// <summary>
        /// 好友状态事件
        /// <para>Friend Status Event</para>
        /// </summary>
        public event Event_ON_EVENT_FRIEND_ADD_STATUS_EventHandler __ON_EVENT_FRIEND_ADD_STATUS;


        /// <summary>
        /// 未识别事件
        /// <para>Unknown Event</para>
        /// </summary>
        public event Event_ON_EVENT_EventHandler __ON_UNMOUNT_EVENT;
        #endregion
    }
}
