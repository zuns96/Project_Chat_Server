using Microsoft.AspNetCore.Mvc;
using System;
using static ASPDotNetCore.ASPPacket;

namespace ASPDotNetCore.Controllers.api.auth
{
    [ApiController]
    [Route("api/auth/memberCheck")]
    public class MemberCheckController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_MemberCheck> Post(Req_MemberCheck req)
        {
            Log.Write("[0/{0}] api/auth/memberCheck 시작 --------->>", req.strID);
            string strID = req.strID;

            Rpy_MemberCheck rpy = db_member_check(strID);
            Log.Write("[0/{0}] api/auth/memberCheck 끝 <<---------", req.strID);
            return Ok(rpy);
        }

        private Rpy_MemberCheck db_member_check(string strID)
        {
            Log.Write("db_member_check({0}) 시작 --------->>", strID);

            byte byRet = 0;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_member_check`('{0}')", strID), (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                Log.Write("db_member_check({0}) : Ret({1})", strID,
                    byRet);
            });

            Log.Write("db_member_check({0}) 끝 <<---------", strID);
            
            return new Rpy_MemberCheck() { byRet = byRet, };
        }
    }
}
