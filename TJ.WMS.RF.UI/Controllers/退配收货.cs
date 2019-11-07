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
    public class RedistController : BaseController
    {
        //
        // GET: /Redist/
        RedistService service = null;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidatPaperNO(Redist model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string PaperNO = this.Request.Form["ID"];
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("PaperTypeID", "3");//退配收
                service.ValidatPaperNO();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult Detail(Redist model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            model.PaperNO = Request.Form["hdPaperNO"];
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
    Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                DataTable dt = service.QueryStoreInfo();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("<script>alert('退配单输入有误!')</script>");
                }
                else
                {
                    model.Storeinfo = dt.Rows[0]["Storeinfo"].ToString();
                    model.TrayNO = dt.Rows[0]["TrayNO"].ToString();
                }
            }
            catch (RFException rfex)
            {
                Loger.Error(rfex.Message, rfex);
                return Content(string.Format("<script>alert('{0}')</script>", rfex.Message));
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(string.Format("<script>alert('{0}')</script>", ex.Message));
            }
            return View(model);
        }

        public ActionResult ValidateBarcode(Redist model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
             try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.ValidateBarcode();
                DataTable dt = service.QueryGoods();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("商品条码输入有误!");
                }
                else
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                //return Content("");
            }
             catch (Exception ex)
             {
                 Loger.Error(ex.Message, ex);
                 return Content(ex.Message);
             }
        }

        public ActionResult Save(Redist model)
        { 
            
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Qty", model.Qty);
                service.SetParameter("PaperTypeID", "3");//配送收
                service.ExecuteBusinessProcess("RF_ReDistModify");
               // service.Accept();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult Over(Redist model)
        {

            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Qty", model.Qty);
                service.ExecuteBusinessProcess("RF_ReDistAcptOK");
                //service.Over();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateTrayNO(Redist model)
        {

            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RedistService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Qty", model.Qty);
                service.ValidateTrayNO();
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
