using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    /// <summary>
    /// 收货
    /// </summary>
    public class AcceptV2
    {
        public string PaperNO { get; set; }
        public string TrayNO { get; set; }
        public string Barcode { get; set; }
        public int Qty { get; set; }
    }
}