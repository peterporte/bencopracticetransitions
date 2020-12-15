using Microsoft.AspNetCore.Mvc;

namespace BencoPracticeTransitions.Controllers
{
    public class ThankYouController : Controller
    {
        public IActionResult Index()
        { 
            return View(nameof(Index));
        }
    }


}