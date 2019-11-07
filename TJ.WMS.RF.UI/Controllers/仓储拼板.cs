using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using System.Data;
using TJ.WMS.RF.UI.Utils;
using TJ.WMS.RF.UI.ViewModels;

namespace TJ.WMS.RF.UI.Controllers
{
    public class PastesCCHController :BaseController
    {
        //
        // GET: /PastesCCH/
        PastesCCHService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidateOldTrayNO(PastesCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PastesCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ValidateOldTrayNO();
                return Content("");
            }
            catch (Exception refx)
            {
                Loger.Error(refx.Message, refx);
                return Content(refx.Message);
            }
        }
        public ActionResult ValidateTrayNO(PastesCCH model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PastesCCHService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string TrayNO = Request.Form["TrayNO"];
            string OldTrayNO = Request.Form["OldTrayNO"];
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.Accept();
                return Content("");
            }
            catch (Exception refx)
            {
                Loger.Error(refx.Message, refx);
                return Content(refx.Message);
            }
        }
    }
}
