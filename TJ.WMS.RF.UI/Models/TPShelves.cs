using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    public class TPShelves
    {
        public string PaperNO { get; set; }
        /// <summary>
        /// 门店信息
        /// </summary>
        public string Storeinfo { get; set; }
        public string Barcode { get; set; }
        //商品说明
        public string GoodsDesc { get; set; }
        public string TrayNO { get; set; }
        public double Qty { get; set; }
        /// <summary>
        /// 退配数量
        /// </summary>
        public double ShipQty { get; set; }
        /// <summary>
        /// 己退数量
        /// </summary>
        public double SumQty { get; set; }
        public string ReceiptTypeID { get; set; }
        public string BatchTypeID { get; set; }
        public string LocationNO { get; set; }
        public string ProductDate { get; set; }
        public string EffectiveDate { get; set; }
        public string ShelfLife { get; set; }
    }
}