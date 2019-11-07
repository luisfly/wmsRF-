using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace TJ.WMS.RF.Service
{
    public class StdQuery:RFBase
    {
        /// <summary>
        /// 无需验证用户直接查询
        /// </summary>
        public StdQuery()
        {

        }
        public StdQuery(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }

        public DataTable Execute(string StdQueryName)
        {
            ValidateUser();
            QueryObject query = GetQueryObject(StdQueryName);
            if (query == null)
                throw new RFException("查询对象[" + StdQueryName + "]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public int ExcuteSQL(string QueryName)
        {
            QueryObject query = GetQueryObject(QueryName);
            if (query == null)
                throw new RFException("查询对象[" + QueryName + "]不存在");
            return Bss_Helper.ExcuteSQL(query.Sql);
        }
    }
}
