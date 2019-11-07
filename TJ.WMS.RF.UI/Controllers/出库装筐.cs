using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.ViewModels;

namespace TJ.WMS.RF.UI.Controllers
{
    public class OutBoxInController :BaseController
    {
        /*
          功能：实现胶筐与出库箱绑定
         * 1.输入胶筐进行验证
         * 2.输入出库箱进行验证
         * 3.验证通过后 update tCratesGoods 实现胶筐与出库箱绑定
         * 4.成功后清空胶筐与出库箱的输入值，让焦点回到胶筐的输入框，上面1到3的操作，
         * 重复进行胶筐与出库箱绑定
         */
        OutBoxInService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return  Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidateContainerNO(OutBoxInViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new OutBoxInService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {

                service.SetParameter("ContainerNO", model.ContainerNO);
                service.ValidateTrayNO();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateTrayNO(OutBoxInViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new OutBoxInService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {

                service.SetParameter("ContainerNO", model.ContainerNO);
                service.SetParameter("TrayNO", model.TrayNO);
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
