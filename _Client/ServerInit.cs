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
        /// <param name="url">
        /// 你的IOT连接位置
        /// <para>IOT backend</para>
        /// </param>
        /// <param name="qq">
        /// 你的QQ号
        /// <para>qq number</para>
        /// </param>
        /// <param name="logflag">
        /// 一个是否记录日志的标志(默认关闭)
        /// <para>an entryset that if you want a log * (usually not)</para>
        /// </param>
        public MeowServiceClient(string url, string qq, bool logflag = false) : base(url, qq, logflag) { }
        /// <summary>
        /// 一个自枚举的多功能解释端
        /// <para>an Enumable multi-purpose explain backend</para>
        /// </summary>
        public MeowServiceClient CreateClient()
        {
            var meow = new MeowClient(url, qq, logFlag).Connect();
            meow.OnServerAction += (s, e) => { };//默认失去作用的
            meow.OnGroupMsgs += Meow_OnGroupMsgs;
            meow.OnEventMsgs += Meow_OnEventMsgs;
            meow.OnFriendMsgs += Meow_OnFriendMsgs;
            return this;
        }
        /// <summary>
        /// 日志输出
        /// </summary>
        /// <param name="s">字串</param>
        /// <param name="Fore">前景色</param>
        /// <param name="Back">背景色</param>
        private static void Log(string s, ConsoleColor Fore = ConsoleColor.White, ConsoleColor Back = ConsoleColor.Black)
        {
            if (logFlag)
            {
                Console.ForegroundColor = Fore;
                Console.BackgroundColor = Back;
                Console.WriteLine($"{DateTime.Now} : : {s}");
                Console.ResetColor();
            }
        }
    }
}
