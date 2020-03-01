

CREATE TABLE [dbo].[TKS_FAS_BasicData](
	[Id] [varchar](50) NOT NULL,
	[UserId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[DataType] [varchar](50) NULL,
	[Remark] [varchar](300) NULL,
	[CreateUser] [varchar](50) NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_TKS_FAS_BasicData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'客户：Customer  供应商：Vendor' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TKS_FAS_BasicData', @level2type=N'COLUMN',@level2name=N'DataType'
GO


