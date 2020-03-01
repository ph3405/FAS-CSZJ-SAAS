DECLARE AccountInfo CURSOR FOR select Id from tks_fas_AccountInfo
open AccountInfo
declare @AccountId varchar(100)
fetch next from AccountInfo into @AccountId
while @@fetch_status<>-1
 begin 
 --循环账套下科目信息
DECLARE test CURSOR FOR SELECT Id,RootCode,Code,Category FROM TKS_FAS_AccountSubject
  WHERE SLevel=1 and accountId = @AccountId
OPEN test 
declare @Id varchar(100),
@RootCode varchar(100),
@Code varchar(100),
@Category  varchar(100)
fetch next from test into @Id,@RootCode,@Code,@Category
while @@fetch_status<>-1
 begin 
 --print @Id
update TKS_FAS_AccountSubject set ParentId=@Id
where category = @Category and SLevel=2 and RootCode=@Code
 and accountId = @AccountId
fetch next from test into @Id,@RootCode,@Code,@Category
end 
close test 
deallocate test

fetch next from AccountInfo into @AccountId
end 
close AccountInfo 
deallocate AccountInfo
