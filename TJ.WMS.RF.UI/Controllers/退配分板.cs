using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.Utils;
using System.Data;

namespace TJ.WMS.RF.UI.Controllers
{
    public class MatchplateTPController :BaseController
    {
        //
        // GET: /MatchplateTP/
        MatchplateTPService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OldTrayNO = service.GetOldTrayNO();
            string ShipperNO = service.GetDefaultShipper();
            ViewData["OldTrayNO"] = OldTrayNO;
            ViewData["ShipperNO"] = ShipperNO;
            return View();
        }

        public ActionResult ValidaBarCode()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string BarCode = Request.Form["BarCode"];
            string OldTrayNO = Request.Form["OldTrayNO"];
            string TrayNO = Request.Form["TrayNO"];
            string ShipperNO = Request.Form["ShipperNO"];
            try
            {
                service.SetParameter("TPBarcode", BarCode);
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.SetParameter("ShipperNO", ShipperNO);
                service.ValidaBarCode();
                DataTable dt = service.QueryGoods();
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
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string ProductDate = Request.Form["ProductDate"];
            string Barcode = Request.Form["BarCode"];
            string ShipperNO = Request.Form["ShipperNO"];
            try
            {
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("TPBarcode", Barcode);
                service.SetParameter("OldTrayNO", Request.Form["OldTrayNO"]);
                service.SetParameter("ShipperNO", ShipperNO);
                service.ValidateProductDate();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateEffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string ProductDate = Request.Form["ProductDate"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string ShelfLife = Request.Form["ShelfLife"];
            string ModuleID = Request.Form["ModuleID"];
            string ShipperNO = Request.Form["ShipperNO"];
            try
            {
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("EffectiveDate", EffectiveDate);
                service.SetParameter("ShelfLife", ShelfLife);
                service.SetParameter("ModuleID", ModuleID);
                service.SetParameter("OldTrayNO", Request.Form["OldTrayNO"]);
                service.SetParameter("ShipperNO", ShipperNO);
                service.ValidateEffectiveDate();
                return Content("");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateVendorNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string VendorNO = Request.Form["VendorNO"];
            string Barcode = Request.Form["BarCode"];
            string ShipperNO = Request.Form["ShipperNO"];
            try
            {
                service.SetParameter("VendorNO", VendorNO);
                service.SetParameter("TPBarcode", Barcode);
                service.SetParameter("OldTrayNO", Request.Form["OldTrayNO"]);
                service.SetParameter("ShipperNO", ShipperNO);
                service.ValidateVendorNO();
                return Content("");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateDistTray(string OldTrayNO)
        {
            RFBase service1;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service1 = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service1.SetParameter("OldTrayNO", OldTrayNO);
                service1.ExecuteBusinessProcess("RF_ValidateDistTray");
                return Content("[{\"isSuccess\":\"1\",\"sMessage\":\"" + service1.GetParameter("ShipperNO").ToString() + "\"}]");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                //return Content(ex.Message);
                return Content("[{\"isSuccess\":\"-1\",\"sMessage\":\"" + ex.Message.Replace("\"", "'") + "\"}]");
            }
        }
        public ActionResult ValidateTrayNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string BarCode = Request.Form["BarCode"];
            string TrayNO = Request.Form["TrayNO"];
            string ProductDate = Request.Form["ProductDate"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string OldTrayNO = Request.Form["OldTrayNO"];
            string ShipperNO = Request.Form["ShipperNO"];
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("TPBarcode", BarCode);
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("EffectiveDate", EffectiveDate);
                service.SetParameter("ShipperNO", ShipperNO);
                service.SetParameter("StockBatchNO", StockBatchNO);
                service.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                service.ValidateTrayNO();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateProductNum()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string Barcode = Request.Form["BarCode"];
            string OldTrayNO = Request.Form["OldTrayNO"];
            string ProductDate = Request.Form["ProductDate"];
            string EffectiveDate = Request.Form["EffectiveDate"];
            string VendorNO = Request.Form["VendorNO"];
            string AQty = Request.Form["AQty"];
            string TrayNO = Request.Form["TrayNO"];
            string ShelfLife = Request.Form["ShelfLife"];
            string ModuleID = Request.Form["ModuleID"];
            string ShipperNO = Request.Form["ShipperNO"];
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                service.SetParameter("TPBarcode", Barcode);
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.SetParameter("EffectiveDate", EffectiveDate);
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("VendorNO", VendorNO);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("AQty", AQty);
                service.SetParameter("ShelfLife", ShelfLife);
                service.SetParameter("ModuleID", ModuleID);
                service.SetParameter("ShipperNO", ShipperNO);
                service.SetParameter("StockBatchNO", StockBatchNO);
                service.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                service.Accept();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateShipper()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OldTrayNO = Request.Form["OldTrayNO"];
            string ShipperNO = Request.Form["ShipperNO"];
            try
            {
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.SetParameter("ShipperNO", ShipperNO);
                service.ExecuteBusinessCheck("RF_DistributionDtl", "ShipperNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateOldTrayNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OldTrayNO = Request.Form["OldTrayNO"];
            try
            {
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.ExecuteBusinessCheck("RF_DistributionDtl", "OldTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
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
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string StockBatchNO = Request.Form["StockBatchNO"];
            string BatchBuildTypeID = Request.Form["BatchBuildTypeID"];
            try
            {
                service.SetParameter("StockBatchNO", StockBatchNO);
                service.SetParameter("BatchBuildTypeID", BatchBuildTypeID);
                service.ExecuteBusinessCheck("RF_DistributionDtl", "StockBatchNO");
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
