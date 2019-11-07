using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 检查对象
    /// </summary>
    internal class CheckObject
    {
        public string Summary; //提示信息
        public string ValidateType; //检查类型
        public string CompareType; //比较类型
        public string MaxValue; //最大值
        public string MinValue; //最小值
        public string ValidateParamName; //检查参数名
        public string CompareParamName; //比较参数名
        public string Sql; //SQL语句
        public bool Repeated; //是否重复执行 //2016-06-22
        public int CheckType = 0;//警告类型 0.警告终止;1.警告继续;2.警告是否继续
        public int Index;//当前行次

        public void DoValidate(Dictionary<string, SqlParameter> paras)
        {
            if (!paras.ContainsKey(ValidateParamName))
                throw new RFException("参数" + ValidateParamName + "不存在");
            object val = paras[ValidateParamName].Value;
            switch (ValidateType)
            {
                case "Required":
                    if (val == null || val.ToString() == "")
                        throw new RFException(FormatSummary(Summary, paras));
                    break;
                case "Range":
                    decimal d = 0;
                    if (val == null || !decimal.TryParse(val.ToString(), out d))
                        throw new RFException(Summary);
                    decimal max = 0, min = 0;
                    decimal.TryParse(MaxValue, out max);
                    decimal.TryParse(MinValue, out min);
                    if (!(d >= min && d <= max))
                        throw new RFException(FormatSummary(Summary, paras));
                    break;
                case "Query":
                case "SQL":
                case "QueryGrid":
                    object v = RFBase.Bss_Helper.GetValue(Sql, CommandType.Text, paras.Values.ToArray());
                    if (v == null)
                        throw new RFException(FormatSummary(Summary, paras));
                    break;
                default:
                    break;
            }
        }
        private string FormatSummary(string summary, Dictionary<string, SqlParameter> paras)
        {
            string[] lst = summary.Split('[');
            for (var n = 0; n < lst.Count(); n++)
            {
                if (lst[n].IndexOf(']') != -1)
                {
                    string FieldName_Value = lst[n].ToString().Substring(0, lst[n].IndexOf(']'));
                    string FieldName = FieldName_Value.Replace("_Value", "");
                    if (paras.ContainsKey(FieldName))
                    {
                        FieldName_Value = "[" + FieldName_Value + "]";
                        summary = summary.Replace(FieldName_Value, paras[FieldName].Value.ToString());
                    }
                }
            }
            return summary;
        }
    }
}
