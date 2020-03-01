using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using DapperExtensions;
using TKS.FAS.Entity;
using TKS.FAS.Entity.FAS;
using TKS.FAS.Common;
using System.Data;


namespace TKS.FAS.BLL.FAS
{
    public class ReportBLL : CommonBase
    {

        List<TKS_FAS_GLBalanceExt> GetData(string periodId, IDbTransaction ts)
        {


            string sql = @" 


with MM as (
select K.*,J.RootCode,(select top 1 A.Credit_Debit  from TKS_FAS_AccountSubject A 
where A.Code=j.rootCode and A.accountid=j.AccountId) as Credit_Debit,j.SLevel,j.IsLeaf from (

		select SubjectCode,
--SCredit_Debit, 
sum(YearStartBWBBAL) as YearStartBWBBAL,sum(BWBCreditTotal) as BWBCreditTotal,
			sum(BWBDebitTotal) as BWBDebitTotal, sum(BWBEndBAL_J) as BWBEndBAL_J,sum(BWBEndBAL_D) as BWBEndBAL_D,
         sum(BWBStartBAL_J) as BWBStartBAL_J,sum(BWBStartBAL_D) as BWBStartBAL_D,
		sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,accountId 
from(
			select SubjectCode, 
--SCredit_Debit,
			sum(YearStartBWBBAL) as YearStartBWBBAL,
		 
			sum(BWBCreditTotal) as BWBCreditTotal,
			sum(BWBDebitTotal) as BWBDebitTotal,
		 
			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
			sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
			sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D,
            sum((case when  SCredit_Debit=0 then  BWBStartBAL else 0 end)) as  BWBStartBAL_J,
			sum((case when  SCredit_Debit=1 then  BWBStartBAL else 0 end)) as  BWBStartBAL_D
			,AccountId
		from TKS_FAS_GLBalance
			where PeriodId=@PeriodId 
		group by SubjectCode,AccountId
--,SCredit_Debit
union
			select SubjectCode,
-- SCredit_Debit,
			sum(YearStartBWBBAL) as YearStartBWBBAL,
			sum(BWBCreditTotal) as BWBCreditTotal,
			sum(BWBDebitTotal) as BWBDebitTotal, 
			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
			 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
			sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
			sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D,
            sum((case when  SCredit_Debit=0 then  BWBStartBAL else 0 end)) as  BWBStartBAL_J,
			sum((case when  SCredit_Debit=1 then  BWBStartBAL else 0 end)) as  BWBStartBAL_D,

			AccountId
		from TKS_FAS_FGLBalance
		 where PeriodId=@PeriodId 
		group by SubjectCode,AccountId
--,SCredit_Debit
) A
 group by SubjectCode,AccountId 
--,SCredit_Debit
) K
  left join
  TKS_FAS_AccountSubject J
   on K.SubjectCode=J.Code and K.AccountId=J.AccountId
 ) 
 
select
SubjectCode,


YearStartBWBBAL,
 --(case when MM.Credit_Debit<> MM.SCredit_Debit then  -YearStartBWBBAL else YearStartBWBBAL end ) as YearStartBWBBAL,
BWBCreditTotal,
BWBDebitTotal,		
(case when(abs(BWBEndBAL_J) - abs(BWBEndBAL_D)) > 0 then BWBEndBAL_J-BWBEndBAL_D else 0 end) BWBEndBAL_J,
(case when(abs(BWBEndBAL_J) - abs(BWBEndBAL_D)) < 0 then BWBEndBAL_D-BWBEndBAL_J else 0 end) BWBEndBAL_D,
(case when(abs(BWBStartBAL_J) - abs(BWBStartBAL_D)) > 0 then BWBStartBAL_J-BWBStartBAL_D else 0 end) BWBStartBAL_J,
(case when(abs(BWBStartBAL_J) - abs(BWBStartBAL_D)) < 0 then BWBStartBAL_D-BWBStartBAL_J else 0 end) BWBStartBAL_D,

BWBDebitTotal_Y,
BWBCreditTotal_Y from MM where
  IsLeaf = 1
 union

select

rootCode,YearStartBWBBAL,BWBCreditTotal,BWBDebitTotal,
(case when (abs(BWBEndBAL_J)-abs(BWBEndBAL_D))>0 then BWBEndBAL_J-BWBEndBAL_D else 0 end) BWBEndBAL_J,
(case when (abs(BWBEndBAL_J)-abs(BWBEndBAL_D))<0 then BWBEndBAL_D-BWBEndBAL_J else 0 end) BWBEndBAL_D,
(case when (abs(BWBStartBAL_J)-abs(BWBStartBAL_D))>0 then BWBStartBAL_J-BWBStartBAL_D else 0 end) BWBStartBAL_J,
(case when (abs(BWBStartBAL_J)-abs(BWBStartBAL_D))<0 then BWBStartBAL_D-BWBStartBAL_J else 0 end) BWBStartBAL_D,
BWBDebitTotal_Y,
BWBCreditTotal_Y from (
		select RootCode, 
       sum(YearStartBWBBAL) YearStartBWBBAL,
		--sum(case when MM.Credit_Debit<>MM.SCredit_Debit then  -YearStartBWBBAL else YearStartBWBBAL end ) as YearStartBWBBAL,
		sum( BWBCreditTotal ) as BWBCreditTotal,
		sum( BWBDebitTotal  ) as BWBDebitTotal, 
		sum( BWBEndBAL_J  ) as BWBEndBAL_J, 
		sum( BWBEndBAL_D  ) as BWBEndBAL_D,
        sum( BWBStartBAL_J  ) as BWBStartBAL_J, 
		sum( BWBStartBAL_D  ) as BWBStartBAL_D,
		sum( BWBDebitTotal_Y  ) as BWBDebitTotal_Y, 
		sum( BWBCreditTotal_Y  ) as BWBCreditTotal_Y from MM 
		 where RootCode is not null
	 group by RootCode
) K
";
            List<TKS_FAS_GLBalanceExt> balance = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
                new { PeriodId = periodId }, ts).ToList();

