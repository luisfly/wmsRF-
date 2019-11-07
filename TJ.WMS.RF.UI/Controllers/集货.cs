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
    public class SetController : BaseController
    {
        #region 自定变量
        SetService service;
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
        /// 检查出库箱是否已提交过程,false为未提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult CheckTrayNOSubmited(SetViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new SetService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_SetCheckTray","*");
                service.ExecuteBusinessProcess("RF_SetCheckTray");
                return Content(service.GetParameter("IsCheck").ToString());
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交出库箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult SubmitTrayNO(SetViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new SetService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_SetUpdateTray", "*");
                service.ExecuteBusinessProcess("RF_SetUpdateTray");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateTrayNO(SetViewModel model)
        {
            //tStore 表返回 门店编码 和名称  集货道口编码
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new SetService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_Set", "TrayNO");
                DataTable dt = service.QueryStore();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("出库箱异常，请检查!");
                }
                else
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                //return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }

        }

        public ActionResult ValidatePostion(SetViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new SetService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Postion1", model.Postion1);
                service.ExecuteBusinessCheck("RF_Set", "*");
                service.ExecuteBusinessProcess("RF_Set");
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
