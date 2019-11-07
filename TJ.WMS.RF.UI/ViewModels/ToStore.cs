using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    public class ToStoreModels
    {
        /// <summary>
        /// 收货托盘码
        /// </summary>
        public string OldTrayNO { get; set; }
        /// <summary>
        /// 目标托盘、出库箱
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 越库货位
        /// </summary>
        public string Postion2 { get; set; }
        /// <summary>
        /// 当前越库任务Item
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 当前越库单号
        /// </summary>
        public string PaperNO { get; set; }
        public string SeedQty { get; set; }
        public string Barcode { get; set; }

    }
}