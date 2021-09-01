using Data.Enums;
using Microsoft.EntityFrameworkCore;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Services.Accounts;
using Services.Accounts.Models;
using Services.Common;
using Services.Transactions;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests
{
    public class MultiThreadMultipleTransactionsTest : UnitTestsWithInMemoryDb
    {
        // Emulating usage of scoped Context like in Controller
        private static ITransactionService CreateTransactionService()
        {
            var newContext = CreateLoymaxTestContext();
            var accountService = new AccountService(newContext);
            return new TransactionService(newContext, new TransactionValidator(accountService));
        }

        private static readonly IRandomizerString FirstNameGenerator;
        private static readonly IRandomizerString LastNameGenerator;
        private static readonly IRandomizerDateTime DateOfBirthGenerator;

        static MultiThreadMultipleTransactionsTest()
        {
            DateOfBirthGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime
            {
                From = DateTime.Now.Date.AddYears(-80),
                To = DateTime.Now.Date.AddYears(-CommonConstants.MinimalUserAgeInYears),
                IncludeTime = false,
                UseNullValues = false
            });
            FirstNameGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsFirstName());
            LastNameGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsLastName());
        }

        [Fact]
        public async Task TestFromTaskDescription()
        {
            //        -------------------------------------        Arrange        -------------------------------------
            const int accountsNumber = 50;
            var accountIdsList = await SeedAccounts(accountsNumber);

            //        -------------------------------------        Act        -------------------------------------
            var allTasks = new List<Task>();
            foreach (var accountId in accountIdsList)
            {
                // 10 threads operating each user.
                // In total: 50 Deposits and 50 Withdrawals per user.
                // Deposit = Withdrawal = 100. So expected balance of all users is 0.
                for (var i = 0; i < 10; i++)
                    allTasks.Add(new Task(async () => await DoDepositAndWithdrawal(accountId, timesToRepeat: 5)));
            }

            foreach (var task in allTasks) task.Start();
            Task.WaitAll(allTasks.ToArray());
            Thread.Sleep(300);

            var balances = new List<decimal>();
            var transactionsNumber = new List<int>();
            foreach (var accountId in accountIdsList)
            {
                balances.Add(await new AccountService(Context).CalculateBalance(accountId));
                transactionsNumber.Add(await Context.Transactions.CountAsync(x => x.AccountId == accountId));
            }

            //        -------------------------------------        Assert        -------------------------------------
            Assert.True(balances.TrueForAll(x => x == 0));
            Assert.True(transactionsNumber.TrueForAll(x => x == 100));
        }

        private async Task<List<int>> SeedAccounts(int accountsNumber)
        {
            var accountIdsList = new List<int>();

            for (var i = 0; i < accountsNumber; i++)
            {
                var addAccountDto = new AddAccountDto
                {
                    FirstName = FirstNameGenerator.Generate(),
                    LastName = LastNameGenerator.Generate(),
                    Patronymic = null,
                    DateOfBirth = DateOfBirthGenerator.Generate()?.Date ?? DateTime.Now.Date.AddYears(-40)
                };

                var accountService = new AccountService(CreateLoymaxTestContext());
                var accountId = await accountService.AddAccount(addAccountDto);
                accountIdsList.Add(accountId);
            }

            return accountIdsList;
        }

        private const int DepositAmount = 100;
        private const int WithdrawalAmount = 100;
        private async Task DoDepositAndWithdrawal(int accountId, int timesToRepeat)
        {
            for (var i = 0; i < timesToRepeat; i++)
            {
                var deposit = DepositAmount;
                var withdrawal = WithdrawalAmount;

                var depositDto = new AddTransactionDto(accountId, deposit, TransactionType.Deposit);
                var withdrawalDto = new AddTransactionDto(accountId, withdrawal, TransactionType.Withdrawal);

                await CreateTransactionService().SaveTransactionLockedByAccountId(depositDto);
                await CreateTransactionService().SaveTransactionLockedByAccountId(withdrawalDto);
            }
        }
    }
}
