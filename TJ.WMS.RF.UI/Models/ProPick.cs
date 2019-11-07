using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    /// <summary>
    /// 成品拣货
    /// </summary>
    public class ProPick
    {
        public string OrgPaperNO { get; set; }
        /// <summary>
        /// 任务号
        /// </summary>
        public string PickTaskNO { get; set; }
        public string WorkRoomNO { get; set; }
        public int GoodsID { get; set; }
        public string TrayNO { get; set; }
        //拣货数量
        public int PickQty { get; set; }
        //待接数量
        public int Qty { get; set; }
        public string StoreNO { get; set; }
    }
}