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

            mapping.References(x => x.ZoneId, "ZONE_ID").Fetch.Join();
            mapping.References(x => x.PartnerId, "PARTNER_ID").Fetch.Join();
            mapping.References(x => x.CustomerId, "CUSTOMER_ID").Fetch.Join();
            mapping.References(x => x.PersonId, "PERSON_ID").Fetch.Join();
            mapping.References(x => x.AddressId, "ADDRESS_ID").Fetch.Join();

            mapping.References(x => x.TLSId, "TLS_ID");
            mapping.References(x => x.SalesmanId, "SALESMAN_ID");
            mapping.References(x => x.SurveyorId, "SURVEYOR_ID");
            mapping.References(x => x.CollectorId, "COLLECTOR_ID");

            mapping.Map(x => x.LoanNo, "LOAN_NO");
            mapping.Map(x => x.LoanCode, "LOAN_CODE");
            
            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
        }

        #endregion
    }
}
