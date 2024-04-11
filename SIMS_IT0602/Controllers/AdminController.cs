using Microsoft.AspNetCore.Mvc;

namespace SIMS_IT0602.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
