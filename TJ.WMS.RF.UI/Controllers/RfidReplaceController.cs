using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using Newtonsoft.Json.Linq;
using System.Data;
using TJ.WMS.RF.UI.Utils;

namespace TJ.WMS.RF.UI.Controllers
{
    public class RfidReplaceController: BaseController
    {
        private  RfidReplace service;
        public UIntPtr hreader
        {
            get { return (UIntPtr)Session["_hreader"]; }
            set { Session["_hreader"] = value; }
        }
        
        public ActionResult Index()
        {
            hreader = UIntPtr.Zero;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            RFIDLIB.rfidlib_reader.RDR_LoadReaderDrivers(Server.MapPath(".") + "\\bin\\Drivers");//加载驱动
            LoadShipper();
            LoadDrivesSelectOption();
            return View();
        }
        /// <summary>
        /// 加载货主下拉选择项
        /// </summary>
        public void LoadShipper()
        {
            string DefaultShipper = Request.Cookies["DefaultShipper"] == null ? "" : Request.Cookies["DefaultShipper"].Value;
            string options = "";
            StdQuery StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            StdQuery1.SetParameter("ShipperNO", "%");
            StdQuery1.SetParameter("ShipperDesc", "%");
            DataTable dt = StdQuery1.Execute("SelUserShipper");
            foreach (DataRow dr in dt.Rows)
            {
                if (dr["ShipperNO"].ToString() == StringUtil.GetTextID(DefaultShipper))
                {
                    options += "<option selected = 'selected'>" + dr["ShipperNO"] +"."+ dr["ShipperDesc"] + "</option>";
                }
                else
                {
                    options += "<option>" + dr["ShipperNO"] +"."+ dr["ShipperDesc"] + "</option>";
                }

            }
            ViewBag.selShipper = options;

        }
        /// <summary>
        /// 加载设备下拉选择项
        /// </summary>
        public void LoadDrivesSelectOption()
        {
            List<string> rfid = DBHelper.GetSystemCommon("RFID");
            string RfidDrives = Request.Cookies["RfidDrives2"] == null ? "" : Request.Cookies["RfidDrives2"].Value;
            string options = "";
            foreach (string item in rfid)
            {
                if (StringUtil.GetTextID(item) == StringUtil.GetTextID(RfidDrives))
                {
                    options += "<option selected = 'selected'>" + item + "</option>";
                }
                else
                {
                    options += "<option>" + item + "</option>";
                }

            }
            ViewBag.selRfidDrives = options;
        }
        /// <summary>
        /// 连接设备
        /// </summary>
        /// <param name="bConnected"></param>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public ActionResult ConnectDriver(bool bConnected, string ipAddr)
        {

            if (bConnected)//已连接
            {
                if (hreader != UIntPtr.Zero)
                {
                    RFIDLIB.rfidlib_reader.RDR_Close(hreader);
                    hreader = UIntPtr.Zero;
                }
                return Content("");
            }
            else
            {
                int iret = 0;
                iret = Connecting(StringUtil.GetTextName(ipAddr));
                if (iret == 0)
                {
                    HttpCookie cook = new HttpCookie("RfidDrives2", ipAddr);
                    cook.Expires = DateTime.MaxValue;
                    Response.Cookies.Add(cook);
                    return Content("");
                }
                else
                {
                    //连接失败
                    return Content("设备连接失败，请重试！" + iret.ToString());
                }
            }

        }
        /// <summary>
        /// 连接或断开设备
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public int Connecting(string ipAddr)
        {
            UIntPtr hreader1 = UIntPtr.Zero;
            string connstr = string.Empty;
            UInt16 port;
            //ipAddr = "192.168.250.160";
            port = 4800;
            string readerDriverName = "RPAN";
            connstr = RFIDLIB.rfidlib_def.CONNSTR_NAME_RDTYPE + "=" + readerDriverName + ";" +
                      RFIDLIB.rfidlib_def.CONNSTR_NAME_COMMTYPE + "=" + RFIDLIB.rfidlib_def.CONNSTR_NAME_COMMTYPE_NET + ";" +
                      RFIDLIB.rfidlib_def.CONNSTR_NAME_REMOTEIP + "=" + ipAddr + ";" +
                      RFIDLIB.rfidlib_def.CONNSTR_NAME_REMOTEPORT + "=" + port.ToString() + ";" +
                      RFIDLIB.rfidlib_def.CONNSTR_NAME_LOCALIP + "=" + "";
            int n = RFIDLIB.rfidlib_reader.RDR_Open(connstr, ref hreader1);
            Session["_hreader"] = hreader1;

            return n;
        }
        /// <summary>
        /// 读取设备数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadRfid()
        {
            try
            {
                string strData = "";
                int iret = 0;
                Byte gFlg = 0x00;//0x00 从头开始读;0x01连续读;
                UIntPtr dnhReport = UIntPtr.Zero;

                iret = RFIDLIB.rfidlib_reader.RDR_BuffMode_FetchRecords(hreader, gFlg); // send command to device
                if (iret == 0)//取数据
                {
                    // Get records from dll buffer memory
                    dnhReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_FIRST);
                    while (dnhReport != UIntPtr.Zero)// 有数据
                    {
                        Byte[] byData = new Byte[32];
                        UInt32 len = (UInt32)byData.Length;
                        if (RFIDLIB.rfidlib_reader.RDR_ParseTagDataReportRaw(dnhReport, byData, ref len) == 0)
                        {
                            if (len > 0)
                            {
                                strData += ";" + BitConverter.ToString(byData, 0, (int)len).Replace("-", string.Empty);
                            }
                        }
                        dnhReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_NEXT);
                    }
                    gFlg = 0x01;  // if received ok ,get next records from device
                }
                strData = strData == "" ? "" : strData.Substring(1);
                //strData = "123;456;9999";
                
