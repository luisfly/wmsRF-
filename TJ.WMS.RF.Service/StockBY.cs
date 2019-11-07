using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 采购收货
    /// </summary>
    public class StockBY : RFBase
    {
        public StockBY(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_BYDModify";

            ValidateUser();
        }
        public DataTable QueryBYQty()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_SelBYGoodsInfo");
            if (query == null)
                throw new RFException("查询对象[RF_SelBYGoodsInfo]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataTable QueryBYTray()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_GetBYTrayInfo");
            if (query == null)
                throw new RFException("查询对象[RF_GetBYTrayInfo]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }

        public DataTable ValidateOrderCode()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_CheckBYD");
            if (query == null)
                throw new RFException("查询对象[RF_CheckBYD]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
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

        public void ValidateTrayNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("TrayNO");
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

        public void Delete()
        {
            this.BusinessID = "RF_StockBYDtlDel";
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
                    this.BusinessID = "RF_BYDModify";
                    throw;
                }
            }
            Bss_Helper.Commit();
            this.BusinessID = "RF_BYDModify";
        }

        public void BY()
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
        //取参数是否只允许界面输入收货箱数
        public DataTable QueryOnlyAllowBoxAcpt()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_OnlyAllowBoxAcpt");
            if (query == null)
                throw new RFException("查询对象[RF_OnlyAllowBoxAcpt]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public string GetBatchBuildTypeID()
        {
            //ValidateUser();
            QueryObject query = GetQueryObject("RF_GetBatchBuildTyp4");
            if (query == null)
                throw new RFException("查询对象[RF_GetBatchBuildTyp4]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return ds.Tables[0].Rows[0][0].ToString();
            }
            else
                return "1";
        }
    }
}
