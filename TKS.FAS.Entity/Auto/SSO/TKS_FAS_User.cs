using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //TKS_FAS_User
    public class TKS_FAS_User
    {

        /// <summary>
        /// Id
        /// </summary>		
        public string Id
        {
            get;
            set;
        }
        /// <summary>
        /// UserName
        /// </summary>		
        public string UserName
        {
            get;
            set;
        }
        /// <summary>
        /// TrueName
        /// </summary>		
        public string TrueName
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        /// <summary>
        /// 性别，0：女，1：男
        /// </summary>		
        public string Sex
        {
            get;
            set;
        }
        /// <summary>
        /// 手机
        /// </summary>		
        public string Mobile
        {
            get;
            set;
        }
        /// <summary>
        /// Memo
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 0:停用，1：启用
        /// </summary>		
        public string Status
        {
            get;
            set;
        }

        /// <summary>
        /// 代帐服务 0 否 1 是
        /// </summary>
        public int DZService { get; set; }

        /// <summary>
        /// 工商注册服务 0 否 1  是
        /// </summary>
        public int ZCService { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string Town { get; set; }

        /// <summary>
        /// CreateDate
        /// </summary>		
        public DateTime? CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// CreateUser
        /// </summary>		
        public string CreateUser
        {
            get;
            set;
        }
        /// <summary>
        /// UpdateDate
        /// </summary>		
        public DateTime? UpdateDate
        {
            get;
            set;
        }
        /// <summary>
        /// UpdateUser
        /// </summary>		
        public string UpdateUser
        {
            get;
            set;
        }

    }
}