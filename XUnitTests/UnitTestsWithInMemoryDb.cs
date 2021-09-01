using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;

namespace XUnitTests
{
    public class UnitTestsWithInMemoryDb : IDisposable
    {
        private static readonly DbContextOptionsBuilder<LoymaxTestContext> OptionsBuilder;
        protected readonly LoymaxTestContext Context;

        static UnitTestsWithInMemoryDb()
        {
            var dataBaseId = Guid.NewGuid();
            OptionsBuilder = new DbContextOptionsBuilder<LoymaxTestContext>();
            OptionsBuilder.EnableSensitiveDataLogging();
            OptionsBuilder.UseInMemoryDatabase(dataBaseId.ToString());
            OptionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        }

        protected UnitTestsWithInMemoryDb()
        {
            Context = CreateLoymaxTestContext();
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        protected static LoymaxTestContext CreateLoymaxTestContext()
        {
            return new LoymaxTestContext(OptionsBuilder.Options);
        }
    }
}