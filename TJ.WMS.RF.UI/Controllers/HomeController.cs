using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.UI.Models;
using TJ.WMS.RF.Service;
using System.Configuration;


namespace TJ.WMS.RF.UI.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            //if (Session["User_ID"] != null)
            //{
            //   return Redirect("/Menu");
            //}
            //Response.Write("<a style='text-decoration: none; color: #0954A6; font-family: 宋体, Arial, Helvetica, sans-serif; font-size: 12px;' href='http://www.FSTJGL.com'>锐志诚达电子科技</a>");
            Session["User_ID"] = null;
            string TitleInfo = ConfigurationManager.AppSettings["TitleInfo"];
            Session["CompanyTitle"] = "锐志启航WMS_RF系统"+TitleInfo;
            return View();
            
        }

        public ActionResult Submit(UserModel model)
        {
            string user_id = model.User_ID;
            string passsword = model.Password;
            if (model == null)
            {
                return Content("您输入的数据不合法！");
            }
            UserLogin loginService = new UserLogin();
            string username=string.Empty;
            string token=string.Empty;
            try
            {
                if (loginService.Login(user_id, passsword, ref username, ref token))
                {
                    Session["User_ID"] = user_id;
                    //string sessionTime = ConfigurationManager.AppSettings["sessionTime"];
                    //int time = 30;
                    //if(!int.TryParse(sessionTime,out time))
                    //{
                    //    time=30;
                    //}
                    //Session.Timeout = time;
                    Session["User_Name"] = username;
                    Session["Token"] = token;
                    return Content("");
                }
                else
                {
                    return Content("用户名或者密码输入不正确！");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content("服务异常，请联系管理员");
            }
            
        }
    }
}
