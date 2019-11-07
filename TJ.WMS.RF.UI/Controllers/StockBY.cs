using System;
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
    public class StockBYController :BaseController
    {
        #region 参数定义
        //string token = "";
        StockBY byd;

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
            string ID = this.Request.Form["BYPaperNO"];
            ViewData["BillCode"]=ID;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                byd.SetParameter("BYPaperNO", ID);
                DataTable dt = byd.ValidateOrderCode();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content("");
                }
                return Content("单号无效！");
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
            string BYPaperNO = Request.Form["BYPaperNO"];
            string TrayNO = Request.Form["TrayNO"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                ///0001201306070001
                byd.SetParameter("Barcode", Barcode.Trim());
                byd.SetParameter("BYPaperNO", BYPaperNO);
                byd.SetParameter("TrayNO", TrayNO);
                byd.ValidateBarcode();
                DataTable dt = byd.QueryBYQty();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("商品条码无效！");
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string BYPaperNO = Request.Form["BYPaperNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            try
            {
                byd.SetParameter("BYPaperNO", BYPaperNO);
                byd.SetParameter("Barcode", Barcode);
                byd.SetParameter("TrayNO", TrayNO);
                byd.ValidateTrayNO();
                byd.ValidateBarcode();
                DataTable dt = byd.QueryBYTray();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("条码无效");
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];
          
            try
            {
                byd.SetParameter("ProductDate", productDate);
                byd.ValidateProductDate();
              
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string ProductDate = Request.Form["ProductDate"];
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string BYPaperNO = Request.Form["BYPaperNO"];
            try
            {
                byd.SetParameter("ProductDate", ProductDate);
                byd.SetParameter("EffectiveDate", effectiveDate);
                byd.SetParameter("shelfLife", shelfLife);
                byd.SetParameter("Barcode", Barcode);
                byd.SetParameter("BYPaperNO", BYPaperNO);
                byd.ValidateEffectiveDate();
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string Qty = Request.Form["Qty"];
            try
            {
                byd.SetParameter("Qty", Qty);
                byd.SetParameter("OnlyAllowBoxAcpt", Request.Form["OnlyAllowBoxAcpt"]);
                byd.ValidateQty();
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                 Login_Info.Token);
            byd.SetParameter("BYPaperNO", id);
            ViewData["BatchBuildTypeID"] = byd.GetBatchBuildTypeID();
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
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string BYPaperNO = Request.Form["BYPaperNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string Qty = Request.Form["Qty"];
            string Itme = Request.Form["Item"];
            string VendorID = Request.Form["VendorID"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                byd.SetParameter("BYPaperNO", BYPaperNO);
                byd.SetParameter("Barcode", Barcode);
                byd.SetParameter("TrayNO", TrayNO);
                byd.SetParameter("ProductDate", ProductDate);
                byd.SetParameter("shelfLife", shelfLife);
                byd.SetParameter("EffectiveDate", EffectiveDate);
                byd.SetParameter("Qty", Qty);
                byd.SetParameter("VendorID", VendorID);
                byd.SetParameter("DtlItem", Itme);
                byd.SetParameter("OnlyAllowBoxAcpt", Request.Form["OnlyAllowBoxAcpt"]);
                byd.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                byd.BY();
                //*************20140724**********************
                //byd.paras.Clear();
                //byd.SetParameter("Barcode", Barcode.Trim());
                //byd.SetParameter("BYPaperNO", BYPaperNO);
                //DataTable dt = byd.QueryDcacptQty();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //return Content(JsonHelper.ToJson(dt));
                //}
                //*******************************************
                return Content("");

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
            
        }

        public ActionResult DeleteGoods()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string BYPaperNO = Request.Form["BYPaperNO"];
            string Item = Request.Form["Item"];
            string GoodsID = Request.Form["GoodsID"];
            try
            {
                byd.SetParameter("BYPaperNO", BYPaperNO);
                byd.SetParameter("Item", Item);
                byd.SetParameter("GoodsID", GoodsID);
                byd.Delete();
                //*************20140724**********************
                //byd.paras.Clear();
                //byd.SetParameter("Barcode", Barcode.Trim());
                //byd.SetParameter("BYPaperNO", BYPaperNO);
                //DataTable dt = byd.QueryDcacptQty();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //return Content(JsonHelper.ToJson(dt));
                //}
                //*******************************************
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }
        //public ActionResult Over()
        //{
        //    GetLoginInfo();
        //    if (Login_Info == null)
        //    {
        //        return Content("<script>location.href='/Home'</script>");
        //    }
        //    byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name,
        //        Login_Info.Token);
        //    string BYPaperNO = Request.Form["BYPaperNO"];
        //    string TrayNO = Request.Form["TrayNO"];
        //    string Barcode = Request.Form["Barcode"];
        //    string ProductDate = Request.Form["ProductDate"];
        //    string shelfLife = Request.Form["shelfLife"];
        //    string EffectiveDate = Request.Form["EffectiveDate"];
        //    string Qty = Request.Form["Qty"];
        //    try
        //    {
        //        byd.SetParameter("BYPaperNO", BYPaperNO);
        //        byd.SetParameter("Barcode", Barcode);
        //        byd.SetParameter("TrayNO", TrayNO);
        //        byd.SetParameter("ProductDate", ProductDate);
        //        byd.SetParameter("shelfLife", shelfLife);
        //        byd.SetParameter("EffectiveDate", EffectiveDate);
        //        byd.SetParameter("Qty", Qty);
        //        byd.Over();
        //        return Content("");

        //    }
        //    catch (Exception ex)
        //    {
        //        Loger.Error(ex.Message, ex);
        //        return Content(ex.Message);
        //    }
        //}
        public ActionResult ValidateStockBatchNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            byd = new StockBY(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                byd.SetParameter("StockBatchNO", StockBatchNO);
                byd.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                byd.ExecuteBusinessCheck("RF_DcAcptModify", "StockBatchNO");
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
