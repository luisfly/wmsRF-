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
    public class LoadController : BaseController
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

        public ActionResult QryLoadPaper(string PaperNO, string LoadNO)
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
                StdQuery1.SetParameter("LoadNO", LoadNO);
                DataTable dt = StdQuery1.Execute("RFQtyLoadPaper");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return null;// Content("null");
                }

            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoadNO"></param>
        /// <returns></returns>
        public ActionResult NewPaperNO(string LoadNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("LoadNO", LoadNO);
                service.ExecuteBusinessProcess("RF_LoadNewPaperNO");
                return Content("[{\"isSuccess\":\"1\",\"sMessage\":\""+ service.GetParameter("PaperNO").ToString() + "\"}]");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                //return Content(ex.Message);
                return Content("[{\"isSuccess\":\"-1\",\"sMessage\":\"" + ex.Message.Replace("\"", "'") + "\"}]");
            }
        }
        public ActionResult RF_LoadAdd(string PaperNO, string StoreNO, string TrayNO)
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
                service.SetParameter("StoreNO", StoreNO);
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("RF_LoadAdd");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult RF_LoadOver(string PaperNO)
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
                service.ExecuteBusinessProcess("RF_LoadOver");
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
                DataTable dt = StdQuery1.Execute("RFQtyLoadPaper");
                DataTable dt2 = StdQuery1.Execute("RFQtyLoadTrayCount");
                ViewData["LoadTrayCount"] = dt2.Rows[0]["LoadTrayCount"].ToString();
                ViewBag.data = dt;
                return View();
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
                DataTable dt = StdQuery1.Execute("RFQtyLoadPaper");
                ViewBag.data = dt;
                return View();
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 查询装载商品明细
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <param name="TrayNO"></param>
        /// <returns></returns>
        public ActionResult QryLoadTrayGoods(string PaperNO, string StoreNO, string TrayNO)
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
                StdQuery1.SetParameter("StoreNO", StoreNO);
                StdQuery1.SetParameter("TrayNO", TrayNO);
                DataTable dt = StdQuery1.Execute("RFQryLoadTrayGoods");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return null;// Content("null");
                }

            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <param name="StoreNO"></param>
        /// <param name="TrayNO"></param>
        /// <returns></returns>
        public ActionResult RF_LoadUnLoad(string PaperNO, string StoreNO, string TrayNO)
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
                service.SetParameter("StoreNO", StoreNO);
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("RF_LoadUnLoad");
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