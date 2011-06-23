using FluentNHibernate.Automapping;
using YTech.SIMK.WMTI.Core.Transaction;
using FluentNHibernate.Automapping.Alterations;

namespace YTech.SIMK.WMTI.Data.NHibernateMaps.Transaction
{
    public class TLoanSurveyMap : IAutoMappingOverride<TLoanSurvey>
    {
        #region Implementation of IAutoMappingOverride<TLoanSurvey>
        
        public void Override(AutoMapping<TLoanSurvey> mapping)
        {
            mapping.DynamicUpdate();
            mapping.DynamicInsert();
            mapping.SelectBeforeUpdate();

            mapping.Table("T_LOAN_SURVEY");
            mapping.Id(x => x.Id, "LOAN_SURVEY_ID").GeneratedBy.Assigned();

            mapping.References(x => x.LoanId, "LOAN_ID").Fetch.Join();

            mapping.Map(x => x.SurveyDate, "SURVEY_DATE");
            mapping.Map(x => x.SurveyNeighbor, "SURVEY_NEIGHBOR");
            mapping.Map(x => x.SurveyNeighborCharacter, "SURVEY_NEIGHBOR_CHARACTER");
            mapping.Map(x => x.SurveyNeighborAsset, "SURVEY_NEIGHBOR_ASSET");
            mapping.Map(x => x.SurveyNeighborConclusion, "SURVEY_NEIGHBOR_CONCLUSION");
            mapping.Map(x => x.SurveyHouseType, "SURVEY_HOUSE_TYPE");
            mapping.Map(x => x.SurveyUnitDeliverDate, "SURVEY_UNIT_DELIVER_DATE");
            mapping.Map(x => x.SurveyUnitDeliverAddress, "SURVEY_UNIT_DELIVER_ADDRESS");
            mapping.Map(x => x.SurveyDesc, "SURVEY_DESC");
            mapping.Map(x => x.SurveyStatus, "SURVEY_STATUS");

            mapping.Map(x => x.DataStatus, "DATA_STATUS");
            mapping.Map(x => x.CreatedBy, "CREATED_BY");
            mapping.Map(x => x.CreatedDate, "CREATED_DATE");
            mapping.Map(x => x.ModifiedBy, "MODIFIED_BY");
            mapping.Map(x => x.ModifiedDate, "MODIFIED_DATE");
            mapping.Version(x => x.RowVersion)
                .Column("ROW_VERSION")
                //.CustomType("BinaryBlob")
                .CustomSqlType("Timestamp")
                .Not.Nullable()
                .Generated.Always();
        }
        
        #endregion
    }
}
