using MeowIOTBot.Basex;
using System;

namespace MeowIOTBot
{
    /// <summary>
    /// 带有详细回调的端
    /// <para>Client With a detailed delegate</para>
    /// </summary>
    public sealed partial class MeowServiceClient : MeowClient
    {
        /// <summary>
        /// 一个自枚举的多功能解释端
        /// <para>an Enumable multi-purpose explain backend</para>
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
        public MeowServiceClient(string url, LogType logflag = LogType.None,
            double ReconnectInterval = 30, bool enableForceReconnection = false,
            long connectionTimedOutTick = 10000, int reconnectionDelay = 1,
            int reconnectionDelayMax = 10, int eIO = 3,
            bool reconnection = true, bool allowedRetryFirstConnection = true) :
            base(url, logflag,ReconnectInterval,enableForceReconnection,
             connectionTimedOutTick,reconnectionDelay,reconnectionDelayMax,eIO,
             reconnection,allowedRetryFirstConnection) { }
        /// <summary>
        /// 一个自枚举的多功能解释端
        /// <para>an Enumable multi-purpose explain backend</para>
        /// </summary>
        public MeowServiceClient CreateClient()
        {
            var meow = new MeowClient(url, logFlag).Connect();
            meow.OnServerAction += (s, e) => { };//默认失去作用的
            meow.OnGroupMsgs += Meow_OnGroupMsgs;
            meow.OnEventMsgs += Meow_OnEventMsgs;
            meow.OnFriendMsgs += Meow_OnFriendMsgs;
            return this;
        }
    }
}
