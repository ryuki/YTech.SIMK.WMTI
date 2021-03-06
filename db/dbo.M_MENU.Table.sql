USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[M_MENU]    Script Date: 10/19/2013 02:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[M_MENU](
	[MENU_ID] [nvarchar](50) NOT NULL,
	[MENU_PARENT_ID] [nvarchar](50) NULL,
	[MENU_TYPE] [nvarchar](50) NULL,
	[MENU_NAME] [nvarchar](50) NULL,
	[MENU_LINK] [nvarchar](200) NULL,
	[MENU_ICON] [nvarchar](50) NULL,
	[MENU_STATUS] [nvarchar](50) NULL,
	[MENU_DESC] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_MENU] PRIMARY KEY CLUSTERED 
(
	[MENU_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
