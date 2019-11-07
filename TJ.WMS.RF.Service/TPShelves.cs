using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class TPShelvesService : RFBase
    {
        public TPShelvesService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_TPShelvesModify";

            ValidateUser();
        }
        //查询门店信息
        public DataTable QueryStoreInfo()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_TPShelvesQuery");
            if (query == null)
                throw new RFException("查询对象[RF_TPShelvesQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataTable QueryGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_TPShelvesGoods");
            if (query == null)
                throw new RFException("查询对象[RF_TPShelvesGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }

        public DataTable QueryGoodsStock()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_GoodsStockQuery");
            if (query == null)
                throw new RFException("查询对象[RF_GoodsStockQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        #region
        public DataTable ValidatPaperNO()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_TPShelvesVerify");
            if (query == null)
                throw new RFException("查询对象[RF_TPShelvesVerify]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
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
        public void ValidateLocationNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("LocationNO");
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

        public void ValidateProductDate()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("ProductDate");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }

        public void ValidateEffectiveDate()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("EffectiveDate");
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
            this.BusinessID = "RF_TPShelvesOK";
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
