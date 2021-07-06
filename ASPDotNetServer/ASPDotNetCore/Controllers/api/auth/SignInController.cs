using Microsoft.AspNetCore.Mvc;
using System;
using static ASPDotNetCore.ASPPacket;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signIn")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_SignIn> Post(Req_SignIn req)
        {
            Log.Write("[0/{0}] api/auth/signIn 시작 --------->>", req.strID);
            string strID = req.strID;
            string strPassword = req.strPassword;

            Rpy_SignIn rpy = db_login(strID, strPassword);
            Log.Write("[0/{0}] api/auth/signIn 끝 <<---------", req.strID);

            return Ok(rpy);
        }

        private Rpy_SignIn db_login(string strID, string strPassword)
        {
            Log.Write("db_login({0},{1}) 시작 --------->>", strID, strPassword);

            byte byRet = 0;
            long lUserNo = 0L;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_login`('{0}', '{1}')", strID, strPassword), (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                lUserNo = Convert.ToInt64(reader[1]);
                strUserName = Convert.ToString(reader[2]);

                Log.Write("db_login({0},{1}) : Ret({2}),UserNo({3}),UserName({4})", strID, strPassword,
                    byRet,
                    lUserNo,
                    strUserName);
            });

            Log.Write("db_login({0},{1}) 끝 <<---------", strID, strPassword);

            return new Rpy_SignIn() { byRet = byRet, lUserNo = lUserNo, strUserName = strUserName, };
        }
    }
}
