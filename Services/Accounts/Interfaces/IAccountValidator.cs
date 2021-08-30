using Services.Accounts.Models;

namespace Services.Accounts
{
    public interface IAccountValidator
    {
        public ValidationReslut Validate(AddAccountDto addAccountDto);
    }
}
