using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class PickGoodsBase:RFBase
    {
        public PickGoodsBase(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_PickGoods";

            ValidateUser();
        }
        public DataTable GetRF_Assignment()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_Assignment");
            if (query == null)
                throw new RFException("查询对象[RF_Assignment]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }


        public string PickGoods(bool tryagain = true)
        {
            object ParamValue = null;
            this.BusinessID = "RF_PickGoods";
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
                    //Bss_Helper.RollBack();
                    //Loger.Error(ex);
                    //throw;

                    Bss_Helper.RollBack();
                    if (ex.Message.Contains("请重新运行该事务") && tryagain == true)
                    {
                        System.Threading.Thread.Sleep(300); //毫秒
                                                            //ValidateQty(model, false);
                        PickGoods(false);

                        break;

                    }
                    else
                    {
                        Loger.Error(ex);
                        if (ex.Message.Contains("请重新运行该事务"))
                        {
                            throw new Exception("操作异常请重试!", ex);
                        }
                        throw;
                    }
                }
            }
            try
            {
                ParamValue = GetParameter("Complete");
            }
            catch (Exception ex)
            {
                Bss_Helper.RollBack();
                Loger.Error(ex);
                throw;
            }
            Bss_Helper.Commit();
            return ParamValue.ToString();
        }

        public string PickRelease()
        {
            object ParamValue = null;
            this.BusinessID = "RF_PickRelease";
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
            try
            {
                ParamValue = GetParameter("Complete");
            }
            catch (Exception ex)
            {
                Bss_Helper.RollBack();
                Loger.Error(ex);
                throw;
            }
            Bss_Helper.Commit();
            return ParamValue.ToString();
        }


    }
}
