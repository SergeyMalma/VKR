using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MyWebApp.Controllers
{
    public class HomeController : Controller
    {


        [Authorize(Roles = "admin, user")]
        public IActionResult Index()
        {
            string role = User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value;
            ViewBag.role = role;
            return View();
        }


        [Authorize(Roles = "admin")]
        public IActionResult About()
        {
            return Content("Вход только для администратора");
        }

        [Authorize(Roles = "admin")]
        public IActionResult Info()
        {
            //var model = new IndexModel{ TestItem=_context.TestItems.};
            return View();
        }
    }
}
