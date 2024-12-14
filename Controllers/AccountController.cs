using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel;

namespace WebApplication1.Controllers;
public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Attempt to sign in the user with the provided username and password
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                // If login is successful, redirect to the Home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // If login fails, add an error to the model state
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
        }

        // If model state is not valid or login fails, return the same view with error message
        return View(model);
    }
}
