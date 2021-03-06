USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[M_COMMISSION_DET]    Script Date: 10/19/2013 02:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_COMMISSION_DET](
	[COMMISSION_DET_ID] [nvarchar](50) NOT NULL,
	[COMMISSION_ID] [nvarchar](50) NULL,
	[DETAIL_TYPE] [nvarchar](50) NULL,
	[DETAIL_VALUE] [numeric](18, 5) NULL,
	[DETAIL_STATUS] [nvarchar](50) NULL,
	[DETAIL_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[COMMISSION_LOW_TARGET] [int] NULL,
	[COMMISSION_HIGH_TARGET] [int] NULL,
	[DETAIL_LOW_TARGET] [numeric](18, 5) NULL,
	[DETAIL_HIGH_TARGET] [numeric](18, 5) NULL,
	[DETAIL_CUSTOMER_NUMBER] [int] NULL,
	[DETAIL_TRANSPORT_ALLOWANCE] [numeric](18, 5) NULL,
	[DETAIL_INCENTIVE] [numeric](18, 5) NULL,
	[DETAIL_INCENTIVE_SURVEY_ACC] [numeric](18, 5) NULL,
	[DETAIL_INCENTIVE_SURVEY_ONLY] [numeric](18, 5) NULL,
 CONSTRAINT [PK_T_COMMISSION_DET] PRIMARY KEY CLUSTERED 
(
	[COMMISSION_DET_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[M_COMMISSION_DET]  WITH CHECK ADD  CONSTRAINT [FK_T_COMMISSION_DET_T_COMMISSION] FOREIGN KEY([COMMISSION_ID])
REFERENCES [dbo].[M_COMMISSION] ([COMMISSION_ID])
GO
ALTER TABLE [dbo].[M_COMMISSION_DET] CHECK CONSTRAINT [FK_T_COMMISSION_DET_T_COMMISSION]
GO
