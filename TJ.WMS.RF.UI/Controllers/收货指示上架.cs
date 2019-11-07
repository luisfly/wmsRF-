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
    public class AutoShelvesController : BaseController
    {
        //
        // GET: /收货指示上架/

        
        #region 自定变量
        AutoShelvesService service;
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

        public ActionResult ValidateTrayNO(AutoShelvesModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new AutoShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("TargetLocationNO", model.TargetLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.ExecuteBusinessCheck("RF_GetShelvesLoc", "TrayNO");
                DataTable dt = service.QueryShelvesGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("托盘数据异常，请检查!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        public ActionResult GetShelvesLocationNO(AutoShelvesModel model)
        {
            string ToLocationNO = "";
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new AutoShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                //service.SetParameter("TargetLocationNO", model.TargetLocationNO);
                //service.SetParameter("LocationNO", model.LocationNO);
                ToLocationNO = service.GetShelvesLocationNO();
                if (ToLocationNO == "")
                {
                    return Content("找不到上架储位！");
                }
                else
                {
                    return Content("[{'ToLocationNO':'" + ToLocationNO+"'}]");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        public ActionResult GetNextShelvesLocationNO(AutoShelvesModel model)
        {
            string ToLocationNO = "";
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new AutoShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                //service.SetParameter("TargetLocationNO", model.TargetLocationNO);
                //service.SetParameter("LocationNO", model.LocationNO);
                service.ExecuteBusinessCheck("RF_GetNextShelvesLoc", "*");
                ToLocationNO = service.GetNextShelvesLocationNO();
                if (ToLocationNO == "")
                {
                    return Content("找不到下一个上架储位！");
                }
                else
                {
                    return Content("[{'ToLocationNO':'" + ToLocationNO + "'}]");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateLocationNO(AutoShelvesModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new AutoShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.ExecuteBusinessCheck("RF_Shelves","*");
                service.ExecuteBusinessProcess("RF_Shelves");
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
