using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 字符串操作类
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 获取字符串的MD5值
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        public static string GetMd5(string strText)
        {
            HashAlgorithm algorithm = MD5.Create();
            byte[] bytes = algorithm.ComputeHash(Encoding.Default.GetBytes(strText));
            StringBuilder Hash = new StringBuilder();
            foreach (byte b in bytes)
            {
                Hash.AppendFormat("{0:x2}", b);
            }
            return Hash.ToString();
        }
        public static string GetTextID(string Text)
        {
            int i = Text.IndexOf(".");
            if (i >= 0)
                return Text.Substring(0, i);
            else
                return Text;
        }
        public static string GetTextName(string Text)
        {
            int i = Text.IndexOf(".");
            if (i >= 0)
                return Text.Substring(i + 1);
            else
                return Text;
        }
    }
}
