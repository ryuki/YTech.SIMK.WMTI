using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YTech.SIMK.WMTI.Web.Controllers.ViewModel;

namespace YTech.SIMK.WMTI.Web.Views.Transaction.Loan
{
    public partial class Survey : System.Web.Mvc.ViewPage<SurveyFormViewModel>
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (!string.IsNullOrEmpty(Request.QueryString["loanSurveyId"]))
            {
                this.MasterPageFile = "~/Views/Shared/MasterPopup.master";
            }
        }
    }
}