using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.ViewModels;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class TruckLoadingController : BaseController
    {
        StdQuery StdQuery1;
        RFBase service;
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidatePaperNO(string PaperNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.ExecuteBusinessProcess("RF_TruckLoadPaperVal");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult Detail(string PaperNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("PaperNO", PaperNO);
                StdQuery1.SetParameter("LoadNO", "");
                DataTable dt = StdQuery1.Execute("RFQryTruckLoading");
                ViewBag.data = dt;
                return View();
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateLoadNO(string PaperNO, string LoadNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                //return Redirect("/Home");
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.ExecuteBusinessCheck("RF_TruckLoadAdd", "LoadNO");
                StdQuery1.SetParameter("PaperNO", PaperNO);
                StdQuery1.SetParameter("LoadNO", LoadNO);
                DataTable dt = StdQuery1.Execute("RFQryTruckLoadingDtl");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult RF_TruckLoadAdd(string PaperNO, string LoadNO, string RealCaseQty, string RealPlateQty)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.SetParameter("RealCaseQty", RealCaseQty);
                service.SetParameter("RealPlateQty", RealPlateQty);
                service.ExecuteBusinessProcess("RF_TruckLoadAdd");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult RF_TruckLoadOver(string PaperNO, string LoadNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.ExecuteBusinessProcess("RF_TruckLoadOver");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult UnLoad(string PaperNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("PaperNO", PaperNO);
                StdQuery1.SetParameter("LoadNO", "");
                DataTable dt = StdQuery1.Execute("RFQryTruckLoading");
                ViewBag.data = dt;
                return View();
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult RF_TruckLoadUnLoad(string PaperNO, string LoadNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.ExecuteBusinessProcess("RF_TruckLoadUnLoad");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
    }
}