using _02082022_SignalRProject.DAL;
using _02082022_SignalRProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
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
        private readonly AppDbContext _context;
        private readonly IHubContext<ChatHub> _hubContext;



        public HomeController(ILogger<HomeController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, AppDbContext context, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _hubContext = hubContext;
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
            List<AppUser> users = _userManager.Users.ToList();
            ViewBag.ChatUsers = users;
            return View(users);
        }

        public async Task<IActionResult> CreateUser()
        {
            var  result = await _userManager.CreateAsync(new AppUser { UserName = "_elgun",Email="elgunpg@code.edu.az", Fullname = "Elgun" }, "12345@Li");
            var result1 = await _userManager.CreateAsync(new AppUser { UserName = "_ferid",Email="feridmma@code.edu.az", Fullname = "Ferid" }, "12345@Li");
            var result2 = await _userManager.CreateAsync(new AppUser { UserName = "_xalid",Email="khalidsr@code.edu.az", Fullname = "Xalid" }, "12345@Li");
            var result3 = await _userManager.CreateAsync(new AppUser { UserName = "_huseyn",Email="huseynfh@code.edu.az", Fullname = "Huseyn" }, "12345@Li");
            var result4 = await _userManager.CreateAsync(new AppUser { UserName = "_tural",Email="turalkhj@code.edu.az", Fullname = "Tural" }, "12345@Li");
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
            var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);
            return RedirectToAction("chat","home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> PrivateSend(string id)
        {
            AppUser appUser = await _userManager.FindByIdAsync(id);
            await _hubContext.Clients.Client(appUser.ConnectId).SendAsync("PrivateMessage");
            return RedirectToAction("chat");
        }
    }
}
