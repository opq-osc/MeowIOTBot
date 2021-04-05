using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot.QQ.QQMessage.QQSendMessage
{
    /// <summary>
    /// 返回状态类
    /// </summary>
    public class SenderStatus 
    {
        /// <summary>
        /// 原消息串
        /// </summary>
        public string MessageStr;
        /// <summary>
        /// 状态码
        /// </summary>
        public Sdstatus StatusCode;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="jo"></param>
        public SenderStatus(JObject jo)
        {
            MessageStr = jo["Msg"]?.ToString();
            StatusCode = (Sdstatus)(jo["Ret"]?.ToObject<int>() ?? -1);
        }
        /// <summary>
        /// 中文输出错误类型
        /// </summary>
        /// <param name="CHS"></param>
        /// <returns></returns>
        public string ToString(bool CHS = false)
        {
            if (CHS)
            {
                return (int)StatusCode switch
                {
                    -1 => "超时空串",
                    0 => "发送成功",
                    34 => "未知错误",
                    46 => "手动完成身份验证",
                    110 => "发送失败,你已被移出群聊",
                    120 => "你已被禁言",
                    241 => "发送频率过高",
                    299 => "超过群发言频率限制",
                    _ => "其他错误"
                };
            }
            else
            {
                return base.ToString();
            }
        }
    }
    /// <summary>
    /// 原信息
    /// </summary>
    public class MsgV2
    {
        /// <summary>
        /// 发送到的位置
        /// </summary>
        public long ToUserUid;
        /// <summary>
        /// 接收人的类型
        /// </summary>
        public int SendToType;
        /// <summary>
        /// 信息的类型
        /// </summary>
        public string SendMsgType;
        /// <summary>
        /// 信息主体
        /// </summary>
        public string Content;
        /// <summary>
        /// 陌生人识别号
        /// </summary>
        public long GroupID;
        /// <summary>
        /// 构造一个原始信息类(内部解析)
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">收件人类型</param>
        /// <param name="sendMsgType">信息类型</param>
        /// <param name="content">信息主体</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2(long sendTo, MessageSendToType sendToType, MessageSendType sendMsgType, string content,
            long GroupId, List<long> atqq = null, bool atAll = false)
        {
            ToUserUid = sendTo;
            SendToType = (int)sendToType;
            SendMsgType = sendMsgType.ToString();
            Content = content;
            if (sendToType == MessageSendToType.Private)
            {
                GroupID = GroupId;
            }
            else if (sendToType == MessageSendToType.Group && (atqq != null || atAll))
            {
                if (atAll == true)
                {
                    Content = $"[ATALL()]{content}";
                }
                else
                {
                    StringBuilder sb = new();
                    sb.Append("[ATUSER(");
                    bool x = false;
                    foreach (var k in atqq)
                    {
                        if (x) { sb.Append(','); }
                        sb.Append(k);
                        x = true;
                    }
                    Content = $"{sb})]{content}";
                }
            }
        }
        /// <summary>
        /// 一个异步的发送信息方法
        /// </summary>
        /// <returns>返回一个成功与否的字符串</returns>
        public async Task<string> Send() =>
            await NetworkHelper.PostHelper.PASA(
                NetworkHelper.PostHelper.UrlType.SendMsgV2,
                JsonConvert.SerializeObject(this, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );
        /// <summary>
        /// 一个异步的发送信息方法
        /// </summary>
        /// <param name="treatAsErr">是否将服务端异常视作错误(默认否)</param>
        /// <returns>返回一个成功与否的类</returns>
        public async Task<SenderStatus> Send(bool treatAsErr = false)
        {
            var ss = new SenderStatus(JObject.Parse(await Send()));
            if (treatAsErr && (ss.StatusCode != Sdstatus.OK))
            {
                throw new Exception($"{ss.ToString(true)}::{ss.StatusCode}::{ss.MessageStr}");
            }
            else
            {
                return ss;
            }
        }
    }
    /// <summary>
    /// 封装的文字信息
    /// </summary>
    public class MsgV2_TxtMsg : MsgV2
    {
        /// <summary>
        /// 文字信息
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">收件人类型</param>
        /// <param name="content">内容</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_TxtMsg(long sendTo, MessageSendToType sendToType, string content, long GroupId = 0, List<long> atqq = null,bool atAll=false)
            : base(sendTo, sendToType, MessageSendType.TextMsg, content, GroupId, atqq, atAll)
        { }
    }
    /// <summary>
    /// 封装的图片信息
    /// </summary>
    public class MsgV2_PicMsg : MsgV2
    {
        /// <summary>
        /// 图像URL
        /// </summary>
        public string PicUrl;
        /// <summary>
        /// 图像地址(服务端)
        /// </summary>
        public string PicPath;
        /// <summary>
        /// 图像的Md5
        /// </summary>
        public string PicMd5s;
        /// <summary>
        /// 图像的Base64类
        /// </summary>
        public string PicBase64Buf;
        /// <summary>
        /// 总体图片构造类
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">信息类型</param>
        /// <param name="content">内容</param>
        /// <param name="picUrl">图片URL</param>
        /// <param name="picPath">图片服务端路径</param>
        /// <param name="picMd5s">图片Md5</param>
        /// <param name="picBase64Buf">图片的Base64</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_PicMsg(long sendTo, MessageSendToType sendToType, string content = null,
            string picUrl = null, string picPath = null, string picMd5s = null, string picBase64Buf = null,
            long GroupId = 0, List<long> atqq = null, bool atAll = false)
            : base(sendTo, sendToType, MessageSendType.PicMsg, content, GroupId, atqq, atAll)
        {
            PicUrl = picUrl;
            PicPath = picPath;
            PicMd5s = picMd5s;
            PicBase64Buf = picBase64Buf;
        }
        /// <summary>
        /// 重写的直接发送bitmap的图片构造
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">信息类型</param>
        /// <param name="content">内容</param>
        /// <param name="b">一个将要使用Base64发送的Bitmap实例</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_PicMsg(long sendTo, MessageSendToType sendToType, Bitmap b, string content = null, 
            long GroupId = 0, List<long> atqq = null, bool atAll = false)
            : base(sendTo, sendToType, MessageSendType.PicMsg, content, GroupId, atqq, atAll)
        {
            if (b != null)
            {
                try
                {
                    using MemoryStream ms = new();
                    b.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arr = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(arr, 0, (int)ms.Length);
                    ms.Close();
                    PicBase64Buf = Convert.ToBase64String(arr);
                }
                catch (Exception ex)
                {
                    throw new Exception($"{EC.E13}:{ex.Message}");
                }
            }
            else
            {
                throw new Exception(EC.E14);
            }
        }
    }
    /// <summary>
    /// 封装的语音信息
    /// </summary>
    public class MsgV2_VocMsg : MsgV2
    {
        /// <summary>
        /// 语音的URL
        /// </summary>
        public string VoiceUrl;
        /// <summary>
        /// 语音的路径
        /// </summary>
        public string VoicePath;
        /// <summary>
        /// 语音信息
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">接收人类型</param>
        /// <param name="voiceUrl">语音URL</param>
        /// <param name="voicePath">语音文件服务端路径</param>
        /// <param name="GroupId">陌生人识别号</param>
        public MsgV2_VocMsg(long sendTo, MessageSendToType sendToType,
            string voiceUrl = null, string voicePath = null,
            long GroupId = 0)
            : base(sendTo, sendToType, MessageSendType.VoiceMsg, null, GroupId, null)
        {
            VoiceUrl = voiceUrl;
            VoicePath = voicePath;
        }
    }
    /// <summary>
    /// 封装的转发信息
    /// </summary>
    public class MsgV2_FwdMsg : MsgV2
    {
        /// <summary>
        /// 转发存档列
        /// </summary>
        public string ForwordBuf;
        /// <summary>
        /// 转发区
        /// </summary>
        public string ForwordField;
        /// <summary>
        /// 转发信息的多定义
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">信息类型</param>
        /// <param name="forwordBuf">框架生成的参数</param>
        /// <param name="forwordField">框架生成的参数</param>
        /// <param name="GroupId">群号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_FwdMsg(long sendTo, MessageSendToType sendToType,
            string forwordBuf, string forwordField,
            long GroupId = 0, List<long> atqq = null, bool atAll = false)
           : base(sendTo, sendToType, MessageSendType.ForwordMsg, null, GroupId, atqq, atAll)
        {
            ForwordBuf = forwordBuf;
            ForwordField = forwordField;
        }
    }
    /// <summary>
    /// 封装的回复信息(仅群聊回复)
    /// </summary>
    public class MsgV2_RplMsg : MsgV2
    {
        /// <summary>
        /// 回复信息内容
        /// </summary>
        public replayInfo ReplayInfo;
        /// <summary>
        /// 回复内容的定义类(内部类)
        /// </summary>
        public class replayInfo
        {
            /// <summary>
            /// 消息序列号
            /// </summary>
            public long MsgSeq;
            /// <summary>
            /// 消息事件
            /// </summary>
            public long MsgTime;
            /// <summary>
            /// 用户ID
            /// </summary>
            public long UserID;
            /// <summary>
            /// 发送的信息
            /// </summary>
            public string RawContent;
            /// <summary>
            /// 构造一个回复内容
            /// </summary>
            /// <param name="msgSeq">消息序列号</param>
            /// <param name="msgTime">消息的随机码</param>
            /// <param name="userID">要回复信息的发送者</param>
            /// <param name="rawContent">内容</param>
            public replayInfo(long msgSeq, long msgTime, long userID, string rawContent)
            {
                MsgSeq = msgSeq;
                MsgTime = msgTime;
                UserID = userID;
                RawContent = rawContent;
            }
        }
        /// <summary>
        /// 回复信息 I
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="replayInfo">一个回复信息内容对象</param>
        public MsgV2_RplMsg(long sendTo, replayInfo replayInfo)
           : base(sendTo, MessageSendToType.Group, MessageSendType.ReplayMsg, null, 0, null)
        {
            ReplayInfo = replayInfo;
        }
        /// <summary>
        /// 回复信息 II
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="msgSeq">消息序列号</param>
        /// <param name="msgTime">消息时间</param>
        /// <param name="userId">用户ID</param>
        /// <param name="rawContent">内容</param>
        public MsgV2_RplMsg(long sendTo, long msgSeq, long msgTime, long userId, string rawContent)
           : base(sendTo, MessageSendToType.Group, MessageSendType.ReplayMsg, null, 0, null)
        {
            ReplayInfo = new replayInfo(msgSeq, msgTime, userId, rawContent);
        }
    }
    /// <summary>
    /// 封装的Json信息
    /// </summary>
    public class MsgV2_JsnMsg : MsgV2
    {
        /// <summary>
        /// Json信息
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">接收人类型</param>
        /// <param name="JsonContent">Json串</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_JsnMsg(long sendTo, MessageSendToType sendToType, string JsonContent,
            long GroupId = 0, List<long> atqq = null, bool atAll = false)
            : base(sendTo, sendToType, MessageSendType.JsonMsg, JsonContent, GroupId, atqq, atAll)
        { }
    }
    /// <summary>
    /// 封装的Xml信息
    /// </summary>
    public class MsgV2_XmlMsg : MsgV2
    {
        /// <summary>
        /// XML信息
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="sendToType">接收人类型</param>
        /// <param name="XmlContent">XML流</param>
        /// <param name="GroupId">陌生人识别号</param>
        /// <param name="atqq">是否atqq 数组qq号 (*暂时群组内不好用)</param>
        /// <param name="atAll">是否At全体 *取消上面的atqq数组 (*暂时群组内不好用)</param>
        public MsgV2_XmlMsg(long sendTo, MessageSendToType sendToType, string XmlContent,
            long GroupId = 0, List<long> atqq = null, bool atAll = false)
            : base(sendTo, sendToType, MessageSendType.XmlMsg, XmlContent, GroupId, atqq, atAll)
        { }
    }

    /// <summary>
    /// 枚举的发送类别
    /// </summary>
    public enum MessageSendToType
    {
        /// <summary>
        /// 好友
        /// </summary>
        Friend = 1,
        /// <summary>
        /// 群
        /// </summary>
        Group = 2,
        /// <summary>
        /// 私聊
        /// </summary>
        Private = 3,
    }
    /// <summary>
    /// 枚举的发送信息类
    /// </summary>
    public enum MessageSendType
    {
        /// <summary>
        /// 文本信息
        /// </summary>
        TextMsg,
        /// <summary>
        /// JSON信息
        /// </summary>
        JsonMsg,
        /// <summary>
        /// XML信息
        /// </summary>
        XmlMsg,
        /// <summary>
        /// 回复信息
        /// </summary>
        ReplayMsg,
        /// <summary>
        /// 特效信息
        /// </summary>
        TeXiaoTextMsg,
        /// <summary>
        /// 图片信息
        /// </summary>
        PicMsg,
        /// <summary>
        /// 语音信息
        /// </summary>
        VoiceMsg,
        /// <summary>
        /// ???
        /// </summary>
        PhoneMsg,
        /// <summary>
        /// 转发信息
        /// </summary>
        ForwordMsg
    }
    /// <summary>
    /// 枚举的发送返回逻辑
    /// </summary>
    public enum Sdstatus
    {
        /// <summary>
        /// 空串
        /// </summary>
        NULLREF = -1,
        /// <summary>
        /// 发送成功
        /// </summary>
        OK = 0,
        /// <summary>
        /// 未知错误
        /// </summary>
        err_STB = 34,
        /// <summary>
        /// 手动完成身份验证
        /// </summary>
        err_Auth = 46,
        /// <summary>
        /// 发送失败,你已被移出群聊
        /// </summary>
        err_GroupOut = 110,
        /// <summary>
        /// 你已被禁言
        /// </summary>
        err_Shut = 120,
        /// <summary>
        /// 发送频率过高
        /// </summary>
        err_Freq = 241,
        /// <summary>
        /// 超过群发言频率限制
        /// </summary>
        err_GroupFreq = 299
    }
}
