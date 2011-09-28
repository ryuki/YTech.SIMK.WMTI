using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.Core.PersistenceSupport.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Core.RepositoryInterfaces
{
    public interface ITCommissionRepository : INHibernateRepositoryWithTypedId<TCommission, string>
    {
        IEnumerable<TCommission> GetListByRecapId(string recPeriodId, EnumDepartment? dep);
    }
}
