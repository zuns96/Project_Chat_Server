using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPDotNetCore.Models
{
    public class Req_MemberCheck
    {
        public string strID { get; set; }
    }

    public class Rpy_MemberCheck
    {
        public byte byRet { get; set; }
    }

    public class Req_SignIn
    {
        public string strID { get; set; }
        public string strPassword { get; set; }
    }

    public class Rpy_SignIn
    {
        public byte byRet { get; set; }
        public string strUserName { get; set; }
    }

    public class Req_SignUp
    {
        public string strID { get; set; }
        public string strPassword { get; set; }
    }

    public class Rpy_SignUp
    {
        public byte byRet { get; set; }
        public string strUserName { get; set; }
    }
}
