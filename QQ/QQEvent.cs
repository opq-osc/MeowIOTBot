using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeowIOTBot.QQ.QQEvent
{
    /// <summary>
    /// 群通知大类
    /// </summary>
    public class ON_EVENT_GROUP_ADMINSYSNOTIFY
    {
        public long Seq;
        public long Type;
        public string MsgTypeStr;
        public long Who;
        public string WhoName;
        public string MsgStatusStr;
        public string Content;
        public string RefuseContent;
        public long Flag_7;
        public long Flag_8;
        public long GroupId;
        public string GroupName;
        public long ActionUin;
        public string ActionName;
        public string ActionGroupCard;
        public long Action;
    }
    /// <summary>
    /// 管理员变更事件
    /// </summary>
    public class ON_EVENT_GROUP_ADMIN
    {
        public long Flag;
        public long GroupID;
        public long UserID;
    }
    /// <summary>
    /// 群禁言事件
    /// </summary>
    public class ON_EVENT_GROUP_SHUT
    {
        public long GroupID;
        public long ShutTime;
        public long UserID;
    }
    /// <summary>
    /// 群成员退出群聊事件
    /// </summary>
    public class ON_EVENT_GROUP_EXIT
    {
        public long UserID;
    }
    /// <summary>
    /// 主动退群成功事件
    /// </summary>
    public class ON_EVENT_GROUP_EXIT_SUCC
    {
        public long GroupID;
    }
    /// <summary>
    /// 某人进群事件
    /// </summary>
    public class ON_EVENT_GROUP_JOIN
    {
        public long InviteUin;
        public long UserID;
        public string UserName;
    }

    /// <summary>
    /// 事件类型枚举类
    /// <para>EventType EnumClass</para>
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 还没识别的事件类型
        /// </summary>
        ON_EVENT_NULL_REF,
        /// <summary>
        /// 管理员设置事件
        /// </summary>
        ON_EVENT_GROUP_ADMINSYSNOTIFY,
        /// <summary>
        /// 管理员变更事件
        /// </summary>
        ON_EVENT_GROUP_ADMIN,
        /// <summary>
        /// 群禁言
        /// </summary>
        ON_EVENT_GROUP_SHUT,
        /// <summary>
        /// 群成员退出群聊事件
        /// </summary>
        ON_EVENT_GROUP_EXIT,
        /// <summary>
        /// 主动退群成功事件
        /// </summary>
        ON_EVENT_GROUP_EXIT_SUCC,
        /// <summary>
        /// 某人进群事件
        /// </summary>
        ON_EVENT_GROUP_JOIN
    }
    /// <summary>
    /// 事件的摘要
    /// <para>Event Hash</para>
    /// </summary>
    public class EventMsg
    {
        /// <summary>
        /// 构造用
        /// </summary>
        /// <param name="jo"></param>
        public EventMsg(JObject jo)
        {
            if (jo.TryGetValue("EventMsg", out var EventMsg))
            {
                var em = JObject.Parse(EventMsg.ToString());
                em.TryGetValue("FromUin", out var FromUin);
                this.FromUser = FromUin?.ToObject<long>() ?? 0;
                em.TryGetValue("ToUin", out var ToUin);
                this.ToUser = ToUin?.ToObject<long>() ?? 0;
                em.TryGetValue("MsgType", out var MsgType);
                this.MsgType = MsgType?.ToString();
                em.TryGetValue("MsgSeq", out var MsgSeq);
                this.MsgSeq = MsgSeq?.ToObject<long>() ?? 0;
                em.TryGetValue("Content", out var Content);
                this.Content = Content?.ToString();
            }
            else { throw new Exception(EC.E00); }
        }
        /// <summary>
        /// 其他数据(扩展类)
        /// <para>eXtensionClass - for InnerData</para>
        /// </summary>
        public object Data { get; } = null;
        /// <summary>
        /// 来源用户
        /// <para>For The Msg From</para>
        /// </summary>
        public long FromUser { get; }
        /// <summary>
        /// 接收用户
        /// <para>For The Msg To</para>
        /// </summary>
        public long ToUser { get; }
        /// <summary>
        /// 信息类型
        /// <para>MsginnerType</para>
        /// </summary>
        public string MsgType { get; }
        /// <summary>
        /// 消息序列号
        /// <para>Msg Sequence</para>
        /// </summary>
        public long MsgSeq { get; }
        /// <summary>
        /// 信息内容
        /// <para>Msg Innerexcption</para>
        /// </summary>
        public string Content { get; }
    }
    /// <summary>
    /// 继承用事件内部数据类
    /// <para>for inherit classes (which is EventData-Typed)</para>
    /// </summary>
    public class Event
    {
        /// <summary>
        /// 构造用
        /// </summary>
        /// <param name="jo"></param>
        public Event(JObject jo)
        {
            EMsg = new EventMsg(jo);
            if (jo.TryGetValue("EventName", out var EventName))
            {
                var en = EventName.ToString();
                this.EType = en switch
                {
                    "ON_EVENT_GROUP_ADMIN" => EventType.ON_EVENT_GROUP_ADMIN,
                    "ON_EVENT_GROUP_ADMINSYSNOTIFY" => EventType.ON_EVENT_GROUP_ADMINSYSNOTIFY,
                    "ON_EVENT_GROUP_SHUT" => EventType.ON_EVENT_GROUP_SHUT,
                    "ON_EVENT_GROUP_EXIT" => EventType.ON_EVENT_GROUP_EXIT,
                    "ON_EVENT_GROUP_EXIT_SUCC" => EventType.ON_EVENT_GROUP_EXIT_SUCC,
                    "ON_EVENT_GROUP_JOIN" => EventType.ON_EVENT_GROUP_JOIN,
                    _ => EventType.ON_EVENT_NULL_REF
                };
            }
            else { throw new Exception(EC.E01); }
            if (jo.TryGetValue("EventData", out var EventData))
            {
                JObject jed = JObject.Parse(EventData.ToString());
                this.Data = EventName.ToString() switch
                {
                    "ON_EVENT_GROUP_ADMIN" => jed.ToObject<ON_EVENT_GROUP_ADMIN>(),
                    "ON_EVENT_GROUP_ADMINSYSNOTIFY" => jed.ToObject<ON_EVENT_GROUP_ADMINSYSNOTIFY>(),
                    "ON_EVENT_GROUP_SHUT" => jed.ToObject<ON_EVENT_GROUP_SHUT>(),
                    "ON_EVENT_GROUP_EXIT" => jed.ToObject<ON_EVENT_GROUP_EXIT>(),
                    "ON_EVENT_GROUP_EXIT_SUCC" => jed.ToObject<ON_EVENT_GROUP_EXIT_SUCC>(),
                    "ON_EVENT_GROUP_JOIN" => jed.ToObject<ON_EVENT_GROUP_JOIN>(),
                    _ => null
                };
            }
            else { throw new Exception(EC.E02); }
        }
        /// <summary>
        /// 枚举类型(用于转换下面的InnerData)
        /// <para>inner Class For Cast InnerData</para>
        /// </summary>
        public EventType EType { get; } = EventType.ON_EVENT_NULL_REF;
        /// <summary>
        /// 事件的摘要信息
        /// <para>Event Hash</para>
        /// </summary>
        public EventMsg EMsg { get; } = null;
        /// <summary>
        /// 事件的内部转换信息
        /// <para>inner DataCast -> Object</para>
        /// </summary>
        public object Data { get; }
    }
}
