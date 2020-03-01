

DECLARE test CURSOR FOR SELECT Id,RootCode,Code,Category FROM TKS_FAS_AccountSubjectBak
  WHERE SLevel=1
OPEN test 
declare @Id varchar(100),
@RootCode varchar(100),
@Code varchar(100),
@Category  varchar(100)
fetch next from test into @Id,@RootCode,@Code,@Category
while @@fetch_status<>-1
 begin 
 --print @Id
update TKS_FAS_AccountSubjectBak set ParentId=@Id
where category = @Category and SLevel=2 and RootCode=@Code
fetch next from test into @Id,@RootCode,@Code,@Category
end 
close test 
deallocate test;
update TKS_FAS_AccountSubjectBak set ParentId='FE98E3E7-DAF3-4B00-9C01-F15012C18BAE'
where Code in('217100101','217100102','217100103','217100104','217100105','217100106','217100107','217100108','217100109')



