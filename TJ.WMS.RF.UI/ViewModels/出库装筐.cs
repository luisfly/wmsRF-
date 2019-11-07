using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.ViewModels
{
    /// <summary>
    /// 出库装筐
    /// </summary>
    public class OutBoxInViewModel
    {
        /// <summary>
        /// 胶筐码
        /// </summary>
        public string ContainerNO { get; set; }
        /// <summary>
        /// 出库箱编码
        /// </summary>
        public string TrayNO { get; set; }
    }
}