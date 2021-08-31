using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using System;
using Services.Common;

namespace Services
{
    public class AccountValidator : IAccountValidator
    {
        private readonly DateTime _latestDateOfBirthRestricted = DateTime.Now.Date.AddYears(-18);

        public ValidationResult Validate(AddAccountDto addAccountDto)
        {
            return addAccountDto.DateOfBirth > _latestDateOfBirthRestricted 
                ? ValidationResult.ValidResult().AddError("User must be 18 years old or over to be registered") 
                : ValidationResult.ValidResult();
        }
    }
}
