using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.UI.Models;
using TJ.WMS.RF.Service;
using System.Data;

namespace TJ.WMS.RF.UI.Controllers
{
    public class MenuController :BaseController
    {
        public ActionResult Index()
        {
            
            GetLoginInfo();
            if(Login_Info==null)
            {
                return Redirect("/Home");
            }
            try
            {
                MainPage menuService = new MainPage(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                DataTable dt = menuService.GetModuleList();
                if (dt == null || dt.Rows.Count <= 0)
                {
                    return Content("<script type='text/javarscipt'>alert('没有任何权限');</script>");
                }

                List<MenuModel> listMenu = GetMenuList();
                List<MenuModel> tmp = new List<MenuModel>();
                foreach (MenuModel lst in listMenu)
                {
                    DataRow[] dr = dt.Select(string.Format(" ModuleID='{0}'", lst.ID));
                    if (dr != null && dr.Length > 0)
                    {
                        tmp.Add(lst);
                    }
                }

                return View(tmp);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        public List<MenuModel> GetMenuList()
        {
            List<MenuModel> list = new List<MenuModel>() { 
                new MenuModel{ID="RFDCACPT",Name="采购收货",Url="/PurchaseReceipt"},//
                new MenuModel{ID="RFCS2ACPTCFMTRAYQRY",Name="未确认收货托盘",Url="/CS2AcptCFMTrayQry"},//
                //new MenuModel{ID="RFCS2ACPTCFM",Name="二步越库收货确认",Url="/CS2AcptCFM"},//
                new MenuModel{ID="RFDCACPTV2",Name="采购收货V2",Url="/PurchaseReceiptV2"},
                new MenuModel{ID="RFDCACPTV3",Name="无单收货",Url="/PurchaseReceiptV3"},
                new MenuModel{ID="RFSHELVES",Name="人工上架",Url="/Shelves"},
                new MenuModel{ID="RFAUTOSHELVES",Name="指示上架",Url="/AutoShelves"},
                new MenuModel{ID="RFTOSTORE",Name="越库播种",Url="/ToStore"},
                new MenuModel{ID="RFREDIST",Name="退配收货",Url="/Redist"},
                new MenuModel{ID="RFMATCHPLATE_TP",Name="退配分板",Url="MatchplateTP"},
                new MenuModel{ID="RFSTOCKMOVE",Name="移仓",Url="/StockMove"},
                new MenuModel{ID="RFMATCHPLATE_CCH",Name="仓储分板",Url="MatchplateCCH"},
                new MenuModel{ID="RFPASTES_CCH",Name="仓储拼板",Url="PastesCCH"},
                new MenuModel{ID="RFTAKESTOCK",Name="盘点",Url="InvTask"},
                new MenuModel{ID="RFPICK0",Name="拣货",Url="PICK?AssignTypeID=0"},
                new MenuModel{ID="RFPICK",Name="零拣",Url="PICK?AssignTypeID=4"},
                new MenuModel{ID="RFPICK2",Name="箱拣",Url="PICK?AssignTypeID=3"},
                new MenuModel{ID="RFREP",Name="箱补/托拣",Url="PICK?AssignTypeID=2"},
                new MenuModel{ID="RFREP2",Name="零补",Url="Replenish?GrpTypeID=2"},
                new MenuModel{ID="RFREP3",Name="零拣箱补",Url="Replenish?GrpTypeID=1"},
                new MenuModel{ID="RFSET",Name="集货",Url="SET"},
                new MenuModel{ID="RFMATCHPLATE",Name="集货分板",Url="MatchpLate?Name=1"},
                new MenuModel{ID="RFCRATES",Name="复核分板",Url="MatchpLate?Name=2"},
                new MenuModel{ID="RFMATCHPLATEV2",Name="集货分板V2",Url="MatchpLateV2"},
                new MenuModel{ID="RFMATCHPLATE_PC",Name="集货分板PC",Url="MatchpLatePC"},
                new MenuModel{ID="RFPASTES",Name="集货拼板",Url="Pastes"},
                new MenuModel{ID="RFPUTCAGE",Name="装笼",Url="PutCage"},
                new MenuModel{ID="RFLOAD",Name="装载",Url="Load"},
                new MenuModel{ID="RFLOADCHECK",Name="装载复核",Url="LoadCheck"},
                new MenuModel{ID="RFLOADPRINT",Name="装载单打印",Url="LoadPrint"},
                new MenuModel{ID="RFLOADQUERY",Name="未装载查询",Url="LoadQuery"},
                new MenuModel{ID="TRUCKLOAD",Name="装车",Url="TruckLoad"},//每一角落
                new MenuModel{ID="RFTRUCKLOAD",Name="装车",Url="TruckLoading"},
                new MenuModel{ID="RFCONACPT",Name="载具回收",Url="ContainerAcpt"},
                new MenuModel{ID="RFGOODSQRY",Name="商品查询",Url="GoodsQuery"},
                new MenuModel{ID="RFLOCATIONQUERY",Name="储位查询",Url="LocationQuery"},
                new MenuModel{ID="RFOUTBOXQUERY",Name="出库箱查询",Url="OutBoxQuery"},
                new MenuModel{ID="RFOUTBOXCHECK",Name="出库复核",Url="OutBoxCheck"},
                new MenuModel{ID="RFCRATESQUERY",Name="未复核查询",Url="CratesQuery"},
                new MenuModel{ID="RFINTERNALPICK",Name="内部销售",Url="InternalPick"},
                new MenuModel{ID="RFSMTask",Name="移仓任务",Url="StockMoveTask"},
                new MenuModel{ID="RFLC",Name="货架采集",Url="LocationCollection"},
                new MenuModel{ID="RFSTOCKBY",Name="报溢",Url="StockBY"},
                new MenuModel{ID="CRATESRFID",Name="出库复检",Url="CratesRFID"},
                new MenuModel{ID="RFIDREPLACE",Name="商品换标",Url="RfidReplace"},
                new MenuModel{ID="RFIDINSTOCK",Name="贴标入库",Url="RfidInStock"},
                new MenuModel{ID="RFIDMODIFYINSTOCK",Name="库标修改",Url="RfidModifyInStock"},
                 new MenuModel{ID="RFACPTRFIDS",Name="收货贴标",Url="RFAcptRfids"},
                  new MenuModel{ID="RFIDTPRESET",Name="退标回收",Url="RfidTPReset"},
                new MenuModel{ID="RFIDBATCHQUERY",Name="库标查询",Url="RfidBatchQry"},
                new MenuModel{ID="RFCratesToStore",Name="集货分播",Url="CratesToStore"},
                 new MenuModel{ID="RFCS2MOVE",Name="越库移仓",Url="CS2Move"}
            };
            return list;
        }

    }
}
