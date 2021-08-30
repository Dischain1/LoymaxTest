namespace Services.Common
{
    public static class ErrorMessages
    {
        public static string AccountDoesNotExist(int accountId) => $"Account with ID={accountId} does not exist!";
    }
}
