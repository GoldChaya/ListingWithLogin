using Homework_April_19.Data;
using Homework_April_19.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Homework_April_19.Web.Controllers
{
    public class HomeController : Controller
    {

        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=Listing;Integrated Security=True";
        public IActionResult Index()
        {
            ListingsRepository repo = new(_connectionString);
            ListingViewModel vm = new();
            User user = null;
            vm.listings = repo.GetAll(user);
            if (User.Identity.Name != null)
            {
                var currentUserEmail = User.Identity.Name;
                var user2 = repo.GetByEmail(currentUserEmail);
                foreach (Listing l in vm.listings)
                {
                    if (l.UserId == user2.Id)
                    {
                        l.CanDelete = true;
                    }
                }
            }
            return View(vm);
        }
        [Authorize]
        public IActionResult Add()
        {
            ListingsRepository repo = new(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = repo.GetByEmail(currentUserEmail);
            return View(new AddPageViewModel
            {
                CurrentUserEmail = user.Email,
                CurrentUserId = user.Id,
                CurrentUserName = user.Name
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult Add(Listing listing, int userId, string name)
        {
            ListingsRepository repo = new(_connectionString);
            listing.Date = DateTime.Now;
            listing.UserId = userId;
            listing.Name = name;
            repo.Add(listing);
            return Redirect ("/home/index");
        }
        [Authorize]
        public IActionResult MyAccount()
        {
            ListingsRepository repo = new(_connectionString);
            ListingViewModel vm = new();
            var currentUserEmail = User.Identity.Name;
            var user = repo.GetByEmail(currentUserEmail);
            vm.listings=repo.GetAll(user);
            foreach (Listing l in vm.listings)
            {
                if (l.UserId == user.Id)
                {
                    l.CanDelete = true;
                }
            }

            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            ListingsRepository repo = new(_connectionString);
            repo.Delete(id);
            return Redirect("/home/index");
        }
    }
}