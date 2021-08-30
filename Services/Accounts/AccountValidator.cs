using Services.Accounts;
using Services.Accounts.Models;
using System;

namespace Services
{
    public class AccountValidator : IAccountValidator
    {
        readonly DateTime latestDateOfBirthRestricted = DateTime.Now.Date.AddYears(-18);

        public ValidationReslut Validate(AddAccountDto addAccountDto)
        {
            if (addAccountDto.DateOfBirth > latestDateOfBirthRestricted)
                return ValidationReslut.Create().AddError("User must be 18 years old or over to be registered");

            return ValidationReslut.Create();
        }
    }
}
