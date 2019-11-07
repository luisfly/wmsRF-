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

    public class PastesController : BaseController
    {
        //
        // GET: /Pastes/
        #region 自定变量
        PastesService service;
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
        public ActionResult ValidateOldTrayNO(PastesModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PastesService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ExecuteBusinessCheck("RF_PastesModify", "OldTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 效验目标出库箱并提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateTrayNO(PastesModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PastesService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_PastesModify", "*");
                service.ExecuteBusinessProcess("RF_PastesModify");
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
