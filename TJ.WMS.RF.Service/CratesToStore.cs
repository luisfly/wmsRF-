﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class CratesToStoreService:RFBase
    {
        public CratesToStoreService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_CratesToStore";

            ValidateUser();
        }


        public DataTable GetCratesGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_CratesGoods");
            if (query == null)
                throw new RFException("查询对象[RF_CratesGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataTable GetCratesStore()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_CratesStore");
            if (query == null)
                throw new RFException("查询对象[RF_CratesStore]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public DataTable CratesGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_CratesGoods");
            if (query == null)
                throw new RFException("查询对象[RF_CratesGoods]不存在");
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
        public void ValidateToLocationNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("ToLocationNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }

        public DataTable QueryGoods()
        {
            ValidateUser();
            QueryObject query = GetQueryObject("RF_CratesToStore");
            if (query == null)
                throw new RFException("查询对象[RF_LocationGoods]不存在");
            DataSet ds = Bss_Helper.GetDataSet_SQL(query.Sql, paras.Values.ToArray());
            return ds.Tables[0];
        }
        public void Accept()
        {
            ExecuteBusinessProcess("RF_CratesToStore");
        //    //验证每一项
        //    ValidateUser();
        //    CheckObject[] objs = GetCheckObjects("*");
        //    foreach (CheckObject obj in objs)
        //    {
        //        obj.DoValidate(paras);
        //    }

            //    BusinessObject[] bos = GetBusinessObjects(BusinessID);
            //    Bss_Helper.BeginTrans();
            //    foreach (BusinessObject obj in bos)
            //    {
            //        try
            //        {
            //            obj.DoValidate(paras, this);
            //        }
            //        catch (Exception ex)
            //        {
            //            Bss_Helper.RollBack();
            //            Loger.Error(ex);
            //            throw;
            //        }
            //    }
            //    Bss_Helper.Commit();
        }
    }
}
