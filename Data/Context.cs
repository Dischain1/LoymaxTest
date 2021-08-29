using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class FinancialAccountsContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
    }
}
