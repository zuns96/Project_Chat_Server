using ASPDotNetCore.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ASPDotNetCore.Controllers.api.auth
{
    [Route("api/auth/signUp")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        public SignUp Post()
        {
            SignUp rpy = P_Account_Create();

            return rpy;
        }

        private SignUp P_Account_Create()
        {
            byte byRet = 0;
            string strUserName = string.Empty;

            DBManager.ExcuteQuery(E_DB.E_DB_ACCOUNT, "CALL `p_account_create`('zuns96', 'IAmProgrammer@960115')", (reader) =>
            {
                reader.Read();
                byRet = Convert.ToByte(reader[0]);
                strUserName = Convert.ToString(reader[1]);
            });

            return new SignUp() { byRet = byRet, strUserName = strUserName, };
        }
    }
}
