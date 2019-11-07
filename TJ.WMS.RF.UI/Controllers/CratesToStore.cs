using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.Models;
using System.Data;
using TJ.WMS.RF.UI.Utils;
using TJ.WMS.RF.UI.ViewModels;

namespace TJ.WMS.RF.UI.Controllers
{
    public class CratesToStoreController :BaseController
    {
        //
        // GET: /CratesToStore/
        #region 参数定义
        CratesToStoreService service;
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
        /// 效验原始出库箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateOldTrayNO(CratesToStoreModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ExecuteBusinessCheck("RF_CratesToStoreAdd", "OldTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 效验条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateBarcode(CratesToStoreModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.ExecuteBusinessCheck("RF_PastesPartof", "Barcode");
                DataTable dt = service.GetCratesGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("数据异常，请检查!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult GetStore(CratesToStoreModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                
                DataTable dt = service.GetCratesStore();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("数据异常，请检查!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 效验目标出库箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateTrayNO(CratesToStoreModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("StoreNO", StringUtil.GetTextID(model.Store));
                //service.SetParameter("IsCheckTrayNO", model.IsCheckTrayNO);
                service.ExecuteBusinessCheck("RF_CratesToStoreAdd", "TrayNO");

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 效验数量并提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateQty(CratesToStoreModel model, bool tryagain = true)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesToStoreService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("SeedQty", model.AQty);
                service.SetParameter("StoreNO", StringUtil.GetTextID(model.Store) );
                //service.SetParameter("IsCheckTrayNO", model.IsCheckTrayNO);
                service.ExecuteBusinessCheck("RF_CratesToStoreAdd", "*");
                service.ExecuteBusinessProcess("RF_CratesToStoreAdd");
                return Content("");
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("请重新运行该事务") && tryagain == true)
                {
                    System.Threading.Thread.Sleep(300); //毫秒
                    //ValidateQty(model, false);
                    return ValidateQty(model, false);
                }
                else
                {
                    Loger.Error(ex);
                    return Content(ex.Message);
                }

            }
        }

    }
}
