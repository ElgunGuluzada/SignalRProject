using _02082022_SignalRProject.DAL;
using _02082022_SignalRProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace _02082022_SignalRProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private static string CurUserName ;
        private readonly AppDbContext _context;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Chat()
        {
            List<AppUser> users = _userManager.Users.Where(u=>u.ConnectId!=null).ToList();
            ViewBag.ChatUsers = users;
            return View();
        }

        public async Task<IActionResult> CreateUser()
        {
            var result = await _userManager.CreateAsync(new AppUser { UserName = "_elgun",Email="elgunpg@code.edu.az",isOnline=false, Fullname = "Elgun" }, "12345@Li");
            var result1 = await _userManager.CreateAsync(new AppUser { UserName = "_ferid",Email="feridmma@code.edu.az", isOnline = false, Fullname = "Ferid" }, "12345@Li");
            var result2 = await _userManager.CreateAsync(new AppUser { UserName = "_xalid",Email="khalidsr@code.edu.az", isOnline = false, Fullname = "Xalid" }, "12345@Li");
            var result3 = await _userManager.CreateAsync(new AppUser { UserName = "_huseyn",Email="huseynfh@code.edu.az", isOnline = false, Fullname = "Huseyn" }, "12345@Li");
            var result4 = await _userManager.CreateAsync(new AppUser { UserName = "_tural",Email="turalkhj@code.edu.az", isOnline = false, Fullname = "Tural" }, "12345@Li");
            return Ok("User Created");

        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            var user = _userManager.FindByNameAsync(model.Username).Result;
            CurUserName = user.Email;
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            user.isOnline = true;
            _context.SaveChanges();
            ViewBag.UserName = user.Fullname;
            return RedirectToAction("chat","home");

        }
        public async Task<IActionResult> Logout()
        {
            AppUser appUser = await _userManager.FindByEmailAsync(CurUserName);
            appUser.isOnline = false;
            _context.SaveChanges();
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
