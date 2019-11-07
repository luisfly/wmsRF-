using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 核心异常基类
    /// </summary>
    public class RFException : Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RFException()
            : base()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">描述错误的信息</param>
        /// <param name="resultType">错误类型</param>
        public RFException(string message)
            : base(message)
        {
        }
    }
}
