using Homework_April_19.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Homework_April_19.Web.Controllers
{
    public class AccountController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Listing;Integrated Security=true;";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            if (TempData["message"]!=null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View();
        }
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            ListingsRepository repo = new(_connectionString);
            var user= repo.Login(email, password);
            if(user==null)
            {
                TempData["message"] = "Invalid login";
                return Redirect("/account/login");
            }

            //this code logs in the current user
            var claims = new List<Claim>
            {
                new Claim("user", email) //this will store the users email into the login cookie
            };

            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role")))
                .Wait();

            return Redirect("/home/index");
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(User user, string password)
        {
            ListingsRepository repo = new(_connectionString);
            repo.AddUser(user, password);
            return Redirect("/home/index");
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync().Wait();
            return Redirect("/home/index");
        }
    }
}
