using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    //辅助核算项明细
    public class TKS_FAS_CaculateHelperDetail
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
        /// 父项ID
        /// </summary>		
        public string ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 账套ID
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }
        /// <summary>
        /// IsValid
        /// </summary>		
        public int IsValid
        {
            get;
            set;
        }
        /// <summary>
        /// 编码
        /// </summary>		
        public string Code
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>		
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 类别
        /// </summary>		
        public string Type
        {
            get;
            set;
        }
        /// <summary>
        /// Address
        /// </summary>		
        public string Address
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人
        /// </summary>		
        public string ContactPerson
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
        /// 税号
        /// </summary>		
        public string TaxNum
        {
            get;
            set;
        }
        /// <summary>
        /// 部门负责人
        /// </summary>		
        public string HeadPerson
        {
            get;
            set;
        }
        /// <summary>
        /// 成立日期
        /// </summary>		
        public DateTime? FoundDate
        {
            get;
            set;
        }
        /// <summary>
        /// 撤销日期
        /// </summary>		
        public DateTime? CancelDate
        {
            get;
            set;
        }
        /// <summary>
        /// 备注
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// Custom1
        /// </summary>		
        public string Custom1
        {
            get;
            set;
        }
        /// <summary>
        /// Custom2
        /// </summary>		
        public string Custom2
        {
            get;
            set;
        }
        /// <summary>
        /// Custom3
        /// </summary>		
        public string Custom3
        {
            get;
            set;
        }
        /// <summary>
        /// Custom4
        /// </summary>		
        public string Custom4
        {
            get;
            set;
        }
        /// <summary>
        /// Custom5
        /// </summary>		
        public string Custom5
        {
            get;
            set;
        }
        /// <summary>
        /// Custom6
        /// </summary>		
        public string Custom6
        {
            get;
            set;
        }
        /// <summary>
        /// Custom7
        /// </summary>		
        public string Custom7
        {
            get;
            set;
        }
        /// <summary>
        /// Custom8
        /// </summary>		
        public string Custom8
        {
            get;
            set;
        }

    }

    public class Max_CaculateHelperDetailCode
    {
        public int Code
        {
            get;
            set;
        }
    }
}