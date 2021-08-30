namespace Services.Transactions.Models
{
    public class TransactionResult
    {
        public bool Succeded => string.IsNullOrWhiteSpace(_errors);
        public string Errors => _errors;

        private readonly string _errors;

        private TransactionResult() { }

        private TransactionResult(string error)
        {
            _errors = error;
        }

        public static TransactionResult CreateFailedResult(string errors)
        {
            return new TransactionResult(errors);
        }

        public static TransactionResult CreateSuccededResult()
        {
            return new TransactionResult();
        }
    }
}