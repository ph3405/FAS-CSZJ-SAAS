using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///公式
    /// </summary>	
    public class TKS_FAS_Formula:ICloneable
    {

        /// <summary>
        /// Id
        /// </summary>		
        public string Id
        {
            get;
            set;
        }

        public string AccountId
        {
            get;set;
        }
        /// <summary>
        /// SubjectCode
        /// </summary>		
        public string SubjectCode
        {
            get;
            set;
        }
        /// <summary>
        /// SubjectName
        /// </summary>		
        public string SubjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 运算符
        /// </summary>		
        public string OperatorCharacter
        {
            get;
            set;
        }
        /// <summary>
        /// 取数规则  0 发生额  1   余额
        /// </summary>		
        public int ValueRule
        {
            get;
            set;
        }
        /// <summary>
        /// 金额1
        /// </summary>		
        public decimal M1
        {
            get;
            set;
        }
        /// <summary>
        /// 金额2
        /// </summary>		
        public decimal M2
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
        /// CreateDate
        /// </summary>		
        public DateTime? CreateDate
        {
            get;
            set;
        }
        /// <summary>
        /// 报表列ID，这个公式属于哪个报表列ID
        /// </summary>		
        public string ReportDetailTPLId
        {
            get;
            set;
        }

        /// <summary>
        /// 属于哪个报表模板
        /// </summary>
        public string ReportTPLId
        {
            get;set;
        }

        public object Clone()
        {
            TKS_FAS_Formula entity = new Entity.TKS_FAS_Formula();
            entity.Id = this.Id;
            entity.OperatorCharacter = this.OperatorCharacter;
            entity.AccountId = this.AccountId;
            entity.SubjectCode = this.SubjectCode;
            entity.SubjectName = this.SubjectName;
            entity.ValueRule = this.ValueRule;
            entity.ReportTPLId = this.ReportTPLId;
            entity.ReportDetailTPLId = this.ReportDetailTPLId;
            entity.M1 = this.M1;
            entity.M2 = this.M2;
            entity.CreateUser = this.CreateUser;
            entity.CreateDate = this.CreateDate;
            return entity;
        }
    }
}