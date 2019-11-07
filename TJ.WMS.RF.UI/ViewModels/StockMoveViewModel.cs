using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// 移仓
    /// </summary>
    public class StockMoveViewModel
    {
        /// <summary>
        /// 原托盘
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 目标储位
        /// </summary>
        public string ToLocationNO { get; set; }
        public string LocationNO { get; set; }
    }
}