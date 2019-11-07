using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// RF服务类
    /// </summary>
    public class RFService
    {

        private Dictionary<string, string> tokens = new Dictionary<string, string>(); //用户令牌

        static RFService _instance;

        /// <summary>
        /// 服务实例
        /// </summary>
        public static RFService Instance
        {
            get
            {
                try
                {
                    if (_instance == null)
                    {
                        _instance = new RFService();
                    }
                    return _instance;
                }
                catch (Exception ex)
                {
                    Loger.Error(ex);
                    return null;
                }
            }
        }
        //构造函数
        private RFService()
        {

        }

        /// <summary>
        /// 用户登录系统后，需要在RF服务中注册，产生一个唯一的Token
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <returns>服务令牌</returns>
        public string UserRegist(string user_id)
        {
            string newToken = StringUtil.GetMd5(System.Guid.NewGuid().ToString());
            tokens[user_id] = newToken;
            return newToken;
        }

        /// <summary>
        /// 用户登录系统后，需要在RF服务中注销
        /// </summary>
        /// <param name="user_id">用户ID</param>
        public void UserUnRegist(string user_id)
        {
            if (tokens.ContainsKey(user_id))
            {
                tokens.Remove(user_id);
            }
        }

        /// <summary>
        /// 检查用户的口令是否匹配
        /// </summary>
        /// <param name="user_id">用户ID</param>
        /// <param name="token">服务令牌</param>
        /// <returns></returns>
        public bool CheckUser(string user_id, string token)
        {
            if (!tokens.ContainsKey(user_id))
                throw new RFException("该用户已经退出系统");

            string curr_token = tokens[user_id];
            if (curr_token != token)
                throw new Exception("该用户已经在其他设备登录");

            return true;
        }
    }
}
