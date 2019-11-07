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
    public class ReplenishController : BaseController
    {
        //
        // GET: /Replenish/
        ReplenishService service;

        //零拣位补货
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            //<!--AssignTypeID=2.托拣;3.箱拣;4.零拣;5.自动补货;6.手工移仓;7.全仓补货-->
            //<!--GrpTypeID=1.箱拣位零拣补货;2.地堆/存储位零拣补货-->
            //只处理补货指令
            object GrpTypeID = Request.QueryString["GrpTypeID"];
            string sType = GrpTypeID.ToString();
            switch (sType)
            {
                case "1":
                    sType = "rep_x2l";//箱拣位零补
                    break;
                case "2":
                    sType = "rep_c2l";//存储位零补
                    break;
                default:
                    sType = "";
                    break;
            }
            if (sType == "")
            {
                return Redirect("/Menu");
            }
            service.SetParameter("Type", sType);
            DataTable dt = service.GetRF_Assignment();
            if (dt != null && dt.Rows.Count > 0)
            {
                string sAssignTypeID = dt.Rows[0]["AssignTypeID"].ToString();
                ViewData["StoreDesc"] = dt.Rows[0]["StoreNO"].ToString() + "." + dt.Rows[0]["StoreDesc"].ToString();
                ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                ViewData["LocationNO"] = dt.Rows[0]["LocationNO"].ToString();
                //ViewData["SrcQty"] = dt.Rows[0]["SrcQty"].ToString();
                ViewData["FromLocationNO"] = dt.Rows[0]["FromLocationNO"].ToString();//拣货储位
                ViewData["FromTrayNO"] = dt.Rows[0]["FromTrayNO"].ToString();//拣货托盘
                ViewData["ToDoNO"] = dt.Rows[0]["ToDoNO"].ToString();
                ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                ViewData["AssignTypeID"] = sAssignTypeID;// dt.Rows[0]["AssignTypeID"].ToString();
                //ViewData["ShowPickQty"] = dt.Rows[0]["ShowPickQty"].ToString();//提示拣货数量
                ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
                //ViewData["MinSrcQty"] = dt.Rows[0]["MinSrcQty"].ToString();
                ViewData["LocationTypeID"] = dt.Rows[0]["LocationTypeID"].ToString();
                ViewData["GrpTypeID"] = dt.Rows[0]["GrpTypeID"].ToString();
                ViewData["CaseSrcQty"] = dt.Rows[0]["CaseSrcQty"].ToString();
                if (sAssignTypeID == "2") // 进入托拣的箱补任务，跳到托拣界面
                {
                    return Redirect("/PICK?AssignTypeID=2");
                }
                else
                {
                    return View("");
                }
            }
            else //无指令执行
            {
                return View("NoReplenish");
            } 
        }

        /// <summary>
        /// 箱拣位补货
        /// </summary>
        /// <returns></returns>
        public ActionResult BoxRep()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);

            service.SetParameter("Type", "rep_pick_t");//箱补/托拣
            DataTable dt = service.GetRF_Assignment();
            if (dt != null && dt.Rows.Count > 0)
            {
                string sAssignTypeID = dt.Rows[0]["AssignTypeID"].ToString();
                ViewData["StoreDesc"] = dt.Rows[0]["StoreNO"].ToString() + "." + dt.Rows[0]["StoreDesc"].ToString();
                ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                ViewData["LocationNO"] = dt.Rows[0]["LocationNO"].ToString();
                //ViewData["SrcQty"] = dt.Rows[0]["SrcQty"].ToString();
                ViewData["FromLocationNO"] = dt.Rows[0]["FromLocationNO"].ToString();//拣货储位
                ViewData["FromTrayNO"] = dt.Rows[0]["FromTrayNO"].ToString();//拣货托盘
                ViewData["ToDoNO"] = dt.Rows[0]["ToDoNO"].ToString();
                ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                //ViewData["AssignTypeID"] = sAssignTypeID;// dt.Rows[0]["AssignTypeID"].ToString();
                //ViewData["ShowPickQty"] = dt.Rows[0]["ShowPickQty"].ToString();//提示拣货数量
                ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
                //ViewData["MinSrcQty"] = dt.Rows[0]["MinSrcQty"].ToString();
                //ViewData["LocationTypeID"] = dt.Rows[0]["LocationTypeID"].ToString();
                //ViewData["GrpTypeID"] = dt.Rows[0]["GrpTypeID"].ToString();
                ViewData["Spec"] = dt.Rows[0]["Spec"].ToString();
                ViewData["CaseQty"] = dt.Rows[0]["CaseQty"].ToString();
                if (sAssignTypeID == "2") // 进入托拣的箱补任务，跳到托拣界面
                {
                    return Redirect("/PICK?AssignTypeID=2");
                }
                else
                {
                    return View("");
                }
            }
            else //无指令执行
            {
                //return View("NoReplenish");
                return Redirect("/PICK?AssignTypeID=2");
            } 
        }
        /// <summary>
        /// 箱拣位补货,效验拣货托盘
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateBoxRepFromTrayNO(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.ExecuteBusinessCheck("RF_ReplenishCheck", "FromTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 箱拣位补货,补货目标储位并提交补货
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult PickBoxRep(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("LocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.ExecuteBusinessCheck("RF_ReplenishCheck", "LocationNO");
                service.ExecuteBusinessProcess("RF_ReplenishCheck");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }


        public ActionResult ValidateTrayNO(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GrpTypeID", model.GrpTypeID);
                service.ExecuteBusinessCheck("RF_RepRetailPick", "TrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateFromLocationNO(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GrpTypeID", model.GrpTypeID);
                service.ExecuteBusinessCheck("RF_RepRetailPick", "FromLocationNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public ActionResult ValidateFromTrayNO(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GrpTypeID", model.GrpTypeID);
                service.ExecuteBusinessCheck("RF_RepRetailPick", "FromTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 提交业务过程处理拣货到中间托盘
        /// </summary>
        public ActionResult PickGoods(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GrpTypeID", model.GrpTypeID);
                service.ExecuteBusinessCheck("RF_RepRetailPick", "*");
                service.ExecuteBusinessProcess("RF_RepRetailPick");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        
        /// <summary>
        /// 跳过指令
        /// </summary>
        /// <returns></returns>
        public ActionResult PickSkip(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("GrpTypeID", model.GrpTypeID);
                service.ExecuteBusinessCheck("RF_RepRetailPickSkip", "*");
                service.ExecuteBusinessProcess("RF_RepRetailPickSkip");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 输入补货上架中间托盘视图
        /// </summary>
        /// <returns></returns>
        ///
        public ActionResult RepInputTray()
        {
            return View("");
        }

        /// <summary>
        /// 提交中间托盘检查是否有未上架数据
        /// </summary>
        /// <returns></returns>
        ///
        public ActionResult SubmitMiddleTrayNO(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                DataTable dt = service.SubmitMiddleTrayNO("RF_RepRetailGoods");
                if (dt != null && dt.Rows.Count > 0) //
                  return Content("");
                else
                    return Content("托盘无效，请重新输入");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 补货上架页面视图
        /// </summary>
        /// <returns></returns>
        ///
        public ActionResult RepShelves()
        {
            object TrayNO = Request.QueryString["TrayNO"];
            if ((TrayNO == null)||(TrayNO.ToString()==""))
            {
                return Content("<script>location.href='/Replenish/RepInputTray'</script>");
            }
            else
            {
                GetLoginInfo();
                if (Login_Info == null)
                {
                    return Content("<script>location.href='/Home'</script>");
                }
                service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("TrayNO", TrayNO.ToString());
                DataTable dt = service.SubmitMiddleTrayNO("RF_RepRetailGoods");
                if (dt != null && dt.Rows.Count > 0) //
                {
                    ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                    ViewData["Barcode"] = dt.Rows[0]["Barcode"].ToString();
                    ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                    ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                    ViewData["CaseQty"] = dt.Rows[0]["CaseQty"].ToString();
                    ViewData["ToLocationNO"] = dt.Rows[0]["ToLocationNO"].ToString();
                    ViewData["TrayNO"] = dt.Rows[0]["TrayNO"].ToString();
                    return View("");
                }
                else
                    return Content("<script>alert('没有补货数据，确认后返回！');location.href='/Replenish/RepInputTray'</script>;");
            }
        }
        /// <summary>
        /// 效验上架条码
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult ValidateRepBarcode(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("PaperNO", model.PaperNO);
                service.ExecuteBusinessCheck("RF_RepRetailShelves", "Barcode");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交处理补货上架过程
        /// </summary>
        /// <returns></returns>
        /// 
        public ActionResult SubmitRepShelves(ReplenishViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new ReplenishService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("ToLocationNO", model.ToLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.ExecuteBusinessCheck("RF_RepRetailShelves", "*");
                service.ExecuteBusinessProcess("RF_RepRetailShelves");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

    }
}
