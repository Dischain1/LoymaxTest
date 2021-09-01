using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using Services.Common;
using System;

namespace Services.Accounts
{
    public class AccountValidator : IAccountValidator
    {
        private readonly DateTime _latestDateOfBirthRestricted = DateTime.Now.Date.AddYears(CommonConstants.MinimalUserAgeInYears);

        public ValidationResult Validate(AddAccountDto addAccountDto)
        {
            return addAccountDto.DateOfBirth > _latestDateOfBirthRestricted
                ? ValidationResult.NotValidResult($"User must be {CommonConstants.MinimalUserAgeInYears} years old or over to be registered")
                : ValidationResult.ValidResult();
        }
    }
}
