using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TKS.FAS.Common
{
    public  class AppException:ApplicationException
    {
        public string UserId { get; set; }

        /// <summary>
        /// 目标对象
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 用于展示给用户看的异常信息
        /// </summary>
        public string ShowError { get; set; }

        /// <summary>
        /// 详细的异常信息
        /// </summary>
        public string Error { get; set; }

        public string GUID { get; set; }

        public AppException(string userId,string target,string showError,string error):base(showError) {
            this.UserId = userId;
            this.Error = error;
            this.ShowError = showError;
            this.Target = target;
            this.GUID = Guid.NewGuid().ToString("N");
          
        }

        public AppException(string userId,string target,string showError,string error,string guidKey) : 
            base(showError +",错误代码["+guidKey+"]") {
            this.UserId = userId;
            this.Error = error;
            this.ShowError = showError;
            this.Target = target;
            this.GUID = guidKey;
        }
        /// <summary>
        /// 日志
        /// </summary>
        void Log() {

        }
    }
}
