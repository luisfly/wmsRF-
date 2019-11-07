using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class InvTaskService: RFBase
    {
        public InvTaskService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;

            ValidateUser();
        }

        public DataTable GetUserInvTask()
        {
            QueryObject query = GetQueryObject("RF_StockSearch");
            if (query == null)
            {
                throw new RFException("查询对象[RF_StockSearch]不存在");
            }
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
    }
}
