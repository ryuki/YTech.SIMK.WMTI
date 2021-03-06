USE [DB_SIMK_WMTI]
GO
/****** Object:  Table [dbo].[T_LOAN_FEEDBACK]    Script Date: 10/19/2013 02:29:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_LOAN_FEEDBACK](
	[LOAN_FEEDBACK_ID] [nvarchar](50) NOT NULL,
	[LOAN_ID] [nvarchar](50) NULL,
	[LOAN_FEEDBACK_TYPE] [nvarchar](50) NULL,
	[LOAN_FEEDBACK_DESC] [nvarchar](max) NULL,
	[LOAN_FEEDBACK_BY] [nvarchar](50) NULL,
	[LOAN_FEEDBACK_STATUS] [nvarchar](50) NULL,
	[DATA_STATUS] [nvarchar](50) NULL,
	[CREATED_BY] [nvarchar](50) NULL,
	[CREATED_DATE] [datetime] NULL,
	[MODIFIED_BY] [nvarchar](50) NULL,
	[MODIFIED_DATE] [datetime] NULL,
	[ROW_VERSION] [timestamp] NULL,
 CONSTRAINT [PK_T_LOAN_FEEDBACK] PRIMARY KEY CLUSTERED 
(
	[LOAN_FEEDBACK_ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[T_LOAN_FEEDBACK]  WITH CHECK ADD  CONSTRAINT [FK_T_LOAN_FEEDBACK_T_LOAN] FOREIGN KEY([LOAN_ID])
REFERENCES [dbo].[T_LOAN] ([LOAN_ID])
GO
ALTER TABLE [dbo].[T_LOAN_FEEDBACK] CHECK CONSTRAINT [FK_T_LOAN_FEEDBACK_T_LOAN]
GO
