using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// RF基类
    /// </summary>
    public class RFBase
    {
        public Dictionary<string, SqlParameter> paras = new Dictionary<string, SqlParameter>();
        public string BusinessID; //业务逻辑ID
        public string Token; //服务令牌
        public static string LocalStoreNO = string.Empty; //本地店号

        private static SqlDbHelper frm_helper = null;
        /// <summary>
        /// 框架数据库操作类
        /// </summary>
        public static SqlDbHelper Frm_Helper
        {
            get
            {
                if (frm_helper == null)
                {
                    frm_helper = DbHelperFactory.GetDbHelper(ConfigurationManager.ConnectionStrings["FrameworkDBConnectionString"].ConnectionString);
                }
                return frm_helper;
            }
        }

        private static SqlDbHelper bss_helper = null;
        /// <summary>
        /// 业务数据库操作类
        /// </summary>
        public static SqlDbHelper Bss_Helper
        {
            get
            {
                if (bss_helper == null)
                {
                    bss_helper = DbHelperFactory.GetDbHelper(ConfigurationManager.ConnectionStrings["BusinessDBConnectionString"].ConnectionString);
                }
                return bss_helper;
            }
        }

        public RFBase()
        {
            SetParameter("Operator", string.Empty);
            SetParameter("OperName", string.Empty);
            SetParameter("LocalStoreNO", LocalStoreNO);
            SetParameter("LastUpdateTime", DateTime.Now);
            SetParameter("Old_LastUpdateTime", DateTime.Now);
        }

        public RFBase(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);
            SetParameter("LocalStoreNO", LocalStoreNO);
            SetParameter("LastUpdateTime", DateTime.Now);
            SetParameter("Old_LastUpdateTime", DateTime.Now);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }
        /// <summary>
        /// 验证用户
        /// </summary>
        internal void ValidateUser()
        {
            RFService.Instance.CheckUser(paras["Operator"].Value.ToString(), Token);
        }

        internal CheckObject[] GetCheckObjects(string paramName)
        {
            return GetCheckObjects(this.BusinessID, paramName);
        }

        internal CheckObject[] GetCheckObjects(string businessId, string paramName)
        {
            try
            {
                string sql = "";
                if (paramName == "*")
                    sql = string.Format("select Summary,ValidateType,CompareType,MaximumValue,MinimumValue,ParamToValidate,ParamToCompare,SQL from t_frm_BusinessChecks where BusinessID = '{0}' and Enabled=1 order by Idx", businessId);
                else
                    sql = string.Format("select Summary,ValidateType,CompareType,MaximumValue,MinimumValue,ParamToValidate,ParamToCompare,SQL from t_frm_BusinessChecks where BusinessID = '{0}' and ParamToValidate = '{1}' and Enabled=1 order by Idx", businessId, paramName);
                DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
                if (data.Rows.Count == 0)
                    throw new RFException("业务检查参数["+paramName+"]不存在！");
                CheckObject[] objects = new CheckObject[data.Rows.Count];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    objects[i] = new CheckObject();
                    objects[i].Summary = data.Rows[i]["Summary"].ToString();
                    objects[i].ValidateType = data.Rows[i]["ValidateType"].ToString();
                    objects[i].CompareType = data.Rows[i]["CompareType"].ToString();
                    objects[i].MaxValue = data.Rows[i]["MaximumValue"].ToString();
                    objects[i].MinValue = data.Rows[i]["MinimumValue"].ToString();
                    objects[i].ValidateParamName = data.Rows[i]["ParamToValidate"].ToString();
                    objects[i].CompareParamName = data.Rows[i]["ParamToCompare"].ToString();
                    objects[i].Sql = data.Rows[i]["SQL"].ToString();
                    objects[i].Sql = objects[i].Sql.Replace(':', '@');
                }
       
                return objects;

            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                throw ex;
            }
        }

        internal BusinessObject[] GetBusinessObjects(string businessId)
        {
            try
            {
                string sql = string.Format("select Summary, SQL, Repeated, InterActive, AffectedParam from t_frm_BusinessProcs where BusinessID = '{0}' and Enabled=1 order by Idx", businessId);
                DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
                BusinessObject[] objects = new BusinessObject[data.Rows.Count];
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    objects[i] = new BusinessObject();
                    objects[i].Summary = data.Rows[i]["Summary"].ToString();
                    objects[i].Repeated = data.Rows[i]["Repeated"].ToString() == "True" ? true : false;
                    objects[i].InterActive = data.Rows[i]["InterActive"].ToString() == "True" ? true : false;
                    objects[i].AffectedParam = data.Rows[i]["AffectedParam"].ToString();
                    objects[i].Sql = data.Rows[i]["SQL"].ToString();
                    objects[i].Sql = objects[i].Sql.Replace(':', '@');
                }
                return objects;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                throw ex;
            }
        }

        internal QueryObject GetQueryObject(string query_name)
        {
            try
            {
                string sql = string.Format("select SQL from t_frm_StdQuery where QueryName = '{0}'", query_name);
                DataTable data = Frm_Helper.GetDataSet_SQL(sql).Tables[0];
                QueryObject obj = null;
                if (data.Rows.Count > 0)
                {
                    obj = new QueryObject();
                    obj.Sql = data.Rows[0]["SQL"].ToString().Replace(':', '@');
                }
                return obj;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 设置参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        public void SetParameter(string paramName, object val)
        {
            if (val == null || val.ToString() == "")
                val = DBNull.Value;

            if (paras.ContainsKey(paramName))
                paras[paramName].Value = val;
            else
            {
                //paras[paramName] = new SqlParameter(paramName, val);
                if (val.GetType() == Type.GetType("System.String"))
                {
                    paras[paramName] = new SqlParameter(paramName, SqlDbType.VarChar);
                    paras[paramName].Value = val;
                }
                else
                {
                    paras[paramName] = new SqlParameter(paramName, val);
                }
            }
            //paras[paramName] = new SqlParameter(paramName, val);
        }
        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="val"></param>
        public object GetParameter(string paramName)
        {
            try
            {
                return paras[paramName].Value;
            }
            catch (Exception ex)
            {
                Loger.Error(ex);
                throw ex;
            }
        }

        /// <summary>
        /// 执行业务检查
        /// 创建：TIM 2015-05-16
        /// </summary>
        public void ExecuteBusinessCheck(string BusinessName, string ParamName)
        {
            this.BusinessID = BusinessName;
            ValidateUser();
            CheckObject[] objs = GetCheckObjects(ParamName);
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }
        /// <summary>
        /// 执行业务过程
        /// 创建：TIM 2015-05-16
        /// </summary>
        public void ExecuteBusinessProcess(string BusinessName, bool tryagain = true)
        {
            this.BusinessID = BusinessName;
            //验证每一项
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("*");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }

            BusinessObject[] bos = GetBusinessObjects(BusinessID);
            Bss_Helper.BeginTrans();
            foreach (BusinessObject obj in bos)
            {
                try
                {
                    obj.DoValidate(paras, this);
                }
                catch (Exception ex)
                {
                    Bss_Helper.RollBack();
                    if (ex.Message.Contains("请重新运行该事务") && tryagain == true)
                    {
                        System.Threading.Thread.Sleep(300); //毫秒
                                                            //ValidateQty(model, false);
                        ExecuteBusinessProcess(BusinessName, false);

                        return;

                    }
                    else {
                        Loger.Error(ex);
                        if (ex.Message.Contains("请重新运行该事务"))
                        {
                            throw new Exception("操作异常请重试!", ex);
                        }
                        throw;
                    }
                   
                }
            }
            Bss_Helper.Commit();
        }

        /// <summary>
        /// 返回SqlDbType
        /// </summary>
        /// <param name="ParamType"></param>
        /// <returns></returns>
        public static SqlDbType GetSqlDbType(string ParamType)
        {
            SqlDbType DbType;
            switch (ParamType.ToLower())
            {
                case "string":
                case "ansistring":
                    DbType = SqlDbType.VarChar;
                    break;
                case "float":
                case "currency":
                case "decimal":
                case "double":
                case "single":
                    DbType = SqlDbType.Decimal;
                    break;
                case "datetime":
                case "enddatetime":
                    DbType = SqlDbType.DateTime;
                    break;
                case "integer":
                case "int16":
                case "int32":
                case "int64":
                case "uint16":
                case "uint32":
                case "uint64":
                    DbType = SqlDbType.Int;
                    break;
                case "date":
                case "enddate":
                    DbType = SqlDbType.Date;
                    break;
                case "time":
                    DbType = SqlDbType.Time;
                    break;
                case "boolean":
                    DbType = SqlDbType.Bit;
                    break;
                default:
                    DbType = SqlDbType.VarChar;
                    break;
            }
            return DbType;
        }
        /// <summary>
        /// 取服务器当前时间
        /// 创建：TIM 2017-07-12
        /// </summary>
        /// <returns></returns>
        //
        public static DateTime GetServerDateTime()
        {
            DateTime dt = DateTime.Now;
            DateTime.TryParse(Bss_Helper.GetDataSet_SQL("select getdate()").Tables[0].Rows[0][0].ToString(), out dt);
            return dt;
        }
        /// <summary>
        /// ClearParameter()
        /// 清空当前参数，保留系统参数
        /// 创建：TIM 2016-06-24
        /// </summary>
        public void ClearParameter()
        {
            Dictionary<string, SqlParameter> tmpParas = new Dictionary<string, SqlParameter>();

            foreach (KeyValuePair<string, SqlParameter> p in paras)
            {
                //if (p.Key == "Operator" || p.Key == "OperName" || p.Key == "LocalStoreNO" || p.Key == "LastUpdateTime" || p.Key == "Old_LastUpdateTime")
                tmpParas[p.Key] = new SqlParameter(p.Key, p.Value);
            }
            foreach (KeyValuePair<string, SqlParameter> p in tmpParas)
            {
                if (p.Key == "Operator" || p.Key == "OperName" || p.Key == "LocalStoreNO"
                    || p.Key == "LastUpdateTime" || p.Key == "Old_LastUpdateTime" || p.Key == "ProjectID")
                    continue;
                else
                    paras.Remove(p.Key);
            }

        }
        public void SetParameter(string paramName, string ParamType, object val)
        {
            if (!paras.ContainsKey(paramName))
            {
                paras[paramName] = new SqlParameter(paramName, GetSqlDbType(ParamType));
            }
            if (val == null || (val.ToString() == string.Empty && paras[paramName].DbType != DbType.String
                                 && paras[paramName].DbType != DbType.AnsiString))
                val = DBNull.Value;
            paras[paramName].Value = val;
        }
    }
}
