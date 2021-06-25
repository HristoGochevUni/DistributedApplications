using Microsoft.AspNetCore.Mvc;


namespace RigMag.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}