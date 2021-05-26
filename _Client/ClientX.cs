using MeowIOTBot.Basex;
using System;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using MeowIOTBot.QQ.QQEvent;
using Newtonsoft.Json.Linq;

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
    public class MeowIOTClient : MeowClient
    {
        /// <summary>
        /// 一个自枚举的多功能解释端
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
        public MeowIOTClient(string url, LogType logflag, int eIO = 3) 
        : base(url, logflag, eIO)
        {
            OnServerAction += SocketNullDelegate;
            OnGroupMsgs += Meow_OnGroupMsgs;
            OnEventMsgs += Meow_OnEventMsgs;
            OnFriendMsgs += Meow_OnFriendMsgs;
            _FriendTextMsgRecieve += SocketNullDelegate;
            _FriendPicMsgRecieve += SocketNullDelegate;
            _FriendVocMsgRecieve += SocketNullDelegate;
            _FriendVidMsgRecieve += SocketNullDelegate;
            _GroupAtTextMsgRecieve += SocketNullDelegate;
            _GroupAtPicMsgRecieve += SocketNullDelegate;
            _GroupPicMsgRecieve += SocketNullDelegate;
            _GroupTextMsgRecieve += SocketNullDelegate;
            _GroupVocMsgRecieve += SocketNullDelegate;
            _GroupVidMsgRecieve += SocketNullDelegate;
            __ON_EVENT_GROUP_ADMIN += SocketNullDelegate;
            __ON_EVENT_GROUP_ADMINSYSNOTIFY += SocketNullDelegate;
            __ON_EVENT_GROUP_EXIT += SocketNullDelegate;
            __ON_EVENT_GROUP_EXIT_SUCC += SocketNullDelegate;
            __ON_EVENT_GROUP_INVITE += SocketNullDelegate;
            __ON_EVENT_GROUP_JOIN += SocketNullDelegate;
            __ON_EVENT_GROUP_REVOKE += SocketNullDelegate;
            __ON_EVENT_GROUP_SHUT += SocketNullDelegate;
            __ON_EVENT_FRIEND_ADD += SocketNullDelegate;
            __ON_EVENT_FRIEND_ADD_STATUS += SocketNullDelegate;
            __ON_EVENT_FRIEND_DELETE += SocketNullDelegate;
            __ON_EVENT_FRIEND_PUSHADDFRD += SocketNullDelegate;
            __ON_UNMOUNT_EVENT += SocketNullDelegate;
        }
        /// <summary>
        /// 连接IOT后端
        /// </summary>
        public async System.Threading.Tasks.Task<MeowIOTClient> Connect()
        {
            await ss.ConnectAsync();
            return this;
        }

        #region 自动事件处理 -- Event Trigger --
        private void SocketNullDelegate(object sender, object e) { }
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
                        var msg = new TextMsg(content);
                        _FriendTextMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"好友文本信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}] \n" +
                            $"内容:{msg.Content}", LogType.ClientMessage, ConsoleColor.Magenta);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        _FriendPicMsgRecieve.Invoke(prop, msg);
                        if (msg._isSnapPic)
                        {
                            ServerUtil.Log($"好友闪照信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}]", LogType.ClientMessage, ConsoleColor.Yellow);
                        }
                        else
                        {
                            ServerUtil.Log($"好友图片信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}] \n" +
                            $"内容:{msg.Content} | 图片共 {msg.PicList.Length} 张", LogType.ClientMessage, ConsoleColor.Yellow);
                        }

                    }
                    break;
                case "VoiceMsg":
                    {
                        var msg = new VoiceMsg(content);
                        _FriendVocMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"好友语音信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}]"
                            , LogType.ClientMessage, ConsoleColor.DarkMagenta);
                    }
                    break;
                case "VideoMsg":
                    {
                        var msg = new VideoMsg(content);
                        _FriendVidMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"好友视频信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}]"
                            , LogType.ClientMessage, ConsoleColor.Green);
                    }
                    break;
            };
        }
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
                        var msg = new TextMsg(content);
                        _GroupTextMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"群文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", LogType.ClientMessage, ConsoleColor.Magenta);
                    }
                    break;
                case "AtMsg":
                    {
                        var msg = new AtTextMsg(content);
                        _GroupAtTextMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"群At文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", LogType.ClientMessage, ConsoleColor.Magenta, ConsoleColor.Cyan);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        var d = "图片";
                        if (msg.AtedQQ != null)
                        {
                            _GroupAtPicMsgRecieve.Invoke(prop, msg);
                            d = "At图片";
                        }
                        if (msg._isSnapPic)
                        {
                            d = "闪照";
                        }
                        _GroupPicMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"群{d} [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content} | 图片共 {msg.PicList.Length} 张", LogType.ClientMessage, ConsoleColor.Yellow);
                    }
                    break;
                case "VoiceMsg":
                    {
                        var msg = new VoiceMsg(content);
                        _GroupVocMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"群语音信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}]"
                            , LogType.ClientMessage, ConsoleColor.DarkMagenta);
                    }
                    break;
                case "VideoMsg":
                    {
                        var msg = new VideoMsg(content);
                        _GroupVidMsgRecieve.Invoke(prop, msg);
                        ServerUtil.Log($"群视频信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}]"
                            , LogType.ClientMessage, ConsoleColor.Green);
                    }
                    break;
            };
        }
        private void Meow_OnEventMsgs(object sender, ObjectEvent.ObjectEventArgs e)
        {
            var d = new Event(e.Data);
            switch (d.EType.ToString())
            {
                case "ON_EVENT_GROUP_ADMIN":
                    {
                        var x = (ON_EVENT_GROUP_ADMIN)d.Data;
                        __ON_EVENT_GROUP_ADMIN.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群管理员变更*] 群{x.GroupID}的{x.UserID}{(x.Flag == 0 ? "不再是管理员了" : "成为管理员")}", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_SHUT":
                    {
                        var x = (ON_EVENT_GROUP_SHUT)d.Data;
                        __ON_EVENT_GROUP_SHUT.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群禁言*] 群{x.GroupID}的{(x.UserID == 0 ? "全体" : x.UserID)}被{(x.ShutTime == 0 ? "解除禁言" : $"禁言{x.ShutTime}分钟")}", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_ADMINSYSNOTIFY":
                    {
                        var x = (ON_EVENT_GROUP_ADMINSYSNOTIFY)d.Data;
                        __ON_EVENT_GROUP_ADMINSYSNOTIFY.Invoke(d.EMsg, x);
                        switch (d.EMsg.Content switch
                        {
                            "退群消息" => EventType.ON_EVENT_GROUP_EXIT,
                            "邀请加群" => EventType.ON_EVENT_GROUP_ADMINSYSNOTIFY_INVITE_GROUP,
                            _ => EventType.ON_EVENT_GROUP_ADMINSYSNOTIFY
                        })
                        {
                            case EventType.ON_EVENT_GROUP_ADMINSYSNOTIFY_INVITE_GROUP:
                                __ON_EVENT_GROUP_INVITE.Invoke(d.EMsg, new ON_EVENT_GROUP_ADMINSYSNOTIFY_INVITE_GROUP(x));
                                break;
                        }
                        ServerUtil.Log($"[*{x.MsgTypeStr}*] {x.Who}-->{x.GroupId}::{x.MsgStatusStr}", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_EXIT":
                    {
                        var x = (ON_EVENT_GROUP_EXIT)d.Data;
                        __ON_EVENT_GROUP_EXIT.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群成员退出*] 群{d.EMsg.FromUser}的{x.UserID}用户退出了群聊", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_EXIT_SUCC":
                    {
                        var x = (ON_EVENT_GROUP_EXIT_SUCC)d.Data;
                        __ON_EVENT_GROUP_EXIT_SUCC.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群主动退出*] 你主动退出了群号为{x.GroupID}的群聊", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_JOIN":
                    {
                        var x = (ON_EVENT_GROUP_JOIN)d.Data;
                        __ON_EVENT_GROUP_JOIN.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群成员加入*] {x.UserID}加入了{d.EMsg.FromUser}群", LogType.ClientVerbose);
                    }
                    break;
                /*----------------------------------------------------------*/
                case "ON_EVENT_FRIEND_ADD":
                    {
                        var x = (ON_EVENT_FRIEND_ADD)d.Data;
                        __ON_EVENT_FRIEND_ADD.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*好友添加申请*] {x.UserID}申请成为你的好友", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_NOTIFY_PUSHADDFRD":
                    {
                        var x = (ON_EVENT_NOTIFY_PUSHADDFRD)d.Data;
                        __ON_EVENT_FRIEND_PUSHADDFRD.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*好友添加成功*] {x.UserID}已经成为你的好友", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_FRIEND_ADD_STATUS":
                    {
                        var x = (ON_EVENT_FRIEND_ADD_STATUS)d.Data;
                        __ON_EVENT_FRIEND_ADD_STATUS.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*好友添加状态*] {x.UserID}{x.TypeStatus}", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_FRIEND_DELETE":
                    {
                        var x = (ON_EVENT_FRIEND_DELETE)d.Data;
                        __ON_EVENT_FRIEND_DELETE.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*解除好友*] {x.UserID}不再是你的好友了", LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_REVOKE":
                    {
                        var x = (ON_EVENT_GROUP_REVOKE)d.Data;
                        __ON_EVENT_GROUP_REVOKE.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群成员撤回信息*] 在群{x.GroupID},{x.AdminUserID}撤回了{x.UserID}的信息", LogType.ClientVerbose);
                    }
                    break;
                default:
                    {
                        __ON_UNMOUNT_EVENT.Invoke(null, e.Data);
                        ServerUtil.Log($"[*未解析事件*] 事件源Json:\n{e.Data}", LogType.ClientVerbose);
                    }
                    break;
            }
        }
        #endregion
        #region 触发好友消息事件区域 -- Event Trigger --
        /// <summary>
        /// 好友消息事件 : 文本
        /// <para>Serveric [OnFriend : Text] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventFriendTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        /// <summary>
        /// 好友消息事件 : 图片大类
        /// <para>Serveric [OnFriend : Pic] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventFriendPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 好友消息事件 : 语音大类
        /// <para>Serveric [OnFriend : Voc] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventFriendVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        /// <summary>
        /// 好友消息事件 : 视频大类
        /// <para>Serveric [OnFriend : Vid] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
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
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : AtText] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventGroupAtTextMessageEventHandler(QQRecieveMessage sender, AtTextMsg e);
        /// <summary>
        /// 群消息委托 : 文本大类
        /// <para>Serveric [OnGroup : Text] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventGroupTextMessageEventHandler(QQRecieveMessage sender, TextMsg e);
        /// <summary>
        /// 群消息事件 : 图片大类
        /// <para>Serveric [OnGroup : Pic] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventGroupPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 群消息事件 : 图片大类
        /// <para>Serveric [OnGroup : AtPic] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventGroupAtPicMessageEventHandler(QQRecieveMessage sender, PicMsg e);
        /// <summary>
        /// 群消息事件 : 语音大类
        /// <para>Serveric [OnGroup : Voc] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void EventGroupVocMessageEventHandler(QQRecieveMessage sender, VoiceMsg e);
        /// <summary>
        /// 群消息事件 : 视频大类
        /// <para>Serveric [OnGroup : Vid] Message Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
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
        /// <summary>
        /// 管理员变更事件
        /// <para>GroupAdminChangeEvent</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_ADMIN_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMIN e);
        /// <summary>
        /// 群禁言事件
        /// <para>GroupShut-upEvent</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_SHUT_EventHandler(EventMsg sender, ON_EVENT_GROUP_SHUT e);
        /// <summary>
        /// 群操作相关事件 (一层解析)
        /// <para>*FirstLayerOf*GroupAdministratorEvent</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_EXIT_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT e);
        /// <summary>
        /// 退群相关事件
        /// <para>Someone Exit the Group</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_EXIT_SUCC_EventHandler(EventMsg sender, ON_EVENT_GROUP_EXIT_SUCC e);
        /// <summary>
        /// 主动退群成功事件
        /// <para>SelfExitEvent</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_JOIN_EventHandler(EventMsg sender, ON_EVENT_GROUP_JOIN e);
        /// <summary>
        /// 加群事件
        /// <para>Join Group Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_ADMIN_SYSNOTIFY_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMINSYSNOTIFY e);
        /// <summary>
        /// 邀请加群事件
        /// <para>Invite into group Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_INVITE_EventHandler(EventMsg sender, ON_EVENT_GROUP_ADMINSYSNOTIFY_INVITE_GROUP e);
        /// <summary>
        /// 群成员撤回信息事件
        /// <para>Event for revoke message in group</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_GROUP_REVOKE_EventHandler(EventMsg sender, ON_EVENT_GROUP_REVOKE e);
        /// <summary>
        /// 加好友事件
        /// <para>Add Friend Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_FRIEND_ADD_EventHandler(EventMsg sender, ON_EVENT_FRIEND_ADD e);
        /// <summary>
        /// 删除好友事件
        /// <para>Friend Delete Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_FRIEND_DELETE_EventHandler(EventMsg sender, ON_EVENT_FRIEND_DELETE e);
        /// <summary>
        /// 成为好友事件
        /// <para>Become Friend Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_NOTIFY_PUSHADDFRD_EventHandler(EventMsg sender, ON_EVENT_NOTIFY_PUSHADDFRD e);
        /// <summary>
        /// 好友状态事件
        /// <para>Friend Status Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
        public delegate void Event_ON_EVENT_FRIEND_ADD_STATUS_EventHandler(EventMsg sender, ON_EVENT_FRIEND_ADD_STATUS e);
        /// <summary>
        /// 未识别事件
        /// <para>Unknown Event</para>
        /// </summary>
        /// <param name="sender">报文头</param>
        /// <param name="e">报文体</param>
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
        /// 群成员撤回信息事件
        /// <para>Event for revoke message in group</para>
        /// </summary>
        public event Event_ON_EVENT_GROUP_REVOKE_EventHandler __ON_EVENT_GROUP_REVOKE;

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
