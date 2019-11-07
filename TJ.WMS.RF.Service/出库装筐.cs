using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace TJ.WMS.RF.Service
{
    public class OutBoxInService:RFBase
    {
        public OutBoxInService(string user_id, string user_name, string token)
        {
            SetParameter("Operator", user_id);
            SetParameter("OperName", user_name);

            this.Token = token;
            this.BusinessID = "RF_OutBoxInModify";

            ValidateUser();
        }

        public void ValidateTrayNO()
        {
            CheckObject[] objs = GetCheckObjects("ContainerNO");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }
        }

        public void Accept()
        {
            //验证每一项
            ValidateUser();
            CheckObject[] objs = GetCheckObjects("*");
            foreach (CheckObject obj in objs)
            {
                obj.DoValidate(paras);
            }

            BusinessObject[] bos = GetBusinessObjects(BusinessID);
            Bss_Helper.BeginTrans();
            foreach (BusinessObject obj in bos)
            {
                try
                {
                    obj.DoValidate(paras, this);
                }
                catch (Exception ex)
                {
                    Bss_Helper.RollBack();
                    Loger.Error(ex);
                    throw;
                }
            }
            Bss_Helper.Commit();
        }
    }
}
