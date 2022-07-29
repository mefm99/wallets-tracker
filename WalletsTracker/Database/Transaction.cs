using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WalletsTracker.Database
{
    public class Transaction
    {
        public int Id { get; set; }
        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public int BalanceTransaction { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public bool IsDelete { get; set; }
        public Wallet Wallet { get; set; }

    }
}
