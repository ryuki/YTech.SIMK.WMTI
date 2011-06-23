alter table dbo.REF_PERSON add column PERSON_GUARANTOR_RELATION nvarchar(50) null
alter table dbo.REF_PERSON alter column CREATED_DATE datetime null
alter table dbo.M_ZONE alter column CREATED_DATE datetime null
alter table dbo.T_LOAN_SURVEY add column SURVEY_RECEIVED_BY nvarchar(50) null
alter table dbo.T_LOAN_SURVEY add column SURVEY_PROCESS_BY nvarchar(50) null