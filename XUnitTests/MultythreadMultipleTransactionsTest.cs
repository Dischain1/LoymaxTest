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
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace XUnitTests
{
    public class MultiThreadMultipleTransactionsTest : UnitTestsWithInMemoryDb
    {
        // Emulating usage of scoped Context like in Controller
        private ITransactionService CreateTransactionService()
        {
            var newContext = CreateLoymaxTestContext();
            var accountService = new AccountService(newContext);
            return new TransactionService(newContext, new TransactionValidator(accountService));
        }

        private static readonly IRandomizerString FirstNameGenerator;
        private static readonly IRandomizerString LastNameGenerator;
        private static readonly IRandomizerDateTime DateOfBirthGenerator;
        private static readonly Random Random;

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

        [Theory]
        [Repeat(20)]
        public async Task TestFromTaskDescription(int iterationNumber)
        {
            //        -------------------------------------        Arrange        -------------------------------------
            const int accountsNumber = 50;
            var accountIdsList = await SeedAccounts(accountsNumber);


            //        -------------------------------------        Act        -------------------------------------
            Parallel.ForEach(accountIdsList, accountId =>
            {
                // 10 threads operating each user.
                // In total: 100 Deposits and 100 Withdrawals per user.
                // For each Deposit\Withdrawal pair the statement "Deposit - Withdrawal = 1" is true.
                // So expected balance of all users is 100. Expected transaction count is 200.
                var tasks = new Task[10];

                for (var i = 0; i < 10; i++)
                {
                    tasks[i] = new Task(async () => await DoPositiveBalanceDepositWithdrawal(accountId));
                    tasks[i].Start();
                }

                Task.WaitAll(tasks);
            });

            var accountService = new AccountService(Context);
            var balances = new List<decimal>();
            var transactionsNumber = new List<int>();
            foreach (var accountId in accountIdsList)
            {
                // ToDo wait all
                balances.Add(await accountService.CalculateBalance(accountId));
                transactionsNumber.Add(Context.Transactions.Count(x => x.AccountId == accountId));
            }


            //        -------------------------------------        Assert        -------------------------------------
            Assert.True(balances.TrueForAll(x => x == 100));
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


        private async Task DoPositiveBalanceDepositWithdrawal(int accountId)
        {
            const int operationsNumber = 10;
            for (var i = 0; i < operationsNumber; i++)
            {
                var deposit = Random.Next(1000) + 10;
                var withdrawal = deposit - 1; // Saving 1.0 every time

                var depositDto = new AddTransactionDto(accountId, deposit, TransactionType.Deposit);
                var withdrawalDto = new AddTransactionDto(accountId, withdrawal, TransactionType.Withdrawal);

                await CreateTransactionService().SaveTransaction(depositDto);
                await CreateTransactionService().SaveTransaction(withdrawalDto);
            }
        }

        public sealed class RepeatAttribute : Xunit.Sdk.DataAttribute
        {
            private readonly int count;

            public RepeatAttribute(int count)
            {
                if (count < 1)
                {
                    throw new System.ArgumentOutOfRangeException(
                        paramName: nameof(count),
                        message: "Repeat count must be greater than 0."
                    );
                }
                this.count = count;
            }

            public override System.Collections.Generic.IEnumerable<object[]> GetData(System.Reflection.MethodInfo testMethod)
            {
                foreach (var iterationNumber in Enumerable.Range(start: 1, count: this.count))
                {
                    yield return new object[] { iterationNumber };
                }
            }
        }
    }
}
