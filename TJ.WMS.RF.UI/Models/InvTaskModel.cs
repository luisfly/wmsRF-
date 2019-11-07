using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    public class InvTaskModel
    {
        /// <summary>
        /// 盘点单号
        /// </summary>
        public string InventoryNO { get; set; }
        /// <summary>
        /// 储位
        /// </summary>
        public string LocationNO { get; set; }
        /// <summary>
        /// 储位
        /// </summary>
        public string hdLocationNO { get; set; }
        /// <summary>
        /// 托盘
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string AQty { get; set; }
        /// <summary>
        /// //C.零拣位，按个数盘点，C、B没有托盘
        /// </summary>
        public string StorageTypeID { get; set; }
        /// <summary>
        /// 2.拣货位，没有托盘
        /// </summary>
        public string LocationTypeID { get; set; }
        /// <summary>
        /// 盘点商品ID
        /// </summary>
        public string GoodsID { get; set; }
        /// <summary>
        /// 旧储位
        /// </summary>
        public string OldLocationNO { get; set; }
        /// <summary>
        /// 旧托盘
        /// </summary>
        public string OldTrayNO { get; set; }
        public string IsBoxInput { get; set; }
    }
}