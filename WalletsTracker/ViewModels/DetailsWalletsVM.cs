using System;
using System.Collections.Generic;

namespace WalletsTracker.ViewModels
{
    public class DetailsWalletsVM
    {
        public int Id { get; set; }
        public string NumberPhone { get; set; }
        public string Title { get; set; }
        public string Balance { get; set; }
        public List<TransactionVM> Transactions { get; set; } = new List<TransactionVM>();
    }
    public class TransactionVM
    {
        public int WalletId { get; set; }
        public int BalanceTransaction { get; set; }
        public string TransactionType { get; set; }
        public DateTime TransactionDateTime { get; set; }


    }
}
