using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Data.SqlClient;
namespace TJ.WMS.RF.Service
{
    public class Business : RFBase
    {
        public DataSet dsDataObject = null;//数据对象DataSet
        public string BusinessName = string.Empty;//业务ID
        public bool IsAllRecord = true;//是否提交所有记录，false只提交增删改的记录
        public string ParamsRequiredValid = string.Empty;//返回设置业务非空检查的JSON，用于视图界面录入框的Required属性控制
        public string DataDic = string.Empty;//传入数据字典JSON用于替换业务提示字段[FieldName]中文显示内容
        public string CurrentValidateParam = string.Empty;//返回当前验证的参数名称，用于视图录入控件光标定位
        private string BusinessParams = string.Empty;//业务参数
        private CheckObject[] CheckObjects = null;
        private BusinessObject[] ProcessObjects = null;
        private string Operator = string.Empty;
        private string OperName = string.Empty;
        //private string token = string.Empty;
        private bool Loaded = false;
        private bool NeedTranscation = true;
        private string[] SysParams2 = new string[] { "Operator", "OperName", "LocalStoreNO", /*"LastUpdateTime",*/ /*"Old_LastUpdateTime"*/ };
        public int CheckIndex = -1;//控制业务检查执项次，警告后继续执行的当前次
        public Business(string Operator, string OperName, string token)
        {
            this.Operator = Operator;
            this.OperName = OperName;
            Token = token;
            SetParameter("Operator", Operator);
            SetParameter("OperName", OperName);
        }
        public void Load()
        {
            if (Loaded)
            {
                return;
            }
            if (BusinessName == string.Empty)
            {
                throw new RFException(AlertMessage.Business_ObjNameEmpyt);
            }
            string sql = string.Format("select * from t_frm_Business  where BusinessID = '{0}'", BusinessName);
            DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
            if (data == null || data.Rows.Count <= 0)//"业务过程[" + BusinessName + "]不存在，请检阅相关文档！"
                throw new RFException(string.Format(AlertMessage.Business_NoBusinessName, BusinessName));

            NeedTranscation = (bool)data.Rows[0]["NeedTranscation"] == true ? true : false;
            data.Dispose();

            //if (dsDataObject == null)2018-1-5
            //{
            //    throw new RFException(AlertMessage.Business_ObjNameEmpyt);
            //}
            GetBusinessParams();
            //if (OnlySubmitModify)
            //{
            //    for (int n = 0; n < dsDataObject.Tables.Count; n++)
            //    {
            //        if (dsDataObject.Tables[n].TableName.IndexOf("Detail") != -1)
            //        {
            //            string UpdateFlag = "UpdateFlag";
            //            if (dsDataObject.Tables[n].TableName.IndexOf("Detail_") != -1)
            //                UpdateFlag += dsDataObject.Tables[n].TableName.Substring(8);
            //            dsDataObject.Tables[n].DefaultView.RowFilter = UpdateFlag + " in('I','U','D')";
            //        }
            //    }

            //}
            CheckObjects = GetBusinessCheckObjects("*");
            ProcessObjects = GetBusinessProcessObjects();
            ParamsRequiredValid = GetRequiredValidJson();
            Loaded = true;
        }
        /// <summary>
        /// 执行Business业务 通过数据对象赋值，执行
        /// </summary>
        public void Execute()
        {
            CurrentValidateParam = string.Empty;
            if (!DBHelper.CheckBusinessPower(BusinessName, this.Operator))
            {
                throw new RFException(AlertMessage.Business_NoPower);
            }
            if (dsDataObject == null)//数据对象不能为空
                throw new RFException(AlertMessage.Business_DataObjEmpyt);
            if (!Loaded)
            {
                Load();
            }
            ValidateUser();
            ExecBusinessCheck();
            ExecBusinessProcess();

            for (int n = 0; n < dsDataObject.Tables.Count; n++)
            {
                dsDataObject.Tables[n].AcceptChanges();
            }
        }
        /// <summary>
        /// 清空参数并初始主表参数
        /// </summary>
        private void InitMasterParams()
        {
            ClearParameter();//系统参数不清空
            JObject businessParams = JObject.Parse(BusinessParams);//{"参数":"类型;是否重复"}
            //先初始主表参数
            DataTable dtMaster = dsDataObject.Tables["Master"];//主表
            if (dtMaster != null)
            {
                foreach (var para in businessParams)
                {
                    string ParamName = para.Key;
                    string[] s = para.Value.ToString().Split(';');
                    string ParamType = s[0];
                    bool Repeated = Convert.ToBoolean(s[1]) == true;
                    if (SysParams2.Contains<string>(ParamName)//系统参数不处理;//重复参数不处理
                        || Repeated)
                    {
                        continue;
                    }
                    if (dtMaster.Columns.Contains(ParamName))
                    {
                        SetParameter(ParamName, ParamType, dtMaster.Rows[dtMaster.Rows.Count - 1][ParamName]);
                    }
                }
                if (paras.ContainsKey("LastUpdateTime"))
                {
                    SetParameter("Old_LastUpdateTime", GetParameter("LastUpdateTime"));
                }
            }
            SetParameter("LastUpdateTime", "datetime", GetServerDateTime());//重取一次LastUpdateTime
            if (dtMaster!=null)  dtMaster.Dispose();
        }
        /// <summary>
        /// 执行业务检查处理
        /// </summary>
        /// <returns></returns>
        private bool ExecBusinessCheck()
        {
            InitMasterParams();//初始主表参数
            foreach (CheckObject obj in CheckObjects)
            {
                if (obj.Index <= CheckIndex) continue;//2018-05-29继续执行的控制，少于当前项的不执行，因为是继续执行，从下个起点开始前面无需重复执行
                if (!obj.Repeated)//不是从表参数，即主表参数，直接执行
                {
                    CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
                    obj.DoValidate(paras/*, DataDic*/);//执行业务检查
                    CurrentValidateParam = string.Empty;
                }
                else//执行从表参数检查
                {
                    DataTable dtDetail = null;//记录当前参数循环的从表
                    string[] sqlParams = DBHelper.GetSqlTextParams(obj.Sql);//取出当前语句的参数
                    dtDetail = GetDataObjectTable(obj.ValidateParamName, dsDataObject);//取当前参数的表
                    if (dtDetail != null && dtDetail.TableName.IndexOf("Detail") != -1 && dtDetail.Rows.Count > 0)
                    {
                        //找到从表并有记录
                        switch (obj.ValidateType)
                        {
                            case "Query":
                            case "SQL":
                            case "QueryGrid":
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    foreach (string ParamName in sqlParams)
                                    {
                                        if (dtDetail.Columns.Contains(ParamName))
                                        {
                                            if (ParamName.IndexOf("UpdateFlag") != -1)
                                            {
                                                string val = string.Empty;
                                                if (dr.RowState == DataRowState.Added)
                                                    val = "I";
                                                else if (dr.RowState == DataRowState.Modified)
                                                    val = "U";
                                                else if (dr.RowState == DataRowState.Deleted)
                                                    val = "D";
                                                SetParameter(ParamName, "string", val);
                                            }
                                            else
                                            {
                                                SetParameter(ParamName, dr[ParamName]);
                                            }
                                        }
                                    }
                                    CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
                                    obj.DoValidate(paras/*, DataDic*/);//执行业务检查
                                    CurrentValidateParam = string.Empty;
                                }
                                break;
                            default:
                                foreach (DataRow dr in dtDetail.Rows)
                                {
                                    if (dtDetail.Columns.Contains(obj.ValidateParamName))
                                    {
                                        SetParameter(obj.ValidateParamName, dr[obj.ValidateParamName]);
                                    }
                                    CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
                                    obj.DoValidate(paras/*, DataDic*/);//执行业务检查
                                    CurrentValidateParam = string.Empty;
                                }
                                break;
                        }
                    }
                    else
                    {
                        throw new RFException(AlertMessage.Business_RepeatedParamsError);
                    }
                }
            }

