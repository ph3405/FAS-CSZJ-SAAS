DECLARE AccountInfo CURSOR FOR select Id from tks_fas_AccountInfo
open AccountInfo
declare @AccountId varchar(100)
fetch next from AccountInfo into @AccountId
while @@fetch_status<>-1
 begin 
 --循环账套下科目信息
DECLARE test CURSOR FOR SELECT Id,RootCode,Code,Category,ParentId FROM TKS_FAS_AccountSubject
  WHERE  accountId = @AccountId
OPEN test 
declare @Id varchar(100),
@RootCode varchar(100),
@Code varchar(100),
@Category  varchar(100),
@ParentId varchar(100)
fetch next from test into @Id,@RootCode,@Code,@Category,@ParentId
while @@fetch_status<>-1
 begin 
 --print @Id
update TKS_FAS_GLBalance set ParentId=@ParentId
where category = @Category and subjectId=@Id
 and accountId = @AccountId
fetch next from test into @Id,@RootCode,@Code,@Category,@ParentId
end 
close test 
deallocate test

fetch next from AccountInfo into @AccountId
end 
close AccountInfo 
deallocate AccountInfo