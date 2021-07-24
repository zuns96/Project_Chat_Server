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

        public enum ERROR_MSG_COMMON : byte
        {
            FAILURE = 0,        // 실패
            SUCCESS = 1,        // 성공

            MAX,
        }

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
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strUserName { get; set; }
        }

        public enum ERROR_MSG_LOGIN
        {
            DUPLICATE_LOGIN = 2,  // 로그인 중복
            INVALID_PASSWORD = 3,   // 유효하지 않는 패스워드
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
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strSender { get; set; }
            public string strMsg { get; set; }
            public long lTimeStamp { get; set; }
        }

        public enum ERROR_MSG_CHAT
        {

        }
    }
}
