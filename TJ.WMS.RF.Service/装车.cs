using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class TruckLoadService:RFBase
    {
        public TruckLoadService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_TLPaperNO";
            ValidateUser();
        }

        public DataTable GetPaperInfo()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("GetStorebyPaperNO");
            if (query == null)
                throw new RFException("查询对象[GetStorebyPaperNO]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];

        }
        public DataTable GetBarcodeGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_GetBarcodeGoods");
            if (query == null)
                throw new RFException("查询对象[RF_GetBarcodeGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];

        }
    }
}
