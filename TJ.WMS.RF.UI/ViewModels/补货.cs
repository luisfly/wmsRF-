using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// WMS补货任务
    /// </summary>
    public class ReplenishViewModel
    {
        /// <summary>
        /// 中间托盘码
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        public string ToDoNO { get; set; }
        /// <summary>
        /// 指令号
        /// </summary>
        public string PaperNO { get; set; }
        /// <summary>
        /// 拣货储位
        /// </summary>
        public string FromLocationNO { get; set; }
        /// <summary>
        /// 验证拣货位号
        /// </summary>
        public string CheckLocationNO { get; set; }
        /// <summary>
        /// 拣货托盘
        /// </summary>
        public string FromTrayNO { get; set; }
        /// <summary>
        /// 验证拣货托盘号
        /// </summary>
        public string CheckFromTrayNO { get; set; }
        /// <summary>
        /// 补货类型
        /// </summary>
        public string GrpTypeID { get; set; }
        /// <summary>
        /// 目标储位
        /// </summary>
        public string ToLocationNO { get; set; }
        /// <summary>
        /// 拣货条码
        /// </summary>
        public string PickBarcode { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get; set; }
    }
}