            return balance;
        }


        //        List<TKS_FAS_GLBalanceExt> GetYearData(string periodId, IDbTransaction ts)
        //        {


        //            string sql = @" 


        //with MM as (
        //select K.*,J.RootCode,(select top 1 A.Credit_Debit  from TKS_FAS_AccountSubject A 
        //where A.Code=j.rootCode and A.accountid=j.AccountId) as Credit_Debit,j.SLevel,j.IsLeaf from (

        //		select SubjectCode,
        // SCredit_Debit, 
        //sum(YearStartBWBBAL) as YearStartBWBBAL,sum(BWBCreditTotal) as BWBCreditTotal,
        //			sum(BWBDebitTotal) as BWBDebitTotal, sum(BWBEndBAL_J) as BWBEndBAL_J,sum(BWBEndBAL_D) as BWBEndBAL_D,
        //		sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,accountId 
        //from(
        //			select SubjectCode, 
        //            SCredit_Debit,
        //			sum(YearStartBWBBAL) as YearStartBWBBAL,

        //			sum(BWBCreditTotal) as BWBCreditTotal,
        //			sum(BWBDebitTotal) as BWBDebitTotal,

        //			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
        //			sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
        //			sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D

        //			,AccountId
        //		from TKS_FAS_GLBalance
        //			where PeriodId=@PeriodId
        //		group by SubjectCode,AccountId ,SCredit_Debit
        //union
        //			select SubjectCode,
        // SCredit_Debit,
        //			sum(YearStartBWBBAL) as YearStartBWBBAL,
        //			sum(BWBCreditTotal) as BWBCreditTotal,
        //			sum(BWBDebitTotal) as BWBDebitTotal, 
        //			sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
        //			 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
        //			sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
        //			sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D,

        //			AccountId
        //		from TKS_FAS_FGLBalance
        //		 where PeriodId=@PeriodId
        //		group by SubjectCode,AccountId
        // ,SCredit_Debit
        //) A
        // group by SubjectCode,AccountId 
        // ,SCredit_Debit
        //) K
        //  left join
        //  TKS_FAS_AccountSubject J
        //   on K.SubjectCode=J.Code and K.AccountId=J.AccountId
        // ) 

        // select
        //SubjectCode,


        //--YearStartBWBBAL,
        //             (case when MM.Credit_Debit<> MM.SCredit_Debit then  -YearStartBWBBAL else YearStartBWBBAL end ) as YearStartBWBBAL,
        //            BWBCreditTotal,
        //            BWBDebitTotal,		
        //            (case when(abs(BWBEndBAL_J) - abs(BWBEndBAL_D)) > 0 then BWBEndBAL_J-BWBEndBAL_D else 0 end) BWBEndBAL_J,
        //            (case when(abs(BWBEndBAL_J) - abs(BWBEndBAL_D)) < 0 then BWBEndBAL_D-BWBEndBAL_J else 0 end) BWBEndBAL_D,

        //            BWBDebitTotal_Y,
        //            BWBCreditTotal_Y from MM where
        //              IsLeaf = 1
        //             union

        //select

        //rootCode,YearStartBWBBAL,BWBCreditTotal,BWBDebitTotal,
        //(case when (abs(BWBEndBAL_J)-abs(BWBEndBAL_D))>0 then BWBEndBAL_J-BWBEndBAL_D else 0 end) BWBEndBAL_J,
        //(case when (abs(BWBEndBAL_J)-abs(BWBEndBAL_D))<0 then BWBEndBAL_D-BWBEndBAL_J else 0 end) BWBEndBAL_D,
        //BWBDebitTotal_Y,
        //BWBCreditTotal_Y from (
        //		select RootCode, 
        //       --sum(YearStartBWBBAL) YearStartBWBBAL,
        //		 sum(case when MM.Credit_Debit<>MM.SCredit_Debit then  -YearStartBWBBAL else YearStartBWBBAL end ) as YearStartBWBBAL,
        //		sum( BWBCreditTotal ) as BWBCreditTotal,
        //		sum( BWBDebitTotal  ) as BWBDebitTotal, 
        //		sum( BWBEndBAL_J  ) as BWBEndBAL_J, 
        //		sum( BWBEndBAL_D  ) as BWBEndBAL_D,
        //		sum( BWBDebitTotal_Y  ) as BWBDebitTotal_Y, 
        //		sum( BWBCreditTotal_Y  ) as BWBCreditTotal_Y from MM 

        //	 group by RootCode
        //) K
        //";
        //            List<TKS_FAS_GLBalanceExt> balance = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
        //                new { PeriodId = periodId }, ts).ToList();

        //            return balance;
        //        }

