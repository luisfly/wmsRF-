using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// 越库移仓
    /// </summary>
    public class CS2MoveService : RFBase
    {
        public CS2MoveService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_CS2MoveAdd";

            ValidateUser();
        }
     



        public void ValidateTrayNO()
        {
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("TrayNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }

        public void Accept()
        {
            ExecuteBusinessProcess("RF_CS2MoveAdd");
            
        }



    }
}
