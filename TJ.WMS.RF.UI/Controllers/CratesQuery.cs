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
    public class CratesQueryController : BaseController
    {
        StdQuery StdQuery1;
        MatchpLateService service;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            return View();
        }

        public ActionResult RFCratesQuery(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("TrayNO", TrayNO);
                DataTable dt = StdQuery1.Execute("RFCratesQuery");
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