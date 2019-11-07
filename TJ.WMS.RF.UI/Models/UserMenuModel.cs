using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TJ.WMS.RF.UI.Models
{
    public class UserMenuModel
    {
        public string User_ID { set; get; }
        public List<MenuModel> MenuList { set; get; }
    }
}