using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///报表模板
    /// </summary>	
    public class TKS_FAS_ReportTPL :ICloneable
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
        /// Title
        /// </summary>		
        public string Title
        {
            get;
            set;
        }
        /// <summary>
        /// AccountId
        /// </summary>		
        public string AccountId
        {
            get;
            set;
        }
        /// <summary>
        /// PeriodId
        /// </summary>		
        public string PeriodId
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
        /// UpdateUser
        /// </summary>		
        public string UpdateUser
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

        public object Clone()
        {
            TKS_FAS_ReportTPL entity = new Entity.TKS_FAS_ReportTPL();
            entity.Id = this.Id;
            entity.Title = this.Title;
            entity.AccountId = this.AccountId;
            entity.PeriodId = this.PeriodId;
            entity.CreateDate = this.CreateDate;
            entity.CreateUser = this.CreateUser;
            entity.UpdateDate = this.UpdateDate;
            entity.UpdateUser = this.UpdateUser;
            return entity;
        }
    }
}