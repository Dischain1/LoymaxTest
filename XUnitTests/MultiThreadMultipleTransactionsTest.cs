using Data.Enums;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Services.Accounts;
using Services.Accounts.Models;
using Services.Transactions;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Common;
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

        [Theory]
        [Repeat(3)]
        public async Task TestFromTaskDescription(int repeatIteration)
        {
            //        -------------------------------------        Arrange        -------------------------------------
            const int accountsNumber = 50;
            var accountIdsList = await SeedAccounts(accountsNumber);


            //        -------------------------------------        Act        -------------------------------------
            Parallel.ForEach(accountIdsList, accountId =>
            {
                // 10 threads operating each user.
                // In total: 100 Deposits and 100 Withdrawals per user.
                // Each Deposit = Withdrawal = 100.
                // So expected balance of all users is 0. Expected transaction count is 200.
                var tasks = new Task[10];

                for (var i = 0; i < 10; i++)
                {
                    tasks[i] = new Task(async () => await DoPositiveBalanceDepositWithdrawal(accountId));
                    tasks[i].Start();
                }

                Task.WaitAll(tasks);
            });

            Thread.Sleep(6000);

            var balances = new List<decimal>();
            var transactionsNumber = new List<int>();
            Parallel.ForEach(accountIdsList, async accountId =>
            {
                var context = CreateLoymaxTestContext();
                balances.Add(await new AccountService(context).CalculateBalance(accountId));
                transactionsNumber.Add(await context.Transactions.CountAsync(x => x.AccountId == accountId));
            });

            Thread.Sleep(6000);

            //        -------------------------------------        Assert        -------------------------------------
            Assert.True(balances.TrueForAll(x => x == 0));
            Assert.True(transactionsNumber.TrueForAll(x => x == 200));
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
        private async Task DoPositiveBalanceDepositWithdrawal(int accountId)
        {
            const int operationsNumber = 10;
            for (var i = 0; i < operationsNumber; i++)
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