        List<TKS_FAS_GLBalanceExt> GetYearData(string periodId, IDbTransaction ts)
        {
            string sql = @"

with MM as (
select K.*,J.RootCode,(select top 1 A.Credit_Debit  from TKS_FAS_AccountSubject A 
where A.Code=j.rootCode and A.accountid=j.AccountId) as Credit_Debit,j.SLevel,j.IsLeaf 

	from (
	     select SubjectCode,
 
			 sum(YearStartBWBBAL_J) as YearStartBWBBAL_J,
			 sum(YearStartBWBBAL_D) as YearStartBWBBAL_D,
			 sum(BWBCreditTotal) as BWBCreditTotal,
			 sum(BWBDebitTotal) as BWBDebitTotal,
			 sum(BWBEndBAL_J) as BWBEndBAL_J,
			 sum(BWBEndBAL_D) as BWBEndBAL_D,
			 sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
			 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
		     accountId		
				from(
						select SubjectCode,
					 
						--sum(case when  ECredit_Debit=0 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_J,
                        --sum(case when SCredit_Debit=ECredit_Debit and ECredit_Debit=0  then  YearStartBWBBAL 
						 --when SCredit_Debit<>ECredit_Debit and ECredit_Debit=0
						 --then -YearStartBWBBAL
						--else 0 end) as YearStartBWBBAL_J,				
                        sum(case when  Credit_Debit=0 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_J,


                        --sum(case when SCredit_Debit=ECredit_Debit and ECredit_Debit=1  then  YearStartBWBBAL 
                        --when SCredit_Debit<>ECredit_Debit and ECredit_Debit=1
						 --then -YearStartBWBBAL
						--else 0 end) as YearStartBWBBAL_D,
		 	            --sum(case when  ECredit_Debit=1 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_D,
                        sum(case when  Credit_Debit=1 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_D,


						sum(BWBCreditTotal) as BWBCreditTotal,
						sum(BWBDebitTotal) as BWBDebitTotal,
		 
						sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
						sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
						sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D

						,TKS_FAS_GLBalance.AccountId
					from TKS_FAS_GLBalance,TKS_FAS_AccountSubject
						where PeriodId=@PeriodId  and TKS_FAS_GLBalance.AccountId = TKS_FAS_AccountSubject.AccountId and 
                              SubjectId = TKS_FAS_AccountSubject.Id
					group by SubjectCode,Credit_Debit,TKS_FAS_GLBalance.AccountId ,SCredit_Debit
					union all
						select SubjectCode,
                        sum(case when SCredit_Debit=ECredit_Debit and ECredit_Debit=0  then  YearStartBWBBAL 
						 when SCredit_Debit<>ECredit_Debit and ECredit_Debit=0
						 then -YearStartBWBBAL
						else 0 end) as YearStartBWBBAL_J,	
						--sum(case when  ECredit_Debit=0   then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_J,
                        sum(case when SCredit_Debit=ECredit_Debit and ECredit_Debit=1  then  YearStartBWBBAL 
                        when SCredit_Debit<>ECredit_Debit and ECredit_Debit=1
						 then -YearStartBWBBAL
						else 0 end) as YearStartBWBBAL_D,
						--sum(case when  ECredit_Debit=1 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_D,
		
						sum(BWBCreditTotal) as BWBCreditTotal,
						sum(BWBDebitTotal) as BWBDebitTotal, 
						sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
						 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
						sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
						sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D,

						AccountId
					from TKS_FAS_FGLBalance
					 where PeriodId=@PeriodId 
					group by SubjectCode,AccountId
 
) A
 group by SubjectCode,AccountId 
 
) K
  left join
  TKS_FAS_AccountSubject J
   on K.SubjectCode=J.Code and K.AccountId=J.AccountId
 ) 
 
 select
SubjectCode,
 	
            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D
 
			  from MM where
              IsLeaf = 1
             union all

select

rootCode, 
    (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D
 
  from (
		select RootCode, 
 
	    sum(case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
            sum(case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D
 
		 from MM 
		 
	 group by RootCode
) K";


            List<TKS_FAS_GLBalanceExt> balance = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
                new { PeriodId = periodId }, ts).ToList();

            return balance;

        }

        //        List<TKS_FAS_GLBalanceExt> GetYearData_QC(string periodId, IDbTransaction ts)
        //        {
        //            string sql = @"

        //with MM as (
        //select K.*,J.RootCode,(select top 1 A.Credit_Debit  from TKS_FAS_AccountSubject A 
        //where A.Code=j.rootCode and A.accountid=j.AccountId) as Credit_Debit,j.SLevel,j.IsLeaf 

        //	from (
        //	     select SubjectCode,

        //			 sum(YearStartBWBBAL_J) as YearStartBWBBAL_J,
        //			 sum(YearStartBWBBAL_D) as YearStartBWBBAL_D,
        //			 sum(BWBCreditTotal) as BWBCreditTotal,
        //			 sum(BWBDebitTotal) as BWBDebitTotal,
        //			 sum(BWBEndBAL_J) as BWBEndBAL_J,
        //			 sum(BWBEndBAL_D) as BWBEndBAL_D,
        //			 sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
        //			 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
        //		     accountId		
        //				from(
        //						select SubjectCode, 

        //						--sum(case when  ECredit_Debit=0 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_J,
        //sum(case when    SCredit_Debit=0 then  YearStartBWBBAL 
        //						else 0 end) as YearStartBWBBAL_J,						
        //sum(case when  SCredit_Debit=1 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_D,

        //						sum(BWBCreditTotal) as BWBCreditTotal,
        //						sum(BWBDebitTotal) as BWBDebitTotal,

        //						sum(BWBDebitTotal_Y) as BWBDebitTotal_Y, sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
        //						sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
        //						sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D

        //						,AccountId
        //					from TKS_FAS_GLBalance
        //						where PeriodId=@PeriodId
        //					group by SubjectCode,AccountId ,SCredit_Debit
        //					union all
        //						select SubjectCode,

        //						sum(case when  SCredit_Debit=0 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_J,
        //						sum(case when  SCredit_Debit=1 then  YearStartBWBBAL else 0 end) as YearStartBWBBAL_D,

        //						sum(BWBCreditTotal) as BWBCreditTotal,
        //						sum(BWBDebitTotal) as BWBDebitTotal, 
        //						sum(BWBDebitTotal_Y) as BWBDebitTotal_Y,
        //						 sum(BWBCreditTotal_Y) as BWBCreditTotal_Y ,
        //						sum((case when  ECredit_Debit=0 then  BWBEndBAL else 0 end)) as  BWBEndBAL_J,
        //						sum((case when  ECredit_Debit=1 then  BWBEndBAL else 0 end)) as  BWBEndBAL_D,

        //						AccountId
        //					from TKS_FAS_FGLBalance
        //					 where PeriodId=@PeriodId
        //					group by SubjectCode,AccountId

        //) A
        // group by SubjectCode,AccountId 

        //) K
        //  left join
        //  TKS_FAS_AccountSubject J
        //   on K.SubjectCode=J.Code and K.AccountId=J.AccountId
        // ) 

        // select
        //SubjectCode,

        //            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
        //            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D

        //			  from MM where
        //              IsLeaf = 1
        //             union all

        //select

        //rootCode, 
        //    (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
        //            (case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D

        //  from (
        //		select RootCode, 

        //	    sum(case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) > 0 then YearStartBWBBAL_J-YearStartBWBBAL_D else 0 end) YearStartBWBBAL_J,
        //            sum(case when(abs(YearStartBWBBAL_J) - abs(YearStartBWBBAL_D)) < 0 then YearStartBWBBAL_D-YearStartBWBBAL_J else 0 end) YearStartBWBBAL_D

        //		 from MM 

        //	 group by RootCode
        //) K";


        //            List<TKS_FAS_GLBalanceExt> balance = cnn.Query<TKS_FAS_GLBalanceExt>(sql,
        //                new { PeriodId = periodId }, ts).ToList();

        //            return balance;

        //        }

        #region 资产负债表
        public ResponseZCFZGet ZCFZGet(RequestZCFZGet request)
        {

            ResponseZCFZGet response = new ResponseZCFZGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = "select * from TKS_FAS_ReportDetailTPL where accountId=@AccountId order by seq";
                    //模板
                    List<TKS_FAS_ZCFZReport> data = cnn.Query<TKS_FAS_ZCFZReport>(sql,
                        new
                        {
                            AccountId = user.AccountId

                        }, ts).ToList();
                    //指定期间的余额表
                    //用作计算的分2部分：1.一级科目，2.叶子科目
                    var balance = new List<TKS_FAS_GLBalanceExt>();
                    var yearBalance = new List<TKS_FAS_GLBalanceExt>();
                    //if (string.IsNullOrEmpty(request.PeriodId.Trim()))
                    //{
                    //    //期初
                    //    balance = GetData(user.AccountId, "", ts);
                    //    yearBalance = GetYearData(user.AccountId, "", ts);

                    //    //期初中 期初填写的值 为期末的值 
                    //    foreach (var item in balance)
                    //    {
                    //        item.BWBEndBAL = item.BWBStartBAL;
                    //        item.YBEndBAL = item.YBStartBAL;
                    //        item.NUMEndBAL = item.NUMStartBAL;
                    //        item.BWBEndBAL_D = item.BWBStartBAL_D;
                    //        item.BWBEndBAL_J = item.BWBStartBAL_J;
                    //    }
                    //}
                    //else
                    //{
                    balance = GetData(request.PeriodId, ts);
                    yearBalance = GetYearData(request.PeriodId, ts);
                    //}

                    //查出所有的账套下所有公式

                    sql = "select * from tks_fas_formula where accountId=@AccountId";

                    List<TKS_FAS_Formula> formula = cnn.Query<TKS_FAS_Formula>(sql, new { AccountId = user.AccountId }, ts).ToList();

                    List<TKS_FAS_ZCFZReport> report = new List<TKS_FAS_ZCFZReport>();

                    report = GenZCFZReport(data, balance, yearBalance, formula, ts);


                    List<TKS_FAS_ZCFZReport> ldzc = new List<TKS_FAS_ZCFZReport>() {
                        new TKS_FAS_ZCFZReport { ColumnName = "流动资产",Category=10, SourceType=3 } };

                    List<TKS_FAS_ZCFZReport> fldzc = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "非流动资产",Category=12, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> zchj = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "资产合计",
                             EndBalance =0,YearStartBalance=0,Category=15, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> ldfz = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "流动负债",Category=11, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> fldfz = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "非流动负债",Category=13, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> fzhj = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "负债合计",EndBalance=0,
                             YearStartBalance =0,Category=14, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> syzqy = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "所有者权益（或股东权益）",Category=16, SourceType=3 }  };

                    List<TKS_FAS_ZCFZReport> syzqyhj = new List<TKS_FAS_ZCFZReport>() {
                         new TKS_FAS_ZCFZReport { ColumnName = "负债和所有者权益（或股东权益）合计",
                             EndBalance =0,YearStartBalance=0,Category=17, SourceType=3 }  };


                    foreach (var item in report)
                    {
                        if (item.Category == 10)//流动资产
                        {
                            ldzc.Add(item);
                            if (item.SourceType == 1)
                            {
                                zchj[0].EndBalance += item.EndBalance;
                                zchj[0].YearStartBalance += item.YearStartBalance;
                            }

                        }

                        if (item.Category == 12)//非流动资产
                        {
                            fldzc.Add(item);
                            if (item.SourceType == 1)
                            {
                                zchj[0].EndBalance += item.EndBalance;
                                zchj[0].YearStartBalance += item.YearStartBalance;
                            }
                        }

                        if (item.Category == 11)//流动负债
                        {
                            ldfz.Add(item);
                            if (item.SourceType == 1)
                            {
                                fzhj[0].EndBalance += item.EndBalance;
                                fzhj[0].YearStartBalance += item.YearStartBalance;
                            }
                        }

                        if (item.Category == 13)//非流动负债
                        {
                            fldfz.Add(item);
                            if (item.SourceType == 1)
                            {
                                fzhj[0].EndBalance += item.EndBalance;
                                fzhj[0].YearStartBalance += item.YearStartBalance;
                            }
                        }

                        if (item.Category == 16)//所有者权益
                        {
                            syzqy.Add(item);
                            if (item.SourceType == 1)
                            {
                                syzqyhj[0].EndBalance += item.EndBalance;
                                syzqyhj[0].YearStartBalance += item.YearStartBalance;
                            }
                        }
                    }

                    syzqyhj[0].EndBalance += fzhj[0].EndBalance;
                    syzqyhj[0].YearStartBalance += fzhj[0].YearStartBalance;



                    ts.Commit();
                    response.IsSuccess = true;
                    response.Message = "加载完毕";

                    response.ZCHJ = zchj;
                    response.FLDZC = fldzc;
                    response.LDZC = ldzc;

                    response.SYZQYHJ = syzqyhj;
                    response.SYZQY = syzqy;
                    response.FZHJ = fzhj;
                    response.FLDFZ = fldfz;
                    response.LDFZ = ldfz;
                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseZCFZGet;
                }
            }
        }

        /// <summary>
        /// 打印工具界面，【资产负债表】打印
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public PrintZCFZGet PrintZCFZ(RequestZCFZGet request)
        {
            PrintZCFZGet response = new PrintZCFZGet();
            RequestZCFZGet newRequest = new RequestZCFZGet();
            List<TKS_FAS_MonthPeriodInfo> periods = new List<TKS_FAS_MonthPeriodInfo>();
            TKS_FAS_MonthPeriodInfo period = new TKS_FAS_MonthPeriodInfo();

            using (cnn = GetConnection())
            {
                var user = this.UserInfoGet(request.Token, null);
                //开始期间
                var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
               @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
               new { Id = request.Period_S });
                period = periodS;
                //结束期间
                //var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                //@"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                //new { Id = request.Period_E });
                if (periodS == null)
                {
                    return new PrintZCFZGet { IsSuccess = false, Message = "会计期间没有选择" };
                }
                //if (periodS.EndDate > periodE.EndDate)
                //{
                //    throw new NormalException("开始期间不能大于结束期间");
                //}
                //获取期间集合
                //periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                //     @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                //                and  StartDate >=@StartDateS  
                //                and StartDate<=@StartDateE ",
                //     new
                //     {
                //         StartDateS = periodS.StartDate.ToString(),
                //         StartDateE = periodE.StartDate.ToString(),
                //         AccountId = user.AccountId
                //     }).ToList();

                string periodWhere = string.Empty;//期间条件

            }
            try
            {
                newRequest = request;
                List<ZCFZGetData> lstData = new List<ZCFZGetData>();
                //foreach (var item in periods)
                //{
                newRequest.PeriodId = period.Id;
                ZCFZGetData z = new ZCFZGetData();
                z.Year = period.Year.ToString();
                z.Month = period.Month.ToString();
                z.Data = ZCFZGet(newRequest);
                lstData.Add(z);
                //}

                response.IsSuccess = true;
                response.Message = "加载完毕";
                response.PrintData = lstData;
                return response;
            }
            catch (Exception ex)
            {
                return this.DealException(response, ex) as PrintZCFZGet;
            }

        }

        private List<TKS_FAS_ZCFZReport> GenZCFZReport(List<TKS_FAS_ZCFZReport> data,
            List<TKS_FAS_GLBalanceExt> balance, List<TKS_FAS_GLBalanceExt> yearBalance, List<TKS_FAS_Formula> formula, IDbTransaction ts
            //, bool isAccountStartData
            )
        {

            decimal GD_EndBalance = 0;
            decimal LJ_EndBalance = 0;

            decimal GD_YearStartBalance = 0;
            decimal LJ_YearStartBalance = 0;
            //公式计算
            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 0)
                {
                    data[i].EndBalance = CalculateEndBalance(data[i].Id, balance, formula, ts);

                    data[i].YearStartBalance = CalculateYearStartBalance(data[i].Id, yearBalance, formula, ts);
                    //if (isAccountStartData)
                    //{
                    //    data[i].EndBalance = data[i].YearStartBalance;
                    //    data[i].YearStartBalance = 0;
                    //}
                    if (data[i].Category == 12 && data[i].ColumnName == "固定资产原价")
                    {
                        //获取固定资产原价
                        GD_EndBalance = data[i].EndBalance;
                        GD_YearStartBalance = data[i].YearStartBalance;
                    }
                    if (data[i].Category == 12 && data[i].ColumnName == "减：累计折旧")
                    {
                        //获取减：累计折旧
                        LJ_EndBalance = data[i].EndBalance;
                        LJ_YearStartBalance = data[i].YearStartBalance;
                    }
                }
                //求和
                //if (data[i].SourceType == 1)
                //{
                //    data[i].EndBalance = SumEndBalance(data[i].Category, data);

                //    data[i].YearStartBalance = SumYearStartBalance(data[i].Category, data);
                //    if (IsQiChu == "QC")
                //    {
                //        data[i].EndBalance = data[i].YearStartBalance;
                //        data[i].YearStartBalance = 0;
                //    }
                //}
            }

            //求和
            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 1)
                {
                    ////if (isAccountStartData)
                    ////{
                    ////    data[i].YearStartBalance = SumYearStartBalance_QC(data[i].Category, data);
                    ////    data[i].EndBalance = data[i].YearStartBalance;
                    ////    data[i].YearStartBalance = 0;
                    ////}
                    //else
                    //{
                    data[i].EndBalance = SumEndBalance(data[i].Category, data);

                    data[i].YearStartBalance = SumYearStartBalance(data[i].Category, data);

                    //}
                }
                if (data[i].SourceType == 0)
                {
                    //固定资产净值=固定资产原价-减：累计折旧
                    if (data[i].Category == 12 && data[i].ColumnName == "固定资产净值")
                    {
                        data[i].EndBalance = GD_EndBalance - LJ_EndBalance;

                        data[i].YearStartBalance = GD_YearStartBalance - LJ_YearStartBalance;
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// 年初求和
        /// </summary>
        /// <param name="category"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private decimal SumYearStartBalance(int category, List<TKS_FAS_ZCFZReport> data)
        {
            var fd = data.Where(p => p.Category == category).ToList();
            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                {
                    result += item.YearStartBalance;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= item.YearStartBalance;
                }

            }
            return result;
        }

        /// <summary>
        /// 年初求和(期初)
        /// </summary>
        /// <param name="category"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private decimal SumYearStartBalance_QC(int category, List<TKS_FAS_ZCFZReport> data)
        {
            var fd = data.Where(p => p.Category == category).ToList();
            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                {
                    result += item.EndBalance;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= item.EndBalance;
                }

            }
            return result;
        }
        /// <summary>
        /// 期末求和
        /// </summary>
        /// <param name="category"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private decimal SumEndBalance(int category, List<TKS_FAS_ZCFZReport> data)
        {
            var fd = data.Where(p => p.Category == category).ToList();
            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                {
                    result += item.EndBalance;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= item.EndBalance;
                }


            }
            return result;
        }