            //foreach (CheckObject obj in CheckObjects)
            //{
            //    if (obj.Index <= CheckIndex) continue;//2018-05-29继续执行的控制，少于当前项的不执行，因为是继续执行，从下个起点开始前面无需重复执行
            //    switch (obj.ValidateType)
            //    {
            //        case "Required":
            //        case "Query":
            //        case "SQL":
            //        case "QueryGrid":
            //            DataTable dtDetail = null;//记录当前参数循环的从表
            //            string[] sqlParams = DBHelper.GetSqlTextParams(obj.Sql);//取出当前语句的参数
            //            dtDetail = GetDataObjectTable(obj.ValidateParamName, dsDataObject);//取当前参数的表
            //            if (dtDetail != null && dtDetail.TableName.IndexOf("Detail") != -1 && dtDetail.Rows.Count > 0)
            //            {
            //                foreach (DataRow dr in dtDetail.Rows)
            //                {
            //                    foreach (string ParamName in sqlParams)
            //                    {
            //                        if (dtDetail.Columns.Contains(ParamName))
            //                        {
            //                            if (ParamName.IndexOf("UpdateFlag") != -1)
            //                            {
            //                                string val = string.Empty;
            //                                if (dr.RowState == DataRowState.Added)
            //                                    val = "I";
            //                                else if (dr.RowState == DataRowState.Modified)
            //                                    val = "U";
            //                                else if (dr.RowState == DataRowState.Deleted)
            //                                    val = "D";
            //                                SetParameter(ParamName, "string", val);
            //                            }
            //                            else
            //                            {
            //                                SetParameter(ParamName, dr[ParamName]);
            //                            }
            //                        }
            //                    }
            //                    CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
            //                    obj.DoValidate(paras, DataDic);//执行业务检查
            //                    CurrentValidateParam = string.Empty;
            //                }
            //            }
            //            else
            //            {
            //                if (dtDetail == null || (dtDetail != null && dtDetail.TableName == "Master"))
            //                {
            //                    CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
            //                    obj.DoValidate(paras, DataDic);//执行业务检查
            //                    CurrentValidateParam = string.Empty;
            //                }
            //            }
            //            break;
            //        default:
            //            CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
            //            obj.DoValidate(paras, DataDic);//执行业务检查
            //            CurrentValidateParam = string.Empty;
            //            break;
            //    }
            //}  
            return true;
        }
        /// <summary>
        /// 执行业务过程处理
        /// </summary>
        /// <returns></returns>
        private bool ExecBusinessProcess()
        {
            InitMasterParams();//初始主表参数
            //SetParameter("LastUpdateTime", "datetime", GetServerDateTime());//重取一次LastUpdateTime
            if (NeedTranscation)
            {
                Bss_Helper.BeginTrans();//开始事务
            }

            foreach (BusinessObject bis in ProcessObjects)
            {
                try
                {
                    if (!bis.Repeated)//不是循环执行，直接提交
                    {
                        bis.DoValidate(paras, this);//执行语句提交
                        continue;//直接进入下一个业务语句
                    }
                    DataTable dtDetail = null;//当前处理循环的从表
                    int rowItem = 0;//当前从表循环的记录
                    do//进入循环执行从表数据提交
                    {
                        string[] sqlParams = DBHelper.GetSqlTextParams(bis.Sql);//取出当前语句的参数
                        if (sqlParams != null && sqlParams.Count() > 0)//语句有参数，需要参数赋值
                        {
                            foreach (string ParamName in sqlParams)
                            {
                                if (SysParams2.Contains<string>(ParamName))
                                {
                                    continue;//系统参数不处理
                                }
                                if (dtDetail == null)
                                {
                                    dtDetail = GetDataObjectTable(ParamName, dsDataObject);//按第一个参数从表
                                }
                                if (dtDetail == null || dtDetail.TableName == "Master")
                                {
                                    dtDetail = null;
                                    continue;//没有到参数赋值，直接进入下一个参数;主表参数无需重新赋值
                                }
                                if (dtDetail.Rows.Count == 0)//当前从表没有数据，跳出执行,返回到下一个业务处理
                                {
                                    break; //foreach
                                }
                                if (dtDetail.Columns.Contains(ParamName))//表存在参数值 
                                {
                                    if (ParamName.IndexOf("UpdateFlag") != -1)
                                    {
                                        string val = string.Empty;
                                        if (dtDetail.Rows[rowItem].RowState == DataRowState.Added)
                                            val = "I";
                                        else if (dtDetail.Rows[rowItem].RowState == DataRowState.Modified)
                                            val = "U";
                                        else if (dtDetail.Rows[rowItem].RowState == DataRowState.Deleted)
                                            val = "D";
                                        SetParameter(ParamName, "string", val);
                                    }
                                    else
                                    {
                                        //SetParameter(ParamName, dtDetail.Rows[rowItem][ParamName]);
                                        if (dtDetail.Rows[rowItem].RowState == DataRowState.Deleted)
                                        {
                                            SetParameter(ParamName, dtDetail.Rows[rowItem][ParamName, DataRowVersion.Original]);
                                        }
                                        else
                                        {
                                            SetParameter(ParamName, dtDetail.Rows[rowItem][ParamName]);
                                        }

                                    }
                                }
                            }
                        }

                        if (dtDetail != null && dtDetail.Rows.Count > 0)//当前从表有数据才处理
                        {
                            bis.DoValidate(paras, this);//执行语句提交
                        }
                        rowItem++;
                        //doWhile:;
                    } while (dtDetail != null && rowItem < dtDetail.Rows.Count);//循环完毕后继续下一个业务语句处理
                }
                catch (Exception ex)
                {
                    if (NeedTranscation)
                    {
                        Bss_Helper.RollBack();
                    }
                    throw new RFException("执行【" + FormatSummary(bis.Summary) + "】错误：" + ex.Message);
                }
            }
            if (NeedTranscation)
            {
                Bss_Helper.Commit();//事务提交
            }
            //主表LastUpdateTime更新
            if (dsDataObject.Tables.IndexOf("Master") != -1)
            {
                DataTable dtMaster = dsDataObject.Tables["Master"];
                if (dtMaster != null && dtMaster.Columns.Contains("LastUpdateTime"))
                {
                    dtMaster.Rows[dtMaster.Rows.Count - 1].BeginEdit();
                    dtMaster.Rows[dtMaster.Rows.Count - 1]["LastUpdateTime"] = GetParameter("LastUpdateTime");
                    dtMaster.Rows[dtMaster.Rows.Count - 1].EndEdit();
                }
            }
            //提交内存表所有数据
            for (int n = 0; n < dsDataObject.Tables.Count; n++)
            {
                dsDataObject.Tables[n].AcceptChanges();
            }

            return true;
        }

