using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    public class StockMoveTaskModel
    {

        public string ParamName { get; set; }
        public string PaperNO { get; set; }
        public int GoodsID { get; set; }
        public string ProductDate { get; set; }
        public string FromStorageNO { get; set; }
        public string ToStorageNO { get; set; }
        public string TrayNO { get; set; }
        public double PickQty { get; set; }
        public string StockBatchNO { get; set; }
        public string NewTrayNO { get; set; }
        public string ToLocationNO { get; set; }
        
    }
}