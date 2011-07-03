using System;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Views.Transaction.Loan
{
    public partial class CustomerRequest : System.Web.Mvc.ViewPage<CRFormViewModel>
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!string.IsNullOrEmpty(Request.QueryString["loanCustomerRequestId"]))
            {
                this.MasterPageFile = "~/Views/Shared/MasterPopup.master";
            }
        }
    }
}