        /// <summary>
        /// 根据公式，计算年初
        /// </summary>
        /// <param name="id"></param>
        /// <param name="balance"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        private decimal CalculateYearStartBalance(string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);
            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
#if DEBUG
            if (formula.Where(p => p.SubjectName.Contains("应付账款")).Count() > 0)
            {

            }
#endif
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();
                if (bal == null)
                    continue;
                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                decimal val = 0;

                if (subject.Code == "3131" && bal.YearStartBWBBAL_J < 0)
                {
                    val = bal.YearStartBWBBAL_J - bal.YearStartBWBBAL_D;
                }

                else if (subject.Credit_Debit == 0)
                {
                    val = bal.YearStartBWBBAL_J - bal.YearStartBWBBAL_D;
                }
                else
                {
                    val = bal.YearStartBWBBAL_D - bal.YearStartBWBBAL_J;
                }
                if (item.OperatorCharacter == "+")
                {

                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据公式，计算期末
        /// </summary>
        /// <param name="id"></param>
        /// <param name="balance"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        private decimal CalculateEndBalance(string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);
            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();
                if (bal == null)
                    continue;
                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                decimal val = 0;
                if (subject.Credit_Debit == 0)
                {
                    val = bal.BWBEndBAL_J - bal.BWBEndBAL_D;
                }
                else
                {
                    val = bal.BWBEndBAL_D - bal.BWBEndBAL_J;
                }

                if (item.OperatorCharacter == "+")
                {
                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }

            }

