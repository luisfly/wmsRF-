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
    public class ToStoreController : BaseController
    {
        //
        // GET: /ToStore/
        #region 参数定义
        ToStoreService service;
        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            DataTable dt = service.QueryCSToStoreType();
            if (dt != null && dt.Rows.Count > 0)
            {
                ViewData["CSToStoreType"] = dt.Rows[0]["CSToStoreType"].ToString();
            }
            else
            {
                ViewData["CSToStoreType"] = "1";//0.总量一次播;1.按箱多次播
            }
            return View();
        }

        /// <summary>
        /// 验证收货托盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateOldTrayNO(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ExecuteBusinessCheck("RF_TwoCross", "OldTrayNO");
                //service.ExecuteBusinessProcess("RF_TwoCross");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证收货托盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateOldTrayNO2(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ExecuteBusinessCheck("RF_TwoCross", "OldTrayNO");
                //service.ExecuteBusinessProcess("RF_TwoCross");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 明细视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            object OldTrayNO = Request.QueryString["OldTrayNO"];
            if (OldTrayNO == null)
            {
                return Redirect("/ToStore");
            }
            else
            {
                service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("TrayNO", OldTrayNO.ToString());
                DataTable dt = service.GetCrossGoods();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Redirect("/ToStore");
                }
                else
                {
                    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                    ViewData["Item"] = dt.Rows[0]["Item"].ToString();
                    ViewData["StoreNO"] = dt.Rows[0]["StoreNO"].ToString();
                    ViewData["StoreDesc"] = dt.Rows[0]["StoreDesc"].ToString();
                    ViewData["OldTrayNO"] = dt.Rows[0]["TrayNO"].ToString();
                    ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                    ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                    ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                    ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
                    ViewData["Postion2"] = dt.Rows[0]["Postion2"].ToString();
                    ViewData["CaseQty"] = dt.Rows[0]["CaseQty"].ToString();
                    ViewData["MoreQty"] = dt.Rows[0]["MoreQty"].ToString();
                    return View();
                }
            }
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Detail2( string OldStoreNO, string OldBarcode)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
           // object OldStoreNO = Request.QueryString["OldStoreNO"];
            object OldTrayNO = Request.QueryString["OldTrayNO"];
            //object OldBarcode = Request.QueryString["OldBarcode"];
           
            if (OldTrayNO == null)
            {
                return Redirect("/ToStore");
            }
            else
            {
                service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("TrayNO", OldTrayNO.ToString());
                DataTable dt = service.GetCrossGoods2();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Redirect("/ToStore");
                }
                else
                {
                    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                    ViewData["Item"] = dt.Rows[0]["Item"].ToString();
                    ViewData["StoreNO"] = dt.Rows[0]["StoreNO"].ToString();
                    ViewData["StoreDesc"] = dt.Rows[0]["StoreDesc"].ToString();
                    ViewData["OldTrayNO"] = dt.Rows[0]["TrayNO"].ToString();
                    ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                    ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                    ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                    ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
                    ViewData["Postion2"] = dt.Rows[0]["Postion2"].ToString();
                    ViewData["CaseQty"] = dt.Rows[0]["CaseQty"].ToString();
                    ViewData["MoreQty"] = dt.Rows[0]["MoreQty"].ToString();
                    ViewData["NeedSeedQty"] = dt.Rows[0]["NeedSeedQty"].ToString();
                    ViewData["isCheckToStoreBarcode"] = dt.Rows[0]["isCheckToStoreBarcode"].ToString();
                    //
                    ViewData["OldStoreNO"] = OldStoreNO;//上次门店
                    ViewData["OldBarcode"] = OldBarcode;//上次条码
                    ViewData["SrcQty"] = dt.Rows[0]["SrcQty"].ToString();
                    return View();
                }
            }
        }

        /// <summary>
        /// 验证越库货位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidatePostion2(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Item", model.Item);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("CPCrossNO", model.Postion2);
                service.ExecuteBusinessCheck("RF_TwoCross", "CPCrossNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 验证越库货位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidatePostion22(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Item", model.Item);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("CPCrossNO", model.Postion2);
                service.ExecuteBusinessCheck("RF_TwoCross2", "CPCrossNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证越库货位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateBarcode(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Item", model.Item);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("CPCrossNO", model.Postion2);
                service.SetParameter("Barcode", model.Barcode);
                service.ExecuteBusinessCheck("RF_TwoCross2", "Barcode");
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
        public ActionResult ValidateTrayNO(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Item", model.Item);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("CPCrossNO", model.Postion2);
                service.ExecuteBusinessCheck("RF_TwoCross", "*");
                service.ExecuteBusinessProcess("RF_TwoCross");
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
        public ActionResult ValidateTrayNO2(ToStoreModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Item", model.Item);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("CPCrossNO", model.Postion2);

                service.SetParameter("Barcode", model.Barcode);

                service.SetParameter("SeedQty", model.SeedQty);
                service.ExecuteBusinessCheck("RF_TwoCross2", "*");

                service.ExecuteBusinessProcess("RF_TwoCross2");
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
