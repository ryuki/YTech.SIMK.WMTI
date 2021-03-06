USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[T_NEWS]    Script Date: 10/19/2013 02:29:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_NEWS](
	[NEWS_ID] [nvarchar](50) NOT NULL,
	[NEWS_TITLE] [nvarchar](200) NULL,
	[NEWS_DESC] [nvarchar](max) NULL,
	[NEWS_START_DATE] [datetime] NULL,
	[NEWS_END_DATE] [datetime] NULL,
	[NEWS_STATUS] [nvarchar](50) NULL,
	[NEWS_TO] [nvarchar](200) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
	[NEWS_TYPE] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_NEWS] PRIMARY KEY CLUSTERED 
(
	[NEWS_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
