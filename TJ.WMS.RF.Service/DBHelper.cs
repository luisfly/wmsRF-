/* 
 * 版    权：TMain @ rzcd
 * 过    程：DBHelper
 * 建    立：TIM 
 * 创建时间：2017-11-17
 * 说    明：一般处理过程
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json.Linq;
namespace TJ.WMS.RF.Service
{

    public class DBHelper
    {
        private static string[] SysParams = new string[] { "Operator", "OperName", "LocalStoreNO", "LastUpdateTime", "Old_LastUpdateTime" };
        public class DropDownListClass
        {
            private string _Value;

            public string Value
            {
                get { return _Value; }
                set { _Value = value; }
            }
            private string _Text;
            public string Text
            {
                get { return _Text; }
                set { _Text = value; }
            }
            public DropDownListClass(string Value, string Text)
            {
                _Value = Value;
                _Text = Text;
            }
        }
        /// <summary>
        /// 字符串转FineUI下拉列表
        /// </summary>
        /// <param name="pickList"></param>
        /// <param name="valueList"></param>
        /// <returns></returns>
        public static List<DropDownListClass> StringToDropDownList(string pickList, Boolean valueList = true)
        {
            List<DropDownListClass> myList = new List<DropDownListClass>();
            List<string> ListItem = new List<string>(pickList.Split(';'));
            if (ListItem.Count > 0)
            {
                foreach (var Item in ListItem)
                {
                    if (valueList)
                    {
                        myList.Add(new DropDownListClass(Item, Item));
                    }
                    else
                    {
                        myList.Add(new DropDownListClass(StringUtil.GetTextID(Item), Item));

                    }
                }

            }
            return myList;
        }
        /// <summary>
        /// 通过共通资料返回DataTable，绑定到下拉框列表
        /// </summary>
        /// <param name="commonNO"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public static DataTable ResultDropDownListDataTable(string commonNO, string where = "")
        {
            string sql = "select sComID+'.'+sComDesc as Text, sComID+'.'+sComDesc as Value from tCommon where sLangID=936 and sCommonNO='{0}' {1}";
            sql = string.Format(sql, commonNO, where);
            return RFBase.Bss_Helper.GetDataSet_SQL(sql).Tables[0];
        }
        
        /// <summary>
        /// 获取字符型数值的小数位数，字典用
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetDecimalCount(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 2;
            }
            else
            {
                int precission = 0;
                string[] strArr = value.ToString().Split('.');//分开整数与小数部分
                if (strArr.GetLength(0) == 2)
                {
                    precission = strArr[1].Length;
                }
                else
                    precission = 0;
                return precission;
            }
        }
        /// <summary>
        /// 获取系统参数
        /// </summary>
        /// <param name="sCode"></param>
        /// <param name="Item"></param>
        /// <returns></returns>
        public static string GetSystemCtrl(string sCode, int Item = 0)
        {
            string SQL = "select sValue1, sValue2, sValue3 from tSystemCtrl where sCode = '" + sCode + "'";
            DataTable dt = RFBase.Bss_Helper.GetDataSet_SQL(SQL).Tables[0];
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0][Item].ToString();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 获取系统共通资料
        /// </summary>
        /// <param name="commonNO"></param>
        /// <param name="sWhere"></param>
        /// <returns></returns>
        public static List<string> GetSystemCommon(string commonNO, string sWhere = "")
        {
            string SQL = "select ResultValue = case when isnull(sComDesc,'')='' then sComID else sComID+'.'+sComDesc end from tCommon where sCommonNO='" + commonNO + "' " + sWhere;
            DataTable dt = RFBase.Bss_Helper.GetDataSet_SQL(SQL).Tables[0];
            List<string> sList = DataTableToList(dt);
            dt.Dispose();
            return sList;
        }
        /// <summary>
        /// Table字段ResultValue转为List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<string> DataTableToList(DataTable dt)
        {
            List<string> sList = new List<string>();
            if (dt == null || dt.Rows.Count == 0)
                return sList;
            else
            {

                foreach (DataRow dr in dt.Rows)
                {
                    sList.Add(dr["ResultValue"].ToString());
                }

                return sList;
            }
        }
        /// <summary>
        /// 获取模块名称
        /// </summary>
        /// <param name="modelID"></param>
        /// <returns></returns>
        public static string GetModelName(string modelID)
        {
            string strSQL = "select sModuleDesc from tModule where sModuleID='" + modelID + "'";
            DataSet dt = RFBase.Bss_Helper.GetDataSet_SQL(strSQL);
            if (dt == null || dt.Tables[0].Rows.Count == 0) //
                return modelID;
            else
                return dt.Tables[0].Rows[0]["sModuleDesc"].ToString();

        }
        /// <summary>
        /// 检查用户执行业务过程的权限
        /// </summary>
        /// <param name="BusinessName"></param>
        /// <param name="Operator"></param>
        /// <returns></returns>
        public static bool CheckBusinessPower(string businessName, string Operator)
        {
            string strSQL = "select 1 from t_frm_Business where Property&2=2 and BusinessID='" + businessName + "'";
            DataSet dt = RFBase.Frm_Helper.GetDataSet_SQL(strSQL);
            if (dt == null || dt.Tables[0].Rows.Count == 0) //检查权限控制
            {
                dt.Dispose();
                strSQL = "SELECT 1 FROM tRolePower rp inner join tUserRole ur on rp.sRoleID = ur.sRoleID and ur.sUserNO = '" + Operator + "' and rp.sPowerID = '" + businessName + "'";
                DataSet dt1 = RFBase.Bss_Helper.GetDataSet_SQL(strSQL);
                return !(dt1 == null || dt1.Tables[0].Rows.Count == 0);
            }
            else
                return true; //免权限控制

        }
        /// <summary>
        /// 检查模块使用权限
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Operator"></param>
        /// <returns></returns>
        public static bool CheckModulePower(string moduleID, string Operator)
        {
            //string strSQL = "SELECT 1 FROM tRolePower rp inner join tUserRole ur on rp.sRoleID = ur.sRoleID and ur.sUserNO = '{0}' and rp.sPowerID = '{1}'";
            //strSQL = string.Format(strSQL, Operator, ModuleID);
            string strSQL = "SELECT 1 FROM tRolePower rp inner join tUserRole ur on rp.sRoleID = ur.sRoleID and ur.sUserNO = '" + Operator + "' and rp.sPowerID = '" + moduleID + "'";
            DataSet dt = RFBase.Bss_Helper.GetDataSet_SQL(strSQL);
            return !(dt == null || dt.Tables[0].Rows.Count == 0);
        }
       
        /// <summary>
        /// 根据参数修改主表dt的值
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dbParams"></param>
        /// <param name="allowSysParams"></param>
        public static void DataObjectMasterTableModify(DataTable dt, SmartDbParams dbParams, bool allowSysParams = false)
        {
            //从视图界面传入，如有系统参数不处理
            if (dt == null || dbParams == null || dbParams.paras.Count == 0)
            {
                return;
            }
            foreach (var par in dbParams.paras)
            {
                string a = par.Key;
                if (!allowSysParams && SysParams.Contains<string>(par.Key))
                {
                    continue;
                }
                if (dt.Columns.Contains(par.Key))
                {
                    dt.Rows[0][par.Key] = dbParams.GetParameter(par.Key);
                }
            }
        }
        /// <summary>
        /// 修改主表数据,allowSysParams=false是否处理系统参数
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="Params"></param>
        /// <param name="allowSysParams=false"></param>
        //public static void DataObjectMasterTableModify(DataTable dt, JObject Params, bool allowSysParams = false)
        //{
        //    //从视图界面传入，如有系统参数不处理
        //    if (dt == null || Params == null || Params.Count == 0)
        //    {
        //        return;
        //    }
        //    foreach (var par in Params)
        //    {
        //        string a = par.Key;
        //        if (!allowSysParams && SysParams.Contains<string>(par.Key))
        //        {
        //            continue;
        //        }
        //        if (dt.Columns.Contains(par.Key))
        //        {
        //            dt.Rows[0][par.Key] = par.Value;
        //        }
        //    }
        //}
        
        /// <summary>
        /// 修改数据对象DataTable内容
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dtDataSource"></param>
        /// <param name="KeyField"></param>
        public static void DataObjectDetailTableModify(DataTable dt, DataTable dtDataSource, string KeyField)
        {
            //存在修改不存在插入
            if (dt == null || dtDataSource.Rows.Count == 0 || string.IsNullOrEmpty(KeyField) || !dtDataSource.Columns.Contains(KeyField))
            {
                return;
            }
            string UpdateFlag = "UpdateFlag";
            string[] lst = dt.TableName.Split('_');
            if (lst.Count() > 1)
            {
                UpdateFlag += lst[1].ToString();
            }
            foreach (DataRow dr in dtDataSource.Rows)
            {
                object KeyFieldValue = dr[KeyField];
                string filterExpression = KeyField + "='" + KeyFieldValue.ToString() + "'";
                DataRow[] rowList = dt.Select(filterExpression);
                if (rowList.Count() > 0)//有则修改
                {
                    for(var n = 0; n < rowList.Count(); n++)
                    {
                        rowList[n].BeginEdit();
                        foreach(DataColumn dc in dt.Columns)
                        {
                            string FileName = dc.ColumnName;
                            if (dt.Columns.Contains(FileName))
                            {
                                if (FileName == UpdateFlag)
                                {
                                    rowList[n][FileName] = "U";//暂先处理，目前没用到，业务过程由表记录状态控制U、I、D 2018-02-28
                                }
                                else
                                {
                                    rowList[n][FileName] = dr[FileName];
                                }
                            }
                        }
                        rowList[n].EndEdit();
                    }
                }
                else//没有则新增
                {
                    DataRow drNew = dt.NewRow();
                    foreach (DataColumn dc in dtDataSource.Columns)
                    {
                        string FileName = dc.ColumnName;
                        if (dt.Columns.Contains(FileName))
                        {
                            if (FileName == UpdateFlag)
                            {
                                drNew[dc.ColumnName] = "I";//暂先处理，目前没用到，业务过程由记录状态控制U、I、D 2018-02-28
                            }
                            else
                            {
                                drNew[dc.ColumnName] = dr[FileName];
                            }
                        }
                    }
                    dt.Rows.Add(drNew);
                }
            }
            
        }
        
        /// <summary>
        /// 设置行的默认值
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="paraDefaultValue"></param>
        public static void SetDataRowDefaultValue(DataRow dr, JArray paraDefaultValue)
        {
            //dtMasterNew.Clear();
            if (paraDefaultValue != null)
            {
                dr.BeginEdit();
                foreach (var par in paraDefaultValue)
                {
                    string FieldName = par["FieldName"].ToString();
                    string Value = par["DefaultValue"].ToString();
                    if (dr.Table.Columns.Contains(FieldName) && Value != string.Empty)
                    {
                        if (dr.Table.Columns[FieldName].DataType == Type.GetType("System.DateTime"))
                        {
                            dr[FieldName] = RFBase.GetServerDateTime().AddDays(int.Parse(Value));
                        }
                        else
                        {
                            dr[FieldName] = Value;
                        }
                    }
                }
                dr.EndEdit();
            }
            
            //dt.RejectChanges();
        }
        /// <summary>
        /// 数据行转为Json字符串格式
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static string DataRowToJsonString(DataRow dr)
        {
            string json = string.Empty;
            foreach(DataColumn dc in dr.Table.Columns)
            {
                switch (dc.DataType.Name.ToString())
                {
                    case "Decimal":
                    case "Double":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "Boolean":
                    case "Byte":
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                    case "SByte":
                        object val = string.IsNullOrEmpty(dr[dc.ColumnName.ToString()].ToString()) ? "null" : dr[dc.ColumnName.ToString()];
                        json += ",\"" + dc.ColumnName.ToString() + "\":" + val;
                        break;
                    default:
                        json += ",\"" + dc.ColumnName.ToString() + "\":\"" + dr[dc.ColumnName.ToString()].ToString() + "\"";
                        break;
                }
            }
            if (json != string.Empty)
            {
                json = "{" + json.Substring(1) + "}";
            }
            return json;

        }
        /// <summary>
        /// 把DataTable转为Json
        /// Json = [{"字段1":"值1"},{}]
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToJsonList(DataTable dt)
        {
            string json = string.Empty;
            foreach(DataRow dr in dt.Rows)
            {
                json += "," + DataRowToJsonString(dr);
            }
            if (json != string.Empty)
            {
                json = "[" + json.Substring(1) + "]";
            }

            return json;
        }
        /// <summary>
        /// 获取SQL语句的参数列表
        /// </summary>
        /// <param name="sqlText"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string[] GetSqlTextParams(string sqlText, string p = "@")
        {
            //string Params = string.Empty;
            List<string> Params = new List<string>();
            //实例化r，第二个参数为匹配的要求，这里为忽略大小写
            string pat = p + @"(\w+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);
            Match m = r.Match(sqlText);
            //此属性判断是否匹配成功
            while (m.Success)
            {
                for (int i = 1; i <= 2; i++)
                {
                    //获取由正则表达式匹配的组的集合,这行代码相当于下面两句
                    //GroupCollection gc = mt.Groups;
                    //Group g = gc[i];
                    Group g = m.Groups[i];
                    //获取由捕获组匹配的所有捕获的集合
                    CaptureCollection cc = g.Captures;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        Capture c = cc[j];
                        string par = c.ToString();
                        if (par != string.Empty && !Params.Contains(par))
                        {
                            Params.Add(par);
                        }
                    }
                }
                //匹配下一个
                m = m.NextMatch();
            }
            return Params.ToArray();
        }
        /// <summary>
        /// 更新记录行DataRow
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="newDr"></param>
        public static void UpdateDataRow(DataRow dr, DataRow newDr)
        {
            dr.BeginEdit();
            foreach(DataColumn dc in dr.Table.Columns)
            {
                if (newDr.Table.Columns.Contains(dc.ColumnName))
                {
                    dr[dc.ColumnName] = newDr[dc.ColumnName];
                }
            }
            dr.EndEdit();
        }
        /// <summary>
        /// 根据条件，更新dt中的记录
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="newDr"></param>
        /// <param name="filterExpression"></param>
        public static void UpdateDataRow(DataTable dt, DataRow newDr, string filterExpression)
        {
            DataRow[] dataRows = dt.Select(filterExpression);
            foreach (DataRow dr in dataRows)
            {
                UpdateDataRow(dr, newDr);
            }
        }
       
        #region OLD

        /*OLD PRO

        //////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// ///////////////////////
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>



        /// <summary>
        /// GetUserDC
        /// 返回用户DC列表，NotResultDel=true则不返回逻辑删除的DC
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="NotResultDel"></param>
        /// <returns></returns>
        public static List<string> GetUserDC(string UserID, bool NotResultDel = true)
        {
            RFBase service;
            string Flag = "1";
            service = new RFBase();

            if (!NotResultDel)
                Flag = "0";
            service.SetParameter("Operator", UserID);
            service.SetParameter("Flag", Flag);
            DataTable dt = service.ExecStdQuery("GetUserDCList");
            List<string> DCList = new List<string>();
            if (dt == null || dt.Rows.Count == 0)
                return DCList;
            else
            {
                //List<string> DCList = new List<string>();
                foreach (DataRow dr in dt.Rows)
                {
                    DCList.Add(dr["ResultValue"].ToString());
                }
                dt.Dispose();
                return DCList;
            }
        }
        /// <summary>
        /// 取模块名称
        /// 创建:TIM 2016-07-02
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>

        /// <summary>
        /// 取模块上级名
        /// </summary>
        /// <param name="ModelID"></param>
        /// <returns></returns>
        public static string GetModelNavigationPath(string ModelID)
        {
            string strSQL = "select sModuleDesc from tModule where sMenuID = (select substring(sMenuID, 1, 3) from tModule where sModuleID = '" + ModelID + "')";
            DataSet dt = RFBase.Bss_Helper.GetDataSet_SQL(strSQL);
            if (dt == null || dt.Tables[0].Rows.Count == 0) //
                return ModelID;
            else
                return dt.Tables[0].Rows[0]["sModuleDesc"].ToString();
        }
        /// <summary>
        /// CheckBusinessPower
        /// 功能：检查用户是否有执行业务处理的权限
        /// 创建：TIM 2016-06-29
        /// </summary>
        /// <param name="BusinessName"></param>
        /// <param name="Operator"></param>
        /// <returns></returns>

        /// <summary>
        /// 取List的ID，返回List
        /// 创建：TIM 2016-07-05
        /// </summary>
        /// <param name="lsList"></param>
        /// <returns></returns>
        public static List<object> GetTextIDList(List<object> lsList)
        {
            List<object> strList = new List<object>();
            for (int i = 0; i < lsList.Count; i++)
                strList.Add(StringUtil.GetTextID(lsList[i].ToString()));
            return strList;
        }
        /// <summary>
        /// 取List的Name，返回List
        /// 创建：TIM 2016-07-05
        /// </summary>
        /// <param name="lsList"></param>
        /// <returns></returns>
        public static List<object> GetTextNameList(List<object> lsList)
        {
            List<object> strList = new List<object>();
            for (int i = 0; i < lsList.Count; i++)
                strList.Add(StringUtil.GetTextName(lsList[i].ToString()));
            return strList;
        }
        /// <summary>
        /// ModifyDataTableForList
        /// 功能：根据实体List修改表数据
        /// 创建：TIM 2016-6-28
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="KeyField"></param>
        /// <param name="ModifyField"></param>
        /// <param name="KeyFieldValue"></param>
        /// <param name="ModifyFieldValue"></param>
        public static void ModifyDataTableForList(DataTable dt, string KeyField, string ModifyField, List<object> KeyFieldValue, List<object> ModifyFieldValue)
        {
            //暂时不考虑D删除操作,2016-08-18增加UpdateFlag删除操作
            string UpdateFlag = "UpdateFlag";
            if (dt.TableName == "Detail")
                UpdateFlag = "UpdateFlag";
            else
            {
                int i = dt.TableName.IndexOf("Detail_");
                if (i >= 0)
                    UpdateFlag = UpdateFlag + dt.TableName.Substring(7, dt.TableName.Length - 7);
            }

            if (dt.TableName != "Master" && !dt.Columns.Contains(UpdateFlag))
            {
                dt.Columns.Add(UpdateFlag, typeof(string));
                dt.Columns[UpdateFlag].Caption = "*" + UpdateFlag;
            }
            if (!dt.Columns.Contains(KeyField))
            {
                dt.Columns.Add(KeyField, typeof(string));
                dt.Columns[KeyField].Caption = KeyField;
            }
            if (!dt.Columns.Contains(ModifyField))
            {
                dt.Columns.Add(ModifyField, typeof(string));
                dt.Columns[ModifyField].Caption = ModifyField;
            }

            for (int i = 0; i < KeyFieldValue.Count; i++)
            {
                DataRow[] dr = dt.Select(KeyField + "='" + KeyFieldValue[i].ToString() + "'");
                //for (int n = 0; n < dr.Count(); n++)
                if (dr.Count() > 0) //只考虑唯一的记录
                {
                    DataRow dr1 = dr[0]; //只考虑唯一的记录
                    dr1.BeginEdit();
                    //string ss = ModifyFieldValue[i].ToString();
                    if (dr1.Table.Columns[ModifyField].DataType == Type.GetType("System.DateTime")
                        && ModifyFieldValue[i].ToString().Trim() == "")
                        dr1[ModifyField] = DBNull.Value;
                    else
                        dr1[ModifyField] = ModifyFieldValue[i];
                    if (dr1.Table.Columns.Contains(UpdateFlag) && ModifyField != UpdateFlag) //
                    {   //如果当前为修改UpdateFlag，则按修改的值为准,2016-08-18加入&& ModifyField != UpdateFlag
                        dr1[UpdateFlag] = dr1[UpdateFlag].ToString() == "I" ? "I" : "U";
                    }
                    dr1.EndEdit();
                }
                else
                {
                    //新增记录
                    DataRow dr1 = dt.NewRow();
                    dr1[KeyField] = KeyFieldValue[i];
                    //string ss = ModifyFieldValue[i].ToString();
                    if (dr1.Table.Columns[ModifyField].DataType == Type.GetType("System.DateTime")
                        && ModifyFieldValue[i].ToString().Trim() == "")
                        dr1[ModifyField] = DBNull.Value;
                    else
                        dr1[ModifyField] = ModifyFieldValue[i];
                    if (dr1.Table.Columns.Contains(UpdateFlag))
                        dr1[UpdateFlag] = "I";
                    dt.Rows.Add(dr1);
                }
            }
        }

        public static void ModifyDataTableFieldValue(DataTable dt, string locationField, string locationValue, string FieldName, object FieldValue)
        {
            string UpdateFlag = "UpdateFlag";//暂时只处理一个从表的控制
            if (dt.TableName == "Detail")
                UpdateFlag = "UpdateFlag";
            else
            {
                int i = dt.TableName.IndexOf("Detail_");
                if (i >= 0)
                    UpdateFlag = UpdateFlag + dt.TableName.Substring(7, dt.TableName.Length - 7);
            }
            DataRow[] dr = dt.Select(locationField + "='" + locationValue + "'");
            if (dr.Count() > 0)
            {
                foreach (DataRow dr1 in dr)
                {
                    dr1.BeginEdit();
                    //string ss = ModifyFieldValue[i].ToString();
                    if (dr1.Table.Columns[FieldName].DataType == Type.GetType("System.DateTime")
                        && FieldValue.ToString().Trim() == "")
                        dr1[FieldName] = DBNull.Value;
                    else
                        dr1[FieldName] = FieldValue;
                    if (dr1.Table.Columns.Contains(UpdateFlag) && FieldName != UpdateFlag) //
                    {   //如果当前为修改UpdateFlag，则按修改的值为准,2016-08-18加入&& FieldName != UpdateFlag
                        dr1[UpdateFlag] = dr1[UpdateFlag].ToString() == "I" ? "I" : "U";
                    }
                    dr1.EndEdit();
                }
            }
            else
            {
                //新增记录
                DataRow newDr = dt.NewRow();
                newDr[locationField] = locationValue;
                newDr[FieldName] = FieldValue;
                if (dt.Columns.Contains(UpdateFlag))  //2016-08-18改为多从表
                    newDr[UpdateFlag] = "I";
                dt.Rows.Add(newDr);
            }
        }
        /// <summary>
        /// 把dt1中的KeyField的数据复制到dt中
        /// 创建：TIM 2016-07-07
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt"></param>
        /// <param name="KeyField"></param>
        /// <param name="KeyFieldValue"></param>
        public static void CopyDataTableToDataTable(DataTable dt1, DataTable dt, string KeyField, List<object> KeyFieldValue)
        {
            if (dt1 != null)
            {
                //string TableName = dt.TableName;
                //dt = null;
                //dt = new DataTable(TableName);
                string UpdateFlag = "UpdateFlag";
                if (dt.TableName == "Detail")
                    UpdateFlag = "UpdateFlag";
                else
                {
                    //2016-08-18改为多从表
                    //if (dt.TableName == "Detail_2")
                    //    UpdateFlag = "UpdateFlag2";
                    int i = dt.TableName.IndexOf("Detail_");
                    if (i >= 0)
                        UpdateFlag = UpdateFlag + dt.TableName.Substring(7, dt.TableName.Length - 7);
                }



                foreach (DataColumn dc in dt1.Columns) //循环处理增加每一个字段
                    dt.Columns.Add(dc.ColumnName, dc.DataType);
                if (!dt.Columns.Contains(UpdateFlag))
                {
                    dt.Columns.Add(UpdateFlag, typeof(string));
                    dt.Columns[UpdateFlag].Caption = "*" + UpdateFlag;
                }
                if (KeyField != null)
                {
                    for (int i = 0; i < KeyFieldValue.Count; i++)
                    {
                        DataRow[] lstDR = dt1.Select(KeyField + "='" + KeyFieldValue[i].ToString() + "'");
                        foreach (DataRow dr in lstDR)
                        {
                            //新增记录
                            DataRow newDr = dt.NewRow();
                            foreach (DataColumn dc in dt1.Columns)
                            {
                                if (dc.DataType == Type.GetType("System.DateTime")
                                      && dr[dc.ColumnName].ToString() == "")
                                    newDr[dc.ColumnName] = DBNull.Value;
                                else
                                    newDr[dc.ColumnName] = dr[dc.ColumnName];
                            }

                            newDr[UpdateFlag] = "I";
                            dt.Rows.Add(newDr);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// SyncBusinessDataObject(DataSet dsDataObject, DataSet ClientDataSet)
        /// 功能：同步业务数据,用于Business提交，把ClientDataSet合并到dsDataObject
        /// 创建：TIM 2016-06-23
        /// </summary>
        /// <param name="dsDataObject"></param>
        /// <param name="ClientDataSet"></param>
        public static void SyncBusinessDataObject(DataSet dsDataObject, DataSet ClientDataSet)
        {
            if (dsDataObject == null)
                throw new FrmException("数据对象为空，请检阅相关文档！");
            if (ClientDataSet == null)
                return;
            foreach (DataTable dt in dsDataObject.Tables)   //遍历所有的DataTable，第一个表为主表，第二个表为从表
            {
                //按表名处理，区分是主表数据还是多表数据，暂时只实现一个主表一个从表数据，Master：主表；Detail：从表
                if (dt.TableName == "Master" && dt.Rows.Count > 1)
                    throw new FrmException("Master数据异常，请检阅相关文档！");

                if (ClientDataSet.Tables.Contains(dt.TableName)) //客户端存在数据
                {
                    DataTable csTable = ClientDataSet.Tables[dt.TableName];
                    //foreach (DataRow dr in dt.Rows)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        foreach (DataColumn dc in dt.Columns) //循环处理每一个字段数据
                        {
                            if (csTable.Columns.Contains(dc.ColumnName))
                                dr[dc] = csTable.Rows[i][dc];
                        }
                    }
                }
            }
        }

        /// <summary>
        /// DataTableAddDataRow(DataTable dt,DataTable dt1, int row)
        /// 从dt1中取row行的数据，向dt增加一行数据
        /// 创建：TIM 2016-06-24
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dt1"></param>
        /// <param name="row"></param>
        public static void DataTableAddDataRow(DataTable dt, DataTable dt1, int row)
        {
            if (row < 0 || row > dt1.Rows.Count - 1)
                return;
            if (dt == null || dt.Columns.Count == 0)
            {
                foreach (DataColumn dc in dt1.Columns) //循环处理增加每一个字段
                    dt.Columns.Add(dc.ColumnName, dc.DataType);
            }
            DataRow newDr = dt.NewRow();
            foreach (DataColumn dc in dt.Columns) //循环处理每一个字段数据
            {
                if (dt1.Columns.Contains(dc.ColumnName))
                    newDr[dc] = dt1.Rows[row][dc.ColumnName];
            }
            dt.Rows.Add(newDr);
        }

        /// <summary>
        /// ReplaceDataTableDataRow(DataTable dt, DataRow dr, int row)
        /// 替换dt中指定行的数据
        /// 创建：TIM 2016-06-24
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="dr"></param>
        /// <param name="row"></param>
        public static void ReplaceDataTableDataRow(DataTable dt, DataRow dr, int row)
        {
            if (dt.Rows.Count == 0 || row > dt.Rows.Count)
                throw new FrmException("数据异常，请检阅相关文档！");
            foreach (DataColumn dc in dt.Columns) //循环处理每一个字段数据
            {
                if (dr.Table.Columns.Contains(dc.ColumnName))
                    dt.Rows[row - 1][dc] = dr[dc];
            }
        }
        /// <summary>
        /// 同步数据表，从dtSource同步到dtTarget，按KeyField同步
        /// 创建：TIM 2016-07-18
        /// </summary>
        /// <param name="dtTarget"></param>
        /// <param name="dtSource"></param>
        /// <param name="KeyField"></param>
        public static void SyncDataTable(DataTable dtTarget, DataTable dtSource, string KeyField)
        {
            string FieldValue = "";
            if (dtSource == null || dtSource.Rows.Count == 0)
                return;
            foreach (DataRow drSource in dtSource.Rows)
            {
                FieldValue = drSource[KeyField].ToString();
                DataRow[] drTarget = dtTarget.Select(KeyField + "='" + FieldValue + "'");
                if (drTarget.Count() > 0)
                {
                    //修改原来有的数据
                    foreach (DataRow dr in drTarget)
                    {
                        dr.BeginEdit();
                        foreach (DataColumn dc in dtTarget.Columns)
                        {
                            if (dtSource.Columns.Contains(dc.ColumnName))
                            {
                                if (dtTarget.Columns[dc.ColumnName].DataType == Type.GetType("System.DateTime")
                                    && drSource[dc.ColumnName].ToString() == "")
                                    dr[dc.ColumnName] = DBNull.Value;
                                else
                                    dr[dc.ColumnName] = drSource[dc.ColumnName];
                            }
                        }
                        dr.EndEdit();
                    }
                }
                else
                {
                    //原来没有数据则增加
                    DataRow newDr = dtTarget.NewRow();
                    foreach (DataColumn dc in dtTarget.Columns) //循环处理每一个字段数据
                    {
                        if (dtSource.Columns.Contains(dc.ColumnName))
                        {
                            if (dtTarget.Columns[dc.ColumnName].DataType == Type.GetType("System.DateTime")
                                    && drSource[dc.ColumnName].ToString() == "")
                                newDr[dc.ColumnName] = DBNull.Value;
                            else
                                newDr[dc.ColumnName] = drSource[dc.ColumnName];
                        }
                    }
                    dtTarget.Rows.Add(newDr);
                }
            }
        }

        public static void DataTableAddDataRow(DataTable dt, DataRow[] dr)
        {
            if (dr.Count() <= 0)
                return;
            if (dt == null || dt.Columns.Count == 0)
            {
                //foreach (DataColumn dc in dr[0].Table.Columns) //循环处理增加每一个字段
                //    dt.Columns.Add(dc.ColumnName, dc.DataType);
                string dtName = dt.TableName;
                dt = dr[0].Table.Clone();//克隆表结构   
                dt.TableName = dtName;
            }

            for (int row = 0; row < dr.Count(); row++)
            {
                //dt2.ImportRow(dr);//复制行数据到新表   
                DataRow newDr = dt.NewRow();
                foreach (DataColumn dc in dt.Columns) //循环处理每一个字段数据
                {
                    if (dr[0].Table.Columns.Contains(dc.ColumnName))
                        newDr[dc] = dr[row][dc.ColumnName];
                }
                dt.Rows.Add(newDr);
            }
        }
        */
        #endregion
    }
}
