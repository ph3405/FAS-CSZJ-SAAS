using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Common
{
    public class ConfigHelper : CommonBase
    {
        public static string Read(string key)
        {

            string val = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrEmpty(val))
            {
                throw new AppException("", "Read", "初始化异常", key + "未配置", Guid.NewGuid().ToString("N"));
            }
            return val;

        }
    }
}
