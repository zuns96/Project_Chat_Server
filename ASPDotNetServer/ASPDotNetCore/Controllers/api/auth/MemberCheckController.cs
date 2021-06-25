using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/memberCheck")]
    [ApiController]
    public class MemberCheckController : ControllerBase
    {
        public MemberCheck Post()
        {
            MemberCheck rpy = P_Member_Check();

            return rpy;
        }

        private MemberCheck P_Member_Check()
        {
            byte byRet = 0;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, "CALL `p_member_check`('zuns96')", (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
            });
            
            return new MemberCheck() { byRet = byRet, };
        }
    }
}
