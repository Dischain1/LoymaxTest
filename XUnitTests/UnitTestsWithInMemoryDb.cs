using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace XUnitTests
{
    public class UnitTestsWithInMemoryDb : IDisposable
    {
        private readonly Guid _databaseId;
        protected readonly LoymaxTestContext Context;

        protected UnitTestsWithInMemoryDb()
        {
            _databaseId = Guid.NewGuid();
            Context = CreateLoymaxTestContext();

            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }

        private LoymaxTestContext CreateLoymaxTestContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LoymaxTestContext>();
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseInMemoryDatabase(_databaseId.ToString());
            optionsBuilder.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            return new LoymaxTestContext(optionsBuilder.Options);
        }
    }
}