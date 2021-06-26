using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signIn")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Rpy_SignIn> Post(Req_SignIn req)
        {
            string strID = req.strID;
            string strPassword = req.strPassword;

            Rpy_SignIn rpy = db_login(strID, strPassword);

            return Ok(rpy);
        }

        private Rpy_SignIn db_login(string strID, string strPassword)
        {
            byte byRet = 0;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, string.Format("CALL `p_login`('{0}', '{1}')", strID, strPassword), (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                strUserName = Convert.ToString(reader[1]);
            });

            return new Rpy_SignIn() { byRet = byRet, strUserName = strUserName, };
        }
    }
}
