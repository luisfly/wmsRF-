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
    public class InternalPickController : BaseController
    {
        StdQuery StdQuery1;
        RFBase service;
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询显示拣货单任务
        /// </summary>
        /// <returns></returns>
        public ActionResult RFQryItlPickTask()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                DataTable dt = StdQuery1.Execute("RFQryItlPickTask");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("没有拣货任务，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 商品拣货任务
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <returns></returns>
        public ActionResult Detail(string PaperNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("PaperNO", PaperNO);
                DataTable dt = StdQuery1.Execute("RFQryItlPickGoods");
                ViewBag.data = dt;
                return View();
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                //    ViewData["OutShipper"] = dt.Rows[0]["OutShipper"].ToString();
                //    ViewData["InShipper"] = dt.Rows[0]["InShipper"].ToString();
                //    ViewData["GoodsID"] = dt.Rows[0]["GoodsID"].ToString();
                //    ViewData["GoodsNO"] = dt.Rows[0]["GoodsNO"].ToString();
                //    ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                //    ViewData["ProductDate"] = dt.Rows[0]["ProductDate"].ToString();
                //    ViewData["ApplyQty"] = dt.Rows[0]["ApplyQty"].ToString();
                //    ViewData["SaleQty"] = dt.Rows[0]["SaleQty"].ToString();
                //    return View();
                //}
                //else
                //{
                //    return Content("没有拣货任务，请刷新重新！");
                //}
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 查询商品可拣货批次明细
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RFQryPickBatchDtl(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("PaperNO", model.PaperNO);
                StdQuery1.SetParameter("GoodsID", model.GoodsID);
                StdQuery1.SetParameter("ProductDate", model.ProductDate);
                StdQuery1.SetParameter("StorageNO", model.StorageNO);
                DataTable dt = StdQuery1.Execute("RFQryPickBatchDtl");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("没有拣货批次库存，请刷新重试！");
                }
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult ExecBusinessCheck(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("NewLocationNO", model.NewLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("OutShipperNO", model.OutShipperNO);
                service.SetParameter("StorageNO", model.StorageNO);
                service.ExecuteBusinessCheck("RF_ItlPickGoodsAdd", model.ParamName);
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }

        }

        public ActionResult RF_ItlPickGoodsAdd(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("NewLocationNO", model.NewLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("OutShipperNO", model.OutShipperNO);
                service.SetParameter("StorageNO", model.StorageNO);
                service.ExecuteBusinessProcess("RF_ItlPickGoodsAdd");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 处理0拣货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RF_ItlPickGoodsZero(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("NewLocationNO", model.NewLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("OutShipperNO", model.OutShipperNO);
                service.SetParameter("StorageNO", model.StorageNO);
                service.ExecuteBusinessProcess("RF_ItlPickGoodsZero");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 处理跳过
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RF_ItlPickGoodsSkip(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("NewLocationNO", model.NewLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("OutShipperNO", model.OutShipperNO);
                service.SetParameter("StorageNO", model.StorageNO);
                service.ExecuteBusinessProcess("RF_ItlPickGoodsSkip");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 完成拣货任务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RF_ItlPickTaskOver(InternalPickModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GoodsID", model.GoodsID);
                service.SetParameter("ProductDate", model.ProductDate);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.SetParameter("NewLocationNO", model.NewLocationNO);
                service.SetParameter("LocationNO", model.LocationNO);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("OutShipperNO", model.OutShipperNO);
                service.SetParameter("StorageNO", model.StorageNO);
                service.ExecuteBusinessProcess("RF_ItlPickTaskOver");
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