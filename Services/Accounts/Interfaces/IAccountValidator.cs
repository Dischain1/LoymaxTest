using Services.Accounts.Models;
using Services.Common;

namespace Services.Accounts.Interfaces
{
    public interface IAccountValidator
    {
        public ValidationResult Validate(AddAccountDto addAccountDto);
    }
}
