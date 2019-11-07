using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// 仓储分板
    /// </summary>
    public class MatchplateCCH
    {
        /// <summary>
        /// 原始托盘
        /// </summary>
        public string OldTrayNO { get; set; }
        /// <summary>
        /// 目标托盘
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        public string AQty { get; set; }
        /// <summary>
        /// 目标储位
        /// </summary>
        public string NewLocation { get; set; }
        /// <summary>
        /// 旧托盘储位类型
        /// </summary>
        public string StorageTypeID { get; set; }
        public string IsFirstSave { get; set; }
        public string LocationTypeID { get; set; }
        public string IsBoxPick { get; set; }
        public int GoodsID { get; set; }
        public string sStockBatchNO { get; set; }
    }
}