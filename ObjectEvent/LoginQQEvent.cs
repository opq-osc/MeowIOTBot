using MeowIOTBot.NetworkHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowIOTBot
{
    /// <summary>
    /// 框架登录处理
    /// <para>Interfaceable QQ Login</para>
    /// </summary>
    public class LoginQQ
    {
        /// <summary>
        /// 登录接口
        /// <para>Login Interface</para>
        /// </summary>
        /// <param name="imgHeight">
        /// 打印时控制台的高度(建议50),过大可能导致分页
        /// <para>console output selected height -by default is 50 (be aware if too large may cause to split-page)</para>
        /// </param>
        /// <param name="imgWidth">
        /// 打印时控制台的宽度(建议50),过大可能导致分页
        /// <para>console output selected width -by default is 50 (be aware if too large may cause to split-page)</para>
        /// </param>
        public static async void Login(int imgHeight = 50, int imgWidth = 50)
        {
            string d = await PostHelper.PASA(PostHelper.UrlType.LoginQQ, "");
            var st = d.IndexOf("base64,");
            string base64 = d.Substring(st, d[st..].IndexOf("\"")).Replace("base64,","");
            PrintQRCodeFromBase64(base64, imgHeight, imgWidth);
        }
        /// <summary>
        /// 打印QR码
        /// </summary>
        /// <param name="base64img">base64子串</param>
        /// <param name="height">高度</param>
        /// <param name="width">宽度</param>
        private static void PrintQRCodeFromBase64(string base64img, int height, int width)
        {
            MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64img));
            Bitmap b = new(new Bitmap(stream), width, height);
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var d = b.GetPixel(x, y);
                    if (d.GetBrightness() < (float)0.5)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Write("  ");
                        Console.ResetColor();
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