        /// <summary>
        /// 获取业务检查对象
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        internal CheckObject[] GetBusinessCheckObjects(string paramName)
        {
            try
            {
                string sql = "";
                if (paramName == "*")
                    sql = string.Format("select a.Summary,a.ValidateType,a.CompareType,a.MaximumValue,a.MinimumValue,a.ParamToValidate,a.ParamToCompare,a.SQL,b.Repeated, a.CheckType from t_frm_BusinessChecks a left join t_frm_BusinessParams b on a.BusinessID = b.BusinessID and a.ParamToValidate = b.ParamName where a.BusinessID = '{0}' and a.Enabled=1 order by a.Idx", BusinessName);
                else
                    sql = string.Format("select a.Summary,a.ValidateType,a.CompareType,a.MaximumValue,a.MinimumValue,a.ParamToValidate,a.ParamToCompare,a.SQL,b.Repeated, a.CheckType from t_frm_BusinessChecks a left join t_frm_BusinessParams b on a.BusinessID = b.BusinessID and a.ParamToValidate = b.ParamName where a.BusinessID = '{0}' and a.ParamToValidate = '{1}' and a.Enabled=1 order by a.Idx", BusinessName, paramName);
                DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
                if (data.Rows.Count == 0 && paramName != "*") //2016-7-01 && paramName != "*"
                    throw new RFException(string.Format(AlertMessage.Business_NoCheckParam, paramName));
                CheckObject[] objects = new CheckObject[data.Rows.Count];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    objects[i] = new CheckObject();
                    objects[i].Summary = FormatSummary(data.Rows[i]["Summary"].ToString());
                    objects[i].ValidateType = data.Rows[i]["ValidateType"].ToString();
                    objects[i].CompareType = data.Rows[i]["CompareType"].ToString();
                    objects[i].MaxValue = data.Rows[i]["MaximumValue"].ToString();
                    objects[i].MinValue = data.Rows[i]["MinimumValue"].ToString();
                    objects[i].ValidateParamName = data.Rows[i]["ParamToValidate"].ToString();
                    objects[i].CompareParamName = data.Rows[i]["ParamToCompare"].ToString();
                    objects[i].Sql = data.Rows[i]["SQL"].ToString();
                    objects[i].Sql = objects[i].Sql.Replace(':', '@');
                    objects[i].Repeated = data.Rows[i]["Repeated"].ToString() == "True" ? true : false; //2016-06-23 增加初始参数是否为重复执行
                    objects[i].CheckType = data.Rows[i]["CheckType"] == DBNull.Value ? 0 : int.Parse(data.Rows[i]["CheckType"].ToString());//0.警告后终止;1.警告继续;2.警告后确认是否继续
                    objects[i].Index = i;
                }

                return objects;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取业务执行过程
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        internal BusinessObject[] GetBusinessProcessObjects()
        {
            try
            {
                string sql = string.Format("select Summary, SQL, Repeated, ExpectedRows, InterActive, AffectedParam from t_frm_BusinessProcs where BusinessID = '{0}' and Enabled=1 order by Idx", BusinessName);
                DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
                BusinessObject[] objects = new BusinessObject[data.Rows.Count];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    objects[i] = new BusinessObject();
                    objects[i].Summary = FormatSummary(data.Rows[i]["Summary"].ToString());
                    objects[i].Repeated = data.Rows[i]["Repeated"].ToString() == "True" ? true : false;
                    objects[i].InterActive = data.Rows[i]["InterActive"].ToString() == "True" ? true : false;
                    objects[i].AffectedParam = data.Rows[i]["AffectedParam"].ToString();
                    objects[i].Sql = data.Rows[i]["SQL"].ToString().Replace(':', '@');
                    //objects[i].Sql = objects[i].Sql.Replace(':', '@');
                    objects[i].ExpectedRows = int.Parse(data.Rows[i]["ExpectedRows"].ToString());
                }
                return objects;
            }
            catch (Exception ex)
            {
                //Loger.Error(ex);
                throw ex;
            }
        }
        /// <summary>
        /// 取当前参数的所在的DataTable
        /// </summary>
        /// <param name="Param"></param>
        /// <param name="dsDataObject"></param>
        /// <returns></returns>
        private DataTable GetDataObjectTable(string Param, DataSet dsDataObject)
        {
            DataTable dt = null;// dsDataObject.Tables[0];
            for (int i = 0; i < dsDataObject.Tables.Count; i++)
            {
                if (dsDataObject.Tables[i].Columns.Contains(Param))
                {
                    //cdt = OnlySubmitModify && dsDataObject.Tables[i].TableName.IndexOf("Detail") != -1 ? dsDataObject.Tables[i].GetChanges() : dsDataObject.Tables[i];
                    //if (cdt == null && dsDataObject.Tables[i].TableName.IndexOf("Detail") != -1)
                    //{
                    //    cdt = dsDataObject.Tables[i].Clone();
                    //}
                    if (dsDataObject.Tables[i].TableName == "Master")//主表
                    {
                        dt = dsDataObject.Tables[i].Rows[dsDataObject.Tables[i].Rows.Count - 1].RowState == DataRowState.Added ? dsDataObject.Tables[i].GetChanges(DataRowState.Added) : dsDataObject.Tables[i];
                    }
                    else//从表
                    {
                        dt = IsAllRecord ? dsDataObject.Tables[i] : dsDataObject.Tables[i].GetChanges();
                        if (dt == null)//当IsAllRecord=false只处理有增删改的记录时，表没有修改时GetChanges()为空
                        {
                            dt = dsDataObject.Tables[i].Clone();//返回空数据结构
                        }
                    }
                    break;
                }
            }
            return dt;
        }
        //private DataTable GetDataObjectTable(DataSet dsDataObject, string SQL)
        //{
        //    //必需有主表，第一个表为主表，第二个表为从表1；每三个表为从表2……
        //    string UpdateFlag = "@UpdateFlag";
        //    if (SQL.IndexOf(UpdateFlag) == -1)
        //    {
        //        return dsDataObject.Tables[0];
        //    }
        //    int n = 0;
        //    if (dsDataObject.Tables["Master"] != null)//如果有主表，
        //        n = 1;
        //    for (int i = dsDataObject.Tables.Count; i > 1; i--)
        //    {
        //        if (i - n == 1)
        //            UpdateFlag = "@UpdateFlag";
        //        else
        //            UpdateFlag = "@UpdateFlag" + (i - n).ToString();
        //        if (SQL.IndexOf(UpdateFlag) >= 0)
        //            return dsDataObject.Tables[i - n];
        //    }
        //    return dsDataObject.Tables[0];
        //}
        private void GetBusinessParams()
        {
            string Params = string.Empty;
            DataSet dsParams = RFBase.Frm_Helper.GetDataSet_SQL("select ParamName,ParamType,Repeated from t_frm_BusinessParams where BusinessID='" + BusinessName + "' order by Idx");
            if (dsParams != null || dsParams.Tables[0].Rows.Count > 0)
            {
                //Params = dsParams.Tables[0].ToJson();
                foreach (DataRow dr in dsParams.Tables[0].Rows)
                {
                    Params += ",\"" + dr["ParamName"].ToString() + "\":\"" + dr["ParamType"].ToString() + ";" + dr["Repeated"].ToString() + "\"";
                }
                if (Params == string.Empty)
                {
                    throw new RFException(AlertMessage.Business_NoBusinessParams);
                }
                Params = "{" + Params.Substring(1) + "}";
            }
            BusinessParams = Params;
            //if (BusinessParams == string.Empty)
            //{
            //    throw new RFException(AlertMessage.Business_NoBusinessParams);
            //}
        }
        /// <summary>
        /// 返回必填字段到视图界面，做界面提交表单的必填控制
        /// </summary>
        /// <returns></returns>
        private string GetRequiredValidJson()
        {
            string json = string.Empty;
            foreach (CheckObject obj in CheckObjects)
            {
                if (obj.ValidateType.ToString() == "Required")
                {
                    json += ",\"" + obj.ValidateParamName + "\":\"" + FormatSummary(obj.Summary.ToString()) + "\"";
                }
            }
            if (json != string.Empty)
            {
                json = "{" + json.Substring(1) + "}";
            }
            return json;
        }
        /// <summary>
        /// 格式化业务提示信息，替换[Field];[Field_Value]
        /// </summary>
        /// <param name="summary"></param>
        /// <returns></returns>
        private string FormatSummary(string summary)
        {
            //格式为[FieldName]
            if (string.IsNullOrEmpty(DataDic)) return summary;

            JObject j = JObject.Parse(DataDic);

            string[] lst = summary.Split('[');
            for (var n = 0; n < lst.Count(); n++)
            {
                if (lst[n].IndexOf(']') != -1)
                {
                    string FieldName = lst[n].ToString().Substring(0, lst[n].IndexOf(']'));
                    if (FieldName.IndexOf("_Value") == -1 && j.Property(FieldName) != null)
                    {
                        string[] value = j[FieldName].ToString().Split(';');
                        FieldName = "[" + FieldName + "]";
                        summary = summary.Replace(FieldName, value[0].ToString());
                    }
                    else
                    {
                        string FieldName_Value = FieldName;
                        FieldName = FieldName.Replace("_Value", "");
                        if (FieldName_Value.IndexOf("_Value") != -1 && paras.ContainsKey(FieldName))
                        {
                            FieldName_Value = "[" + FieldName_Value + "]";
                            summary = summary.Replace(FieldName_Value, paras[FieldName].Value.ToString());
                        }
                    }

                }
            }
            return summary;
        }

