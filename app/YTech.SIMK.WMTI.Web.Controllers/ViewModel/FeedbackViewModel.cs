using System.Collections.Generic;
using System.Linq;
using YTech.SIMK.WMTI.Core.RepositoryInterfaces;
using YTech.SIMK.WMTI.Core.Transaction;
using YTech.SIMK.WMTI.Enums;

namespace YTech.SIMK.WMTI.Web.Controllers.ViewModel
{
    public class FeedbackViewModel
    {
        public static FeedbackViewModel Create(ITLoanRepository tLoanRepository, ITLoanFeedbackRepository tLoanFeedbackRepository, string loanId)
        {
            var viewModel = new FeedbackViewModel();

            if (!string.IsNullOrEmpty(loanId))
            {
                viewModel.LoanFeedbackCommon = GetLoanFeedback(tLoanFeedbackRepository, loanId, EnumLoanFeedbackType.Common);
                viewModel.LoanFeedbackPaymentCharacter = GetLoanFeedback(tLoanFeedbackRepository, loanId, EnumLoanFeedbackType.PaymentCharacter);
                viewModel.LoanFeedbackProblem = GetLoanFeedback(tLoanFeedbackRepository, loanId, EnumLoanFeedbackType.Problem);
                viewModel.LoanFeedbackSolution = GetLoanFeedback(tLoanFeedbackRepository, loanId, EnumLoanFeedbackType.Solution);
            }
            return viewModel;
        }

        private static string GetLoanFeedback(ITLoanFeedbackRepository tLoanFeedbackRepository, string loanId, EnumLoanFeedbackType feedbackType)
        {
            TLoanFeedback lastFeedback = tLoanFeedbackRepository.GetLastFeedback(loanId, feedbackType);
            if (lastFeedback != null)
            {
                return lastFeedback.LoanFeedbackDesc;
            }
            return string.Empty;
        }

        public TLoanFeedback LoanFeedback { get; set; }
        public string LoanFeedbackCommon { get; set; }
        public string LoanFeedbackPaymentCharacter { get; set; }
        public string LoanFeedbackProblem { get; set; }
        public string LoanFeedbackSolution { get; set; }
    }
}
