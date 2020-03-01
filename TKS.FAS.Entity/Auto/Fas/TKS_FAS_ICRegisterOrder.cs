using System;
using System.Text;
using System.Collections.Generic;
using System.Data;
namespace TKS.FAS.Entity
{
    /// <summary>
    ///工商注册订单明细
    /// </summary>	
    public class TKS_FAS_ICRegisterOrder
    {

        /// <summary>
        /// 订单编号
        /// </summary>		
        public string OrderNo
        {
            get;
            set;
        }
        /// <summary>
        /// 省份代码
        /// </summary>		
        public string Province
        {
            get;
            set;
        }
        /// <summary>
        /// 市区代码
        /// </summary>		
        public string City
        {
            get;
            set;
        }
        /// <summary>
        /// 乡镇代码
        /// </summary>		
        public string Town
        {
            get;
            set;
        }
        /// <summary>
        /// 工商注册服务需求
        /// </summary>		
        public string IsNeedICRegister
        {
            get;
            set;
        }
        /// <summary>
        /// 银行开户服务需求
        /// </summary>		
        public string IsNeedKH
        {
            get;
            set;
        }
        /// <summary>
        /// 一般纳税人需求
        /// </summary>		
        public string IsNeedYBNS
        {
            get;
            set;
        }
        /// <summary>
        /// 环评报告需求
        /// </summary>		
        public string IsNeedHPBG
        {
            get;
            set;
        }
        /// <summary>
        /// 企业性质
        /// </summary>		
        public string Company_Type
        {
            get;
            set;
        }
        /// <summary>
        /// 注册资金
        /// </summary>		
        public decimal Registered_Capital
        {
            get;
            set;
        }
        /// <summary>
        /// 经营范围
        /// </summary>		
        public string Bussiness_Range
        {
            get;
            set;
        }
        /// <summary>
        /// 法人
        /// </summary>		
        public string Legal_Person
        {
            get;
            set;
        }
        /// <summary>
        /// 法人身份证正面图片ID
        /// </summary>		
        public string Legal_ID_FrontPic
        {
            get;
            set;
        }
        /// <summary>
        /// 法人身份证反面图片ID
        /// </summary>		
        public string Legal_ID_BackPic
        {
            get;
            set;
        }
        /// <summary>
        /// 公司字号1
        /// </summary>		
        public string CompanyName
        {
            get;
            set;
        }
        
        /// <summary>
        /// 公司地址
        /// </summary>		
        public string CompanyAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 租赁合同附件
        /// </summary>		
        public string Lease_Agreement_Pic
        {
            get;
            set;
        }
        /// <summary>
        /// 房产证附件
        /// </summary>		
        public string POC_Pic
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人名称
        /// </summary>		
        public string Contact_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 联系人手机
        /// </summary>		
        public string Contact_Phone
        {
            get;
            set;
        }
        /// <summary>
        /// 监事名称
        /// </summary>		
        public string Supervise_Name
        {
            get;
            set;
        }
        /// <summary>
        /// 监事省份证正面照片ID
        /// </summary>		
        public string Supervise_ID_FrontPic
        {
            get;
            set;
        }
        /// <summary>
        /// 监事省份证正面背面ID
        /// </summary>		
        public string Supervise_ID_BackPic
        {
            get;
            set;
        }
        /// <summary>
        /// 代理供应商代码
        /// </summary>		
        public string VendorCode
        {
            get;
            set;
        }
        /// <summary>
        /// 订单备注
        /// </summary>		
        public string Order_Memo
        {
            get;
            set;
        }
        /// <summary>
        /// 订单状态 0 草稿 1 已提交，待支付，2 预付款，已支付 3 已受理，4 已完成，5 已付全款 ，6 已结案（文件交割完成）
        /// </summary>		
        public string Status
        {
            get;
            set;
        }
        /// <summary>
        /// 外包状态 1 已外发，2 已受理，3 已完成，4 已交割，5 已付款
        /// </summary>		
        public string Out_Source_Status
        {
            get;
            set;
        }
        /// <summary>
        /// 预付款金额
        /// </summary>		
        public decimal Pre_Pay_Money
        {
            get;
            set;
        }
        /// <summary>
        /// 付款标志
        /// </summary>		
        public string Pay_Status
        {
            get;
            set;
        }
        /// <summary>
        /// 付款金额
        /// </summary>		
        public decimal Pay_Money
        {
            get;
            set;
        }
        /// <summary>
        /// 收款金额
        /// </summary>		
        public decimal Get_Money
        {
            get;
            set;
        }
        /// <summary>
        /// 收款标志
        /// </summary>		
        public string Get_Status
        {
            get;
            set;
        }
        /// <summary>
        /// 递交日期
        /// </summary>		
        public DateTime? Submit_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 受理日期
        /// </summary>		
        public DateTime? Accept_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 外发日期
        /// </summary>		
        public DateTime? Out_Source_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 外发完成日期
        /// </summary>		
        public DateTime? Out_Complete_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 外包文件交割日期
        /// </summary>		
        public DateTime? Out_File_Get_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 收款完成日期
        /// </summary>		
        public DateTime? Get_Money_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 文件交割日期
        /// </summary>		
        public DateTime? File_Get_Date
        {
            get;
            set;
        }
        /// <summary>
        /// 付款完成日期
        /// </summary>		
        public DateTime? Pay_Money_Date
        {
            get;
            set;
        }

        public string NodeId { get; set; }

        public string Create_User { get; set; }

        public DateTime? Create_Date { get; set; }

    }

  
}