        /// <summary>
        /// 执行Business业务 通过参数赋值，执行
        /// </summary>
        /// <returns></returns>
        public void ExecuteForParam(SmartDbParams dbParams = null)
        {
            CurrentValidateParam = string.Empty;
            if (!DBHelper.CheckBusinessPower(BusinessName, this.Operator))
            {
                throw new RFException(AlertMessage.Business_NoPower);
            }
            if (!Loaded)
            {
                Load();
            }
            ValidateUser();
            if (dbParams != null)
            {
                foreach (var par in dbParams.paras)
                {
                    SetParameter(par.Key.ToString(), par.Value.Value);
                }
            }
            ExecBusinessCheckForParams();//执行业务检查

            object val = GetParameter("LastUpdateTime");
            SetParameter("Old_LastUpdateTime", val.ToString() == string.Empty ? null : val);
            SetParameter("LastUpdateTime", GetServerDateTime());

            string Summary = string.Empty;
            if (NeedTranscation)
            {
                Bss_Helper.BeginTrans();//开始事务
            }
            try
            {
                foreach (BusinessObject bis in ProcessObjects)
                {
                    Summary = bis.Summary;
                    bis.DoValidate(paras, this);
                }
                if (NeedTranscation)
                {
                    Bss_Helper.Commit();//事务提交
                }
            }
            catch (Exception ex)
            {
                if (NeedTranscation)
                {
                    Bss_Helper.RollBack();
                }
                throw new RFException("执行【" + FormatSummary(Summary) + "】错误：<br/><br/>" + ex.Message);
            }
        }
        /// <summary>
        /// 通过设置参数执行业务检查，所有一次性检查
        /// </summary>
        private bool ExecBusinessCheckForParams()
        {

            foreach (CheckObject obj in CheckObjects)
            {
                if (obj.Index <= CheckIndex) continue;//2018-06-11继续执行的控制，少于当前项的不执行，因为是继续执行，从下个起点开始前面无需重复执行
                CurrentValidateParam = obj.ValidateParamName;//返回检查失败的参数
                obj.DoValidate(paras/*, DataDic*/);//执行业务检查
                CurrentValidateParam = string.Empty;
            }
            return true;
        }
    }
}
