using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot
{
    /// <summary>
    /// 用于枚举错误类型
    /// <para>for Enum the Exception Type</para>
    /// </summary>
    public sealed class EC
    {
        public readonly static string E00 = "Json事件缺少 [EventMsg] ,无法初始化事件";
        public readonly static string E01 = "Json事件缺少 [EventName] , 无法确定类型";
        public readonly static string E02 = "Json事件缺少 [EventData] , 没有内部初始化";
        public readonly static string E11 = "同意类型缺失,没有参数呈递";

    }
}