            return result;
        }
        #endregion

        #region 利润表

        public ResponseLRGet LRGet(RequestLRGet request)
        {

            ResponseLRGet response = new ResponseLRGet();
            using (cnn = GetConnection())
            {
                var ts = cnn.BeginTransaction();
                try
                {
                    var user = this.UserInfoGet(request.Token, ts);

                    string sql = "select * from TKS_FAS_ReportDetailTPL where accountId=@AccountId order by seq";
                    //模板
                    List<TKS_FAS_LRReport> tpl = cnn.Query<TKS_FAS_LRReport>(sql,
                        new
                        {
                            AccountId = request.AccountList ?? user.AccountId

                        }, ts).ToList();
                    List<LRGetData> PrintData = new List<LRGetData>();
                    if (!string.IsNullOrEmpty(request.AccountList))
                    {
                        //开始期间
                        var periodS = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                       @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                       new { Id = request.Period_S }, ts);
                        //结束期间
                        var periodE = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                         @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                         new { Id = request.Period_E }, ts);
                        if (periodS == null)
                        {
                            return new ResponseLRGet { IsSuccess = false, Message = "会计期间没有选择" };
                        }
                        if (periodS.EndDate > periodE.EndDate)
                        {
                            throw new NormalException("开始期间不能大于结束期间");
                        }
                        //获取期间集合
                        List<TKS_FAS_MonthPeriodInfo> periods = cnn.Query<TKS_FAS_MonthPeriodInfo>(
                            @"select * from TKS_FAS_MonthPeriodInfo where AccountId=@AccountId 
                                and  StartDate >=@StartDateS  
                                and StartDate<=@StartDateE ",
                            new
                            {
                                StartDateS = periodS.StartDate.ToString(),
                                StartDateE = periodE.StartDate.ToString(),
                                AccountId = user.AccountId
                            }, ts).ToList();

                        foreach (var period in periods)
                        {
                            //指定期间的余额表
                            var balance = GetData(period.Id, ts);
                            //查出所有的账套下所有公式
                            sql = "select * from tks_fas_formula where accountId=@AccountId";
                            List<TKS_FAS_Formula> formula = cnn.Query<TKS_FAS_Formula>(sql, new { AccountId = request.AccountList }, ts).ToList();

                            List<TKS_FAS_LRReport> report = GenLRReport(tpl, balance, formula, ts);
                            var data = (from item in report
                                        where item.Category == 20 || item.Category == 21 || item.Category == 22 || item.Category == 23
                                        select item).ToList();
                            LRGetData LR = new LRGetData();
                            LR.Year = period.Year.ToString();
                            LR.Month = period.Month.ToString();
                            LR.Data = data;
                            PrintData.Add(LR);
                        }

                        ts.Commit();
                        response.PrintData = PrintData;
                        response.IsSuccess = true;
                        response.Message = "加载完毕";
                    }
                    else
                    {
                        //指定期间的余额表
                        var period = cnn.QueryFirstOrDefault<TKS_FAS_MonthPeriodInfo>(
                     @"select * from TKS_FAS_MonthPeriodInfo where id=@Id",
                     new { Id = request.PeriodId }, ts);
                        var balance = new List<TKS_FAS_GLBalanceExt>();
                        //if (string.IsNullOrEmpty(request.PeriodId.Trim()))
                        //{
                        //    //期初
                        //    balance = GetData(user.AccountId, "", ts);

                        //    //期初中 期初填写的值 为期末的值 
                        //    foreach (var item in balance)
                        //    {
                        //        item.BWBEndBAL = item.BWBStartBAL;
                        //        item.YBEndBAL = item.YBStartBAL;
                        //        item.NUMEndBAL = item.NUMStartBAL;
                        //        item.BWBEndBAL_D = item.BWBStartBAL_D;
                        //        item.BWBEndBAL_J = item.BWBStartBAL_J;
                        //    }
                        //}
                        //else
                        //{
                        balance = GetData(request.PeriodId, ts);
                        //}
                        #region 损益科目，利润表 结转的数据也计算
                        List<TKS_FAS_GLBalanceExt> SYMonthBalance = new List<TKS_FAS_GLBalanceExt>();
                        List<TKS_FAS_GLBalanceExt> SYYearBalance = new List<TKS_FAS_GLBalanceExt>();
                        if (balance.Where(p => p.SubjectCode.StartsWith("5")).Count() > 0)
                        {
                            sql = @"select SubjectCode,sum((case when  Credit_Debit=1 then  Money_Credit else -1*Money_Debit end)) as  BWBCreditTotal  
                               from TKS_FAS_DocDetail
                               where Source='SY' and PeriodId=@PeriodId and SubjectCode like '5%'
                               group by SubjectCode";
                            SYMonthBalance = cnn.Query<TKS_FAS_GLBalanceExt>(sql, new { PeriodId = period.Id }, ts).ToList();
                            sql = @"select SubjectCode,sum((case when  Credit_Debit=1 then  Money_Credit else -1*Money_Debit end)) as  BWBCreditTotal  
                               from TKS_FAS_DocDetail,TKS_FAS_MonthPeriodInfo
                               where Source='SY' and TKS_FAS_DocDetail.PeriodId = TKS_FAS_MonthPeriodInfo.Id  and SubjectCode like '5%'
                               and TKS_FAS_MonthPeriodInfo.Month>=1 and TKS_FAS_MonthPeriodInfo.Month<=@Month and TKS_FAS_MonthPeriodInfo.Year  =@Year
                               and TKS_FAS_DocDetail.AccountId = @AccountId
                               group by SubjectCode";
                            SYYearBalance = cnn.Query<TKS_FAS_GLBalanceExt>(sql, new { AccountId = period.AccountId, Month = period.Month, Year = period.Year }, ts).ToList();

                            foreach (var bal in balance.Where(p => p.SubjectCode.StartsWith("5")).ToList())
                            {
                                var month = SYMonthBalance.Where(p => p.SubjectCode == bal.SubjectCode || p.SubjectCode.StartsWith(bal.SubjectCode)).ToList();
                                var year = SYYearBalance.Where(p => p.SubjectCode == bal.SubjectCode || p.SubjectCode.StartsWith(bal.SubjectCode)).ToList();

                                bal.BWBDebitTotal += (year.Count > 0 ? month.Sum(p => p.BWBCreditTotal) : 0);
                                bal.BWBDebitTotal_Y += (year.Count > 0 ? year.Sum(p => p.BWBCreditTotal) : 0);


                            }
                        }
                        #endregion

                        //查出所有的账套下所有公式
                        sql = "select * from tks_fas_formula where accountId=@AccountId";
                        List<TKS_FAS_Formula> formula = cnn.Query<TKS_FAS_Formula>(sql, new { AccountId = user.AccountId }, ts).ToList();

                        List<TKS_FAS_LRReport> report = GenLRReport(tpl, balance, formula, ts);
                        var data = (from item in report
                                    where item.Category == 20 || item.Category == 21 || item.Category == 22 || item.Category == 23
                                    select item).ToList();
                        LRGetData LR = new LRGetData();
                        LR.Year = period.Year.ToString();
                        LR.Month = period.Month.ToString();
                        LR.Data = data;
                        PrintData.Add(LR);
                        ts.Commit();
                        response.PrintData = PrintData;
                        response.Data = data;
                        response.IsSuccess = true;
                        response.Message = "加载完毕";
                    }

                    return response;
                }
                catch (Exception ex)
                {
                    ts.Rollback();
                    return this.DealException(response, ex) as ResponseLRGet;
                }
            }
        }

        private List<TKS_FAS_LRReport> GenLRReport(List<TKS_FAS_LRReport> data,
            List<TKS_FAS_GLBalanceExt> balance, List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {




            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 0)
                {

                    data[i].Money_Month = CalculateMoneyMonth(data[i].SourceValue, data[i].Id, balance, formula, ts);
                    data[i].Money_Year = CalculateMoneyYear(data[i].SourceValue, data[i].Id, balance, formula, ts);
                }
            }


            for (var i = 0; i < data.Count; i++)
            {

                if (data[i].SourceType == 1)
                {
                    data[i].Money_Month = SumMoneyMonth(data[i].Category, data);

                    data[i].Money_Year = SumMoneyYear(data[i].Category, data);
                }
            }

            return data;
        }

        private decimal CalculateMoneyYear(string type, string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);
            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();
                if (bal == null)
                    continue;
                decimal val = 0;

                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                if (subject.Credit_Debit == 1)
                {
                    val = bal.BWBCreditTotal_Y - bal.BWBDebitTotal_Y;// 贷方减去借方
                }
                else
                {
                    val = bal.BWBDebitTotal_Y - bal.BWBCreditTotal_Y;// 用借方减去贷方
                }
                if (item.OperatorCharacter == "+")
                {

                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }
            }

            return result;
        }

        private decimal CalculateMoneyMonth(string type, string id, List<TKS_FAS_GLBalanceExt> balance,
            List<TKS_FAS_Formula> formula, IDbTransaction ts)
        {
            SubjectBLL bll = new SubjectBLL(cnn);

            var fun = formula.Where(p => p.ReportDetailTPLId == id).ToList();
            decimal result = 0;
            foreach (var item in fun)
            {
                var bal = balance.Where(p => p.SubjectCode == item.SubjectCode).First();

                if (bal == null)
                    continue;
                decimal val = 0;

                var subject = bll.GetSubject(item.SubjectCode, item.AccountId, ts);
                if (subject.Credit_Debit == 1)
                {
                    val = bal.BWBCreditTotal - bal.BWBDebitTotal;// 贷方减去借方
                }
                else
                {
                    val = bal.BWBDebitTotal - bal.BWBCreditTotal;// 用借方减去贷方
                }

                if (item.OperatorCharacter == "+")
                {

                    result += val;
                }
                else if (item.OperatorCharacter == "-")
                {
                    result -= val;
                }
            }

            return result;
        }


        private decimal SumMoneyYear(int category, List<TKS_FAS_LRReport> data)
        {
            List<TKS_FAS_LRReport> fd = new List<TKS_FAS_LRReport>();
            //根据category分别取计算列
            if (category == 21)
            {
                fd = data.Where(p => p.Category == 20 && p.SourceType == 0).ToList();
            }
            else if (category == 22)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21)
                      select item).ToList();
            }
            else if (category == 23)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21 || item.Category == 22)
                      select item).ToList();
            }

            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                    result += item.Money_Year;
                else if (item.OperatorCharacter == "-")
                    result -= item.Money_Year;


            }
            return result;
        }

        private decimal SumMoneyMonth(int category, List<TKS_FAS_LRReport> data)
        {
            List<TKS_FAS_LRReport> fd = new List<TKS_FAS_LRReport>();
            //根据category分别取计算列
            if (category == 21)
            {
                fd = data.Where(p => p.Category == 20 && p.SourceType == 0).ToList();
            }
            else if (category == 22)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21)
                      select item).ToList();
            }
            else if (category == 23)
            {
                fd = (from item in data
                      where item.SourceType == 0 && (item.Category == 20 || item.Category == 21 || item.Category == 22)
                      select item).ToList();
            }

            decimal result = 0;
            foreach (var item in fd)
            {
                if (item.OperatorCharacter == "+")
                    result += item.Money_Month;
                else if (item.OperatorCharacter == "-")
                    result -= item.Money_Month;

            }
            return result;
        }

        #endregion
    }
}
