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
    public class InvTaskController : BaseController
    {
        //
        // GET: /InvTask/
        #region 参数定义
        InvTaskService service;
        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            service = new InvTaskService(Login_Info.User_ID, Login_Info.User_Name,Login_Info.Token);
            DataTable dt = service.GetUserInvTask();
            if (dt == null || dt.Rows.Count <= 0)
            {
                //没有任务
                ViewData["LocationNO"] = "";
                ViewData["StorageTypeID"] = "";
                ViewData["LocationTypeID"] = "";
                ViewData["IsBoxInput"] = "0";
            }
            else
            {
                ViewData["InventoryNO"] = dt.Rows[0]["InventoryNO"].ToString();
                ViewData["LocationNO"] = dt.Rows[0]["LocationNO"].ToString();
                ViewData["GoodsID"] = dt.Rows[0]["GoodsID"].ToString();
                ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                ViewData["GoodsName"] = dt.Rows[0]["GoodsName"].ToString();
                ViewData["StorageTypeID"] = dt.Rows[0]["StorageTypeID"].ToString();//C.零拣位，按个数盘点，没有托盘
                ViewData["LocationTypeID"] = dt.Rows[0]["LocationTypeID"].ToString();//2.拣货位没有托盘
                ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                ViewData["TrayNO"] = dt.Rows[0]["TrayNO"].ToString();
                ViewData["StockQty"] = dt.Rows[0]["StockQty"].ToString();
                ViewData["ProductDate"] = dt.Rows[0]["ProductDate"].ToString();
                ViewData["StockBatchNO"] = dt.Rows[0]["StockBatchNO"].ToString();
                ViewData["IsBoxInput"] = dt.Rows[0]["IsBoxInput"].ToString();
                ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
            }
            return View();
        }

        /// <summary>
        /// 储位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateLocationNO(InvTaskModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new InvTaskService(Login_Info.User_ID, Login_Info.User_Name,Login_Info.Token);
            try
            {
                if (model.LocationTypeID != "2")//2.拣货位，没有托盘
                {
                    model.LocationNO = model.hdLocationNO;
                }
                service.SetParameter("InventoryNO", model.InventoryNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("OldLocationNO", model.OldLocationNO);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("IsBoxInput", model.IsBoxInput);
                service.ExecuteBusinessCheck("RF_TakeStockSave", "LocationNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 托盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateTrayNO(InvTaskModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new InvTaskService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                if (model.LocationTypeID != "2")//2.拣货位，没有托盘
                {
                    model.LocationNO = model.hdLocationNO;
                }
                service.SetParameter("InventoryNO", model.InventoryNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("OldLocationNO", model.OldLocationNO);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("IsBoxInput", model.IsBoxInput);
                service.ExecuteBusinessCheck("RF_TakeStockSave", "TrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateQty(InvTaskModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new InvTaskService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                if (model.LocationTypeID != "2")//2.拣货位，没有托盘
                {
                    model.LocationNO = model.hdLocationNO;
                }
                service.SetParameter("InventoryNO", model.InventoryNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("AQty", model.AQty);
                service.SetParameter("OldLocationNO", model.OldLocationNO);
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("IsBoxInput", model.IsBoxInput);
                service.ExecuteBusinessCheck("RF_TakeStockSave", "*");
                service.ExecuteBusinessProcess("RF_TakeStockSave");
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
