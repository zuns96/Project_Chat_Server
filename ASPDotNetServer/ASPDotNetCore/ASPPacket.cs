namespace ASPDotNetCore
{
    public class ASPPacket
    {
        static public class APIDefine
        {
            public const string API_MEMBER_CHECK = "/api/auth/memberCheck";
            public const string API_SIGNUP = "/api/auth/signUp";
            public const string API_SIGNIN = "/api/auth/signIn";
        }

        public enum ERROR_MSG_COMMON : byte
        {
            FAILURE = 0,        // 실패
            SUCCESS = 1,        // 성공

            MAX,
        };

        public abstract class Model
        {
            public abstract new string ToString();
        }


        public class Req_MemberCheck : Model
        {
            public string strID { get; set; }

            public override string ToString()
            {
                return "?id=" + strID;
            }
        }

        public class Rpy_MemberCheck : Model
        {
            public byte byRet { get; set; }

            public override string ToString()
            {
                return "?ret=" + byRet;
            }
        }

        public enum ERROR_MSG_MEMBERCHECK : byte
        {

        }

        public class Req_SignIn : Model
        {
            public string strID { get; set; }
            public string strPassword { get; set; }


            public override string ToString()
            {
                return "?id=" + strID + "&password=" + strPassword;
            }
        }

        public class Rpy_SignIn : Model
        {
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strUserName { get; set; }

            public override string ToString()
            {
                return "?ret=" + byRet + "&userNo=" + lUserNo + "&userName=" + strUserName;
            }
        }

        public enum ERROR_MSG_SIGNIN : byte
        {

        }

        public class Req_SignUp : Model
        {
            public string strID { get; set; }
            public string strPassword { get; set; }

            public override string ToString()
            {
                return "?id=" + strID + "&password=" + strPassword;
            }
        }

        public class Rpy_SignUp : Model
        {
            public byte byRet { get; set; }
            public long lUserNo { get; set; }
            public string strUserName { get; set; }

            public override string ToString()
            {
                return "?ret=" + byRet + "&userNo=" + lUserNo + "&userName=" + strUserName;
            }
        }

        public enum ERROR_MSG_SIGNUP : byte
        {

        }
    }
}
