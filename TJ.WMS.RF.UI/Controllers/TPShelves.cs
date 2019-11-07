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
    public class TPShelvesController : BaseController
    {
        //
        // GET: /Redist/
        TPShelvesService service = null;
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }
        public ActionResult ValidatPaperNO(TPShelves model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            string PaperNO = this.Request.Form["ID"];
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", PaperNO);
                //service.SetParameter("PaperTypeID", "3");//退配收
                DataTable dt = service.ValidatPaperNO();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("订单不存在！");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult Detail(TPShelves model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            //model.PaperNO = Request.Form["hdPaperNO"];
            //model.ReceiptTypeID = Request.Form["hdReceiptTypeID"];
            //model.BatchTypeID = Request.Form["hdBatchTypeID"];
            model.PaperNO = Request.QueryString["PaperNO"];
            model.ReceiptTypeID = Request.QueryString["ReceiptTypeID"];
            model.BatchTypeID = Request.QueryString["BatchTypeID"];
            //  model.TrayNO = Request.Form["TrayNO"];
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
    Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
               // service.SetParameter("TrayNO", model.TrayNO);

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

        public ActionResult ValidateBarcode(TPShelves model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("BatchTypeID", model.BatchTypeID);
                service.SetParameter("ReceiptTypeID", model.ReceiptTypeID);
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

       ///生产日期
        public ActionResult ValidateProductDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string productDate = Request.Form["ProductDate"];

            try
            {
                service.SetParameter("ProductDate", productDate);
                service.ValidateProductDate();

                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        ///到期日期
        public ActionResult ValidateEffectiveDate()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
                Login_Info.Token);
            string ProductDate = Request.Form["ProductDate"];
            string effectiveDate = Request.Form["EffectiveDate"];
            string shelfLife = Request.Form["shelfLife"];
            string Barcode = Request.Form["Barcode"];
            string PaperNO = Request.Form["PaperNO"];
            try
            {
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("EffectiveDate", effectiveDate);
                service.SetParameter("shelfLife", shelfLife);
                service.SetParameter("Barcode", Barcode);
                service.SetParameter("PaperNO", PaperNO);
                service.ValidateEffectiveDate();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ActionResult ValidateLocationNO(TPShelves model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("BatchTypeID", model.BatchTypeID);
                service.SetParameter("ReceiptTypeID", model.ReceiptTypeID);
                service.SetParameter("LocationNO", model.LocationNO);
                service.ValidateLocationNO();

                return Content("");
                //return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult QryGoodsStock(TPShelves model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("ProductDate", model.ProductDate);
                DataTable dt = service.QueryGoodsStock();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult Save(TPShelves model)
        {

            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Qty", model.Qty);
                service.SetParameter("PaperTypeID", "3");//配送收
                service.SetParameter("BatchTypeID", model.BatchTypeID);
                service.SetParameter("ReceiptTypeID", model.ReceiptTypeID);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("EffectiveDate", model.EffectiveDate);
                service.SetParameter("ShelfLife", model.ShelfLife);
                service.ValidateLocationNO();
                service.Accept();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
    
        public ActionResult Over(TPShelves model)
        {

            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
              //  service.SetParameter("Barcode", model.Barcode);
                //service.SetParameter("TrayNO", model.TrayNO);
                //service.SetParameter("Qty", model.Qty);
                service.Over();
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateTrayNO(TPShelves model)
        {

            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new TPShelvesService(Login_Info.User_ID, Login_Info.User_Name,
   Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("BatchTypeID", model.BatchTypeID);
                service.SetParameter("ReceiptTypeID", model.ReceiptTypeID);
                //service.SetParameter("Qty", model.Qty);
                service.ValidateTrayNO();
                DataTable dt = service.QueryGoods();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("商品条码输入有误!");
                }
                else
                {
                    return Content(JsonHelper.ToJson(dt));
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

    }
}

