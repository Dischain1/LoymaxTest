using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class LoymaxTestContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
