using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;

namespace Data
{
    public class LoymaxTestContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public LoymaxTestContext(DbContextOptions<LoymaxTestContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity("Data.Models.Account", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("datetime2");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<string>("Panronimic")
                    .HasMaxLength(50)
                    .HasColumnType("nvarchar(50)");

                b.Property<DateTime>("RegistrationDate")
                    .HasColumnType("datetime2");

                b.HasKey("Id");

                b.ToTable("Accounts");
            });

            modelBuilder.Entity("Data.Models.Transaction", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<int>("AccountId")
                    .HasColumnType("int");

                b.Property<decimal>("Amount")
                    .HasPrecision(18, 2)
                    .HasColumnType("decimal(18,2)");

                b.Property<DateTime>("Date")
                    .HasColumnType("datetime2");

                b.Property<int>("Type")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("AccountId");

                b.ToTable("Transactions");
            });

            modelBuilder.Entity("Data.Models.Transaction", b =>
            {
                b.HasOne("Data.Models.Account", "Account")
                    .WithMany("Transactions")
                    .HasForeignKey("AccountId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Account");
            });

            modelBuilder.Entity("Data.Models.Account", b =>
            {
                b.Navigation("Transactions");
            });
        }
    }
}
