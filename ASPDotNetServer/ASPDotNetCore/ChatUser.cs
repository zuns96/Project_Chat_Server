using System;
using System.Text;
using System.Web;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using static ASPDotNetCore.WSPacket;

namespace ASPDotNetCore
{
    public class ChatUser
    {
        WebSocket m_webSocket = null;

        long m_lUserNo;
        string m_strUserName;

        public long UserNo { get { return m_lUserNo; } }

        public ChatUser(long userNo, string strUserName, WebSocket webSocket)
        {
            m_lUserNo = userNo;
            m_strUserName = strUserName;
            m_webSocket = webSocket;

            Task task = new Task(CheckStatus);
            task.Start();
        }

        public async Task Do_Req_Login(Req_Login req, WebSocketReceiveResult result)
        {
            long lUserNo = m_lUserNo;
            string strUserName = m_strUserName;

            await Send_Rpy_Login(lUserNo, strUserName, result);
        }

        async Task Send_Rpy_Login(long lUserNo, string strUserName, WebSocketReceiveResult result)
        {
            Rpy_Login rpy = new Rpy_Login();
            rpy.lUserNo = lUserNo;
            rpy.strUserName = strUserName;

            Packet packet = new Packet();
            CommonHeader hd = new CommonHeader();
            hd.iRmiID = (int) E_RMIID.E_RMIID_RPY_LOGIN;
            packet.hd = hd;
            packet.strJson = JsonConvert.SerializeObject(rpy);

            await Send(packet, result);
        }

        public async Task Do_Req_Chat(Req_Chat req, WebSocketReceiveResult result)
        {
            long lUserNo = req.lUserNo;
            string strSender = req.strSender;
            string strMsg = req.strMsg;
            long lTimeStamp = req.lTimeStamp;

            await Send_Rpy_Chat(lUserNo, strSender, strMsg, lTimeStamp, result);
        }

        async Task Send_Rpy_Chat(long lUserNo, string strSender, string strMsg, long lTimeStamp, WebSocketReceiveResult result)
        {
            Rpy_Chat rpy = new Rpy_Chat();
            rpy.lUserNo = lUserNo;
            rpy.strSender = strSender;
            rpy.strMsg = strMsg;
            rpy.lTimeStamp = lTimeStamp;

            Packet packet = new Packet();
            CommonHeader hd = new CommonHeader();
            hd.iRmiID = (int)E_RMIID.E_RMIID_RPY_CHAT;
            packet.hd = hd;
            packet.strJson = JsonConvert.SerializeObject(rpy);

            await Send(packet, result);
        }

        private async Task Send(Packet packet, WebSocketReceiveResult result)
        {
            string json = JsonConvert.SerializeObject(packet);
            byte[] buffer = Encoding.UTF8.GetBytes(json);
            
            await m_webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Binary, false, CancellationToken.None);

        }

        void CheckStatus()
        {
            while(m_webSocket.State <= WebSocketState.Open)
            {
                Thread.Sleep(10);
            }

            Dispose();
        }

        void Dispose()
        {
            m_webSocket = null;
            ChatManager.DisposeUser(this);
        }
    }
}
