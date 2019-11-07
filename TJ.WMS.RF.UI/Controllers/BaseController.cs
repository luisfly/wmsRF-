using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TJ.WMS.RF.UI.Controllers
{
    public class BaseController : Controller
    {
        public LoginInfo Login_Info = null;
        public class LoginInfo
        {
            public string User_ID { get; set; }
            public string User_Name { get; set; }
            public string Token { get; set; }
        }
        public void GetLoginInfo()
        {
            
            if (Session["User_ID"] == null)
            {
                Login_Info= null;
            }
            else
            {
                Login_Info = new LoginInfo();
                string user_id = Session["User_ID"] == null ? "" : Session["User_ID"].ToString();
                string user_name = Session["User_Name"] == null ? "" : Session["User_Name"].ToString();
                string token = Session["Token"] == null ? "" : Session["Token"].ToString();
                Login_Info.User_ID = user_id;
                Login_Info.User_Name = user_name;
                Login_Info.Token = token;
            }
        }
    }
}
