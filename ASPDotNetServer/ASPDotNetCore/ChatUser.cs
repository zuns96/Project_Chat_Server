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
        public string UserName { get { return m_strUserName; } }

        public ChatUser(long userNo, string strUserName, WebSocket webSocket)
        {
            m_lUserNo = userNo;
            m_strUserName = strUserName;
            m_webSocket = webSocket;
        }

        public async Task Do_Req_Login(Req_Login req)
        {
            long lUserNo = m_lUserNo;
            string strUserName = m_strUserName;

            await Send_Rpy_Login(lUserNo, strUserName);
        }

        async Task Send_Rpy_Login(long lUserNo, string strUserName)
        {
            Log.Write("[{0}/{1}]Send_Rpy_Login({2},{3}) 시작 --------->>", m_lUserNo, m_strUserName, lUserNo, strUserName);

            Rpy_Login rpy = new Rpy_Login();
            rpy.lUserNo = lUserNo;
            rpy.strUserName = strUserName;

            Packet packet = new Packet();
            CommonHeader hd = new CommonHeader();
            hd.iRmiID = (int) E_RMIID.E_RMIID_RPY_LOGIN;
            packet.hd = hd;
            packet.strJson = JsonConvert.SerializeObject(rpy);

            await Send(packet);

            Log.Write("[{0}/{1}]Send_Rpy_Login({2},{3}) 끝 <<---------", m_lUserNo, m_strUserName, lUserNo, strUserName);
        }

        public async Task Do_Req_Chat(Req_Chat req)
        {
            long lUserNo = req.lUserNo;
            string strSender = req.strSender;
            string strMsg = req.strMsg;
            long lTimeStamp = req.lTimeStamp;

            await Send_Rpy_Chat(lUserNo, strSender, strMsg, lTimeStamp);
        }

        async Task Send_Rpy_Chat(long lUserNo, string strSender, string strMsg, long lTimeStamp)
        {
            Log.Write("[{0}/{1}]Send_Rpy_Chat({2},{3},{4},{5}) 시작 --------->>", m_lUserNo, m_strUserName, lUserNo, strSender, strMsg, lTimeStamp);

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

            await Send(packet);

            Log.Write("[{0}/{1}]Send_Rpy_Chat({2},{3},{4},{5}) 끝 <<---------", m_lUserNo, m_strUserName, lUserNo, strSender, strMsg, lTimeStamp);
        }

        private async Task Send(Packet packet)
        {
            try
            {
                string json = JsonConvert.SerializeObject(packet);
                byte[] buffer = Encoding.UTF8.GetBytes(json);

                Log.Write("[{0}/{1}] Json({2} Bytes) : {3}", m_lUserNo, m_strUserName, buffer.Length, json);

                await m_webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Binary, false, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Log.Write("#### Exception!!! THROWN ####");
                Log.Write(ex.Message);
                Log.Write(ex.StackTrace);
                Log.Write("#### Exception!!! THROWN ####");
            }
        }
    }
}
