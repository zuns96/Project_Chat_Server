using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDotNetCore
{
    static public class WSPacket
    {
        public enum E_RMIID : int
        {
            E_RMIID_REQ_LOGIN = 1,
            E_RMIID_RPY_LOGIN,
            E_RMIID_REQ_CHAT,
            E_RMIID_RPY_CHAT,
        };

        public class CommonHeader
        {
            public int iRmiID { get; set; }
        }

        public class Packet
        {
            public CommonHeader hd { get; set; }
            public string strJson { get; set; }
        }

        public class Req_Login
        {
            public long lUserNo { get; set; }
            public string strUserName { get; set; }
        }

        public class Rpy_Login
        {
            public long lUserNo { get; set; }
            public string strUserName { get; set; }
        }

        public class Req_Chat
        {
            public long lUserNo { get; set; }
            public string strSender { get; set; }
            public string strMsg { get; set; }
            public long lTimeStamp { get; set; }
        }

        public class Rpy_Chat
        {
            public long lUserNo { get; set; }
            public string strSender { get; set; }
            public string strMsg { get; set; }
            public long lTimeStamp { get; set; }
        }
    }
}
