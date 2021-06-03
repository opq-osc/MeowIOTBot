using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot.NetworkHelper
{
    /// <summary>
    /// 一个用于和IOT交流的PostHelper
    /// <para>an IOT PostHelper for user delegate</para>
    /// </summary>
    public class PostHelper
    {
        /// <summary>
        /// POST的URL地址
        /// <para>for POST URL Position</para>
        /// </summary>
        public static string CallerUrl { get; set; }
        /// <summary>
        /// 登录QQ
        /// <para>Login QQ number</para>
        /// </summary>
        public static string LoginQQ { get; set; }
        /// <summary>
        /// 超时设置(可以进行动态设置,默认是30s)
        /// <para>setting for Timeout (*which COULD be dynamic and init with 30s)</para>
        /// </summary>
        public static int Timeout { get; set; } = 30;
        /// <summary>
        /// 一个Nginx的Header识别标
        /// <para>A nginx Header Surffix</para>
        /// </summary>
        public static WebHeaderCollection Header { get; set; }
        /// <summary>
        /// 准备并且发送一个请求
        /// <para>Prepare And Send Async (PASA)</para>
        /// </summary>
        /// <param name="urlType">
        /// 要发送的"指令"类型
        /// <para>the Command Type you want to Send</para>
        /// </param>
        /// <param name="Json">
        /// 要发送的Json集
        /// </param>
        /// <returns></returns>
        public static async Task<string> PASA(UrlType urlType, string Json) {
            try
            {
                var Url = urlType switch
                {
                    UrlType.init => throw (new Exception("Initialization is done by Server!")),
                    UrlType.ClusterInfo => $"{CallerUrl}/v1/ClusterInfo",
                    UrlType.Announce => $"{CallerUrl}/v1/Group/Announce?qq={LoginQQ}",
                    UrlType.LoginQQ => $"{CallerUrl}/v1/Login/GetQRcode",
                    UrlType.__RefreshKeys => $"{CallerUrl}/v1/RefreshKeys?qq={LoginQQ}",
                    UrlType.__Logs => $"{CallerUrl}/v1/Log",
                    UrlType.ShutUpSingle => $"{CallerUrl}/v1/LuaApiCaller?qq={LoginQQ}&funcname=OidbSvc.0x570_8&timeout={Timeout}",
                    UrlType.Tickles => $"{CallerUrl}/v1/LuaApiCaller?qq={LoginQQ}&funcname=OidbSvc.0xed3_1&timeout={Timeout}",
                    UrlType.ShutUpEntirely => $"{CallerUrl}/v1/LuaApiCaller?qq={LoginQQ}&funcname=OidbSvc.0x89a_0&timeout={Timeout}",
                    //UrlType.SendMsgV2 => $"{CallerUrl}/v2/LuaApiCaller?qq={LoginQQ}&funcname={urlType}&timeout={Timeout}",
                    _ => $"{CallerUrl}/v1/LuaApiCaller?qq={LoginQQ}&funcname={urlType}&timeout={Timeout}",
                };

                string result;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = "application/json";
                req.Headers = Header??new WebHeaderCollection();
                byte[] data = Encoding.UTF8.GetBytes(Json);//把字符串转换为字节

                req.ContentLength = data.Length; //请求长度

                using (Stream reqStream = req.GetRequestStream()) //获取
                {
                    reqStream.Write(data, 0, data.Length);//向当前流中写入字节
                    reqStream.Close(); //关闭当前流
                }
                HttpWebResponse resp = (HttpWebResponse)await req.GetResponseAsync(); //响应结果
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 发送的类型(不断更新)
        /// <para>an type that used to PostAction </para>
        /// </summary>
        public enum UrlType
        {
            /// <summary>
            /// 全局日志
            /// </summary>
            __Logs,
            /// <summary>
            /// 刷新Key重新登录
            /// </summary>
            __RefreshKeys,
            /// <summary>
            /// 登录QQ
            /// <para>get one QQ Login</para>
            /// </summary>
            LoginQQ,
            /// <summary>
            /// 登出
            /// <para>LogOut Some QQ</para>
            /// </summary>
            LogOut,
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
            [Obsolete("已弃用", false)]
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
            /// 禁言某人
            /// <para>Shut somebody (Please note This COULD have othe affect)</para>
            /// </summary>
            ShutUpSingle,
            /// <summary>
            /// 禁言整个群组
            /// <para>Shut Whole Group</para>
            /// </summary>
            ShutUpEntirely,
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
            GroupMgr,
            /// <summary>
            /// 戳一戳
            /// <para>repersent the Tickle action</para>
            /// </summary>
            Tickles
        }
    }
}
