using System.Collections.Generic;
using WalletsTracker.Models;
using WalletsTracker.ViewModels;

namespace WalletsTracker.Repoistory
{
    public interface IWalletDb
    {
        bool Login(LoginVM model);
        bool Register(RegisterVM model);
        int GetUserId(string uername);
        bool CheckUserName(string uername);
        List<WalletsVM> AllWallets(int userId);
        bool AddWallet(AddWalletVM model);
        string NumberWallets(int userId);
        string AllBalanceWallets(int userId);
        WalletsVM Wallet(int WalletId);
        bool DeleteWallet(int WalletId);
        List<DahboardWallets> DahboardWallets (int userId);
        bool Withdrawal(int WalletId, int Blance);
        bool Deposit(int WalletId, int Blance);
        int DepositTransctionToday(int Walletid);
        int WithdrawalTransctionToday(int Walletid);
        DetailsWalletsVM DetailsWallets(int WalletId);

    }
}
