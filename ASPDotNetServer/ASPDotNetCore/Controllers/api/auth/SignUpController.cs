using Microsoft.AspNetCore.Mvc;
using System;
using static ASPDotNetCore.ASPPacket;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signUp")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_SignUp> Post(Req_SignUp req)
        {
            Log.Write("[0/{0}] api/auth/signUp 시작 --------->>", req.strID);

            string strID = req.strID;
            string strPassword = req.strPassword;
            Rpy_SignUp rpy = db_account_create(strID, strPassword);

            Log.Write("[0/{0}] api/auth/signUp 끝 <<---------", req.strID);

            return Ok(rpy);
        }

        private Rpy_SignUp db_account_create(string strID, string strPassword)
        {
            Log.Write("db_account_create({0},{1}) 시작 --------->>", strID, strPassword);

            byte byRet = 0;
            long lUserNo = 0L;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_account_create`('{0}', '{1}')", strID, strPassword), (reader) =>
             {
                 reader.Read();
                 byRet = Convert.ToByte(reader[0]);
                 lUserNo = Convert.ToInt64(reader[1]);
                 strUserName = Convert.ToString(reader[2]);

                 Log.Write("db_account_create({0},{1}) : Ret({2}),UserNo({3}),UserName({4})", strID, strPassword,
                    byRet,
                    lUserNo,
                    strUserName);
             });

            Log.Write("db_account_create({0},{1}) 시작 <<---------", strID, strPassword);

            return new Rpy_SignUp() { byRet = byRet, lUserNo = lUserNo, strUserName = strUserName, };
        }
    }
}
