USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[T_LOG]    Script Date: 10/19/2013 02:29:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_LOG](
	[LOG_ID] [nvarchar](50) NOT NULL,
	[LOG_USER_NAME] [nvarchar](50) NULL,
	[LOG_IP_ADDRESS] [nvarchar](50) NULL,
	[LOG_BROWSER] [nvarchar](50) NULL,
	[LOG_REFERAL] [nvarchar](50) NULL,
	[LOG_LINK] [nvarchar](50) NULL,
	[LOG_DATE] [datetime] NULL,
	[LOG_ACTION] [nvarchar](50) NULL,
 CONSTRAINT [PK_T_LOG] PRIMARY KEY CLUSTERED 
(
	[LOG_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
