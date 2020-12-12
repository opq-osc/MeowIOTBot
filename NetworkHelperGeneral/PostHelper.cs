using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot.NetworkHelper
{
    /// <summary>
    /// 一个用于和IOT交流的PostHelper
    /// <para>an IOT PostHelper for user delegate</para>
    /// </summary>
    public class PostHelper : HttpHelper
    {
        /// <summary>
        /// POST的URL地址
        /// <para>for POST URL Position</para>
        /// </summary>
        public string CallerUrl { get; }
        /// <summary>
        /// 登录QQ
        /// <para>Login QQ number</para>
        /// </summary>
        public string LoginQQ { get; }
        /// <summary>
        /// 超时设置(可以进行动态设置,默认是10s)
        /// <para>setting for Timeout (*which COULD be dynamic and init with 10s)</para>
        /// </summary>
        public int Timeout { get; set; } = 10;
        /// <summary>
        /// 构造函数
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="callerUrl">
        /// POST的URL地址
        /// <para>for POST URL Position</para>
        /// </param>
        /// <param name="loginQQ">
        /// 登录QQ
        /// <para>Login QQ number</para>
        /// </param>
        /// <param name="timeout">
        /// 超时设置(可以进行动态设置,默认是10s)
        /// <para>setting for Timeout (*which COULD be dynamic and init with 10s)</para>
        /// </param>
        /// <param name="ua">
        /// Nginx 设置的头检测
        /// <para>HeaderCheck for linux Nginx</para>
        /// </param>
        /// <param name="ContentType">
        /// 内容默认模式
        /// <para>ContentType</para>
        /// </param>
        public PostHelper(
            string callerUrl, string loginQQ, int timeout = 10,
            WebHeaderCollection ua = null, string ContentType = "application/json")
        {
            Header = ua;
            contentType = ContentType;
            CallerUrl = callerUrl;
            LoginQQ = loginQQ;
            Timeout = timeout;
        }
        /// <summary>
        /// 准备一个发送请求
        /// <para>to prepare an Oredr to Server</para>
        /// <para>支持连写用法 见下文 (*Support for Connection Write see below) </para>
        /// <code>
        /// await new PostHelper(para).PrepareSend(para).PostData(para);
        /// </code>
        /// </summary>
        /// <param name="urlType">
        /// 要发送的"指令"类型
        /// <para>the Command Type you want to Send</para>
        /// </param>
        /// <param name="Nginx">
        /// 是否使用了Nginx
        /// <para>is Nginx Enabled</para>
        /// </param>
        /// <returns>
        /// 一个PostHelper实例
        /// <para>a PostHelper Object</para>
        /// </returns>
        public PostHelper PrepareSend(UrlType urlType)
        {
            Url = urlType switch
            {
                UrlType.init => throw (new Exception("Initialization is done by Server!")),
                UrlType.ClusterInfo => $"{CallerUrl}/v1/ClusterInfo",
                UrlType.Announce => $"{CallerUrl}/v1/Group/Announce?qq={LoginQQ}",
                //UrlType.SendMsgV2 => $"{CallerUrl}/v2/LuaApiCaller?qq={LoginQQ}&funcname={urlType}&timeout={Timeout}",
                _ => $"{CallerUrl}/v1/LuaApiCaller?qq={LoginQQ}&funcname={urlType}&timeout={Timeout}",
            };
            return this;
        }
        /// <summary>
        /// 发送的类型(不断更新)
        /// <para>an type that used to PostAction </para>
        /// </summary>
        public enum UrlType
        {
            /// <summary>
            /// 集群信息
            /// <para>get clusterinfo</para>
            /// </summary>
            ClusterInfo,
            /// <summary>
            /// 发送群公告
            /// <para>send Announce</para>
            /// </summary>
            Announce,
            /// <summary>
            /// 自动初始化 (大多数时间服务器已经自动初始化了)
            /// <para>initialization (Most of the time is Done by server backend)</para>
            /// </summary>
            init,
            /// <summary>
            /// 发送信息(弃用)
            /// <para>send Message which is already Obselete</para>
            /// </summary>
            [Obsolete]
            SendMsg,
            /// <summary>
            /// 新的发送信息接口
            /// <para>Version2 SendMsg</para>
            /// </summary>
            SendMsgV2,
            /// <summary>
            /// 获取好友列表
            /// <para>get FirendList</para>
            /// </summary>
            GetQQUserList,
            /// <summary>
            /// 搜索QQ群
            /// <para>search for QQGroup</para>
            /// </summary>
            SearchGroup,
            /// <summary>
            /// 获取群列表
            /// <para>Get Group List</para>
            /// </summary>
            GetGroupList,
            /// <summary>
            /// 获取群成员列表
            /// <para>get GroupUserList</para>
            /// </summary>
            GetGroupUserList,
            /// <summary>
            /// 发送赞
            /// <para>send QQZan to someone</para>
            /// </summary>
            QQZan,
            /// <summary>
            /// 添加好友
            /// <para>Add some QQUser</para>
            /// </summary>
            AddQQUser,
            /// <summary>
            /// 撤回信息
            /// <para>Revoke Message</para>
            /// </summary>
            RevokeMsg,
            /// <summary>
            /// 禁言(接口调用注意)
            /// <para>Shut somebody (Please note This COULD have othe affect)</para>
            /// </summary>
            ShutUp,
            /// <summary>
            /// 登出
            /// <para>LogOut Some QQ</para>
            /// </summary>
            LogOut,
            /// <summary>
            /// 处理好友请求
            /// <para>Dealwith Friend Request</para>
            /// </summary>
            DealFriend,
            /// <summary>
            /// 处理群请求
            /// <para>returnAnswerFromInvite</para>
            /// </summary>
            AnswerInviteGroup,
            /// <summary>
            /// 更改群名片
            /// <para>to Change GroupID</para>
            /// </summary>
            ModifyGroupCard,
            /// <summary>
            /// 设置头衔
            /// <para>set Title</para>
            /// </summary>
            SetUniqueTitle,
            /// <summary>
            /// 获取任意用户信息
            /// <para>getUserInfo</para>
            /// </summary>
            GetUserInfo,
            /// <summary>
            /// 获取当前用户相关Cookie
            /// <para>get User Cookies</para>
            /// </summary>
            GetUserCook,
            /// <summary>
            /// 加群退群管理
            /// <para>add in or leave Group Control</para>
            /// </summary>
            GroupMgr
        }
    }
}
