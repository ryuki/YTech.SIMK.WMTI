using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Transaction;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TInstallmentMap : IAutoMappingOverride<TInstallment>
    {
        #region Implementation of IAutoMappingOverride<TInstallment>

        public void Override(AutoMapping<TInstallment> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_INSTALLMENT");
            mapping.Id(x => x.Id, "INSTALLMENT_ID").GeneratedBy.Assigned();

            mapping.References(x => x.LoanId, "LOAN_ID");
            mapping.References(x => x.EmployeeId, "EMPLOYEE_ID");

            mapping.Map(x => x.InstallmentNo, "INSTALLMENT_NO");
            mapping.Map(x => x.InstallmentBasic, "INSTALLMENT_BASIC");
            mapping.Map(x => x.InstallmentInterest, "INSTALLMENT_INTEREST");
            mapping.Map(x => x.InstallmentOthers, "INSTALLMENT_OTHERS");

            mapping.Map(x => x.InstallmentPaymentDate, "INSTALLMENT_PAYMENT_DATE");
            mapping.Map(x => x.InstallmentMaturityDate, "INSTALLMENT_MATURITY_DATE");
            mapping.Map(x => x.InstallmentFine, "INSTALLMENT_FINE");
            mapping.Map(x => x.InstallmentPaid, "INSTALLMENT_PAID");

            mapping.Map(x => x.InstallmentDesc, "INSTALLMENT_DESC");
            mapping.Map(x => x.InstallmentStatus, "INSTALLMENT_STATUS");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Map(x => x.RowVersion, "ROW_VERSION").ReadOnly();
            //mapping.Version(x => x.RowVersion)
            //    .Column("ROW_VERSION")
            //    //.CustomType("BinaryBlob")
            //    .CustomSqlType("Timestamp")
            //    .Not.Nullable()
            //    .Generated.Always();
        }

        #endregion
    }
}
