using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// WMS拣货任务
    /// </summary>
    public class PickViewModel
    {

        /// <summary>
        /// 出库箱号
        /// </summary>
        public string ToTrayNO { get; set; }
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
        /// 拣货条码
        /// </summary>
        public string PickBarcode { get; set; }
        /// <summary>
        /// 商品条码
        /// </summary>
        public string Barcode { get; set; }
        /// <summary>
        /// 实拣数量
        /// </summary>
        public string DealQty { get; set; }
        /// <summary>
        /// 指令类型:2.托拣;3.箱拣;4.零拣
        /// </summary>
        public string AssignTypeID { get; set; }
        /// <summary>
        /// 外箱单位
        /// </summary>
        public int CaseUnits { get; set; }
        /// <summary>
        /// 拣货最小单位审批数量
        /// </summary>
        public float MinSrcQty { get; set; }
        /// <summary>
        /// 拣货托盘
        /// </summary>
        public string FromTrayNO { get; set; }
        /// <summary>
        /// 验证拣货托盘号
        /// </summary>
        public string CheckFromTrayNO { get; set; }
        /// <summary>
        /// 储位类型
        /// </summary>
        public string LocationTypeID { get; set; }
        /// <summary>
        /// 控制箱拣效验条码
        /// </summary>
        public string BoxPickValidateBarcode { get; set; }//20161026 TIM 1.箱拣效验条码;2.箱拣不效验条码

        public string PickTypeID { get; set; }//20180817 YZW 拣货方式: 0.按箱总量拣货;1.按单箱拣货

    }
}