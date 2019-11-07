using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TJ.WMS.RF.Service;
using TJ.WMS.RF.UI.ViewModels;
using System.Data;
using TJ.WMS.RF.UI.Utils;
using Newtonsoft.Json.Linq;
using System.Text;

namespace TJ.WMS.RF.UI.Controllers
{
    public class MatchpLatePCController : BaseController
    {
        //
        // GET: /MatchpLate/
        #region 自定变量
        MatchpLatePCService service;
        #endregion
        public ActionResult Index()
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Redirect("/Home");
            }
            return View();
        }

        /// <summary>
        /// 效验原始出库箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateOldTrayNO(MatchpLatePCModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchpLatePCService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.ExecuteBusinessCheck("RF_PastesPartofV2", "OldTrayNO");
                DataTable dt = service.GetOldTrayNOGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("数据异常，请检查!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }
        /// <summary>
        /// 效验目标出库箱
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateTrayNO(MatchpLatePCModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchpLatePCService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("IsCheckTrayNO", model.IsCheckTrayNO);
                service.ExecuteBusinessCheck("RF_PastesPartofV2", "TrayNO");
                return Content("");
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }


        /// <summary>
        /// 效验条码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateBarcode(MatchpLatePCModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchpLatePCService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {
                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.ExecuteBusinessCheck("RF_PastesPartofV2", "Barcode");
                DataTable dt = service.GetMatchPlateGoods();
                if (dt != null && dt.Rows.Count > 0)
                {
                    return Content(JsonHelper.ToJson(dt));
                }
                else
                {
                    return Content("数据异常，请检查!");
                }
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// 效验数量并提交
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        public ActionResult ValidateQty(MatchpLatePCModel model)
        {
            GetLoginInfo();
            if (Login_Info == null)
            {
                return Content("<script>location.href='/Home'</script>");
            }
            service = new MatchpLatePCService(Login_Info.User_ID, Login_Info.User_Name, Login_Info.Token);
            try
            {

                service.SetParameter("OldTrayNO", model.OldTrayNO);
                service.SetParameter("TrayNO", model.TrayNO);
                service.SetParameter("Barcode", model.Barcode);
                service.SetParameter("StockBatchNO", model.StockBatchNO);
                service.SetParameter("IsCheckTrayNO", model.IsCheckTrayNO);
                //service.ExecuteBusinessCheck("RF_PastesPartofV2", "*");
                service.ExecuteBusinessProcess("RF_PastesPartofV2");
                DataSet ds = service.GettMatchPlateDtl();
                if (ds.Tables[1].Rows.Count == 0)
                {
                    return Content("error\n" + "操作成功，显示数据失败");
                }
                string strjson = ToJson(ds);
                //string strjson = "{ \"Master\":[{\"Item\":\"1\",\"GoodsInfo\":\"史云生利乐装高级鸡汤【库存单位：盒】\",\"StockBatchNO\":\"2017061320180615\",\"Qty\":\"34.000\"}],\"Detail\":[{\"Item\":\"1\",\"GoodsInfo\":\"史云生利乐装高级鸡汤【库存单位：盒】\",\"StockBatchNO\":\"2017061320180615\",\"Qty\":\"34.000\"}]}";
                return Content(strjson);
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                return Content("error\n" + ex.Message);
            }
        }
        /// <summary>
        /// 将json字符串转换成ds
        /// </summary>
        /// <param name="arrayList"></param>
        /// <returns></returns>
        public string ToJson(DataSet ds)
        {
            int n = ds.Tables.Count;//ds中table的数量
            StringBuilder jsonBuilder = new StringBuilder();
            try
            {
                jsonBuilder.Append("{");
                for (int i = 0; i < n; i++)//遍历从表生成层级ItemList、ItemList1、ItemList2.....
                {
                    jsonBuilder.Append("\"");
                    if (i == 0) {
                        jsonBuilder.Append("Master");
                    }
                    else
                    {
                        if (i == 1)
                        {
                            jsonBuilder.Append("Detail");

                        }
                        else { jsonBuilder.Append("Detail" + (i - 1).ToString()); }

                    }
                    jsonBuilder.Append("\":");
                    if (ds.Tables[i].Rows.Count <= 0) {
                        jsonBuilder.Append("\"\"");
                    }
                    else
                    {
                        jsonBuilder.Append("[");
                        for (int m = 0; m < ds.Tables[i].Rows.Count; m++)
                        {
                            jsonBuilder.Append("{");
                            for (int j = 0; j < ds.Tables[i].Columns.Count; j++)
                            {
                                if (!ds.Tables[i].Columns[j].ColumnName.Contains("UpdateFlag"))
                                {
                                    jsonBuilder.Append("\"");
                                    jsonBuilder.Append(ds.Tables[i].Columns[j].ColumnName);
                                    jsonBuilder.Append("\":\"");
                                    jsonBuilder.Append(ds.Tables[i].Rows[m][j].ToString());
                                    jsonBuilder.Append("\",");
                                }
                            }
                            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                            jsonBuilder.Append("}");
                            jsonBuilder.Append(",");
                        }
                        jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                        jsonBuilder.Append("]");
                    }
                    jsonBuilder.Append(",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("}");
                return jsonBuilder.ToString();
            }
            catch
            {
                return null;

            }
        }
    }
}
