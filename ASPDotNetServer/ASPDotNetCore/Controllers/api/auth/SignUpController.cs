using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signUp")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_SignUp> Post(Req_SignUp req)
        {
            string strID = req.strID;
            string strPassword = req.strPassword;
            Rpy_SignUp rpy = db_account_create(strID, strPassword);

            return Ok(rpy);
        }

        private Rpy_SignUp db_account_create(string strID, string strPassword)
        {
            byte byRet = 0;
            long lUserNo = 0L;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_account_create`('{0}', '{1}')",strID, strPassword), (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                lUserNo = Convert.ToInt64(reader[1]);
                strUserName = Convert.ToString(reader[2]);
            });

            return new Rpy_SignUp() { byRet = byRet, lUserNo = lUserNo, strUserName = strUserName, };
        }
    }
}
