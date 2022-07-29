using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WalletsTracker.Common;
using WalletsTracker.Database;
using WalletsTracker.Models;
using WalletsTracker.ViewModels;

namespace WalletsTracker.Repoistory
{
    public class WalletDb : IWalletDb
    {
        private readonly AppDbContext _context;
        private readonly ILogger<WalletDb> _logger;
        public WalletDb(AppDbContext context,
            ILogger<WalletDb> logger)
        {
            _context = context;
            _logger = logger;
        }
        public bool AddWallet(AddWalletVM model)
        {
            try
            {
                if (CheckNumberPhone(model.NumberPhone))
                {
                    return false;
                }
                Wallet wallet = new Wallet
                {
                    IsDelete = false,
                    UserID = model.UserID,
                    Balance = model.Balance,
                    NumberPhone = model.NumberPhone,
                    Title = model.Title
                };
                _context.Wallets.Add(wallet);
                _context.SaveChanges();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public List<WalletsVM> AllWallets(int userId)
        {
            try
            {
                List<WalletsVM> AllData = new List<WalletsVM>();
                var wallets = _context.Wallets.Where(x => x.UserID == userId && x.IsDelete == false);
                foreach (var item in wallets)
                {
                    WalletsVM w = new WalletsVM
                    {
                        Id = item.Id,
                        Balance = item.Balance,
                        NumberPhone = item.NumberPhone,
                        Title = item.Title
                    };
                    AllData.Add(w);
                }
                return AllData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        private bool CheckNumberPhone(string number)
        {
            return _context.Wallets.Any(a => a.NumberPhone == number && a.IsDelete == false);
        }
        public bool CheckUserName(string uername)
        {
            return _context.Users.Any(a => a.Username == uername);
        }
        public int GetUserId(string uername)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Username == uername);
                if (user != null)
                    return user.Id;
                return 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return 0;
            }
        }
        public bool Login(LoginVM model)
        {
            try
            {
                var user = _context.Users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password && x.IsDelete == false);
                if (user != null)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public bool Register(RegisterVM model)
        {
            try
            {
                User user = new User
                {
                    IsDelete = false,
                    Password = model.Password,
                    Username = model.Username
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public string NumberWallets(int userId)
        {
            try
            {
                var wallets = _context.Wallets.Where(x => x.UserID == userId && x.IsDelete == false);
                return wallets.Count().ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        public string AllBalanceWallets(int userId)
        {
            try
            {
                int sum = 0;
                var wallets = _context.Wallets.Where(x => x.UserID == userId && x.IsDelete == false);
                foreach (var item in wallets)
                {
                    sum += Convert.ToInt32(item.Balance);
                }
                return sum.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        public int DepositTransctionToday(int Walletid)
        {
            var transactions = _context.Transactions.Where(a => a.WalletId == Walletid && a.TransactionType == LocalTransactionType.Deposit && a.IsDelete == false);
            if(transactions == null || transactions.Count() ==0)
            {
                return 30000;
            }
            else
            {
                int allDepositAtMonth = transactions.Where(a => a.TransactionDateTime.Year == LocalTime.Now().Year && a.TransactionDateTime.Month == LocalTime.Now().Month).Sum(a => a.BalanceTransaction);
                int allDepositAtDay = transactions.Where(a => a.TransactionDateTime.Year == LocalTime.Now().Year && a.TransactionDateTime.Month == LocalTime.Now().Month && a.TransactionDateTime.Day == LocalTime.Now().Day).Sum(a => a.BalanceTransaction);

                int ForMeAtDay = 30000 - allDepositAtDay;
                int ForMeAtMonth = 100000 - allDepositAtMonth;

                if(ForMeAtMonth >= 30000)
                {
                    return ForMeAtDay;
                }
                else
                {
                    if(ForMeAtDay > ForMeAtMonth)
                    {
                        return ForMeAtMonth;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }
        }
        public int WithdrawalTransctionToday(int Walletid)
        {
            var transactions = _context.Transactions.Where(a => a.WalletId == Walletid && a.TransactionType == LocalTransactionType.Withdrawal && a.IsDelete == false);
            if (transactions == null || transactions.Count() == 0)
            {
                return 30000;
            }
            else
            {
                int allWithdrawalAtMonth = transactions.Where(a => a.TransactionDateTime.Year == LocalTime.Now().Year && a.TransactionDateTime.Month == LocalTime.Now().Month).Sum(a => a.BalanceTransaction);
                int allWithdrawalAtDay = transactions.Where(a => a.TransactionDateTime.Year == LocalTime.Now().Year && a.TransactionDateTime.Month == LocalTime.Now().Month && a.TransactionDateTime.Day == LocalTime.Now().Day).Sum(a => a.BalanceTransaction);

                int ForMeAtDay = 30000 - allWithdrawalAtDay;
                int ForMeAtMonth = 100000 - allWithdrawalAtMonth;

                if (ForMeAtMonth >= 30000)
                {
                    return ForMeAtDay;
                }
                else
                {
                    if (ForMeAtDay > ForMeAtMonth)
                    {
                        return ForMeAtMonth;
                    }
                    else
                    {
                        return 0;
                    }

                }
            }

        }
        public List<DahboardWallets> DahboardWallets(int userId)
        {
            try
            {
                List<DahboardWallets> AllData = new List<DahboardWallets>();
                var wallets = _context.Wallets.Where(x => x.UserID == userId && x.IsDelete == false);
                foreach (var item in wallets)
                {
                    DahboardWallets dw = new DahboardWallets
                    {
                        WalletTitle = item.Title,
                        WalletNumber = item.NumberPhone,
                        WalletAmountAvailableToday = DepositTransctionToday(item.Id),
                        WalletId = item.Id,
                        WalletBalance = item.Balance,
                    };
                    AllData.Add(dw);
                }
                return AllData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        public WalletsVM Wallet(int WalletId)
        {
            try
            {
                var wallet = _context.Wallets.SingleOrDefault(x => x.Id == WalletId && x.IsDelete == false);
                if (wallet != null)
                {
                    WalletsVM data = new WalletsVM
                    {
                        Id = wallet.Id,
                        Balance = wallet.Balance,
                        NumberPhone = wallet.NumberPhone,
                        Title = wallet.Title
                    };
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
        public bool DeleteWallet(int WalletId)
        {
            try
            {
                var wallet = _context.Wallets.SingleOrDefault(x => x.Id == WalletId && x.IsDelete == false);
                if(wallet != null)
                {
                    wallet.IsDelete = true;
                    _context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public bool Withdrawal(int WalletId, int Blance)
        {
            try
            {
                var wallet = _context.Wallets.SingleOrDefault(x => x.Id == WalletId && x.IsDelete == false);
                if (wallet != null)
                {
                    int result = Convert.ToInt32(wallet.Balance) - Blance;
                    wallet.Balance = result.ToString();
                    Transaction transaction = new Transaction
                    {
                        BalanceTransaction = Blance,
                        WalletId = WalletId,
                        IsDelete = false,
                        TransactionDateTime = LocalTime.Now(),
                        TransactionType = LocalTransactionType.Withdrawal
                    };
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public bool Deposit(int WalletId, int Blance)
        {
            try
            {
                var wallet = _context.Wallets.SingleOrDefault(x => x.Id == WalletId && x.IsDelete == false);
                if (wallet != null)
                {
                    int result = Convert.ToInt32(wallet.Balance) + Blance;
                    wallet.Balance = result.ToString();
                    Transaction transaction = new Transaction
                    {
                        BalanceTransaction = Blance,
                        WalletId = WalletId,
                        IsDelete = false,
                        TransactionDateTime = LocalTime.Now(),
                        TransactionType = LocalTransactionType.Deposit
                    };
                    _context.Transactions.Add(transaction);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return false;
            }
        }
        public DetailsWalletsVM DetailsWallets(int WalletId)
        {
            try
            {
                var wallet = _context.Wallets.SingleOrDefault(x => x.Id == WalletId && x.IsDelete == false);
                DetailsWalletsVM AllData = new DetailsWalletsVM();
                var transactions = _context.Transactions.Where(x => x.WalletId == WalletId && x.IsDelete == false && x.TransactionDateTime.Year == LocalTime.Now().Year && x.TransactionDateTime.Month == LocalTime.Now().Month);
                foreach (var item in transactions)
                {
                    TransactionVM t = new TransactionVM
                    {
                          TransactionDateTime = item.TransactionDateTime,
                          BalanceTransaction = item.BalanceTransaction,
                          TransactionType = item.TransactionType,
                          WalletId = item.WalletId
                    };
                    AllData.Transactions.Add(t);
                }
                AllData.Balance = wallet.Balance;
                AllData.NumberPhone = wallet.NumberPhone;
                AllData.Title = wallet.Title;
                AllData.Id = wallet.Id;

                return AllData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return null;
            }
        }
    }
}
