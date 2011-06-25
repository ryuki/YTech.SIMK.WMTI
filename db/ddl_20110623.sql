alter table dbo.REF_PERSON add column PERSON_GUARANTOR_RELATION nvarchar(50) null
alter table dbo.REF_PERSON alter column CREATED_DATE datetime null
alter table dbo.M_ZONE alter column CREATED_DATE datetime null
alter table dbo.T_LOAN_SURVEY add column SURVEY_RECEIVED_BY nvarchar(50) null
alter table dbo.T_LOAN_SURVEY add column SURVEY_PROCESS_BY nvarchar(50) null
alter table dbo.T_INSTALLMENT add column INSTALLMENT_PAID numeric(18, 5) null

alter table [dbo].[M_EMPLOYEE] add column ADDRESS_ID nvarchar(50) null

ALTER TABLE [dbo].[M_EMPLOYEE]  WITH CHECK ADD  CONSTRAINT [FK_M_EMPLOYEE_REF_ADDRESS] FOREIGN KEY([ADDRESS_ID])
REFERENCES [dbo].[REF_ADDRESS] ([ADDRESS_ID])
GO

ALTER TABLE [dbo].[M_EMPLOYEE] CHECK CONSTRAINT [FK_M_EMPLOYEE_REF_ADDRESS]

alter table dbo.T_LOAN alter column LOAN_IS_SALESMAN_KNOWN_CUST nvarchar(50) null
