using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class MatchplateCCHService : RFBase
    {
        public MatchplateCCHService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_tStockLocation";

            ValidateUser();
        }
        #region 业务验证
        #region 原托盘号
        public void ValidateOldTrayNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("OldTrayNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion

        #region 目标托盘
        public void ValidateTrayNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("TrayNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion

        #region 商品条码
        public void ValidateBarCode()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("Barcode");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion

        #region 实际数量
        public void ValidateAQty()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("AQty");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion

        #region 目标储位
        public void ValidateNewLocation()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("NewLocation");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        #endregion
        #endregion

        public DataTable QueryGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_CCHGoodsQuery");
            if (query == null)
                throw new RFException("查询对象[RF_CCHGoodsQuery]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
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
       
    }
}
