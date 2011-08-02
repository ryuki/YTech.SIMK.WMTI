using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class MCommissionDetRepository : NHibernateRepositoryWithTypedId<MCommissionDet, string>, IMCommissionDetRepository
    {
        public IList<MCommissionDet> GetCommissionDetListById(string commissionId)
        {
            var sql = new StringBuilder();

            sql.AppendLine(@"   select cd
                                from MCommissionDet as cd");

            if (!string.IsNullOrEmpty(commissionId))
            {
                sql.AppendLine(@" where cd.CommissionId.Id = :commissionId");
            }

            IQuery q = Session.CreateQuery(sql.ToString());

            if (!string.IsNullOrEmpty(commissionId))
            {
                q.SetString("commissionId", commissionId);
            }

            return q.List<MCommissionDet>();
        }
    }
}
