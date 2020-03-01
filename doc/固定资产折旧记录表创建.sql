
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TKS_FAS_FixedAssetsLog](
	[Id] [varchar](36) NOT NULL,
	[AccountId] [varchar](50) NOT NULL,
	[PeriodId] [varchar](50) NOT NULL,
	[FixedId] [varchar](50) NOT NULL,
	[Amount] [decimal](18, 2) NULL,
	[CreateUser] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_FixedAssetsLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO



