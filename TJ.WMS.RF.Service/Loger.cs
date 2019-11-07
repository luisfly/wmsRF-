using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.Data.Common;
using System.IO;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 日志服务
    /// </summary>
    public static class Loger
    {
        #region 变量区

        /// <summary>
        /// 日志对象实例
        /// </summary>
        static ILog log = LogManager.GetLogger(typeof(Loger));

        #endregion

        #region 属性区

        /// <summary>
        /// 获取是否允许显示调试级日志的标记
        /// </summary>
        public static bool IsDebugEnabled
        {
            get
            {
                return log.IsDebugEnabled;
            }
        }

        /// <summary>
        /// 获取是否允许显示信息级日志的标记
        /// </summary>
        public static bool IsInfoEnabled
        {
            get
            {
                return log.IsInfoEnabled;
            }
        }

        /// <summary>
        /// 获取是否允许显示警告级日志的标记
        /// </summary>
        public static bool IsWarnEnabled
        {
            get
            {
                return log.IsWarnEnabled;
            }
        }

        /// <summary>
        /// 获取是否允许显示错误级日志的标记
        /// </summary>
        public static bool IsErrorEnabled
        {
            get
            {
                return log.IsErrorEnabled;
            }
        }

        /// <summary>
        /// 获取是否允许显示致命级日志的标记
        /// </summary>
        public static bool IsFatalEnabled
        {
            get
            {
                return log.IsFatalEnabled;
            }
        }

        #endregion

        #region 静态构造函数

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static Loger()
        {
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
        }

        #endregion

        #region 公共静态方法区

        /// <summary>
        /// 调试日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        public static void Debug(object message)
        {
            log.Debug(message);
        }

        /// <summary>
        /// 调试日志记录(自定义格式化)
        /// </summary>
        /// <param name="format">包含零个或多个格式项</param>
        /// <param name="args">包含零个或多个要格式化的对象的 Object 数组。</param>
        public static void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        /// <summary>
        /// 信息级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        public static void Info(object message)
        {
            log.Info(message);
        }

        /// <summary>
        /// 信息级日志记录(自定义格式化)
        /// </summary>
        /// <param name="format">包含零个或多个格式项</param>
        /// <param name="args">包含零个或多个要格式化的对象的 Object 数组。</param>
        public static void InfoFormatted(string format, params object[] args)
        {
            log.InfoFormat(format, args);
        }

        /// <summary>
        /// 警告级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        public static void Warn(object message)
        {
            log.Warn(message);
        }

        /// <summary>
        /// 警告级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        /// <param name="exception">异常</param>
        public static void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        /// <summary>
        /// 警告级日志记录(自定义格式化)
        /// </summary>
        /// <param name="format">包含零个或多个格式项</param>
        /// <param name="args">包含零个或多个要格式化的对象的 Object 数组。</param>
        public static void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        /// <summary>
        /// 错误级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        public static void Error(object message)
        {
            log.Error(message);
        }

        /// <summary>
        /// 错误级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        /// <param name="exception">异常</param>
        public static void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        /// <summary>
        /// 错误级日志记录(自定义格式化)
        /// </summary>
        /// <param name="format">包含零个或多个格式项</param>
        /// <param name="args">包含零个或多个要格式化的对象的 Object 数组。</param>
        public static void ErrorFormatted(string format, params object[] args)
        {
            log.ErrorFormat(format, args);
        }

        /// <summary>
        /// 致命级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        public static void Fatal(object message)
        {
            log.Fatal(message);
        }

        /// <summary>
        /// 致命级日志记录
        /// </summary>
        /// <param name="message">需要记录的消息</param>
        /// <param name="exception">异常</param>
        public static void Fatal(object message, Exception exception)
        {
            log.Fatal(message, exception);
        }

        /// <summary>
        /// 致命级日志记录(自定义格式化)
        /// </summary>
        /// <param name="format">包含零个或多个格式项</param>
        /// <param name="args">包含零个或多个要格式化的对象的 Object 数组。</param>
        public static void FatalFormatted(string format, params object[] args)
        {
            log.FatalFormat(format, args);
        }

        /// <summary>
        /// 获取参数数组的字符串形式
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static string GetStrFromArray(DbParameter[] parameters)
        {
            if (parameters == null || parameters.Length == 0)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            foreach (DbParameter para in parameters)
            {
                sb.AppendFormat("@{0}:{1}; ", para.ParameterName, para.Value);
            }
            return sb.ToString();
        }
        #endregion
    }
}
