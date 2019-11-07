using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 用户登录
    /// </summary>
    public class UserLogin : RFBase
    {
        //[DllImport("PasswordEncrypt.dll", EntryPoint = "EncryptPwd")]
        //private static extern string EncryptPwd(string str);

        public UserLogin()
        {
        }

        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="password"></param>
        /// <param name="user_name"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool Login(string user_id, string password, ref string user_name, ref string token)
        {
            SetParameter("UserNO", user_id);
            //SetParameter("Password", EncryptPwd(password));
            SetParameter("Password", password);
            QueryObject query = GetQueryObject("RF_Login");
            if (query == null)
                throw new RFException("查询对象[RF_Login]不存在");
            paras.Remove("LocalStoreNO");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            if (ds.Tables[0].Rows.Count == 0)
                return false;

            user_name = ds.Tables[0].Rows[0]["OperName"].ToString();
            RFBase.LocalStoreNO = ds.Tables[0].Rows[0]["LocalStoreNO"].ToString();
            token = RFService.Instance.UserRegist(user_id);
            return true;
        }
    }
}
