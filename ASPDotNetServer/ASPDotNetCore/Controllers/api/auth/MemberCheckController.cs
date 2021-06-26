using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [ApiController]
    [Route("api/auth/memberCheck")]
    public class MemberCheckController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_MemberCheck> Post(Req_MemberCheck req)
        {
            string strID = req.strID;

            Rpy_MemberCheck rpy = db_member_check(strID);
            return Ok(rpy);
        }

        private Rpy_MemberCheck db_member_check(string strID)
        {
            byte byRet = 0;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_member_check`('{0}')", strID), (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
            });
            
            return new Rpy_MemberCheck() { byRet = byRet, };
        }
    }
}
