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
    public class CS2AcptCFMController : BaseController
    {
       
        #region 参数定义
        //string token = "";
        DcAccept acpt;
        StdQuery StdQuery1;
        RFBase service;

        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            //ViewData["ToDay"] = DateTime.Today;
            string id = Request.Form["hdCS2AcptCFM"];
            ViewData["BillCode"] = id;
            return View();
        }

        public ActionResult Detele(string OrderNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            
            ViewData["BillCode"] = OrderNO;
            return View();
        }


        public ActionResult ValidateCS2AcptCFMBarCode()
        {
            string Barcode = Request.Form["Barcode"];
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string isCheck = Request.Form["isCheck"];
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            acpt = new DcAccept(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                ///0001201306070001
                //service.SetParameter("Barcode", Barcode.Trim());
                //service.SetParameter("OrderNO", OrderNO);
                //service.SetParameter("TrayNO", TrayNO);
                //service.SetParameter("isCheck", isCheck);
                //service.ExecuteBusinessCheck("RF_CS2Acpt2CMF", "Barcode");

                StdQuery1.SetParameter("Barcode", Barcode.Trim());
                StdQuery1.SetParameter("OrderNO", OrderNO);
                StdQuery1.SetParameter("TrayNO", TrayNO);
                StdQuery1.SetParameter("isCheck", isCheck);
                DataTable dt = StdQuery1.Execute("RF_CS2AcptCFMGoods");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("当前订单不存在该商品!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateCS2AcptCFMTrayNO(string OrderNO,string TrayNO,string Barcode,string isCheck)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);

            try
            {
                StdQuery1.SetParameter("Barcode", Barcode.Trim());
                StdQuery1.SetParameter("OrderNO", OrderNO);
                StdQuery1.SetParameter("TrayNO", TrayNO);
                StdQuery1.SetParameter("isCheck", isCheck);
                DataTable dt = StdQuery1.Execute("RF_CS2AcptCFMGoods");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("当前订单不存在该商品!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
  
        /// <summary>
        /// 确认收货
        /// </summary>
        /// <returns></returns>
        public ActionResult CS2AcptConfirm()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
            string GoodsID = Request.Form["GoodsID"];
            string isCheck = Request.Form["isCheck"];
            string Barcode = Request.Form["Barcode"];
            string ProductDate = Request.Form["ProductDate"];
            string shelfLife = Request.Form["shelfLife"];
            string EffectiveDate = Request.Form["EffectiveDate"];
           // string Qty = Request.Form["Qty"];
          //  string TrayTag = null;
            try
            {
                service.SetParameter("isCheck", isCheck);
                service.SetParameter("OrderNO", OrderNO);
                service.SetParameter("Barcode", Barcode);
                service.SetParameter("GoodsID", GoodsID);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("shelfLife", shelfLife);
                service.SetParameter("EffectiveDate", EffectiveDate);
                // acpt.SetParameter("Qty", Qty);
                // acpt.ExecuteBusinessCheck("RF_CS2Acpt2Modify", "*");

                //DataTable dt = acpt.QueryOnlyAllowBoxAcpt();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    TrayTag = dt.Rows[0]["Tag"].ToString();

                //}
                //if(TrayTag == "")


                service.ExecuteBusinessProcess("RF_CS2Acpt2CMF");
                //*************20140724**********************
                //acpt.paras.Clear();
                //acpt.SetParameter("Barcode", Barcode.Trim());
                //acpt.SetParameter("OrderNO", OrderNO);
                //DataTable dt = acpt.QueryDcacptQty();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //return Content(JsonHelper.ToJson(dt));
                //}
                //*******************************************
                if (service.paras["Complete"].Value.ToString() != "true")
                {
                    return Content("0");
                }
                else
                {
                    return Content("");
                }

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }
        public ActionResult CS2AcptCFMDetele()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            string TrayNO = Request.Form["TrayNO"];
        
            try
            {
        
                service.SetParameter("OrderNO", OrderNO);
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("RF_CS2AcptCMFD");
                return Content("");

            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }

        }

        public ActionResult Over()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string OrderNO = Request.Form["OrderNO"];
            try
            {
                service.SetParameter("OrderNO", OrderNO);
                service.ExecuteBusinessProcess("RF_CS2AcptCMFOver");
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
