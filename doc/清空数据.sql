delete from tks_fas_AccountInfo;
delete from TKS_FAS_AccountSubject;
delete from TKS_FAS_Currency;
delete from TKS_FAS_CertificateWord;
delete from TKS_FAS_GLBalance;
delete from TKS_FAS_FGLBalance;
delete FROM TKS_FAS_CaculateHelperItem;
delete FROM TKS_FAS_User WHERE USERNAME!='admin';
delete FROM TKS_FAS_FixedAssets;
delete FROM TKS_FAS_Doc;
delete FROM TKS_FAS_DocDetail;
delete from TKS_FAS_MonthPeriodInfo;



select * from TKS_FAS_AccountInfo;
select * from TKS_FAS_AccountSubject;
select * from TKS_FAS_Currency;
select * from TKS_FAS_CertificateWord;
select * from TKS_FAS_GLBalance;
select * from TKS_FAS_FGLBalance;
SELECT * FROM TKS_FAS_CaculateHelperItem;
SELECT * FROM TKS_FAS_User WHERE USERNAME!='admin';
SELECT * FROM TKS_FAS_FixedAssets;
SELECT * FROM TKS_FAS_Doc;
SELECT * FROM TKS_FAS_DocDetail;
select * from TKS_FAS_MonthPeriodInfo;

select * from TKS_FAS_Attachment
