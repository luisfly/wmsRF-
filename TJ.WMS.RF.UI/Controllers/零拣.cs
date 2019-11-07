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
    public class PICKController : BaseController
    {
        //
        // GET: /PICK/
        PickGoodsBase service;
        public ActionResult SubmitOverDialog()
        {
            //ViewData["ID"] = Request.QueryString["ID"];
            return View("");
        }

        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            //<!--AssignTypeID=2.托拣;3.箱拣;4.零拣-->
            object AssignTypeID = Request.QueryString["AssignTypeID"];
            string OldFromTrayNO = Request.QueryString["OldFromTrayNO"];
            string OldPickBarcode = Request.QueryString["OldPickBarcode"];
            if (AssignTypeID == null)
            {
                return Redirect("/Menu"); 
            }
            string sType = AssignTypeID.ToString();
            switch (sType)
            {
                case "4":
                    sType = "pick_l";//零拣
                    break;
                case "3":
                    sType = "pick_x";//箱拣
                    break;
                case "2":
                    sType = "rep_pick_t";//箱补/托拣
                    break;
                case "0":
                    sType = "pick";//拣货，托、箱、零
                    break;
                default:
                    sType = "";
                    break;
            }
            if (sType=="")
            {
                return Redirect("/Menu");
            }
            service.SetParameter("Type", sType);
            DataTable dt = service.GetRF_Assignment();
            if (dt != null && dt.Rows.Count > 0)
            {
                string sAssignTypeID = dt.Rows[0]["AssignTypeID"].ToString();
                string Barcode = dt.Rows[0]["Barcode"].ToString();
                string FromTrayNO = dt.Rows[0]["FromTrayNO"].ToString();
                ViewData["StoreDesc"] = dt.Rows[0]["StoreNO"].ToString()+"."+dt.Rows[0]["StoreDesc"].ToString();
                ViewData["Barcode"] = Barcode;
                ViewData["GoodsDesc"] = dt.Rows[0]["GoodsDesc"].ToString();
                ViewData["CaseUnits"] = dt.Rows[0]["CaseUnits"].ToString();
                ViewData["LocationNO"] = dt.Rows[0]["LocationNO"].ToString();
                ViewData["SrcQty"] = dt.Rows[0]["SrcQty"].ToString();
                ViewData["FromLocationNO"] = dt.Rows[0]["FromLocationNO"].ToString();//拣货储位
                ViewData["FromTrayNO"] = FromTrayNO;//拣货托盘
                ViewData["ToDoNO"] = dt.Rows[0]["ToDoNO"].ToString();
                ViewData["PaperNO"] = dt.Rows[0]["PaperNO"].ToString();
                ViewData["AssignTypeID"] = sAssignTypeID;// dt.Rows[0]["AssignTypeID"].ToString();
                ViewData["ShowPickQty"] = dt.Rows[0]["ShowPickQty"].ToString();//提示拣货数量
                ViewData["Unit"] = dt.Rows[0]["Unit"].ToString();
                ViewData["TrayNO"] = dt.Rows[0]["TrayNO"].ToString();
                string TrayNO = ViewData["TrayNO"].ToString();
                ViewData["MinSrcQty"] = dt.Rows[0]["MinSrcQty"].ToString();
                ViewData["LocationTypeID"] = dt.Rows[0]["LocationTypeID"].ToString();
                ViewData["BoxPickValidateBarcode"] = dt.Rows[0]["BoxPickValidateBarcode"].ToString();//20161026 TIM 1.箱拣效验条码;2.箱拣不效验条码
                ViewData["PickTypeID"] = dt.Rows[0]["PickTypeID"].ToString();//20180814 YZW 拣货方式: 0.按箱总量拣货;1.按单箱拣货
           // ViewData["PickTypeID"] = "0";
                ViewData["NeedCasePickQty"] = dt.Rows[0]["NeedCasePickQty"].ToString();
                if (FromTrayNO == OldFromTrayNO && String.Compare(Barcode, OldPickBarcode, true) == 0 )
                {
                    ViewData["IsPickSame"] = "true";
                }
                else {
                    ViewData["IsPickSame"] = "false";
                }
                if (sAssignTypeID == "5" || sAssignTypeID == "7") // 进入箱补/托拣的箱补任务，跳到箱补界面
                {
                    //Response.Write("<script>alert('箱补未启用')</script>");
                    return Redirect("/Replenish/BoxRep");
                    
                }
                else
                {
                    return View("");
                }
            }
            else //无指令执行
            {
                return View("NoPick");
            } 
        }

        /// <summary>
        /// 检查出库箱
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateTrayNO(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("AssignTypeID", model.AssignTypeID);
                service.SetParameter("PickTypeID", model.PickTypeID);
                //service.ValidateTrayNO();
                service.ExecuteBusinessCheck("RF_PickGoods", "ToTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// 检查拣货托盘
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateFromTrayNO(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                //service.ValidateTrayNO();
                service.ExecuteBusinessCheck("RF_PickGoods", "FromTrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// 验证拣货位
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateCheckLocationNO(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("AssignTypeID", model.AssignTypeID);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("BoxPickValidateBarcode", model.BoxPickValidateBarcode);
                //service.ValidateCheckLocationNO();
                service.ExecuteBusinessCheck("RF_PickGoods", "CheckLocationNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateBarcode(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("AssignTypeID", model.AssignTypeID);
                service.SetParameter("BoxPickValidateBarcode", model.BoxPickValidateBarcode);
                //service.ValidateBarcode();
                service.ExecuteBusinessCheck("RF_PickGoods", "Barcode");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 实拣数量检查
        /// </summary>
        /// <returns></returns>
        public ActionResult ValidateDealQty(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                //AssignTypeID=2.托拣;3.箱拣;4.零拣
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("AssignTypeID", model.AssignTypeID);
                service.SetParameter("BoxPickValidateBarcode", model.BoxPickValidateBarcode);
                service.SetParameter("DealQty", model.DealQty);
                //if (model.AssignTypeID == "3")
                //{
                //    service.SetParameter("DealQty", int.Parse(model.DealQty) * model.CaseUnits);
                //}
                //else
                //{
                //    if (model.AssignTypeID == "2")
                //    {
                //        service.SetParameter("DealQty", model.MinSrcQty);
                //    }
                //    else
                //    {
                //        service.SetParameter("DealQty", model.DealQty);
                //    }
                //}
                //service.SetParameter("AssignTypeID", model.AssignTypeID);
                //service.ValidateDealQty();
                service.ExecuteBusinessCheck("RF_PickGoods", "DealQty");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交执行拣货过程
        /// </summary>
        /// <returns></returns>
        public ActionResult PickGoods(PickViewModel model)
        {
            string ParamValue;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("Stroe", model.Barcode);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("DealQty", model.DealQty);
                service.SetParameter("LocationTypeID", model.LocationTypeID);
                service.SetParameter("AssignTypeID", model.AssignTypeID);
                service.SetParameter("FromTrayNO", model.FromTrayNO);
                service.SetParameter("CheckFromTrayNO", model.CheckFromTrayNO);
                service.SetParameter("BoxPickValidateBarcode", model.BoxPickValidateBarcode);
                service.SetParameter("PickTypeID", model.PickTypeID);
                //service.ExecuteBusinessProcess("RF_PickGoods");
                ParamValue = service.PickGoods();
                return Content(ParamValue);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交释放指令过程
        /// </summary>
        /// <returns></returns>
        public ActionResult PickRelease(PickViewModel model)
        {
            string ParamValue;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                service.SetParameter("FromLocationNO", model.FromLocationNO);
                service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("DealQty", model.DealQty);
                //service.ExecuteBusinessCheck("RF_PickRelease","*");
                //service.ExecuteBusinessProcess("RF_PickRelease");
                ParamValue = service.PickRelease();
                return Content(ParamValue);
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
        public ActionResult PickSkip(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.SetParameter("ToDoNO", model.ToDoNO);
                service.SetParameter("PaperNO", model.PaperNO);
                //service.SetParameter("FromLocationNO", model.FromLocationNO);
                //service.SetParameter("CheckLocationNO", model.CheckLocationNO);
                service.SetParameter("Barcode", model.Barcode);
                //service.SetParameter("PickBarcode", model.PickBarcode);
                service.SetParameter("DealQty", model.DealQty);
                service.ExecuteBusinessCheck("RF_PickSkip", "*");
                service.ExecuteBusinessProcess("RF_PickSkip");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 提交出库箱
        /// </summary>
        /// <returns></returns>
        public ActionResult SubmitTrayNO(PickViewModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new PickGoodsBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("ToTrayNO", model.ToTrayNO);
                service.ExecuteBusinessCheck("RF_PickCheck", "*");
                service.ExecuteBusinessProcess("RF_PickCheck");
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
