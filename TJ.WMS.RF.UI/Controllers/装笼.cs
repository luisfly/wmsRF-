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
    public class PutCageController : BaseController
    {
        //
        // GET: /PutCage/
        #region 参数定义
        PutCageService service;
        #endregion
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 验证笼车
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateCageNO(PutCage model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PutCageService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("CageNO", model.CageNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_PutCage", "CageNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证出库箱并提交处理过程
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateTrayNO(PutCage model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PutCageService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("CageNO", model.CageNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.ExecuteBusinessCheck("RF_PutCage", "*");
                service.ExecuteBusinessProcess("RF_PutCage");
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
