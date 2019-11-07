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
    public class LoadCheckController : BaseController
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
        /// 新建装载复核的复核单号
        /// </summary>
        /// <param name="LoadNO"></param>
        /// <returns></returns>
        public ActionResult NewPaperNO(string LoadNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.ClearParameter();
                // 获取操作员的装载复核单号
                service.SetParameter("LoadNO", LoadNO);
                service.ExecuteBusinessProcess("LoadCheckAdd");
              
                // 获取商店编码、复核单号、商店名称、配送类型信息
                string PaperNO = service.GetParameter("PaperNO").ToString();
                string StoreNO = service.GetParameter("StoreNO").ToString();
                string StoreDesc = service.GetParameter("StoreDesc").ToString();
                string ProvTypeID = service.GetParameter("ProvTypeID").ToString();
                string TrayNum = service.GetParameter("TrayNum").ToString();
                string finish = service.GetParameter("finish").ToString();
                // return Content(ProvTypeID);
                return Json(new { isSuccess = true, PaperNO = PaperNO, ProvTypeID = ProvTypeID, StoreNO = StoreNO, StoreDesc = StoreDesc, TrayNum = TrayNum, finish = finish });
                //   return Content("[{\"isSuccess\":\"1\",\"sMessage\":\"\"," + "\"PaperNO\":\"" + service.GetParameter("PaperNO").ToString() + "\"}]");
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, sMessage = ex.Message.Replace("\"", "'") });
            }
        }


        /// <summary>
        /// 新增装载复核明细
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <param name="LoadNO"></param>
        /// <param name="TrayNO"></param>
        /// <param name="StoreNO"></param>
        /// <returns></returns>
        public ActionResult RF_LoadCheckTrayAdd(string PaperNO, string LoadNO, string TrayNO, string StoreNO)
        {
            // 登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            // fit 业务服务调用初始化
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                // fit参数设置
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("StoreNO", StoreNO);
                // 装载号与出货箱匹配的业务检查
                service.ExecuteBusinessCheck("LoadCheckTray", "TrayNO");
                // 新增记录执行
                service.ExecuteBusinessProcess("LoadCheckTray");
                int finish = int.Parse(service.GetParameter("finish").ToString());
                string Allfinish = service.GetParameter("Allfinish").ToString();
                switch (finish)
                {
                    // 0 为只有一个商品，并且数量等于箱规
                    case 0: return Json(new { finish = finish, Allfinish = Allfinish, sMessage = "" });break;
                    // 1 为只有一个商品，但是数量不等于箱规
                    case 1:
                    {
                            string Barcode = service.GetParameter("sBarcode").ToString();
                            string GoodsDesc = service.GetParameter("sGoodsDesc").ToString();
                            return Json(new { finish = finish, Barcode = Barcode, GoodsDesc = GoodsDesc, sMessage = "" });
                    }
                    break;
                    // 2 为多个商品并且商品不是一步越库，还是必须全部扫
                    case 2: return Json(new { finish = finish, sMessage = "" }); break;
                    // 3 为多个商品而且是一步越库，可以允许他不逐个扫描，带入默认值
                    case 3: return Json(new { finish = finish, sMessage = "" }); break;
                    default:break;
                }
                return Json(new { finish = finish, sMessage = "请检查finish的值"});
            }
            catch (Exception ex)
            {
                return Json(new {sMessage = ex.Message.Replace("\"", "'") });
            }
        }

        /// <summary>
        /// 检查托盘上的货物是否齐全
        /// </summary>
        /// <param name="PaperNO">复核单号</param>
        /// <param name="LoadNO">装载号</param>
        /// <param name="TrayNO">出库箱号</param>
        /// <param name="GoodsID">商品ID</param>
        /// <param name="CheckQty">实际检查箱数</param>
        /// <returns></returns>
        public ActionResult RF_LoadGoodsCheck(string PaperNO, string LoadNO, string TrayNO, string Barcode, string CheckQty, string condition)
        {
            // 登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            // fit 业务服务调用初始化
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            // 业务流程开始
            try
            {
                // fit参数设置
                service.SetParameter("PaperNO", PaperNO);
                service.SetParameter("LoadNO", LoadNO);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("Barcode", Barcode);
                service.SetParameter("CheckQty", CheckQty);
                service.SetParameter("condition", condition);
                // 商品编码与出货箱匹配的业务检查
                service.ExecuteBusinessCheck("LoadGoodsCheck", "LoadNO");
                // 新增记录执行
                service.ExecuteBusinessProcess("LoadGoodsCheck");

                string finish = service.GetParameter("finish").ToString();
                string Allfinish = service.GetParameter("Allfinish").ToString();

                return Json(new { finish = finish, Allfinish = Allfinish, sMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { sMessage = ex.Message.Replace("\"", "'") });
            }

        }

        /// <summary>
        /// 装载审核完成
        /// </summary>
        /// <param name="PaperNO"></param>
        /// <returns></returns>
        public ActionResult RF_LoadCheckOver(string PaperNO, string LoadNO)
        {
            // 用户登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                // 查询结果为空，即所有装载号完成复核，提交修改标志
                StdQuery1.SetParameter("PaperNO", PaperNO);
                StdQuery1.SetParameter("LoadNO", LoadNO);
                DataTable dt = StdQuery1.Execute("RFQtyLoadCheckFinish");
                // 返回值为0 即完成所有扫描
                if (dt.Rows[0]["ResultValue"].ToString().Equals("0")) {
                    // 调用fit业务过程
                    service.SetParameter("PaperNO", PaperNO);
                    service.ExecuteBusinessCheck("LoadCheckOver", "PaperNO");
                    service.ExecuteBusinessProcess("LoadCheckOver");
                    return Content("");
                }
                else
                {
                    return Json (new{ sMessage = dt.Rows[0]["ResultValue"].ToString() , other = false});
                }
            }
            catch (Exception ex)
            {
                return Json(new { sMessage = ex.Message, other = true });
                //Loger.Error(ex);
                //return Content(ex.Message);
            }
        }

        // 点击继续按钮后继续插入
        public ActionResult RF_LoadCheckContinue(string PaperNO)
        {
            // 用户登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                // 调用fit业务过程
                service.SetParameter("PaperNO", PaperNO);
                service.ExecuteBusinessCheck("LoadCheckOver", "PaperNO");
                service.ExecuteBusinessProcess("LoadCheckOver");
                return Content("");
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoadNO"></param>
        /// <param name="TrayNO"></param>
        /// <param name="Barcode"></param>
        /// <returns></returns>
        public ActionResult RF_IndentifyBar(string LoadNO, string TrayNO, string Barcode, string PaperNO)
        {
            // 用户登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new RFBase(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                // 调用fit业务过程
                service.SetParameter("LoadNO", LoadNO);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("Barcode", Barcode);
                service.SetParameter("PaperNO", PaperNO);
                service.ExecuteBusinessCheck("LoadGoodsDet", "Barcode");
                service.ExecuteBusinessProcess("LoadGoodsDet");
                string GoodsDesc = service.GetParameter("GoodsDesc").ToString();
                return Json(new { GoodsDesc = GoodsDesc, sMessage = "" });
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Json(new { sMessage = ex.Message });
            }
        }

        public ActionResult RF_NumChange(double CaseQty, double QtySg, string Barcode, string LoadNO, string TrayNO)
        {
            // 用户登录验证
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.SetParameter("Barcode", Barcode);
                StdQuery1.SetParameter("LoadNO", LoadNO);
                StdQuery1.SetParameter("TrayNO", TrayNO);

                DataTable dt = StdQuery1.Execute("RFCaseUnit");
                double CaseUnits = double.Parse(dt.Rows[0]["nCaseUnits"].ToString());
                double total = CaseQty * CaseUnits + QtySg;
                return Json( new { total = total });
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                return Json(new { sMessage = ex.Message });
            }
        }
    }
}
