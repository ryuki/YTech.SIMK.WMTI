using System;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Views.Transaction.Loan
{
    public partial class Feedback : System.Web.Mvc.ViewPage<FeedbackViewModel>
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!string.IsNullOrEmpty(Request.QueryString["loanId"]))
            {
                this.MasterPageFile = "~/Views/Shared/MasterPopup.master";
            }
        }
    }
}