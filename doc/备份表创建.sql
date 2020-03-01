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

GO

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

GO