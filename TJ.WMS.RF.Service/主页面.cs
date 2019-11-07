using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 主页面
    /// </summary>
    public class MainPage : RFBase
    {
        public MainPage(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            ValidateUser();
        }

        /// <summary>
        /// 查询模块列表
        /// </summary>
        /// <returns></returns>
        public DataTable GetModuleList()
        {
            QueryObject query = GetQueryObject("RF_Menu");
            if (query == null)
                throw new RFException("查询对象[RF_Menu]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
    }
}
