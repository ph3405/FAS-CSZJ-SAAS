using System; 
using System.Text;
using System.Collections.Generic; 
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///固定资产
    /// </summary>	
    public class TKS_FAS_FixedAssets
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
        /// 资产编号
        /// </summary>		
        public string DocNo
        {
            get;
            set;
        }
        /// <summary>
        /// 资产名称
        /// </summary>		
        public string Name
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
        /// 录入期间
        /// </summary>		
        public string StartPeriod
        {
            get;
            set;
        }
        /// <summary>
        /// 增加方式   购入   在建工程转入
        /// </summary>		
        public string AddType
        {
            get;
            set;
        }
        /// <summary>
        /// 资产类别  001 房屋、建筑物 002 机器机械生产设备 003 器具、工具、家具 004 运输工具 005 电子设备 006 其他固定资产
        /// </summary>		
        public string AssetsClass
        {
            get;
            set;
        }
        /// <summary>
        /// 规格型号
        /// </summary>		
        public string SpecificationType
        {
            get;
            set;
        }
        /// <summary>
        /// 开始使用日期
        /// </summary>		
        public DateTime? StartUseDate
        {
            get;
            set;
        }
        /// <summary>
        /// 使用部门Id
        /// </summary>		
        public string UseDeptId
        {
            get;
            set;
        }
        /// <summary>
        /// 使用部门名称
        /// </summary>		
        public string UseDeptName
        {
            get;
            set;
        }
        /// <summary>
        /// 供应商
        /// </summary>		
        public string Supplier
        {
            get;
            set;
        }
        /// <summary>
        /// 折旧方法 1 平均年限法 2 不折旧
        /// </summary>		
        public string DepreciationMethod
        {
            get;
            set;
        }
        /// <summary>
        /// 录入当期是否折旧 0 否 1 是
        /// </summary>		
        public int IsStartPeriodDepreciation
        {
            get;
            set;
        }
        /// <summary>
        /// 累计折旧科目code
        /// </summary>		
        public string ADSubjectCode
        {
            get;
            set;
        }
        /// <summary>
        /// 累计折旧科目名
        /// </summary>		
        public string ADSubjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 折旧费用科目代码
        /// </summary>		
        public string DCostSubjectCode
        {
            get;
            set;
        }
        /// <summary>
        /// 折旧费用科目名称
        /// </summary>		
        public string DCostSubjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 资产清理科目代码
        /// </summary>		
        public string AssetImpairmentSubjectCode
        {
            get;
            set;
        }
        /// <summary>
        /// 资产清理科目名称
        /// </summary>		
        public string AssetImpairmentSubjectName
        {
            get;
            set;
        }
        /// <summary>
        /// 资产原值
        /// </summary>		
        public decimal InitialAssetValue
        {
            get;
            set;
        }
        /// <summary>
        /// 残值率
        /// </summary>		
        public decimal ScrapValueRate
        {
            get;
            set;
        }
        /// <summary>
        /// 预计残值
        /// </summary>		
        public decimal ScrapValue
        {
            get;
            set;
        }
        /// <summary>
        /// 预计使用月份
        /// </summary>		
        public int PreUseMonth
        {
            get;
            set;
        }
        /// <summary>
        /// 已折旧月份
        /// </summary>		
        public int DpreMonth
        {
            get;
            set;
        }
        /// <summary>
        /// 剩余使用月份
        /// </summary>		
        public int RemainderUseMonth
        {
            get;
            set;
        }
        /// <summary>
        /// 累计折旧
        /// </summary>		
        public decimal AccumulativeDpre
        {
            get;
            set;
        }
        /// <summary>
        /// 本年累计折旧
        /// </summary>		
        public decimal AccumulativeDpre_Y
        {
            get;
            set;
        }
        /// <summary>
        /// 以前年度累计折旧
        /// </summary>		
        public decimal PreviousAccumulativeDpre
        {
            get;
            set;
        }
        /// <summary>
        /// 每月折旧额
        /// </summary>		
        public decimal DprePerMonth
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
        /// 备注
        /// </summary>		
        public string Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 是否生成凭证 0 否 1 是
        /// </summary>		
        public int IsGenPZ
        {
            get;
            set;
        }

        public string GDCode { get; set; }

        public string GDName { get; set; }

        public decimal InputVAT { get; set; }

        /// <summary>
        /// 固定资产状态 0 正常 1 报废
        /// </summary>
        public int Status { get; set; }

    }

    public class FixedAssetsInitialAssetValue
    {
        public decimal InitialAssetValue
        {
            get;
            set;
        }
    }
}