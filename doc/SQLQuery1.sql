select TKS_FAS_GLBalance.id,
SubjectCode,Name,PeriodId,TKS_FAS_MonthPeriodInfo.Year,TKS_FAS_MonthPeriodInfo.Month,
SCredit_Debit,
BWBStartBAL �ڳ����,
BWBDebitTotal �跽�ۼ�,
BWBDebitTotal_Y ��Ƚ跽�ۼ�,
BWBCreditTotal �����ۼ� ,
BWBCreditTotal_Y ��ȴ����ۼ�,
ECredit_Debit,
BWBEndBAL ��ĩ���,
YearStartYBBAL ���,
YearStartBWBBAL �������
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
BWBStartBAL �ڳ����,
BWBDebitTotal �跽�ۼ�,
BWBDebitTotal_Y ��Ƚ跽�ۼ�,
BWBCreditTotal �����ۼ� ,
BWBCreditTotal_Y ��ȴ����ۼ�,
BWBEndBAL ��ĩ���,
YearStartYBBAL ���,
YearStartBWBBAL �������
,TKS_FAS_FGLBalance.* from TKS_FAS_FGLBalance
left outer join TKS_FAS_MonthPeriodInfo on TKS_FAS_FGLBalance.PeriodId = TKS_FAS_MonthPeriodInfo.Id
where (BWBEndBAL !=0 or BWBStartBAL!=0) and 
 TKS_FAS_FGLBalance.AccountId ='3b50947881374178a98a76c124e9df0a'
--and SubjectCode='2121'
--and Month=1
--and SCredit_Debit=1
order by TKS_FAS_FGLBalance.Year ,TKS_FAS_MonthPeriodInfo.Month
 