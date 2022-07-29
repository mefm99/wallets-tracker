using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WalletsTracker.Filter;
using WalletsTracker.Models;
using WalletsTracker.Repoistory;

namespace WalletsTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWalletDb _walletDb;

        public HomeController(ILogger<HomeController> logger, IWalletDb WalletDb)
        {
            _logger = logger;
            _walletDb = WalletDb;
        }
        [AuthorizeUser]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                bool login = _walletDb.Login(model);
                if (login)
                {
                    int userId = _walletDb.GetUserId(model.Username);
                    HttpContext.Session.SetString("UserIdKeyWalletApp", Convert.ToString(userId));
                    HttpContext.Session.SetString("UserNameWalletApp", Convert.ToString(model.Username));
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            ViewData["msgLogin"] = "password or username is invaild";
            return View(model);
        }
        [AuthorizeUser]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                bool checkUsername = _walletDb.CheckUserName(model.Username);
                if (checkUsername)
                {
                    ModelState.AddModelError("Username", "*Username is used");
                    return View(model);
                }
                bool checkRegister = _walletDb.Register(model);
                if (!checkRegister)
                {
                    ViewData["msgRegister"] = "Error, try again.";
                    return View(model);
                }
                if (checkRegister)
                {
                    int userId = _walletDb.GetUserId(model.Username);
                    HttpContext.Session.SetString("UserIdKeyWalletApp", Convert.ToString(userId));
                    HttpContext.Session.SetString("UserNameWalletApp", Convert.ToString(model.Username));
                    return RedirectToAction("Index", "Dashboard");
                }
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
