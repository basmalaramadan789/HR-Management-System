using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;
public class LandingController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
