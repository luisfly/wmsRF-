using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.Models;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class TruckLoadController : BaseController
    {
        #region 参数定义
        TruckLoadService service;
        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        /// <summary>
        /// 验证装车单号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidatePaperNO(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.ExecuteBusinessCheck("RF_CheckTruckPaperNO", "*");
                service.ExecuteBusinessProcess("RF_CheckTruckPaperNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 装车明细页面视图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Detail()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            object PaperNO = Request.QueryString["PaperNO"];
            if (PaperNO == null)
            {
                return Redirect("/TruckLoad");
            }
            else
            {
                service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("PaperNO", PaperNO.ToString());
                DataTable dt = service.GetPaperInfo();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Redirect("/TruckLoad");
                }
                else
                {
                    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                    ViewData["StoreNO"] = dt.Rows[0]["StoreNO"].ToString();
                    ViewData["StoreDesc"] = dt.Rows[0]["StoreDesc"].ToString();
                    ViewData["TruckNO"] = dt.Rows[0]["TruckNO"].ToString();
                    return View();
                }
            }
        }

        /// <summary>
        /// 验证车牌号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateTruckNO(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TruckNO", model.TruckNO);
                service.ExecuteBusinessCheck("RF_LoadingAdd", "TruckNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证出库箱并提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateTrayNO(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name,Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TruckNO", model.TruckNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_LoadingAdd", "*");
                service.ExecuteBusinessProcess("RF_LoadingAdd");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 装车完成
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult LoadingOver(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TruckNO", model.TruckNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_LoadingCheck", "*");
                service.ExecuteBusinessProcess("RF_LoadingCheck");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 撤销装车页面视图UnLoad
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult UnLoad()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            object PaperNO = Request.QueryString["PaperNO"];
            object StoreNO = Request.QueryString["StoreNO"];
            if (PaperNO == null || StoreNO == null)
            {
                return Redirect("/TruckLoad");
            }
            else
            {
                service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                try
                {
                    service.SetParameter("PaperNO", PaperNO.ToString());
                    service.ExecuteBusinessCheck("RF_TruckMath", "PaperNO");
                    ViewData["PaperNO"]=PaperNO.ToString();
                    ViewData["StoreNO"] = StoreNO.ToString();
                    return View();
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                    return Redirect("/TruckLoad");
                }
            }
        }

        /// <summary>
        /// 验证撤销装车原始出库箱ValidateOldTrayNO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateOldTrayNO(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("IsFirst", model.IsFirst);
                service.ExecuteBusinessCheck("RF_TruckMath", "TrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证撤销装车原始目标出库箱ValidateNewTrayNO
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateNewTrayNO(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("IsFirst", model.IsFirst);
                service.ExecuteBusinessCheck("RF_TruckMath", "NewTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证撤销装车输入的条码ValidateBarcode
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateBarcode(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("IsFirst", model.IsFirst);
                service.ExecuteBusinessCheck("RF_TruckMath", "Barcode");
                DataTable dt = service.GetBarcodeGoods();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("获取商品信息异常，请重试！");
                }
                else
                {
                    return Content(JsonHelper.ToJson(dt));
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证撤销装车数量并提交处理过程ValidateQty
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateQty(TruckLoad model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("StoreNO", model.StoreNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("IsFirst", model.IsFirst);
                service.ExecuteBusinessCheck("RF_TruckMath", "*");
                service.ExecuteBusinessProcess("RF_TruckMath");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 载具配送页面视图
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult CntainerSend()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            object PaperNO = Request.QueryString["PaperNO"];
            if (PaperNO == null)
            {
                return Redirect("/TruckLoad");
            }
            else
            {
                service = new TruckLoadService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("PaperNO", PaperNO.ToString());
                DataTable dt = service.GetPaperInfo();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Redirect("/TruckLoad");
                }
                else
                {
                    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                    ViewData["StoreNO"] = dt.Rows[0]["StoreNO"].ToString();
                    ViewData["StoreDesc"] = dt.Rows[0]["StoreDesc"].ToString();
                    ViewData["TruckNO"] = dt.Rows[0]["TruckNO"].ToString();
                    return View();
                }
            }
        }
    }
}
