using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class MatchpLatePCService: RFBase
    {
        public MatchpLatePCService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }

        public DataTable GetMatchPlateGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_MatchPlateGoodsV2");
            if (query == null)
                throw new RFException("查询对象[RF_MatchPlateGoodsV2]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataSet GettMatchPlateDtl()
        {
            ValidateUser();
            QueryObject query1 = GetQueryObject("RF_OldTrayNOGoods");
            if (query1 == null)
                throw new RFException("查询对象[RF_OldTrayNOGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query1.Sql, paras.Values.ToArray());
            ds.Tables[0].TableName = "Master";
            QueryObject query2 = GetQueryObject("RF_NewTrayGoods");
            if (query2 == null)
                throw new RFException("查询对象[RF_NewTrayGoods]不存在");
            DataSet ds2 = Bss_Helper.GetDataSet_SQL(query2.Sql, paras.Values.ToArray());
            DataTable dt = ds2.Tables[0];
            dt.TableName = "Detail";
            ds.Tables.Add(dt.Copy());
            return ds;
        }
        public DataTable GetOldTrayNOGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_OldTrayNOGoods");
            if (query == null)
                throw new RFException("查询对象[RF_OldTrayNOGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
    }
}
