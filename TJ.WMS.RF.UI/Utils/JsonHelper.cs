using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Data;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using Newtonsoft.Json.Linq;

namespace TJ.WMS.RF.UI.Utils
{
    /// <summary>
    /// Json 操作类
    /// </summary>
    public static class JsonHelper
    {
        #region DataTable 转换为Json 字符串
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(this DataTable dt)
        {
            //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            //javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            //ArrayList arrayList = new ArrayList();
            //foreach (DataRow dataRow in dt.Rows)
            //{
            //    Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
            //    foreach (DataColumn dataColumn in dt.Columns)
            //    {
            //        if (dataColumn.DataType == typeof(string) || dataColumn.DataType == typeof(DateTime))
            //            dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToStr());
            //        else
            //            dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName]);
            //    }
            //    arrayList.Add(dictionary); //ArrayList集合中添加键值
            //}
            //return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串

            //return JsonConvert.SerializeObject(dt, new DataTableConverter());

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                //jw.WriteStartObject();
                //jw.WritePropertyName(dt.TableName);
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        jw.WritePropertyName(dc.ColumnName);
                        if (dc.DataType == typeof(string) || dc.DataType == typeof(DateTime))
                            ser.Serialize(jw, dr[dc].ToString());
                        else
                            ser.Serialize(jw, dr[dc]);
                    }
                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                //jw.WriteEndObject();
                sw.Close();
                jw.Close();
            }

            return sb.ToString();
        }
        public static string ToJson(this DataTable dt, string[] cloumns)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                JsonSerializer ser = new JsonSerializer();
                jw.WriteStartArray();
                foreach (DataRow dr in dt.Rows)
                {
                    jw.WriteStartObject();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if(cloumns.Contains(dc.ColumnName))
                        {
                            jw.WritePropertyName(dc.ColumnName);
                            if (dc.DataType == typeof(string) || dc.DataType == typeof(DateTime))
                            {
                                ser.Serialize(jw, dr[dc].ToString());
                            }
                            else
                            {
                                ser.Serialize(jw, dr[dc]);
                            }
                        }
                    }
                    jw.WriteEndObject();
                }
                jw.WriteEndArray();
                sw.Close();
                jw.Close();
            }
            return sb.ToString();
        }
        #endregion


        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }

                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }

                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }
                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }

        #endregion

        /// <summary>
        /// 获取Json字符串中的指定项的值
        /// </summary>
        /// <param name="jsonText"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetJsonValue(string jsonText, string key)
        {
            try
            {
                JObject json = JObject.Parse(jsonText);
                JToken token = json[key];
                if (token == null)
                    return null;
                else
                    return token.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}
