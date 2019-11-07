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
    public class LocationCollectionController : BaseController
    {
        StdQuery StdQuery1;
        RFBase service;
        public ActionResult Index()
        {
      
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            return View();
        }
        /// <summary>
        /// 校验储位
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateLocationNO(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                StdQuery1.SetParameter("LocationNO", model.LocationNO);
                DataTable dt = StdQuery1.Execute("RF_QryLCLocation");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("储位编码无效！");
                }

            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 校验托盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateTrayNO(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                StdQuery1.SetParameter("LocationNO", model.LocationNO);
                StdQuery1.SetParameter("TrayNO", model.TrayNO);
                DataTable dt = StdQuery1.Execute("RF_QryLCTray");

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["LocationNO"].ToString() != "")
                    {
                        if (dt.Rows[0]["Barcode"].ToString() != "")
                        {
                            return Content(JsonHelper.ToJson(dt));
                        }
                        else { return Content("托盘不属于该储位！"); }
                    }
                    else {
                        return Content("储位无效！");
                    }
                    
                }
                else
                {
                    if (dt.Columns.Count == 1)
                    {
                        return Content("托盘无效！");
                    }
                    return Content("储位无效！");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 校验条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ValidateBarcode(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                StdQuery1.SetParameter("LocationNO", model.LocationNO);
                StdQuery1.SetParameter("TrayNO", model.TrayNO);
                StdQuery1.SetParameter("Barcode", model.Barcode);
                DataTable dt = StdQuery1.Execute("RF_QryLCGoods");

                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("没有相关记录！");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex.Message, ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 采集保存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Save(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
         
            try
            {
                service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("DealQty", model.DealQty);
                service.SetParameter("StorageTypeID", model.StorageTypeID);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.ExecuteBusinessProcess("RF_LCAdd");
                StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                string ResultValue = service.GetParameter("ResultValue").ToString();
                return Content(ResultValue);
            }
            catch (Exception ex)
            {
                return Content("error" + ex.Message);
            }
        }

        /// <summary>
        /// 查询储位未采集的托盘数
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult QryMissTray(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                DataTable dt = StdQuery1.Execute("RF_QryMissTray");
                if (dt != null && dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["ResultValue"].ToString() != "0")
                    {
                        return Content(JsonHelper.ToJson(dt));
                    }
                    else {
                        service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                        service.ExecuteBusinessProcess("RF_LCPost");
                        return Content("");
                    }
                    
                }
                else
                {
                    return Content("没有要提交的货架采集单！");
                }

            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 采集提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult Post(LocationCollectionModels model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.ExecuteBusinessProcess("RF_LCPost");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
    }
}
