using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///报表模板详细列
    /// </summary>	
    public class TKS_FAS_ReportDetailTPL:ICloneable
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
        /// ColumnName
        /// </summary>		
        public string ColumnName
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
        /// 列的值来源类型  0  表达式计算  1  其它列合计
        /// </summary>		
        public int SourceType
        {
            get;
            set;
        }
        /// <summary>
        /// 值来源的配置，利润表中用来标识是收入还是费用成本 0 收入 1 费用 2 成本
        /// </summary>		
        public string SourceValue
        {
            get;
            set;
        }
        /// <summary>
        /// ParentId
        /// </summary>		
        public string ParentId
        {
            get;
            set;
        }
        /// <summary>
        /// 分类，以便在页面画报表的时候分块进行渲染
        /// 10，流动资产，11，流动负债，12，非流动资产，13，非流动负债，14，负债合计，
        /// 15，资产合计，16，所有者权益（或股东权益），17，负债和所有者权益（或股东权益）合计，
        /// 20，营业收入，21,营业利润，22，利润总额，23，净利润
        /// </summary>		
        public int Category
        {
            get;
            set;
        }
        /// <summary>
        /// 列排序
        /// </summary>		
        public int Seq
        {
            get;
            set;
        }

        /// <summary>
        /// 操作符号 + - #
        /// </summary>
        public string OperatorCharacter
        {
            get;set;
        }

        public object Clone()
        {
            TKS_FAS_ReportDetailTPL entity = new Entity.TKS_FAS_ReportDetailTPL();
            entity.Id = this.Id;
            entity.AccountId = this.AccountId;
            entity.PeriodId = this.PeriodId;
            entity.Seq = this.Seq;
            entity.SourceType = this.SourceType;
            entity.SourceValue = this.SourceValue;
            entity.ColumnName = this.ColumnName;
            entity.Category = this.Category;
            entity.ParentId = this.ParentId;
            entity.OperatorCharacter = this.OperatorCharacter;
            return entity;
        }
    }
}