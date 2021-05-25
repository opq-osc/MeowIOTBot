using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using MeowIOTBot.QQ.QQEvent;
using System;
using MeowIOTBot.Basex;

namespace MeowIOTBot
{
    public sealed partial class MeowServiceClient : MeowClient
    {
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
                            $"内容:{msg.Content}", LogType.ClientMessage, ConsoleColor.Magenta,ConsoleColor.Cyan);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        var d = "图片";
                        if(msg.AtedQQ != null)
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
                            ,LogType.ClientMessage, ConsoleColor.Green);
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
                        ServerUtil.Log($"[*群管理员变更*] 群{x.GroupID}的{x.UserID}{(x.Flag==0?"不再是管理员了":"成为管理员")}",LogType.ClientVerbose);
                    }
                    break;
                case "ON_EVENT_GROUP_SHUT":
                    {
                        var x = (ON_EVENT_GROUP_SHUT)d.Data;
                        __ON_EVENT_GROUP_SHUT.Invoke(d.EMsg, x);
                        ServerUtil.Log($"[*群禁言*] 群{x.GroupID}的{(x.UserID==0?"全体":x.UserID)}被{(x.ShutTime==0?"解除禁言":$"禁言{x.ShutTime}分钟")}", LogType.ClientVerbose);
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
    }
}
