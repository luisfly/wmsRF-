using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;

namespace TJ.WMS.RF.UI.Controllers
{
    public class ContainerAcptController :BaseController
    {

        #region 参数定义
        ContainerAcptService service;
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
        public ActionResult ValidateContainerNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string ContainerNO = Request.Form["ContainerNO"];
            service = new ContainerAcptService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("ContainerNO", ContainerNO);
                service.ValidateContainerNO();
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
