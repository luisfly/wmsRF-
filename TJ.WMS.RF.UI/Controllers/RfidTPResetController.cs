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
    public class RfidTPResetController: BaseController
    {
        private RfidReplace service;
        // public static UIntPtr hreader;//成功连接的设备
        public UIntPtr hreader {
            get { return (UIntPtr)Session["_hreader"]; }
            set { Session["_hreader"] = value; }
        }
     
       // public static string RfidList;
        public ActionResult Index()
        {
            TempData["RfidList"] = string.Empty;
            hreader = UIntPtr.Zero;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            RFIDLIB.rfidlib_reader.RDR_LoadReaderDrivers(Server.MapPath(".") + "\\bin\\Drivers");//加载驱动
            LoadDrivesSelectOption();
            return View();
        }
        /// <summary>
        /// 加载设备下拉选择项
        /// </summary>
        public void LoadDrivesSelectOption()
        {
            List<string> rfid = DBHelper.GetSystemCommon("RFID");
            string RfidDrives = Request.Cookies["RfidDrives3"] == null ? "" : Request.Cookies["RfidDrives3"].Value;
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
                    HttpCookie cook = new HttpCookie("RfidDrives3", ipAddr);
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
            //RfidList = string.Empty;
            TempData["RfidList"] = "";
            int ReadCount = 1; //控制是否继续读取，大于2不读
            try
            {
                string strData = "";
                int iret = 0;
                Byte gFlg = 0x00;//0x00 从头开始读;0x01连续读;
                UIntPtr dnhReport = UIntPtr.Zero;
                while (ReadCount <= 2)
                {
                    ReadCount++;
                    iret = RFIDLIB.rfidlib_reader.RDR_BuffMode_FetchRecords(hreader, gFlg); // send command to device
                    if (iret == 0)//取数据
                    {
                        // Get records from dll buffer memory
                        dnhReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_FIRST);
                        while (dnhReport != UIntPtr.Zero)// 有数据
                        {
                            ReadCount = 1;
                            Byte[] byData = new Byte[32];
                            UInt32 len = (UInt32)byData.Length;
                            if (RFIDLIB.rfidlib_reader.RDR_ParseTagDataReportRaw(dnhReport, byData, ref len) == 0)
                            {
                                if (len > 0 )
                                {
                                    string RFID = ";" + BitConverter.ToString(byData, 0, (int)len).Replace("-", string.Empty);
                                    if(!strData.Contains(RFID)) {
                                        strData += RFID;
                                    }
                                    
                                }
                            }
                            dnhReport = RFIDLIB.rfidlib_reader.RDR_GetTagDataReport(hreader, RFIDLIB.rfidlib_def.RFID_SEEK_NEXT);
                        }
                        gFlg = 0x01;  // if received ok ,get next records from device
                    }
                }
                strData = strData == "" ? "" : strData.Substring(1);
                //strData = "123;456;9999";

                if (strData == "")
                {
                    return Content("本次读取数据为空，请重试！");
                }
                else
                {
                    const string json = ",{{\"RFID\":\"{0}\"}}";
                    TempData["RfidList"] = strData;
                    string[] list = strData.Split(';');
                    strData = string.Empty;
                    foreach (string v in list)
                    {
                        strData += string.Format(json, v);
                    }
                    strData = "[" + strData.Substring(1) + "]";
                    return Content(strData);
                }

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult ValidateTrayNO(string TrayNO)
        {
            TempData["RfidList"] = string.Empty;
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            try
            {
                service = new RfidReplace(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("RfidTPResetTray");
                string result = "[{{\"Shipper\":\"{0}\",\"Barcode\":\"{1}\",\"GoodsName\":\"{2}\",\"ProductDate\":\"{3}\",\"StockQty\":\"{4}\"}}]";
                result = string.Format(result, service.GetParameter("Shipper").ToString(), service.GetParameter("Barcode").ToString(), service.GetParameter("GoodsName").ToString(), service.GetParameter("ProductDate").ToString(), service.GetParameter("StockQty").ToString());
                return Content(result);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult RfidTPResetApply(string TrayNO)
        {
          // TempData["RfidList"] = "1001;1002;1003;1004;1005";
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("用户已失效，请重新登录!");
            }
            if (TempData["RfidList"].ToString() == string.Empty)
            {
                return Content("RFID不能为空!");
            }
            try
            {
                Business business1 = new Business(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
                DataSet ds = new DataSet();
                DataTable dtMaster = new DataTable("Master");
                dtMaster.Columns.Add("TrayNO", Type.GetType("System.String"));
                DataRow dataRow = dtMaster.NewRow();
                dataRow["TrayNO"] = TrayNO;
                dtMaster.Rows.Add(dataRow);

                DataTable dt = new DataTable();
                dt.TableName = "Detail";
                dt.Columns.Add("RFID", Type.GetType("System.String"));
                foreach (string val in TempData["RfidList"].ToString().Split(';'))
                {
                    dataRow = dt.NewRow();
                    dataRow["RFID"] = val;
                    dt.Rows.Add(dataRow);
                }
                ds.Tables.Add(dtMaster);
                ds.Tables.Add(dt);

                business1.dsDataObject = ds;
                business1.BusinessName = "RfidTPResetApl";
                business1.Load();
                business1.Execute();
                TempData["RfidList"] = string.Empty;
                return Content("");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}