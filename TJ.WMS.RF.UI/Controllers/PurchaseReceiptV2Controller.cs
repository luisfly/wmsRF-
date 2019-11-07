﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.UI.Models;
using TJ.WMS.RF.Service;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class PurchaseReceiptV2Controller : BaseController
    {
        #region 参数定义
        //string token = "";
        DcAcceptV2 acpt;

        #endregion
        public ActionResult Index(string id)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            //ViewData["ToDay"] = DateTime.Today;
            return View();
        }


        #region 数据合法性验证
        #region 订单
        public ActionResult ValidateOrderCode()
        {
            string ID = this.Request.Form["OrderNO"];
            string ProvTypeID = "";
            ViewData["BillCode"] = ID;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                acpt.SetParameter("OrderNO", ID);
                acpt.ValidateOrderCode();
                ProvTypeID = acpt.paras["ProvTypeID"].Value.ToString();
                if (ProvTypeID == "")
                {
                    ProvTypeID = "订单类型错误，请检查！";
                }
                return Content(ProvTypeID);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }

        }
        #endregion
        #region 商品条码
        public ActionResult ValidateBarCode()
        {
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                ///0001201306070001
                acpt.SetParameter("Barcode", Barcode.Trim());
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.ValidateBarcode();
                DataTable dt = acpt.QueryDcacptQty();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("当前订单不存在该商品!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        #endregion
        #region 托盘
        public ActionResult ValidateTrayNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                //return Redirect("/Home");
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.ValidateTrayNO();
                acpt.ValidateBarcode();
                DataTable dt = acpt.QueryDcacptQty();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("当前订单不存在该商品!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        #endregion
        #region 生产日期
        public ActionResult ValidateProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];

            try
            {
                acpt.SetParameter("ProductDate", productDate);
                acpt.ValidateProductDate();

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        #endregion
        #region 到期日期
        public ActionResult ValidateEffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string ProductDate = Request.Form["ProductDate"];
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            try
            {
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("EffectiveDate", effectiveDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.ValidateEffectiveDate();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        #endregion

        public ActionResult ValidateQty()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");

            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string Qty = Request.Form["Qty"];
            try
            {
                acpt.SetParameter("Qty", Qty);
                acpt.SetParameter("OnlyAllowBoxAcpt", Request.Form["OnlyAllowBoxAcpt"]);
                acpt.ValidateQty();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        #endregion
        public ActionResult Detail()
        {

            string id = Request.Form["hdBillCode"];
            ViewData["BillCode"] = id;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                 Login_Info.Token);
            DataTable dt = acpt.QueryOnlyAllowBoxAcpt();
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewData["OnlyAllowBoxAcpt"] = dt.Rows[0]["OnlyAllowBoxAcpt"].ToString();
            }
            else
            {
                ViewData["OnlyAllowBoxAcpt"] = "1";//1.只允许录入箱数；0.可以同时录入个数
            }
            acpt.SetParameter("OrderNO", id);
            ViewData["BatchBuildTypeID"] = acpt.GetBatchBuildTypeID();
            return View("Detail");
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Save()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string StockBatchNO = Request.Form["StockBatchNO"];
            string Qty = Request.Form["Qty"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            string IsAuto = Request.Form["IsAuto"];

            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("EffectiveDate", EffectiveDate);
                acpt.SetParameter("Qty", Qty);
                acpt.SetParameter("OnlyAllowBoxAcpt", Request.Form["OnlyAllowBoxAcpt"]);
                acpt.SetParameter("StockBatchNO", StockBatchNO);
                acpt.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                acpt.SetParameter("IsAuto", IsAuto);
                acpt.ExecuteBusinessProcess("RF_DcAcptModify2");
                //acpt.Accept();
                //*************20140724**********************
                //acpt.paras.Clear();
                //acpt.SetParameter("Barcode", Barcode.Trim());
                //acpt.SetParameter("OrderNO", OrderNO);
                //DataTable dt = acpt.QueryDcacptQty();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //return Content(JsonHelper.ToJson(dt));
                //}
                //*******************************************
                if (acpt.paras["Complete"].Value.ToString() != "true")
                {
                    return Content("0");
                }
                else
                {
                    return Content("");
                }

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }

        public ActionResult Over()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string Qty = Request.Form["Qty"];
            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("EffectiveDate", EffectiveDate);
                acpt.SetParameter("Qty", Qty);
               // acpt.Over();
                acpt.ExecuteBusinessProcess("RF_DcGetAcptNO2");
                return Content("");

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateStockBatchNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAcceptV2(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                acpt.SetParameter("StockBatchNO", StockBatchNO);
                acpt.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                acpt.ExecuteBusinessCheck("RF_DcAcptModify2", "StockBatchNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

     

    }
}
