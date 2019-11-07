using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.WMS.RF.Service
{
    public class RfidReplace:RFBase
    {
        public RfidReplace(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "";

            ValidateUser();
        }
    }
}
