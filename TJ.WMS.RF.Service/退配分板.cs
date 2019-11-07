using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
   public class MatchplateTPService:RFBase
   {
       public MatchplateTPService(string user_id, string user_name, string token)
       {
           SetParameter("Operator", user_id);
           SetParameter("OperName", user_name);

           this.Token = token;
           this.BusinessID = "RF_DistributionDtl";

           ValidateUser();
       }
       public void ValidaBarCode()
       {
           ValidateUser();
           CheckObject[] objs = GetCheckObjects("TPBarcode");
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

       /// <summary>
       /// 商品数量
       /// </summary>
       public void ValidateProductNum()
       {
           ValidateUser();
           CheckObject[] objs = GetCheckObjects("AQty");
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
      public void ValidateVendorNO()
      {
          ValidateUser();
          CheckObject[] objs = GetCheckObjects("VendorNO");
          foreach (CheckObject obj in objs)
          {
              obj.DoValidate(paras);
          }
      }
       

       public DataTable QueryGoods()
       {
           ValidateUser();
           QueryObject query = GetQueryObject("RF_GoodsQuery");
           if (query == null)
               throw new RFException("查询对象[RF_GoodsQuery]不存在");
           DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
           return ds.Tables[0];
       }
        public DataTable RFGoodsQuery(string RFGoodsQuery)
        {
            ValidateUser();
            QueryObject query = GetQueryObject(RFGoodsQuery);
            if (query == null)
                throw new RFException("查询对象["+ RFGoodsQuery + "]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }

        public string GetDefaultShipper()
       {
           ValidateUser();
           QueryObject query = GetQueryObject("RF_GetDefaultShipper");
           if (query == null)
               throw new RFException("查询对象[RF_GetDefaultShipper]不存在");
           object obj = Bss_Helper.GetValue(query.Sql, CommandType.Text);
           if (obj == null)
           {
               //tSystemCtrl
               throw new RFException("没有货主！");
           }
           else
           {
               return obj.ToString();
           }
       }

       public string GetOldTrayNO()
       {
           ValidateUser();
           QueryObject query = GetQueryObject("RF_MatchSearch");
           if (query == null)
               throw new RFException("查询对象[RF_MatchSearch]不存在");
           object obj = Bss_Helper.GetValue(query.Sql,CommandType.Text);
           if (obj == null)
           {
               //tSystemCtrl
               throw new RFException("系统中没有设定退配托盘条码！");
           }
           else
           {
               return obj.ToString();
           }
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
