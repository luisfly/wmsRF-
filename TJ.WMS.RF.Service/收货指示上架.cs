using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class AutoShelvesService : RFBase
    {
        public AutoShelvesService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }

        public DataTable QueryShelvesGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_ShelvesQuery");
            if (query == null)
                throw new RFException("查询对象[RF_ShelvesQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }

        public string GetShelvesLocationNO()
        {
            this.BusinessID = "RF_GetShelvesLoc";
            string ToLocationNO = "";
            ValidateUser();

            BusinessObject[] bos = GetBusinessObjects(BusinessID);
            Bss_Helper.BeginTrans();
            foreach (BusinessObject obj in bos)
            {
                try
                {
                    obj.DoValidate(paras, this);
                }
                catch (Exception ex)
                {
                    Bss_Helper.RollBack();
                    Loger.Error(ex);
                    throw;
                }
            }
            try
            {
                ToLocationNO = GetParameter("ToLocationNO").ToString();
            }
            catch (Exception ex)
            {
                Bss_Helper.RollBack();
                Loger.Error(ex);
                throw;
            }
            Bss_Helper.Commit();
            return ToLocationNO;
        }

        public string GetNextShelvesLocationNO()
        {
            this.BusinessID = "RF_GetNextShelvesLoc";
            string ToLocationNO = "";
            ValidateUser();

            BusinessObject[] bos = GetBusinessObjects(BusinessID);
            Bss_Helper.BeginTrans();
            foreach (BusinessObject obj in bos)
            {
                try
                {
                    obj.DoValidate(paras, this);
                }
                catch (Exception ex)
                {
                    Bss_Helper.RollBack();
                    Loger.Error(ex);
                    throw;
                }
            }
            try
            {
                ToLocationNO = GetParameter("ToLocationNO").ToString();
            }
            catch (Exception ex)
            {
                Bss_Helper.RollBack();
                Loger.Error(ex);
                throw;
            }
            Bss_Helper.Commit();
            return ToLocationNO;
        }
    }
}