                if (strData == "")
                {
                    return Content("本次读取数据为空，请重试！");
                }
                else
                {
                    string[] list = strData.Split(';');
                    if (list.Count() == 1)
                    {
                        strData = "[{\"RFID\":\"" + strData + "\"}]";
                        return Content(strData);
                    }else
                    {
                        return Content("只允许读取一个标签，请重试！");
                    }
                }
                
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult QryRfidReplaceGoods(string ShipperNO, string Barcode)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                StdQuery StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                StdQuery1.SetParameter("ShipperNO", StringUtil.GetTextID(ShipperNO));
                StdQuery1.SetParameter("Barcode", Barcode);
                DataTable dt = StdQuery1.Execute("QryRfidReplaceGoods");
                if (dt.Rows.Count != 0)
                {
                    return Content(dt.ToJson());
                }
                else
                {
                    return Content("商品条码不存在！");
                }

            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }

        }
        public ActionResult ValidateTrayNO(string TrayNO, int GoodsID)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                service = new RfidReplace(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("GoodsID", GoodsID);
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("RfidReplaceTray");
                string result = "[{\"ProductDate\":\"" + service.GetParameter("ProductDate").ToString() + "\"}]";
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult RfidReplaceApply(string ShipperNO, string GoodsID, string Barcode, string TrayNO, string ProductDate, string RFID)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                HttpCookie cook = new HttpCookie("DefaultShipper", ShipperNO);
                cook.Expires = DateTime.MaxValue;
                Response.Cookies.Add(cook);

                service = new RfidReplace(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("ShipperNO", StringUtil.GetTextID(ShipperNO));
                service.SetParameter("GoodsID", GoodsID);
                service.SetParameter("Barcode", Barcode);
                service.SetParameter("TrayNO", TrayNO);
                service.SetParameter("ProductDate", ProductDate);
                service.SetParameter("RFID", RFID);
                service.ExecuteBusinessProcess("RfidReplaceApply");
                return Content("");
            }
            catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}