using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Enums;
using RandomDataGenerator.FieldOptions;
using RandomDataGenerator.Randomizers;
using Services.Accounts;
using Services.Accounts.Interfaces;
using Services.Accounts.Models;
using Services.Transactions;
using Services.Transactions.Interfaces;
using Services.Transactions.Models;
using Xunit;
using Xunit.Sdk;

namespace XUnitTests
{
    public class MultiThreadMultipleTransactionsTest : UnitTestsWithInMemoryDb
    {
        private readonly ITransactionService _transactionService;

        private readonly IAccountService  _accountService;
        private readonly IAccountValidator _accountValidator;

        private static readonly IRandomizerString FirstNameGenerator;
        private static readonly IRandomizerString LastNameGenerator;
        private static readonly IRandomizerDateTime DateOfBirthGenerator;
        private static readonly Random Random;

        public MultiThreadMultipleTransactionsTest()
        {
            _accountValidator = new AccountValidator();
            _accountService = new AccountService(Context);

            _transactionService = new TransactionService(Context, new TransactionValidator(_accountService));
        }

        static MultiThreadMultipleTransactionsTest()
        {
            DateOfBirthGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsDateTime
            {
                From = DateTime.Now.AddYears(-80),
                To = DateTime.Now.AddYears(-18),
                IncludeTime = false,
                UseNullValues = false
            });
            FirstNameGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsFirstName());
            LastNameGenerator = RandomizerFactory.GetRandomizer(new FieldOptionsLastName());

            Random = new Random();
        }

        [Fact]
        public async Task TestFromTaskDescription()
        {
            // Arrange
            const int accountsNumber = 50;
            var accountIdsList = await SeedAccounts(accountsNumber);
            // Getting 10 threads with 10 deposits and 10 withdrawals per user.
            // 10*(10+10)*50 = 10000 operations total.
            var tasks = CreateDepositsAndWithdrawalsTasks(accountIdsList);
            foreach (var task in tasks)
                task.Start();

            await Task.WhenAll(tasks);

            // Act
            // Getting all balances
            var balances = new List<decimal>();
            foreach (var accountId in accountIdsList)
            {
                // ToDo wait all
                balances.Add(await _accountService.CalculateBalance(accountId));
            }

            // Assert
            Assert.True(balances.TrueForAll(x => x == 100));
            
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
                    DateOfBirth = DateOfBirthGenerator.Generate()?.Date
                                  ?? DateTime.Now.Date.AddYears(-40)
                };

                var accountId = await _accountService.AddAccount(addAccountDto);
                accountIdsList.Add(accountId);
            }

            return accountIdsList;
        }

        private List<Task> CreateDepositsAndWithdrawalsTasks(List<int> accountIdsList, int threadsPerAccount = 10)
        {
            var allTasks = new List<Task>(); 
            foreach (var accountId in accountIdsList)
            {
                var tasks = new Task[threadsPerAccount];

                for (var i = 0; i < threadsPerAccount; i++)
                    tasks[i] = new Task(async () => await DoPositiveBalanceDepositWithdrawal(accountId));

                allTasks.AddRange(tasks); // 10 threads per user = 500 threads 
            }

            return allTasks;
        }

        private async Task DoPositiveBalanceDepositWithdrawal(int accountId)
        {
            const int operationsNumber = 10;
            for (int i = 0; i < operationsNumber; i++)
            {
                var deposit = Random.Next(1000) + 10;
                var withdrawal = deposit -1; // Saving 1.0 every time

                var depositDto = new AddTransactionDto(accountId, deposit, TransactionType.Deposit);
                var withdrawalDto = new AddTransactionDto(accountId, withdrawal, TransactionType.Withdrawal);

                await _transactionService.SaveTransaction(depositDto);
                await _transactionService.SaveTransaction(withdrawalDto);
            }
        }
    }
}
