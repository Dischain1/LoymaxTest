using System;

namespace Services.Transactions.Models
{
    public partial class AddTransactionDto
    {
        public DateTime Date { get; set; }
        public int Type { get; set; }
        public decimal Amount { get; set; }
    }
}
