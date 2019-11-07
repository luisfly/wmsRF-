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
    public class StockMoveTaskController: BaseController
    {
        StdQuery StdQuery1;
        RFBase service;
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 查询显示移仓任务
        /// </summary>
        /// <returns></returns>
        public ActionResult RF_QryMoveTask()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                DataTable dt = StdQuery1.Execute("RF_QryMoveTask");
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
                DataTable dt = StdQuery1.Execute("RF_QryMoveGoods");
                ViewBag.data = dt;
                return View();
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
        public ActionResult RF_QtyMoveGoodsDtl(StockMoveTaskModel model)
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
                StdQuery1.SetParameter("FromStorageNO", model.FromStorageNO);
                DataTable dt = StdQuery1.Execute("RF_QtyMoveGoodsDtl");
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
        public ActionResult ExecBusinessCheck(StockMoveTaskModel model)
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
                service.SetParameter("FromStorageNO", model.FromStorageNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                //service.SetParameter("NewLocationNO", model.NewLocationNO);
                
                service.ExecuteBusinessCheck("RF_MoveStoragePick", model.ParamName);
                if (model.ParamName == "NewTrayNO")
                    service.ExecuteBusinessProcess("RF_MoveStoragePick");
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
        public ActionResult RF_MoveStorageZero(StockMoveTaskModel model)
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
                service.SetParameter("FromStorageNO", model.FromStorageNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO); 
                service.ExecuteBusinessProcess("RF_MoveStorageZero");
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
        public ActionResult RF_MoveStoragSkip(StockMoveTaskModel model)
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
                service.SetParameter("FromStorageNO", model.FromStorageNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("PickQty", model.PickQty);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.ExecuteBusinessProcess("RF_MoveStoragSkip");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 移仓上架
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <returns></returns>
        public ActionResult Detail2(string PaperNO)
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
                DataTable dt = StdQuery1.Execute("RF_QryMovePutTask");
                ViewBag.data = dt;
                return View();
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        public ActionResult RF_QryMovePutGoods(StockMoveTaskModel model)
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
                StdQuery1.SetParameter("NewTrayNO", model.NewTrayNO);
                DataTable dt = StdQuery1.Execute("RF_QryMovePutGoods");
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("数据异常，请重试！");
                }
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 移仓上架
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult RF_MoveStoragePut(StockMoveTaskModel model)
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
                service.SetParameter("ToStorageNO", model.ToStorageNO);
                service.SetParameter("ToLocationNO", model.ToLocationNO);
                service.SetParameter("NewTrayNO", model.NewTrayNO);
                service.ExecuteBusinessCheck("RF_MoveStoragePut", model.ParamName);
                if (model.ParamName == "ToLocationNO")
                    service.ExecuteBusinessProcess("RF_MoveStoragePut");
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