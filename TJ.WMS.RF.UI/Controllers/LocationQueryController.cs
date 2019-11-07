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
    public class LocationQueryController : BaseController
    {
        MatchplateTPService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            return View();
        }
        public ActionResult RFLocationQuery(string LocationNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchplateTPService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
           
            try
            {
                service.SetParameter("LocationNO", LocationNO);
                DataTable dt = service.RFGoodsQuery("RFLocationQuery");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("没有发现记录！");
                }

            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
      
    }
}