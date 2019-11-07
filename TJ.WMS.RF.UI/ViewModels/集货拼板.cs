using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TJ.WMS.RF.UI.ViewModels
{
    public class PastesModels
    {
        //
        // GET: /集货拼板/

        /// <summary>
        /// 原始出库箱
        /// </summary>
        public string OldTrayNO { get; set; }
        /// <summary>
        /// 目标出库箱
        /// </summary>
        public string TrayNO { get; set; }
    }
}
