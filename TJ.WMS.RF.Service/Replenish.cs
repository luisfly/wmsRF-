using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class ReplenishService : RFBase
    {
        public ReplenishService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_PickGoods";

            ValidateUser();
        }

        public DataTable GetRF_Assignment()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_Assignment");
            if (query == null)
                throw new RFException("查询对象[RF_Assignment]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }

        public DataTable SubmitMiddleTrayNO(string StdQueryName)
        {
            ValidateUser();
            QueryObject query = GetQueryObject(StdQueryName);
            if (query == null)
                throw new RFException("查询对象[" + StdQueryName + "]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
    }
}
