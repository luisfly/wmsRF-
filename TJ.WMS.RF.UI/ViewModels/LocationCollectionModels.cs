using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// 库存采集
    /// </summary>
   public class LocationCollectionModels
    {
        public string LocationNO { get; set; }
        public string TrayNO { get; set; }
        public string DealQty { get; set; }
        public string StorageTypeID { get; set; }
        public string Barcode { get; set; }
        public string StockBatchNO { get; set; }
        public string GoodsID { get; set; }
        
    }
}
