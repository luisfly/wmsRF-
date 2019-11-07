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
    public class PurchaseReceiptController :BaseController
    {
        #region 参数定义
        //string token = "";
        DcAccept acpt;


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
            string ExtTag = "";
            string PaperNO = "";
            ViewData["BillCode"]=ID;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                acpt.SetParameter("OrderNO", ID);
                acpt.ValidateOrderCode();
                ExtTag = acpt.paras["ExtTag"].Value.ToString();
                ProvTypeID = acpt.paras["ProvTypeID"].Value.ToString();
                if (ProvTypeID == "")
                {
                    ProvTypeID = "订单类型错误，请检查！";
                }
                PaperNO = acpt.paras["PaperNO"].Value.ToString();

                // return Content(ProvTypeID);
                return Json(new { PaperNO = PaperNO, ProvTypeID = ProvTypeID , ExtTag = ExtTag });
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            if (id == null) {
                id = Request.QueryString["hdBillCode"].ToString();
            }
            ViewData["BillCode"] = id;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string Qty = Request.Form["Qty"];
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
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
                //acpt.Accept();
                acpt.ExecuteBusinessProcess("RF_DcAcptModify");
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
                acpt.ExecuteBusinessProcess("RF_DcGetAcptNO");
                //acpt.Over();
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
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                acpt.SetParameter("StockBatchNO", StockBatchNO);
                acpt.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                acpt.ExecuteBusinessCheck("RF_DcAcptModify", "StockBatchNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        //////////////////CS1//////////////////////
        /// <summary>
        /// 一步越库页面视图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult CS1Acpt()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            //ViewData["ToDay"] = DateTime.Today;
            string id = Request.Form["hdCS1OrderNO"];
            ViewData["BillCode"] = id;
            return View();
        }

        /// <summary>
        /// 验证门店
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        ///
        public ActionResult ValidateCSStoreNO(PurchaseReceiptModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                acpt.SetParameter("OrderNO", model.OrderNO);
                acpt.SetParameter("StoreNO", model.StoreNO);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "StoreNO");
                DataTable dt = acpt.QueryCSOrderStoreNO();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("当前收货订单不包含该门店!!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateCSBarCode(PurchaseReceiptModels model)
        {
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                ///0001201306070001
                acpt.SetParameter("Barcode", Barcode.Trim());
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("StoreNO", model.StoreNO);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "Barcode");
                DataTable dt = acpt.QueryCSAcptQty();
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
        public ActionResult ValidateCSTrayNO(PurchaseReceiptModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("StoreNO", model.StoreNO);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "TrayNO");
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "Barcode");
                DataTable dt = acpt.QueryCSAcptQty();
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
        public ActionResult ValidateCSProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];

            try
            {
                acpt.SetParameter("ProductDate", productDate);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "ProductDate");

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateCSEffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            try
            {
                acpt.SetParameter("EffectiveDate", effectiveDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify", "EffectiveDate");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult CS1AcptConfirm()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
                acpt.SetParameter("StoreNO", Request.Form["StoreNO"]);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("EffectiveDate", EffectiveDate);
                acpt.SetParameter("Qty", Qty);
                acpt.ExecuteBusinessCheck("RF_ACAcptModify","*");
                acpt.ExecuteBusinessProcess("RF_ACAcptModify");
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

        //////////////////END_CS1//////////////////////
        //////////////////CS2//////////////////////////////
        /// <summary>
        /// 二步越库页面视图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult CS2Acpt()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            //ViewData["ToDay"] = DateTime.Today;
            string id = Request.Form["hdCS2OrderNO"];
            ViewData["BillCode"] = id;
            return View();
        }

        public ActionResult ValidateCS2BarCode()
        {
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                ///0001201306070001
                acpt.SetParameter("Barcode", Barcode.Trim());
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "Barcode");
                DataTable dt = acpt.QueryCS2AcptQty();
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
        public ActionResult ValidateCS2TrayNO(PurchaseReceiptModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("StoreNO", model.StoreNO);
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "TrayNO");
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "Barcode");
                DataTable dt = acpt.QueryCS2AcptQty();
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
        public ActionResult ValidateCS2ProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];

            try
            {
                acpt.SetParameter("ProductDate", productDate);
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "ProductDate");

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateCS2EffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            try
            {
                acpt.SetParameter("EffectiveDate", effectiveDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "EffectiveDate");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult CS2AcptConfirm()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
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
                acpt.SetParameter("StoreNO", Request.Form["StoreNO"]);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("EffectiveDate", EffectiveDate);
                acpt.SetParameter("Qty", Qty);
                acpt.ExecuteBusinessCheck("RF_CS2AcptModify", "*");
                acpt.ExecuteBusinessProcess("RF_CS2AcptModify");
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
        //////////////////END-CS2//////////////////////////


        //////////////////CS2AcptToStore//////////////////////////////
        public ActionResult CS2AcptToStore()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            //ViewData["ToDay"] = DateTime.Today;
            string id = Request.Form["hdCS2ToStoreOrder"];
            ViewData["BillCode"] = id;
            return View();
        }

        public ActionResult ValidateCS2ToStoreBarCode()
        {
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                ///0001201306070001
                acpt.SetParameter("Barcode", Barcode.Trim());
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "Barcode");
                
                 DataTable dt = acpt.QueryCS2AcptQty2();
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
        public ActionResult ValidateCS2ToStoreTrayNO(PurchaseReceiptModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            try
            {
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("StoreNO", model.StoreNO);
                acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "Barcode");
                acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "TrayNO");
                DataTable dt = acpt.QueryCS2AcptQty();
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
        public ActionResult ValidateCS2ToStoreProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];

            try
            {
                acpt.SetParameter("ProductDate", productDate);
                acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "ProductDate");

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateCS2ToStoreEffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            try
            {
                acpt.SetParameter("EffectiveDate", effectiveDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "EffectiveDate");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult CS2AcptConfirm2()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string Qty = Request.Form["Qty"];
            string TrayTag = null;
            try
            {
                //acpt.SetParameter("StoreNO", Request.Form["StoreNO"]);
                acpt.SetParameter("OrderNO", OrderNO);
                acpt.SetParameter("Barcode", Barcode);
                acpt.SetParameter("TrayNO", TrayNO);
                acpt.SetParameter("ProductDate", ProductDate);
                acpt.SetParameter("shelfLife", shelfLife);
                acpt.SetParameter("EffectiveDate", EffectiveDate);
                acpt.SetParameter("Qty", Qty);
               // acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "*");
                 
                //DataTable dt = acpt.QueryOnlyAllowBoxAcpt();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    TrayTag = dt.Rows[0]["Tag"].ToString();
                    
                //}
                //if(TrayTag == "")


                acpt.ExecuteBusinessProcess("RF_CS2Acpt2Modify");
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
        //////////////////CS2AcpToStore//////////////////////////
    }
}
