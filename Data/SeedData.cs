using Microsoft.AspNetCore.Identity;
using System.Net;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class SeedData
{
    public static async Task SeedUserAsync(UserManager<ApplicationUser> userManager)
    {
        var userEmail = "admin@admin.com";
        var password = "P@ssw0rd";

   
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user == null) 
        {
            user = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                
                Console.WriteLine("Error creating user.");
            }
        }
    }
}