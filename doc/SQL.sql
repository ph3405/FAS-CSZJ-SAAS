



--权限创建表
if not exists(select * from syscolumns where id=object_id('TKS_FAS_PermissionInfo'))
begin
CREATE TABLE [dbo].[TKS_FAS_PermissionInfo](
	[Permission] [varchar](50) NOT NULL,
	[PLevel] [int] NULL,
 CONSTRAINT [PK_TKS_FAS_PermissionInfo] PRIMARY KEY CLUSTERED 
(
	[Permission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO



if not exists(select * from TKS_FAS_PermissionInfo where Permission=N'平台管理员')
begin
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'平台管理员', 1)
end
go
if not exists(select * from TKS_FAS_PermissionInfo where Permission=N'组织机构管理员')
begin
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'组织机构管理员', 10)
end
go
if not exists(select * from TKS_FAS_PermissionInfo where Permission=N'组织机构会计')
begin
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'组织机构会计', 20)
end
go



--备份创建表
if not exists(select * from syscolumns where id=object_id('TKS_FAS_BackUpInfo'))begin
CREATE TABLE [dbo].[TKS_FAS_BackUpInfo](
	[Id] [varchar](50) NOT NULL,
	[AccountId] [varchar](50) NULL,
	[Name] [varchar](200) NULL,
	[Path] [varchar](300) NULL,
	[Size] [varchar](50) NULL,	
	[CreateUser] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_BackUpInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

if not exists(select * from syscolumns where id=object_id('TKS_FAS_BackUpInfoLog'))
begin
CREATE TABLE [dbo].[TKS_FAS_BackUpInfoLog](
	[Id] [varchar](50) NOT NULL,
	[AccountId] [varchar](50) NULL,
	[BackUpInfoId] [varchar](50) NULL,
	[OperationType] [varchar](50) NULL,
	[Size] [varchar](50) NULL,	
	[CreateUser] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_BackUpInfoLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO


--关联用户从创建表
if not exists(select * from syscolumns where id=object_id('TKS_FAS_Account2User'))begin
CREATE TABLE [dbo].[TKS_FAS_Account2User](
	[Id] [varchar](50) NOT NULL,
	[AccountId] [varchar](50) NULL,
	[UserId] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_Account2User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

SET ANSI_PADDING OFF
GO


--手机验证码创建
if not exists(select * from syscolumns where id=object_id('TKS_FAS_MobileVerification'))begin
CREATE TABLE [dbo].[TKS_FAS_MobileVerification](
	[Id] [varchar](50) NOT NULL,
	[Mobile] [varchar](50) NULL,
	[VerCode] [varchar](20) NULL,
	[CodeType] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_MobileVerification] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
end
GO

SET ANSI_PADDING OFF
GO

if not exists(select * from syscolumns where id=object_id('TKS_FAS_Role2Permission'))
CREATE TABLE [dbo].[TKS_FAS_Role2Permission](
	[Id] [varchar](50) NOT NULL,	
	[RoleId] [varchar](36) NULL,
	[Permission] [varchar](36) NULL,
	[PLevel] int NULL,
 CONSTRAINT [PK_TKS_FAS_Role2Permission] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_PADDING OFF
END
GO

