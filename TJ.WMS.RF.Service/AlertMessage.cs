#region
/* 
 * 版    权：TMain @ rzcd
 * 过    程：AlertMessage
 * 建    立：TIM 
 * 创建时间：2017-12-06
 * 说    明：预警提示信息
 */
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TJ.WMS.RF.Service
{
    public class AlertMessage
    {
        /// <summary>
        /// 用户已退出，请重新登录！
        /// </summary>
        public static string User_LogOut = "用户已退出，请重新登录！";
        /// <summary>
        /// 用户权限不足，请与管理员联系！
        /// </summary>
        public static string User_NoPower = "用户权限不足，请与管理员联系！";
        /// <summary>
        /// 数据异常，请刷新后重试！
        /// </summary>
        public static string Data_Abnormality = "数据异常，请刷新后重试！";
        /// <summary>
        /// 数据对象设置异常，请检阅相关文档！
        /// </summary>
        public static string Data_SetObjectAbnormality = "数据对象设置异常，请检阅相关文档！";
        /// <summary>
        /// 数据修改异常，请重试
        /// </summary>
        public static string Data_ModifyError = "数据修改异常，请重试！";
        /// <summary>
        /// 新增数据重复，请重试！
        /// </summary>
        public static string Data_InsertError = "新增数据重复，请重试！";
        /// <summary>
        /// 查询语句不存在，请检阅相关文档！
        /// </summary>
        public static string Query_NoSQL = "查询语句不存在，请检阅相关文档！";
        /// <summary>
        /// 设置字段{0}值错误，请检阅相关数据！<br/>({1})
        /// </summary>
        public static string Control_SetValueError = "设置字段{0}值错误，请检阅相关数据！<br/>({1})";
        /// <summary>
        /// 业务名称不能为空，请检阅相关文档！
        /// </summary>
        public static string Business_ObjNameEmpyt = "业务名称不能为空，请检阅相关文档！";
        /// <summary>
        /// 数据对象不能为空，请检阅相关文档！
        /// </summary>
        public static string Business_DataObjEmpyt = "数据对象不能为空，请检阅相关文档！";
        /// <summary>
        /// 没有权限执行，请联系管理员！
        /// </summary>
        public static string Business_NoPower = "没有权限执行，请联系管理员！";
        /// <summary>
        /// 数据对象未列出业务检查参数，请检阅相关文档！<br/>({0})
        /// </summary>
        public static string Business_NoValidateParam = "数据对象未列出业务检查参数，请检阅相关文档！<br/>({0})";
        /// <summary>
        /// 数据对象未列出业务参数值，请检阅相关文档！<br/>({0})
        /// </summary>
        public static string Business_NoBusinessParams = "数据对象未列出业务参数值，请检阅相关文档！<br/>({0})";
        /// <summary>
        /// 从表未设置重复参数，请检阅相关文档
        /// </summary>
        public static string Business_NoRepeatedParams = "从表未设置重复参数，请检阅相关文档！";
        /// <summary>
        /// 重复参数设置不正确，请检阅相关文档！<br/>({0})
        /// </summary>
        public static string Business_RepeatedParamsError = "重复参数设置不正确，请检阅相关文档！<br/>({0})";
        /// <summary>
        /// 业务过程[{0}]不存在，请检阅相关文档！
        /// </summary>
        public static string Business_NoBusinessName = "业务过程[{0}]不存在，请检阅相关文档！";
        /// <summary>
        /// 业务检查参数[{0}]不存在，请检阅相关文档！
        /// </summary>
        public static string Business_NoCheckParam = "业务检查参数[{0}]不存在，请检阅相关文档！";
    }
}
