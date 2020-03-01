

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

GO

SET ANSI_PADDING OFF
GO


