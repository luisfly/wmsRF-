using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using System.Data;
using TJ.WMS.RF.UI.Utils;
using TJ.WMS.RF.UI.Models;
using TJ.WMS.RF.UI.ViewModels;

namespace TJ.WMS.RF.UI.Controllers
{
    public class MatchplateCCHController:BaseController
    {
        #region 参数定义
        MatchplateCCHService service;
        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            return View();
        }

        public ActionResult ValidateOldTrayNO(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string OldTrayNO = model.OldTrayNO;
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name, 
                Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", OldTrayNO);
                service.ValidateOldTrayNO();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateTrayNO(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string TrayNO = model.TrayNO;
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("IsFirstSave", model.IsFirstSave);
                service.SetParameter("IsBoxPick", model.IsBoxPick);
                service.ValidateTrayNO();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateBarCode(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string BarCode = Request.Form["BarCode"];
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                model.StorageTypeID = null;
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("IsFirstSave", model.IsFirstSave);
                service.SetParameter("IsBoxPick", model.IsBoxPick);
                service.ValidateBarCode();
                DataTable dt = service.QueryGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    
                    return Content(JsonHelper.ToJson(dt));
                }
                return Content("商品条码输入有误！");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateStockBatchNO(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string TrayNO = model.TrayNO;
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("sStockBatchNO",model.sStockBatchNO);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("IsFirstSave", model.IsFirstSave);
                service.SetParameter("IsBoxPick", model.IsBoxPick);
                //service.ValidateTrayNO();
                service.ExecuteBusinessCheck("RF_tStockLocation", "sStockBatchNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateAQty(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("IsFirstSave", model.IsFirstSave);
                service.SetParameter("IsBoxPick", model.IsBoxPick);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("sStockBatchNO", model.sStockBatchNO);
                service.ValidateAQty();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateNewLocation(MatchplateCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string NewLocation = Request.Form["NewLocation"];

            try
            {
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("NewLocation", model.NewLocation);
                service.SetParameter("IsFirstSave", model.IsFirstSave);
                service.SetParameter("IsBoxPick", model.IsBoxPick);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("sStockBatchNO", model.sStockBatchNO);
                service.ValidateNewLocation();
                service.ExecuteBusinessProcess("RF_tStockLocation");
                //service.Accept();
                model.StorageTypeID = null;
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }
    }
}
