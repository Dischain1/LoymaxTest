using Services.Common;

namespace Services.Transactions.Models
{
    public class AddTransactionResult : PostActionResult
    {
        private AddTransactionResult()  { }

        private AddTransactionResult(string error) : base(error) { }

        public static AddTransactionResult FailedResult(string errors)
        {
            return new AddTransactionResult(errors);
        }

        public static AddTransactionResult SucceededResult()
        {
            return new AddTransactionResult();
        }
    }
}