using System;
using System.Collections.Generic;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Data;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TCommissionRepository : NHibernateRepositoryWithTypedId<TCommission, string>, ITCommissionRepository
    {
        public IEnumerable<TCommission> GetListByRecapId(string recPeriodId, EnumDepartment? dep)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@"   select com
                                from TCommission as com
                                    where 1=1 ");
            if (!string.IsNullOrEmpty(recPeriodId))
            {
                sql.AppendLine(@"   and com.RecPeriodId.Id = :recPeriodId");
            }
            if (dep != null)
            {
                sql.AppendLine(@"   and com.CommissionStatus = :dep");
            }
            IQuery q = Session.CreateQuery(sql.ToString());
            if (!string.IsNullOrEmpty(recPeriodId))
            {
                q.SetString("recPeriodId", recPeriodId);
            }
            if (dep != null)
            {
                q.SetString("dep", dep.ToString());
            }
            return q.List<TCommission>();
        }
    }
}
