using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 业务对象
    /// </summary>
    internal class BusinessObject
    {
        public string Summary; //提示信息
        public string Sql; //SQL语句
        public bool Repeated; //是否重复执行
        public bool InterActive; //是否交互
        public string AffectedParam; //交互参数
        public int ExpectedRows;//返回行数 


        public void DoValidate(Dictionary<string, SqlParameter> paras, RFBase rfbase)
        {
            if (InterActive)
            {
                //object v = FrmBase.Bss_Helper.GetValue(Sql, CommandType.Text, paras.Values.ToArray());
                //frmbase.SetParameter(AffectedParam, v);//以后需要扩展，通过DS返回多个参数值，目前AffectedParam只支持单个参数
                DataSet ds = RFBase.Bss_Helper.GetDataSet_SQL(Sql, paras.Values.ToArray());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    //处理返回参数
                    for (var n = 0; n < ds.Tables[0].Columns.Count; n++)
                    {
                        rfbase.SetParameter(ds.Tables[0].Columns[n].ToString(), ds.Tables[0].Rows[0][n]);
                    }
                }
            }
            else
            {
                //FrmBase.Bss_Helper.ExcuteCmd(Sql, CommandType.Text, paras.Values.ToArray());
                // 增加了加入返回行数控制
                int RowCount = RFBase.Bss_Helper.ExcuteCmd(Sql, CommandType.Text, paras.Values.ToArray());
                if (ExpectedRows != 0 && ExpectedRows != RowCount)
                    throw new RFException("数据可能已被其它用户修改，请刷新重试！");
            }
            //if (InterActive)
            //{
            //    object v = RFBase.Bss_Helper.GetValue(Sql, CommandType.Text, paras.Values.ToArray());
            //    rfbase.SetParameter(AffectedParam, v);
            //}
            //else
            //{
            //    RFBase.Bss_Helper.ExcuteCmd(Sql, CommandType.Text, paras.Values.ToArray());
            //}

        }
     
    }
}
