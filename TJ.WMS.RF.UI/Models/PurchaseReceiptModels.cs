using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    public class PurchaseReceiptModels
    {
        //public string ID { get; set; }
        //public int FullNum { get; set; }
        //public int OrderNum { get; set; }
        //public int ReceiveNum { get; set; }

        public string OrderNO { get; set; }
        public string StoreNO { get; set; }
        public string barcode { get; set; }
        public string TrayNO { get; set; }
        public string ProductDate { get; set; }
        public string EffectiveDate { get; set; }
        public string Qty { get; set; }
    }
    
}