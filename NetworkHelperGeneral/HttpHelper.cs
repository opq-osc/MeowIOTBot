using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot.NetworkHelper
{
    /// <summary>
    /// 特征网络包发送类
    /// <para>an Interface (actually class) for PostData to Server</para>
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// 连接节点
        /// <para>Backend Point</para>
        /// </summary>
        public string Url;
        /// <summary>
        /// 内容默认模式
        /// <para>ContentType</para>
        /// </summary>
        public string contentType;
        /// <summary>
        /// Nginx 设置的头检测
        /// <para>HeaderCheck for linux Nginx</para>
        /// </summary>
        public WebHeaderCollection Header = null;
        /// <summary>
        /// 继承用空参构造
        /// <para>empty-constructor for inherit</para>
        /// </summary>
        public HttpHelper() { }
        /// <summary>
        /// 外源用构造
        /// <para>constructor for Extern Usage</para>
        /// </summary>
        /// <param name="url">
        /// 要连接的URL
        /// <para>the url you wanna to get or post</para>
        /// </param>
        /// <param name="contentType">
        /// 内容格式 (多数是Json)
        /// <para>a Type to specific the content Usage : mostly is <code>"application/json"</code></para>
        /// </param>
        public HttpHelper(string url, string contentType)
        {
            Url = url;
            this.contentType = contentType;
        }
        /// <summary>
        /// 构造函数
        /// <para>Constructor</para>
        /// </summary>
        /// <param name="url">
        /// 连接节点 
        /// <para>Backend Point</para>
        /// </param>
        /// <param name="contentType">
        /// 内容格式 
        /// <para>Usually is Json</para>
        /// </param>
        /// <param name="header">
        /// UA识别头 
        /// <para>HeaderCheck for Linux Nginx</para>
        /// </param>
        public HttpHelper(string url, string contentType, WebHeaderCollection header = null)
        {
            Url = url;
            Header = header;
            this.contentType = contentType;
        }
        /// <summary>
        /// 使用连接获取操作
        /// <para>using Connector to get</para>
        /// </summary>
        /// <returns>
        /// 获得的字符
        /// <para>Strings for Response</para>
        /// </returns>
        public async Task<string> HttpGet()
        {
            try
            {
                string retString = string.Empty;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.ContentType = contentType;
                HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync(); //响应结果
                Stream myResponseStream = response.GetResponseStream();
                StreamReader streamReader = new(myResponseStream);
                retString = streamReader.ReadToEnd();
                streamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// 使用连接发送数据
        /// <para>to Send a Post Connection</para>
        /// </summary>
        /// <param name="Content">
        /// 要发送的数据
        /// <para>Content which waiting to send a POST Action</para>
        /// </param>
        /// <returns>
        /// 操作返回的字符
        /// <para>Action return string</para>
        /// </returns>
        public async Task<string> HttpPost(string Content)
        {
            try
            {
                string result;
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "POST";
                req.ContentType = contentType;
                req.Headers = Header;
                byte[] data = Encoding.UTF8.GetBytes(Content);//把字符串转换为字节

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
    }
}
