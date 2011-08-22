using System.Linq;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class FeedbackViewModel
    {
        public static FeedbackViewModel CreateFeedbackViewModel( ITLoanFeedbackRepository tLoanFeedbackRepository, string loanId)
        {
            var viewModel = new FeedbackViewModel();
            var loanFeedback = new TLoanFeedback();

            var listFeedbackCommon =
                tLoanFeedbackRepository.GetLoanFeedbackbyType(EnumLoanFeedbackType.Common.ToString(), loanId);
            var fcommon = from feedback in listFeedbackCommon
                         select new { Id = feedback.Id, Name = feedback.LoanFeedbackDesc};
            viewModel.LoanFeedbackCommon = new string(fcommon);

            return viewModel;
        }

        public TLoanFeedback LoanFeedback { get; internal set; }
        public string LoanFeedbackCommon { get; internal set; }
        public string LoanFeedbackPaymentCharacter { get; internal set; }
        public string LoanFeedbackProblem { get; internal set; }
        public string LoanFeedbackSolution { get; internal set; }
    }
}
