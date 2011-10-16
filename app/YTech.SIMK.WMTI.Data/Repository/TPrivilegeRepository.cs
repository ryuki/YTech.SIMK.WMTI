using System;
using System.Text;
using NHibernate;
using SharpArch.Data.NHibernate;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;

namespace YTech.SIMK.WMTI.Data.Repository
{
    public class TPrivilegeRepository : NHibernateRepositoryWithTypedId<TPrivilege, string>, ITPrivilegeRepository
    {
        public void DeleteByUserName(string userName)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@" delete from TPrivilege where UserName = :userName "); 

            string query = string.Format("{0}", sql);
            IQuery q = Session.CreateQuery(query);
            q.SetString("userName", userName);
            q.ExecuteUpdate();
        }
    }
}
