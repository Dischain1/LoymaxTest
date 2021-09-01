using Services.Common;

namespace Services.Accounts.Models
{
    public class GetBalanceResult : PostActionResult
    {
        public decimal? Balance { get; }

        private GetBalanceResult(decimal currentBalance)
        {
            Balance = currentBalance;
        }

        private GetBalanceResult(string error) : base(error) { }

        public static GetBalanceResult FailedResult(string errors)
        {
            return new GetBalanceResult(errors);
        }

        public static GetBalanceResult SucceededResult(decimal currentBalance)
        {
            return new GetBalanceResult(currentBalance);
        }
    }
}