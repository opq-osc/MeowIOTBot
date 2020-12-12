using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeowIOTBot.ObjectEvent
{
    /// <summary>
    /// 对象化事件
    /// <para>Objective EventArgs</para>
    /// </summary>
    public class ObjectEventArgs : EventArgs
    {
        /// <summary>
        /// 构造函数
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="raw">
        /// 服务端的原始数据 (需要Newtonsoft解析)
        /// <para>Serveric raw data(which needs Newtonsoft Expeclict)</para>
        /// </param>
        public ObjectEventArgs(JObject raw)
        {
            Data = raw["CurrentPacket"]["Data"].ToObject<JObject>();
            CurrentQQ = raw["CurrentQQ"].ToObject<long>();
            WebConnId = raw["CurrentPacket"]["WebConnId"].ToString();
        }
        /// <summary>
        /// 重写的服务端Data字段
        /// <para>Rewrite Data from Server</para>
        /// </summary>
        public JObject Data { get; }
        /// <summary>
        /// 重写的CurrentQQ字段
        /// <para>Rewrite 'currentQQ' from server</para>
        /// </summary>
        public long CurrentQQ { get; }
        /// <summary>
        /// 重写的WebConnId字段
        /// <para>Rewrite 'WebConnId' from server</para>
        /// </summary>
        public string WebConnId { get; }
    }
}
