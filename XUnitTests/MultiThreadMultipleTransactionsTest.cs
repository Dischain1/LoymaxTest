using Data.Enums;
using FluentAssertions;
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
        private static readonly Random Rng;

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
            Rng = new Random();
        }

        [Fact]
        public async Task TestFromTaskDescription()
        {
            //        -------------------------------------        Arrange        -------------------------------------
            // Seeding users
            const int accountsNumber = 50;
            var accountIdsList = await SeedAccounts(accountsNumber);
            // Generating list of tasks 
            var allTasks = new List<Task>();
            foreach (var accountId in accountIdsList)
            {
                // 10 threads operating each user.
                // In total: 100 Deposits and 100 Withdrawals per user.
                // Deposit = Withdrawal = 100 ===> Expected balance of all users is 0.
                for (var i = 0; i < 10; i++)
                    allTasks.Add(new Task(() => DoDepositAndWithdrawal(accountId, timesToRepeat: 10)));
            }

            //        -------------------------------------        Act        -------------------------------------
            // Starting all tasks
            foreach (var task in allTasks.OrderBy(a => Rng.Next())) 
                task.Start();
            Task.WaitAll(allTasks.ToArray());

            // Calculating all balances and saved transactions number for each Account
            var balancesAsSumOfTransactions = new List<decimal>();
            var balancesInsideAccountsEntities = new List<decimal>();
            var transactionsNumber = new List<int>();
            foreach (var accountId in accountIdsList)
            {
                balancesAsSumOfTransactions.Add(await new AccountService(Context).CalculateBalance(accountId));
                transactionsNumber.Add(await Context.Transactions.CountAsync(x => x.AccountId == accountId));
                balancesInsideAccountsEntities.Add(await new AccountService(Context).GetBalance(accountId));
            }

            //        -------------------------------------        Assert        -------------------------------------
            Assert.True(balancesAsSumOfTransactions.TrueForAll(x => x == 0));
            Assert.True(transactionsNumber.TrueForAll(x => x == 200));
            balancesInsideAccountsEntities.Should().Equal(balancesAsSumOfTransactions);
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
        private async void DoDepositAndWithdrawal(int accountId, int timesToRepeat)
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
