namespace ASPDotNetCore
{
    public class ASPPacket
    {
        public enum ERROR_MSG_COMMON : byte
        {
            FAILURE = 0,        // 실패
            SUCCESS = 1,        // 성공

            MAX,
        }

        public class Req_MemberCheck
        {
            public string strID { get; set; }
        }

        public class Rpy_MemberCheck
        {
            public byte byRet { get; set; }
        }

        public enum ERROR_MSG_MEMBERCHECK : byte
        {

        }

        public class Req_SignIn
        {
            public string strID { get; set; }
            public string strPassword { get; set; }
        }

        public class Rpy_SignIn
        {
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strUserName { get; set; }
        }

        public enum ERROR_MSG_SIGNIN : byte
        {

        }

        public class Req_SignUp
        {
            public string strID { get; set; }
            public string strPassword { get; set; }
        }

        public class Rpy_SignUp
        {
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strUserName { get; set; }
        }

        public enum ERROR_MSG_SIGNUP : byte
        {

        }
    }
}
