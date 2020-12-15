using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeowIOTBot.QQ.QQMessage.QQRecieveMessage
{
    /// <summary>
    /// QQ信息的包装类
    /// <para>QQ Message's Class</para>
    /// </summary>
    /// <typeparam name="MsgBody">
    /// 信息类型
    /// <para>Message Type</para>
    /// </typeparam>
    public class QQRecieveMessage
    {
        /// <summary>
        /// 当前服务端登录的QQ
        /// <para>Service login QQ</para>
        /// </summary>
        public long CurrentQQ;
        /// <summary>
        /// IO类型
        /// <para>IO inner type</para>
        /// </summary>
        public MessageIOType IOType;
        /// <summary>
        /// 信息的IO附加参数
        /// <para>Message IO Body Parameter</para>
        /// </summary>
        public IOBody IOBody;
        /// <summary>
        /// 信息的公用属性类
        /// <para>Message Public Properties</para>
        /// </summary>
        public MsgProperty Property;
        /// <summary>
        /// 构造参数
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="iOType">
        /// IO类型
        /// <para>IO inner type</para>
        /// </param>
        /// <param name="iOBody">
        /// 信息的IO附加参数
        /// <para>Message IO Body Parameter</para>
        /// </param>
        /// <param name="body">
        /// 消息体
        /// <para>Message Body</para>
        /// </param>
        /// <param name="property">
        /// 信息的公用属性类
        /// <para>Message Public Properties</para>
        /// </param>
        /// <param name="currentQQ">
        /// 当前服务端登录的QQ
        /// <para>Service login QQ</para>
        /// </param>
        public QQRecieveMessage(MessageIOType iOType, IOBody iOBody, MsgProperty property, long currentQQ)
        {
            IOType = iOType;
            IOBody = iOBody;
            Property = property;
            CurrentQQ = currentQQ;
        }
    }
    /// <summary>
    /// 信息的IO附加参数
    /// <para>Message IO Body Parameter</para>
    /// </summary>
    public class IOBody
    {
        /// <summary>
        /// 发信人
        /// <para>sender QQ</para>
        /// </summary>
        public long MsgFromQQ { get; }
        /// <summary>
        /// 收信人
        /// <para>Reciever QQ</para>
        /// </summary>
        public long MsgRecvQQ { get; }
        /// <summary>
        /// 发信人昵称
        /// <para>Sender NickName</para>
        /// </summary>
        public string NickName { get; }
        /// <summary>
        /// 群聊的群号 (可空类型)
        /// <para>GroupChat's Group Number (as it can be Nullable)</para>
        /// </summary>
        public long? FromGroupId { get; }
        /// <summary>
        /// 群聊的群名 (可空类型)
        /// <para>GroupChat's Group Name (as it can be Nullable)</para>
        /// </summary>
        public string FromGroupName { get; }
        /// <summary>
        /// 构造
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="msgFromQQ">
        /// 发信人
        /// <para>sender QQ</para>
        /// </param>
        /// <param name="msgRecvQQ">
        /// 收信人
        /// <para>Reciever QQ</para>
        /// </param>
        /// <param name="nickName">
        /// 发信人昵称
        /// <para>Sender NickName</para>
        /// </param>
        /// <param name="fromGroupId">
        /// 群聊的群号 (可空类型)
        /// <para>GroupChat's Group Number (as it can be Nullable)</para> 
        /// </param>
        /// <param name="fromGroupName">
        /// 群聊的群名 (可空类型)
        /// <para>GroupChat's Group Name (as it can be Nullable)</para>
        /// </param>
        public IOBody(long msgFromQQ, long msgRecvQQ, string nickName, long? fromGroupId, string fromGroupName)
        {
            MsgFromQQ = msgFromQQ;
            MsgRecvQQ = msgRecvQQ;
            NickName = nickName;
            FromGroupId = fromGroupId;
            FromGroupName = fromGroupName;
        }
    }
    /// <summary>
    /// 信息的公用属性类
    /// <para>Message Public Properties</para>
    /// </summary>
    public class MsgProperty
    {
        /// <summary>
        /// 附属的第二指令
        /// <para>Parametered Second Command Value</para>
        /// </summary>
        public string C2cCmd { get; }
        /// <summary>
        /// 消息的验证时间
        /// <para>Message Sender Time</para>
        /// </summary>
        public long? MsgTime { get; }
        /// <summary>
        /// 消息序列号
        /// <para>Message Sequence Data</para>
        /// </summary>
        public long? MsgSeq { get; }
        /// <summary>
        /// 消息的随机码
        /// <para>Message Random Identify Data</para>
        /// </summary>
        public long? MsgRandom { get; }
        /// <summary>
        /// 消息的UID
        /// <para>Message Uid</para>
        /// </summary>
        public long? MsgUid { get; }
        /// <summary>
        /// 红包信息
        /// <para>Redbag Infomation string</para>
        /// </summary>
        public string RedBagInfo { get; }
        /// <summary>
        /// 构造方法
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="c2cCmd">
        /// 附属的第二指令
        /// <para>Parametered Second Command Value</para>
        /// </param>
        /// <param name="msgSeq">
        /// 消息序列号
        /// <para>Message Sequence Data</para>
        /// </param>
        /// <param name="msgRandom">
        /// 消息的随机码
        /// <para>Message Random Identify Data</para> 
        /// </param>
        /// <param name="msgUid">
        /// 消息的UID
        /// <para>Message Uid</para> 
        /// </param>
        /// <param name="redBagInfo">
        /// 红包信息
        /// <para>Redbag Infomation string</para>
        /// </param>
        /// <param name="msgTime">
        /// 消息的验证时间
        /// <para>Message Sender Time</para>
        /// </param>
        public MsgProperty(string c2cCmd, long? msgSeq, long? msgRandom, long? msgUid, string redBagInfo, long? msgTime)
        {
            C2cCmd = c2cCmd;
            MsgSeq = msgSeq;
            MsgRandom = msgRandom;
            MsgUid = msgUid;
            RedBagInfo = redBagInfo;
            MsgTime = msgTime;
        }
    }
    /// <summary>
    /// 消息类型
    /// <para>MessageIOType</para>
    /// </summary>
    public enum MessageIOType
    {
        /// <summary>
        /// 好友信息
        /// <para>Friend Message</para>
        /// </summary>
        Friend,
        /// <summary>
        /// 群信息
        /// <para>Group Message</para>
        /// </summary>
        Group,
        /// <summary>
        /// 临时信息
        /// <para>Temp Session Message</para>
        /// </summary>
        TempSession
    }


    /// <summary>
    /// 基础消息格式(抽象继承模式)
    /// <para>Message Type * Abstract Inherit Mode</para>
    /// </summary>
    public abstract class Message
    {
        /// <summary>
        /// 信息的基础内容
        /// <para>Message Basic Content</para>
        /// </summary>
        public JObject RawContent;
        /// <summary>
        /// 构造基础的信息模型
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="content">
        /// 信息的基础内容
        /// <para>Message Basic Content</para>
        /// </param>
        protected Message(string content) => RawContent = JObject.Parse(content);
    }
    /// <summary>
    /// 信息类型 : 文本信息
    /// <para>Message Type : Text Message</para>
    /// </summary>
    public class TextMessage : Message
    {
        /// <summary>
        /// 文本信息
        /// <para>Text Message</para>
        /// </summary>
        public string Content;
        /// <summary>
        /// 构造信息类型 : 文本信息
        /// <para>Message Constructor Type : Text Message</para>
        /// </summary>
        /// <param name="content">
        /// 信息的基础内容
        /// <para>Message Basic Content</para>
        /// </param>
        public TextMessage(string content) : base($"{{\"Content\":\"{content}\"}}") => Content = content;
    }
    /// <summary>
    /// at类型的消息 * 仅群聊
    /// <para>Type Of the message [@] * maybe only in Group Chat</para>
    /// </summary>
    public abstract class AtTextMessage : Message
    {
        /// <summary>
        /// 文本信息
        /// <para>Text Message</para>
        /// </summary>
        public string Content;
        /// <summary>
        /// 构造的
        /// </summary>
        /// <param name="content"></param>
        public AtTextMessage(string content) : base(content)
        {
            Content = Regex.Replace(content, @"@[\S]+[\s]", "").Trim();
        }
    }
}
