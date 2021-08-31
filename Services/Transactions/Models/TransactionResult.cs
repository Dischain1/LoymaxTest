namespace Services.Transactions.Models
{
    public class TransactionResult
    {
        public bool Succeeded => string.IsNullOrWhiteSpace(Errors);
        public string Errors { get; }

        private TransactionResult() { }

        private TransactionResult(string error)
        {
            Errors = error;
        }

        public static TransactionResult FailedResult(string errors)
        {
            return new TransactionResult(errors);
        }

        public static TransactionResult SucceededResult()
        {
            return new TransactionResult();
        }
    }
}