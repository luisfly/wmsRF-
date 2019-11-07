using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// 收货指示上架
    /// </summary>
    public class AutoShelvesModel
    {
        /// <summary>
        /// 收货托盘
        /// </summary>
        public string TrayNO { get; set; }
        /// <summary>
        /// 指示上架的储位
        /// </summary>
        public string TargetLocationNO { get; set; }
        /// <summary>
        /// 效验上架储位
        /// </summary>
        public string LocationNO { get; set; }
    }
}