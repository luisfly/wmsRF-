using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class ToStoreService : RFBase
    {
        public ToStoreService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }

        public DataTable GetCrossGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_TwoCrossQuery");
            if (query == null)
                throw new RFException("查询对象[RF_TwoCrossQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];

        }
        public DataTable GetCrossGoods2()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_TwoCrossQuery2");
            if (query == null)
                throw new RFException("查询对象[RF_TwoCrossQuery2]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];

        }

        //取参数越库播种类型0.总量一次播;1.按箱多次播
        public DataTable QueryCSToStoreType()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_CSToStoreType");
            if (query == null)
                throw new RFException("查询对象[RF_CSToStoreType]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
    }
}
