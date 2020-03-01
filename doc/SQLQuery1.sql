select TKS_FAS_GLBalance.id,
SubjectCode,Name,PeriodId,TKS_FAS_MonthPeriodInfo.Year,TKS_FAS_MonthPeriodInfo.Month,
SCredit_Debit,
BWBStartBAL 期初余额,
BWBDebitTotal 借方累计,
BWBDebitTotal_Y 年度借方累计,
BWBCreditTotal 贷方累计 ,
BWBCreditTotal_Y 年度贷方累计,
ECredit_Debit,
BWBEndBAL 期末余额,
YearStartYBBAL 年初,
YearStartBWBBAL 年初本币
,*
  from TKS_FAS_GLBalance 
  left outer join TKS_FAS_MonthPeriodInfo on TKS_FAS_GLBalance.PeriodId = TKS_FAS_MonthPeriodInfo.Id
  where 
  --(BWBEndBAL !=0 or BWBStartBAL!=0) and 
  TKS_FAS_GLBalance.AccountId = 'de3bd0b8dc504f15953d1a856282a000'
  and SubjectCode='1133003'
  --and PeriodId!=''
  --and Month=2
  order by TKS_FAS_MonthPeriodInfo.Year,TKS_FAS_MonthPeriodInfo.Month,TKS_FAS_GLBalance.SubjectCode


  --update TKS_FAS_GLBalance  set BWBDebitTotal=18667.22,BWBDebitTotal_Y=18667.22
  --where id ='21268A2D-B787-4C4B-A337-FBE86F23273F'

  select SubjectCode,Name,PeriodId,TKS_FAS_MonthPeriodInfo.Year,TKS_FAS_MonthPeriodInfo.Month,
SCredit_Debit, ECredit_Debit,
BWBStartBAL 期初余额,
BWBDebitTotal 借方累计,
BWBDebitTotal_Y 年度借方累计,
BWBCreditTotal 贷方累计 ,
BWBCreditTotal_Y 年度贷方累计,
BWBEndBAL 期末余额,
YearStartYBBAL 年初,
YearStartBWBBAL 年初本币
,TKS_FAS_FGLBalance.* from TKS_FAS_FGLBalance
left outer join TKS_FAS_MonthPeriodInfo on TKS_FAS_FGLBalance.PeriodId = TKS_FAS_MonthPeriodInfo.Id
where (BWBEndBAL !=0 or BWBStartBAL!=0) and 
 TKS_FAS_FGLBalance.AccountId ='3b50947881374178a98a76c124e9df0a'
--and SubjectCode='2121'
--and Month=1
--and SCredit_Debit=1
order by TKS_FAS_FGLBalance.Year ,TKS_FAS_MonthPeriodInfo.Month
 