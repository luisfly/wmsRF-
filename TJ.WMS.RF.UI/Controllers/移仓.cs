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
    public class StockMoveController :BaseController
    {
        //
        // GET: /StockMove/
        #region 参数定义
        StockMoveService service;
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
        public ActionResult ValidateTrayNO(StockMoveViewModel model)
        {
             GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                service = new StockMoveService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("LocationNO",model.LocationNO);
                service.ValidateTrayNO();
                DataTable dt = service.QueryGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("原托盘输入有误！");
                }
                //return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateToLocationNO(StockMoveViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                service = new StockMoveService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("ToLocationNO", model.ToLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.Accept();
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
