USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[M_ZONE_EMPLOYEE]    Script Date: 10/19/2013 02:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[M_ZONE_EMPLOYEE](
	[ZONE_EMPLOYEE_ID] [nvarchar](50) NOT NULL,
	[ZONE_ID] [nvarchar](50) NULL,
	[EMPLOYEE_ID] [nvarchar](50) NULL,
	[START_DATE] [datetime] NULL,
	[END_DATE] [datetime] NULL,
	[ZONE_EMPLOYEE_STATUS] [nvarchar](50) NULL,
	[ZONE_EMPLOYEE_DESC] [nvarchar](max) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [varchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_M_ZONE_EMPLOYEE] PRIMARY KEY CLUSTERED 
(
	[ZONE_EMPLOYEE_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[M_ZONE_EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_M_ZONE_EMPLOYEE_M_EMPLOYEE] FOREIGN KEY([EMPLOYEE_ID])
REFERENCES [dbo].[M_EMPLOYEE] ([EMPLOYEE_ID])
GO
ALTER TABLE [dbo].[M_ZONE_EMPLOYEE] CHECK CONSTRAINT [FK_M_ZONE_EMPLOYEE_M_EMPLOYEE]
GO
ALTER TABLE [dbo].[M_ZONE_EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_M_ZONE_EMPLOYEE_M_ZONE] FOREIGN KEY([ZONE_ID])
REFERENCES [dbo].[M_ZONE] ([ZONE_ID])
GO
ALTER TABLE [dbo].[M_ZONE_EMPLOYEE] CHECK CONSTRAINT [FK_M_ZONE_EMPLOYEE_M_ZONE]
GO
