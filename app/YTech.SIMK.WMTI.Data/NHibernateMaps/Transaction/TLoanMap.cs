using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Transaction;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TLoanMap : IAutoMappingOverride<TLoan>
    {
        #region Implementation of IAutoMappingOverride<TLoan>

        public void Override(AutoMapping<TLoan> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_LOAN");
            mapping.Id(x => x.Id, "LOAN_ID").GeneratedBy.Assigned();
            
            mapping.References(x => x.ZoneId, "ZONE_ID");
            mapping.References(x => x.PartnerId, "PARTNER_ID");
            mapping.References(x => x.CustomerId, "CUSTOMER_ID");
            mapping.References(x => x.PersonId, "PERSON_ID");
            mapping.References(x => x.AddressId, "ADDRESS_ID");
            
            mapping.References(x => x.TLSId, "TLS_ID");
            mapping.References(x => x.SalesmanId, "SALESMAN_ID");
            mapping.References(x => x.SurveyorId, "SURVEYOR_ID");
            mapping.References(x => x.CollectorId, "COLLECTOR_ID");
            
            mapping.Map(x => x.LoanNo, "LOAN_NO");
            mapping.Map(x => x.LoanCode, "LOAN_CODE");
            mapping.Map(x => x.LoanSubmissionDate, "LOAN_SUBMISSION_DATE");
            mapping.Map(x => x.LoanSurveyDate, "LOAN_SURVEY_DATE");
            mapping.Map(x => x.LoanUnitPriceTotal, "LOAN_UNIT_PRICE_TOTAL");
            mapping.Map(x => x.LoanDownPayment, "LOAN_DOWN_PAYMENT");
            mapping.Map(x => x.LoanBasicPrice, "LOAN_BASIC_PRICE");
            mapping.Map(x => x.LoanCreditPrice, "LOAN_CREDIT_PRICE");
            mapping.Map(x => x.LoanFactorMultiple, "LOAN_FACTOR_MULTIPLE");
            mapping.Map(x => x.LoanFactorAdd, "LOAN_FACTOR_ADD");
            mapping.Map(x => x.LoanInterest, "LOAN_INTEREST");
            mapping.Map(x => x.LoanTenor, "LOAN_TENOR");
            mapping.Map(x => x.LoanBasicInstallment, "LOAN_BASIC_INSTALLMENT");
            mapping.Map(x => x.LoanInterestInstallment, "LOAN_INTEREST_INSTALLMENT");
            mapping.Map(x => x.LoanOtherInstallment, "LOAN_OTHER_INSTALLMENT");
            
            mapping.References(x => x.LoanAccBy, "LOAN_ACC_BY");
            mapping.Map(x => x.LoanAccDate, "LOAN_ACC_DATE");
            
            mapping.Map(x => x.LoanAdminFee, "LOAN_ADMIN_FEE");
            mapping.Map(x => x.LoanMateraiFee, "LOAN_MATERAI_FEE");
            mapping.Map(x => x.LoanMaturityDate, "LOAN_MATURITY_DATE");
            mapping.Map(x => x.LoanIsSalesmanKnownCustomer, "LOAN_IS_SALESMAN_KNOWN_CUST");
            
            mapping.Map(x => x.LoanDesc, "LOAN_DESC");
            mapping.Map(x => x.LoanStatus, "LOAN_STATUS");

            mapping.HasMany(x => x.Surveys)
                .AsBag()
                .Inverse()
                .KeyColumn("LOAN_ID");

            mapping.HasMany(x => x.LoanUnits)
                .AsBag()
                .Inverse()
                .KeyColumn("LOAN_ID");

            mapping.HasMany(x => x.LoanFeedbacks)
                .AsBag()
                .Inverse()
                .KeyColumn("LOAN_ID");

            mapping.HasMany(x => x.Installments)
                .AsBag()
                .Inverse()
                .KeyColumn("LOAN_ID");
            
            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Version(x => x.RowVersion)
                .Column("ROW_VERSION") 
                .CustomSqlType("Timestamp")
                .Not.Nullable()
                .Generated.Always();
        }

        #endregion
    }
}
