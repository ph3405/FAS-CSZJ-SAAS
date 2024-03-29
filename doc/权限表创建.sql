
/****** Object:  Table [dbo].[TKS_FAS_PermissionInfo]    Script Date: 2018/6/27 19:07:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TKS_FAS_PermissionInfo](
	[Permission] [varchar](50) NOT NULL,
	[PLevel] [int] NULL,
 CONSTRAINT [PK_TKS_FAS_PermissionInfo] PRIMARY KEY CLUSTERED 
(
	[Permission] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'平台管理员', 1)
GO
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'组织机构管理员', 10)
GO
INSERT [dbo].[TKS_FAS_PermissionInfo] ([Permission], [PLevel]) VALUES (N'组织机构会计', 20)
GO
