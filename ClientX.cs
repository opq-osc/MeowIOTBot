using System;
using MeowIOTBot.QQ.QQMessage.QQRecieveMessage;
using System.Collections.Generic;
using System.Text;

namespace MeowIOTBot
{
    public class MeowCreateClient : IDisposable
    {
        private MeowServiceClient socket;
        public MeowCreateClient(string url, string qq, bool logFlag=false)
        {
            MeowServiceClient socket = new MeowServiceClient(url, qq, logFlag);
            socket.CreateClient();
            socket._OnFriendDetailedMsg += (s, e) => { };
        }

        public MeowServiceClient Connect()
        {
            
            return socket;
        }

        public void Dispose()
        {
            socket.Dispose();
        }
    }
}
