using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class LoymaxTestContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public LoymaxTestContext(DbContextOptions<LoymaxTestContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasData(
                new Account[]
                {
                    new Account
                    {
                        Id=1,
                        FirstName="Chester",
                        LastName="Bennington",
                        Panronimic= "Charles",
                        DateOfBirth = new DateTime(1976, 3, 20),
                        RegistrationDate = DateTime.Now
                    },
                });
        }
    }
}
