using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot
{
    /// <summary>
    /// 服务端日志标准
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// 无日志
        /// </summary>
        None = 0,
        /// <summary>
        /// 客户端信息流
        /// </summary>
        ClientMessage = 1,
        /// <summary>
        /// 客户端事件流
        /// </summary>
        ClientVerbose = 2,
        /// <summary>
        /// 服务端信息流
        /// </summary>
        ServerMessage = 3,
        /// <summary>
        /// 完整日志
        /// </summary>
        Verbose = 4
    }
    /// <summary>
    /// 服务端工具包
    /// </summary>
    public class ServerUtil
    {
        /// <summary>
        /// 服务器日志
        /// </summary>
        /// <param name="s">记录</param>
        /// <param name="l">类型</param>
        /// <param name="Fore">前景色</param>
        /// <param name="Back">背景色</param>
        public static void Log(string s, LogType l, ConsoleColor Fore = ConsoleColor.White, ConsoleColor Back = ConsoleColor.Black)
        {
            if ((int)Basex.MeowClient.logFlag >= (int)l)
            {
                Console.ForegroundColor = Fore;
                Console.BackgroundColor = Back;
                Console.WriteLine($"{DateTime.Now} : : {s}");
                Console.ResetColor();
            }
        }
    }
}
