using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class ShelvesController : BaseController
    {
        ShelvesService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidateTrayNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string TrayNO = Request.Form["TrayNO"];
            try
            {
                service.SetParameter("TrayNO", TrayNO);
                service.ValidateTrayNO();
                DataTable dt = service.QueryShelves();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("此托盘下不存在商品!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateLocationNO()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string TrayNO = Request.Form["TrayNO"];
            string LocationNO = Request.Form["LocationNO"];
            try
            {
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("LocationNO", LocationNO);
                service.Accept();
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
