using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PasswordWalletMVC.Models;
using PasswordWalletMVC.Services;
using System;
using System.Linq;

namespace PasswordWalletMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly WalletDbContext _dbContext;
        private readonly IPasswordService _passwordService;

        public AccountController(IUserService userService, WalletDbContext dbContext, IPasswordService passwordService)
        {
            _userService = userService;
            _dbContext = dbContext;
            _passwordService = passwordService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var allUsers = _userService.GetUsers();
            return View("~/Views/Account/Index.cshtml", allUsers);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Account/Register.cshtml");
        }


        [HttpPost]
        public IActionResult Register(User user, string submit)
        {
            if (ModelState.IsValid)
            {
                if (_userService.Register(user, submit) == true)
                {
                    return View("~/Views/Account/Login.cshtml");
                }
            }
            ModelState.AddModelError(string.Empty, "This user already exists in the database");
            return View("~/Views/Account/Register.cshtml");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/Account/Login.cshtml");
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            if (ModelState.IsValid)
            {
                if (_userService.Login(user) == -1)
                {
                    ModelState.AddModelError(string.Empty, "No such user in the database");
                    return View("~/Views/Account/Login.cshtml");
                }
            }
            var userId = _userService.Login(user);

            HttpContext.Session.SetString("UserId", userId.ToString());
            ViewBag.TotalStudents = HttpContext.Session.GetString("UserId");
            return View("LoggedIn");
        }
        [HttpPost]
        public IActionResult LoggedIn(User user, Password password)
        {
            var x = HttpContext.Session.GetString("UserId");
            _dbContext.Passwords.Add(password);
            password.UserId = Int32.Parse(HttpContext.Session.GetString("UserId"));
            _dbContext.SaveChanges();

            return View();
        }

        [HttpGet]
        public IActionResult LoggedIn()
        {
            return View("~/Views/Account/LoggedIn.cshtml");
        }


        [HttpGet]
        public IActionResult DisplayedPasswords()
        {
            var passwordsResult = _passwordService.GetPasswords(Int32.Parse(HttpContext.Session.GetString("UserId")));

            return View("~/Views/Account/DisplayedPasswords.cshtml", passwordsResult);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {           
            return View("~/Views/Account/ChangePassword.cshtml");
            //return RedirectToAction("ChangePassword", "Account");
        }


        [HttpPost]
        public IActionResult ChangePassword(User user)
        {
            _userService.ChangePassword(user, Int32.Parse(HttpContext.Session.GetString("UserId")));
            _dbContext.SaveChanges();

            return View("~/Views/Account/ChangePassword.cshtml");
            //return RedirectToAction("ChangePassword", "Account");
        }

        [HttpGet("/Account/HashPassword/{id}")]
        public IActionResult HashPassword(int id)
        {
            _passwordService.HashingPassword(id);

            return RedirectToAction("DisplayedPasswords", "Account");

            //return View();
        }

        [HttpGet("/Account/UnhashPassword/{id}")]
        public IActionResult UnhashPassword(int id)
        {
            _passwordService.UnhashingPassword(id);

            return RedirectToAction("DisplayedPasswords", "Account");
        }


    }
}