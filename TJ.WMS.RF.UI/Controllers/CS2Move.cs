using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.UI.Models;
using TJ.WMS.RF.Service;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class CS2MoveController : BaseController
    {
        #region 参数定义
        //string token = "";
        //CS2Move service;
        CS2MoveService service;
        #endregion
        public ActionResult Index(string id)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }

            StdQuery StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            DataTable dt = StdQuery1.Execute("RF_AutoLocationNO");

            if (dt != null && dt.Rows.Count > 0)
            {
                ViewData["isAutoLocationNO"] = dt.Rows[0]["isAutoLocationNO"].ToString();
            }
            else
            {
                ViewData["isAutoLocationNO"] = "0";//CS2越库移仓是否要自动录入新储位0为手动
            }
            //ViewData["ToDay"] = DateTime.Today;
            return View();
        }


        #region 数据合法性验证
        public ActionResult ValidateTrayNO(string TrayNo, string isAutoLocationNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {

                // service = new CS2MoveService(Login_Info.User_ID, Login_Info.User_Name,
                //Login_Info.Token);
                // service.SetParameter("TrayNO", TrayNo);
                // service.SetParameter("LocationNO", LocationNO);
                // service.ValidateTrayNO();
                service = new CS2MoveService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("TrayNO", TrayNo);
                service.ExecuteBusinessCheck("RF_CS2MoveAdd", "TrayNO");

                StdQuery StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                StdQuery1.SetParameter("TrayNO", TrayNo);
               // StdQuery1.SetParameter("LocationNO", isAutoLocationNO);
                DataTable dt = StdQuery1.Execute("RF_CS2Goods");

                if (dt != null && dt.Rows.Count > 0)
                {
        
                    if (isAutoLocationNO != "0"&& isAutoLocationNO!="")
                    {
                        service = new CS2MoveService(Login_Info.User_ID, Login_Info.User_Name,
              Login_Info.Token);

                        service.SetParameter("PaperNO", dt.Rows[0]["PaperNO"].ToString());
                        service.SetParameter("TrayNO", TrayNo);
                        service.SetParameter("ToLocationNO", isAutoLocationNO);
                        service.SetParameter("CS2PaperNO", dt.Rows[0]["ResNO"].ToString());
                        service.SetParameter("ShipperNO", dt.Rows[0]["ShipperNO"].ToString());
                        service.SetParameter("OldLocationNO", dt.Rows[0]["LocationNO"].ToString());
                        service.Accept();
                        StdQuery StdQuery2 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                        StdQuery2.SetParameter("TrayNo", TrayNo);
                        dt = StdQuery2.Execute("RF_CS2Goods");
                        //return Content("");

                    }
                    return Content(JsonHelper.ToJson(dt));

                }
                else
                {
                    return Content("原托盘输入有误！");
                }


                //return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateToLocationNO(string TrayNO,string OldLocationNO, string ToLocationNO, string PaperNO,string CS2PaperNO,string ShipperNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CS2MoveService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("ToLocationNO", ToLocationNO);
                service.SetParameter("CS2PaperNO", CS2PaperNO);
                service.SetParameter("ShipperNO", ShipperNO);
                service.SetParameter("OldLocationNO", OldLocationNO);
                service.Accept();
                return Content("");

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }
        #endregion
    }
}
