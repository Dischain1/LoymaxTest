namespace Services.Transactions.Models
{
    public class AddTransactionResult
    {
        public bool Succeeded => string.IsNullOrWhiteSpace(Errors);
        public string Errors { get; }

        private AddTransactionResult() { }

        private AddTransactionResult(string error)
        {
            Errors = error;
        }

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