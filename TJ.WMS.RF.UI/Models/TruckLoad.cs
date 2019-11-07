using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    /// <summary>
    /// 装车
    /// </summary>
    public class TruckLoad
    {
        /// <summary>
        /// 笼车号
        /// </summary>
        public string ContainerNO { get; set; }
        /// <summary>
        /// 出库箱号
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 装车单号
        /// </summary>
        public string PaperNO { get; set; }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string TruckNO { get; set; }
        public string StoreNO { get; set; }

        /// <summary>
        /// 目标出库箱
        /// </summary>
        /// 
        public string NewTrayNO { get; set; }
        /// <summary>
        /// 条码
        /// </summary>
        /// 
        public string Barcode { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        /// 
        public string AQty { get; set; }
        /// <summary>
        /// 控制是否首次操作
        /// </summary>
        /// 
        public string IsFirst { get; set; }
    
    }
}