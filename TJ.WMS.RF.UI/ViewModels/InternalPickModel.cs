using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    public class InternalPickModel
    {
        public string ParamName { get; set; }
        public string PaperNO { get; set; }
        public int GoodsID { get; set; }
        public string ProductDate { get; set; }
        public string TrayNO { get; set; }
        public double PickQty { get; set; }
        public string NewTrayNO { get; set; }
        public string NewLocationNO { get; set; }
        public string LocationNO { get; set; }
        public string StockBatchNO { get; set; }
        public string OutShipperNO { get; set; }
        public string StorageNO { get; set; }
    }
}