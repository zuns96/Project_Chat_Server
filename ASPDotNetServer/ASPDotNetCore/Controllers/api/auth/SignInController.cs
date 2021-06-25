using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signIn")]
    [ApiController]
    public class SignInController : ControllerBase
    {
        public SignIn Post()
        {
            SignIn rpy = P_Login();

            return rpy;
        }

        private SignIn P_Login()
        {
            byte byRet = 0;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, "CALL `p_login`('zuns96', 'IAmProgrammer@960115')", (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                strUserName = Convert.ToString(reader[1]);
            });

            return new SignIn() { byRet = byRet, strUserName = strUserName, };
        }
    }
}
