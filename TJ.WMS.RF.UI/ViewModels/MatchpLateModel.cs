using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    public class MatchpLateModel
    {
        /// <summary>
        /// 原始出库箱
        /// </summary>
        public string OldTrayNO { get; set; }
        /// <summary>
        /// 目标出库箱
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 分板数量
        /// </summary>
        public string AQty { get; set; }
        /// <summary>
        /// 是否检查目标托盘1.检查
        /// </summary>
        public string IsCheckTrayNO { get; set; }

        public string StockBatchNO { get; set; }
    }
}