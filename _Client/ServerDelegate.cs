using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using MeowIOTBot.QQ.QQEvent;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace MeowIOTBot.Basex
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
                case "VoiceMsg":
                    {
                        var msg = new VoiceMsg(content);
                        _FriendVocMsgRecieve.Invoke(prop, msg);
                        Log($"好友语音信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}]"
                            , ConsoleColor.DarkMagenta);
                    }
                    break;
                case "VideoMsg":
                    {
                        var msg = new VideoMsg(content);
                        _FriendVidMsgRecieve.Invoke(prop, msg);
                        Log($"好友视频信息 [{prop.IOBody.MsgFromQQ}->{prop.IOBody.MsgRecvQQ}]"
                            , ConsoleColor.Green);
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
                        Log($"群文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", ConsoleColor.Magenta);
                    }
                    break;
                case "AtMsg":
                    {
                        var msg = new AtTextMsg(content);
                        _GroupAtTextMsgRecieve.Invoke(prop, msg);
                        Log($"群At文本信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content}", ConsoleColor.Magenta,ConsoleColor.Cyan);
                    }
                    break;
                case "PicMsg":
                    {
                        var msg = new PicMsg(content);
                        var d = "";
                        if(msg.AtedQQ != null)
                        {
                            _GroupAtPicMsgRecieve.Invoke(prop, msg);
                            d = "At";
                        }
                        _GroupPicMsgRecieve.Invoke(prop, msg);
                        Log($"群{d}图片信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}] \n" +
                            $"内容:{msg.Content} | 图片共 {msg.PicList.Length} 张", ConsoleColor.Yellow);
                    }
                    break;
                case "VoiceMsg":
                    {
                        var msg = new VoiceMsg(content);
                        _GroupVocMsgRecieve.Invoke(prop, msg);
                        Log($"群语音信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}]"
                            , ConsoleColor.DarkMagenta);
                    }
                    break;
                case "VideoMsg":
                    {
                        var msg = new VideoMsg(content);
                        _GroupVidMsgRecieve.Invoke(prop, msg);
                        Log($"群视频信息 [{prop.IOBody.MsgFromQQ}] " +
                            $"在群聊 [{prop.IOBody.FromGroupId} :: {prop.IOBody.FromGroupName}]"
                            , ConsoleColor.Green);
                    }
                    break;
            };
        }
        private void Meow_OnEventMsgs(object sender, ObjectEvent.ObjectEventArgs e)
        {
            Console.WriteLine(e.Data);
            var d = new Event(e.Data);
            switch (d.EType.ToString())
            {
                case "ON_EVENT_GROUP_ADMIN":
                    {
                        var x = (ON_EVENT_GROUP_ADMIN)d.Data;
                        __ON_EVENT_GROUP_ADMIN.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_GROUP_SHUT":
                    {
                        var x = (ON_EVENT_GROUP_SHUT)d.Data;
                        __ON_EVENT_GROUP_SHUT.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_GROUP_ADMINSYSNOTIFY":
                    {
                        var x = (ON_EVENT_GROUP_ADMINSYSNOTIFY)d.Data;
                        __ON_EVENT_GROUP_ADMINSYSNOTIFY.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_GROUP_EXIT":
                    {
                        var x = (ON_EVENT_GROUP_EXIT)d.Data;
                        __ON_EVENT_GROUP_EXIT.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_GROUP_EXIT_SUCC":
                    {
                        var x = (ON_EVENT_GROUP_EXIT_SUCC)d.Data;
                        __ON_EVENT_GROUP_EXIT_SUCC.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_GROUP_JOIN":
                    {
                        var x = (ON_EVENT_GROUP_JOIN)d.Data;
                        __ON_EVENT_GROUP_JOIN.Invoke(d.EMsg, x);
                    }
                    break;
                case "ON_EVENT_FRIEND_ADD":
                    {
                        var x = (ON_EVENT_FRIEND_ADD)d.Data;
                        __ON_EVENT_FRIEND_ADD.Invoke(d.EMsg, x);
                    }
                    break;
                default:
                    {
                        __ON_UNMOUNT_EVENT.Invoke(null, e.Data);
                    }
                    break;
            }
            Log($"{d.EMsg.FromUser}=>{d.EMsg.ToUser} [{d.EMsg.Content}]");
        }
    }
}
