using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using WalletsTracker.Filter;
using WalletsTracker.Repoistory;
using WalletsTracker.ViewModels;

namespace WalletsTracker.Controllers
{
    [AuthorizeDashboard]
    public class DashboardController : Controller
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IWalletDb _walletDb;

        public DashboardController(ILogger<DashboardController> logger, IWalletDb WalletDb)
        {
            _logger = logger;
            _walletDb = WalletDb;
        }
        public IActionResult Index()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserIdKeyWalletApp"));
            ViewBag.AllBalanceWallets = _walletDb.AllBalanceWallets(userId);
            ViewBag.NumberWallets = _walletDb.NumberWallets(userId);
            List<DahboardWallets> data = _walletDb.DahboardWallets(userId);
            return View(data);
        }
        public IActionResult Wallets()
        {
            int userId = Convert.ToInt32(HttpContext.Session.GetString("UserIdKeyWalletApp"));
            List<WalletsVM> data = _walletDb.AllWallets(userId);
            return View(data);
        }
        public IActionResult AddWallet()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddWallet(AddWalletVM model)
        {
            if (ModelState.IsValid)
            {
                int blance = Convert.ToInt32(model.Balance);
                if (blance > 50000)
                {
                    ModelState.AddModelError("Balance", "*المبلغ مبالغ فيه");
                    return View(model);
                }
                model.UserID = Convert.ToInt32(HttpContext.Session.GetString("UserIdKeyWalletApp"));
                bool checkAdd = _walletDb.AddWallet(model);
                if (!checkAdd)
                {
                    ViewData["msg"] = "حدث خطأ أو ربما تكون المحفظة موجود بالفعل";
                    return View(model);
                }
                if (checkAdd)
                {
                    return RedirectToAction("Wallets", "Dashboard");
                }
            }
            return View(model);
        }

        public IActionResult DeleteWallet(int id)
        {
            if (id.ToString() != null)
            {
                WalletsVM data = _walletDb.Wallet(id);
                if (data != null)
                    return View(data);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteWallet")]
        public IActionResult _DeleteWallet(int id)
        {
            if (id.ToString() != null)
            {
                WalletsVM data = _walletDb.Wallet(id);
                if (data != null)
                    _walletDb.DeleteWallet(data.Id);
            }
            return RedirectToAction("Wallets");
        }
        public IActionResult DetailsWallet(int id)
        {
            if (id.ToString() != null)
            {
                WalletsVM data = _walletDb.Wallet(id);
                if (data != null)
                {
                    var Adata = _walletDb.DetailsWallets(id);
                    return View(Adata);
                }
            }
            return RedirectToAction("Wallets");
        }
        public IActionResult Withdrawal(int id)
        {
            if (id.ToString() != null)
            {
                WalletsVM data = _walletDb.Wallet(id);
                if (data != null)
                {
                    WithdrawalVM dataT = new WithdrawalVM
                    {
                        Id = data.Id,
                        NumberPhone = data.NumberPhone,
                    };
                    return View(dataT);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Withdrawal(WithdrawalVM model)
        {
            if (ModelState.IsValid)
            {
                WalletsVM data = _walletDb.Wallet(model.Id);
                if (data != null)
                {
                    int modelBalance = Convert.ToInt32(model.Balance);
                    int dataBalance = Convert.ToInt32(data.Balance);
                    if (_walletDb.WithdrawalTransctionToday(data.Id) == 0 || _walletDb.WithdrawalTransctionToday(data.Id) < modelBalance)
                    {
                        ModelState.AddModelError("Balance", "*لا يمكن سحب المبلغ");
                        return View(model);
                    }
                    if (modelBalance == 0)
                    {
                        ModelState.AddModelError("Balance", "*إحنا هنهرج؟");
                        return View(model);
                    }
                    if (modelBalance > 30001)
                    {
                        ModelState.AddModelError("Balance", "لا يمكن سحب المبلغ");
                        return View(model);
                    }
                    if (modelBalance > dataBalance)
                    {
                        ModelState.AddModelError("Balance", "*الرصيد في المحفظة لا يكفي");
                        return View(model);
                    }
                    else
                    {
                        bool check = _walletDb.Withdrawal(data.Id, modelBalance);
                        if (!check)
                        {
                            ViewData["msg"] = "حدث خطأ";
                            return View(model);
                        }
                        else
                        {
                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            return View(model);
        }
        public IActionResult Deposit(int id)
        {
            if (id.ToString() != null)
            {
                WalletsVM data = _walletDb.Wallet(id);
                if (data != null)
                {
                    DepositVM dataT = new DepositVM
                    {
                        Id = data.Id,
                        NumberPhone = data.NumberPhone,
                    };
                    return View(dataT);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Deposit(DepositVM model)
        {
            if (ModelState.IsValid)
            {
                WalletsVM data = _walletDb.Wallet(model.Id);
                if (data != null)
                {
                    int modelBalance = Convert.ToInt32(model.Balance);
                    int dataBalance = Convert.ToInt32(data.Balance);
                    if (_walletDb.DepositTransctionToday(data.Id) == 0 || _walletDb.DepositTransctionToday(data.Id) < modelBalance )      
                    {
                        ModelState.AddModelError("Balance", "*لا يمكن إيداع المبلغ");
                        return View(model);
                    }
                    if (modelBalance == 0)
                    {
                        ModelState.AddModelError("Balance", "*إحنا هنهرج؟");
                        return View(model);
                    }
                    if (dataBalance + modelBalance > 50000)
                    {
                        ModelState.AddModelError("Balance", "*المحفظة لن تكفي");
                        return View(model);
                    }
                    if (modelBalance > 30000)
                    {
                        ModelState.AddModelError("Balance", "*لا يمكن إيداع المبلغ");
                        return View(model);
                    }
                    bool check = _walletDb.Deposit(data.Id, modelBalance);
                    if (!check)
                    {
                        ViewData["msg"] = "حدث خطأ";
                        return View(model);
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(model);
        }
    }
}
