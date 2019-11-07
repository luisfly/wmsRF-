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
    public class CratesRFIDController: BaseController
    {
        private  CratesRFID service;
        //private  Business business1;
        private  StdQuery StdQuery1;
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
            //service = new CratesRFID(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            //business1 = new Business(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            //StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
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
            string RfidDrives = Request.Cookies["RfidDrives"] == null ? "" : Request.Cookies["RfidDrives"].Value;
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
                    HttpCookie cook = new HttpCookie("RfidDrives", ipAddr);
                    cook.Expires = DateTime.MaxValue;
                    Response.Cookies.Add(cook);
                    return Content("");
                }
                else
                {
                    //连接失败
                    return Content("设备连接失败，请重试！"+iret.ToString());
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
        public ActionResult ValidateTrayNO(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesRFID(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            string result = "[{{\"Shipper\":\"{0}\",\"Store\":\"{1}\",\"Qty\":{2}}}]";
            try
            {
                service.ClearParameter();
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("CratesRfidTrayNO");
                result = string.Format(result, service.GetParameter("Shipper"), service.GetParameter("Store"), service.GetParameter("Qty").ToString());
                return Content(result);//返回json给页面显示
            } catch(Exception ex)
            {
                return Content(ex.Message);
            }
            
        }
        /// <summary>
        /// 执行完托盘检查通过后，显示
        /// </summary>
        /// <param name="TrayNO"></param>
        /// <returns></returns>
        public ActionResult ShowTrayRFID(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.ClearParameter();
                StdQuery1.SetParameter("TrayNO", TrayNO);
                DataTable dt = StdQuery1.Execute("QryTrayRfidGoods");//查询集货标签商品复检情况
                return Content(dt.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 清空当前RFID
        /// </summary>
        /// <param name="TrayNO"></param>
        /// <returns></returns>
        public ActionResult ClearRfid(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesRFID(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.ClearParameter();
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("CratesRfidClear");

                StdQuery1.ClearParameter();
                StdQuery1.SetParameter("TrayNO", TrayNO);
                DataTable dt = StdQuery1.Execute("QryTrayRfidGoods");//查询集货标签商品复检情况
                return Content(dt.ToJson());
            }catch(Exception ex)
            {
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 读取设备数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ReadRfid(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
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
                                if (len > 0)
                                {
                                    string RFID = ";" + BitConverter.ToString(byData, 0, (int)len).Replace("-", string.Empty);
                                    if (!strData.Contains(RFID))
                                    {
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
                if (strData != "")
                {
                    CratesRfidReading(TrayNO, strData); //循环写入标签
                }
                StdQuery1.ClearParameter();
                StdQuery1.SetParameter("TrayNO", TrayNO);
                DataTable dt = StdQuery1.Execute("QryTrayRfidGoods");//查询集货标签商品复检情况
                return Content(dt.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            
        }
        /// <summary>
        /// 写入读取的RFID
        /// </summary>
        /// <param name="TrayNO"></param>
        /// <param name="RfidData"></param>
        public void CratesRfidReading(string TrayNO, string RfidData)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                throw new RFException("用户已失效，请重新登录!");
            }
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
            foreach (string val in RfidData.Split(';'))
            {
                dataRow = dt.NewRow();
                dataRow["RFID"] = val;
                dt.Rows.Add(dataRow);
            }
            ds.Tables.Add(dtMaster);
            ds.Tables.Add(dt);

            business1.dsDataObject = ds;
            business1.BusinessName = "CratesRfidReading";
            business1.Load();
            business1.Execute();
        }
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="TrayNO"></param>
        /// <returns></returns>
        public ActionResult CratesRfidCheck(string TrayNO)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new CratesRFID(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.ClearParameter();
                service.SetParameter("TrayNO", TrayNO);
                service.ExecuteBusinessProcess("CratesRfidCheck");
                return Content("");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
        public ActionResult QueryRfidDetail(string TrayNO, int GoodsID)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            StdQuery1 = new StdQuery(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                StdQuery1.ClearParameter();
                StdQuery1.SetParameter("TrayNO", TrayNO);
                StdQuery1.SetParameter("GoodsID", GoodsID);
                DataTable dt = StdQuery1.Execute("QueryRfidDetail");//查询集货标签商品复检情况
                return Content(dt.ToJson());
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}