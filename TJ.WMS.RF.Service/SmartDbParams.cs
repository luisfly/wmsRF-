using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json.Linq;
namespace TJ.WMS.RF.Service
{
    public class SmartDbParams
    {
        public Dictionary<string, SqlParameter> paras = new Dictionary<string, SqlParameter>();
        public SmartDbParams()
        {

        }
        public void SetParameter(string paramName, string ParamType, object val)
        {
            if (val == null)
                val = DBNull.Value;

            if (paras.ContainsKey(paramName))//先删除原参数再增加
                RemoveParameter(paramName);

            paras[paramName] = new SqlParameter(paramName, RFBase.GetSqlDbType(ParamType));
            paras[paramName].Value = val;
        }
        public void SetParameter(string paramName, object val)
        {
            if (val == null)
                val = DBNull.Value;

            if (paras.ContainsKey(paramName))//先删除原参数再增加
                RemoveParameter(paramName);

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
        public object GetParameter(string paramName)
        {
            if (paras.ContainsKey(paramName))
                return paras[paramName].Value;
            return null;
        }
        public void ClearParameter()
        {
            paras.Clear();
        }
        public void RemoveParameter(string paramName)
        {
            if (paras.ContainsKey(paramName))
            {
                paras.Remove(paramName);
            }
        }
        public static SmartDbParams Parse(JObject values)
        {
            SmartDbParams dbParams = new SmartDbParams();
            foreach(var val in values)
            {
                dbParams.SetParameter(val.Key, val.Value);
            }
            return dbParams;
        }
        public static SmartDbParams Parse(JArray values)
        {
            //SmartDbParams dbParams = new SmartDbParams();
            //foreach (var item in values.Children())
            //{
            //    foreach (var val in ((JObject)item))
            //    {
            //        dbParams.SetParameter(val.Key, val.Value);
            //    }
                
            //}
            return Parse((JObject)values[0]);
        }
    }
}
