﻿using Services.Common;
using Services.Transactions.Models;

namespace Services.Transactions.Interfaces
{
    public interface ITransactionValidator
    {
        public ValidationResult Validate(AddTransactionDto transactionDto, bool isUsedInsideTransaction);
    }
}