using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YTech.SIMK.WMTI.Core.Master;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class PrivilegeViewModel
    {
        public static PrivilegeViewModel Create(string userName,IMMenuRepository mMenuRepository, ITPrivilegeRepository tPrivilegeRepository)
        {
            var viewModel = new PrivilegeViewModel();
             Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("UserName", userName);
            IList<TPrivilege> privileges = tPrivilegeRepository.FindAll(param);

            var menus = from menu in mMenuRepository.GetAll()
                        where menu.MenuParent == null
                        select new PrivilegeViewModel()
                                   {
                                       Menu = menu,
                                       HasAccess = CheckAccess(menu, privileges) ,
                                       children = (from child in menu.MenuChildren
                                                   select new PrivilegeViewModel()
                                                              {
                                                                  Menu = child,
                                                                  HasAccess = CheckAccess(menu, privileges),
                                                              }).ToArray()
                                   };

            return viewModel;
        }

        private static bool CheckAccess(MMenu menu, IList<TPrivilege> privileges)
        {
            var hasAccess = privileges
                                .Where(e => e.MenuId.Equals(menu))
                                .FirstOrDefault();

            return hasAccess != null ? true : false;
        }

        public PrivilegeViewModel[] children;
        public MMenu Menu { get; set; }
        public bool HasAccess { get; set; }
    }
}
