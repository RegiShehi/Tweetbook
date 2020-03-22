using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TweetBook.Data;

namespace TweetBook
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var serviceScope = host.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();

                await dbContext.Database.MigrateAsync();

                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                if (!await roleManager.RoleExistsAsync("Poster"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Poster"));
                }

                var existingUser = await userManager.FindByEmailAsync("regi.shehi@test.com");

                if (existingUser == null)
                {
                    await userManager.CreateAsync(new IdentityUser
                    {
                        Email = "regi.shehi@test.com",
                        UserName = "regi.shehi@test.com"
                    }, "Password01!");
                }

                var createdUser = await userManager.FindByEmailAsync("regi.shehi@test.com");
                await userManager.AddToRoleAsync(createdUser, "Admin");
            }

            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
