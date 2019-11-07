using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class RedistService : RFBase
    {
        public RedistService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_ReDistModify";

            ValidateUser();
        }
        //查询门店信息
        public DataTable QueryStoreInfo()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_ReDistQuery");
            if (query == null)
                throw new RFException("查询对象[RF_ReDistQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataTable QueryGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_ReDistQGoods");
            if (query == null)
                throw new RFException("查询对象[RF_ReDistQGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        #region
        public void ValidatPaperNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("PaperNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        public void ValidateTrayNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("TrayNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        public void ValidateBarcode()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("Barcode");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        public void ValidateQty()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("Qty");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion

        public void Accept()
        {
            //验证每一项
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("*");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }

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
            Bss_Helper.Commit();
        }

        public void Over()
        {
            //验证每一项
            this.BusinessID = "RF_ReDistAcptOK";
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("*");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }

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
            Bss_Helper.Commit();
        }
    }
}
