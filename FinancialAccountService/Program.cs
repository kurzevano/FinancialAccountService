using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAccountService.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinancialAccountService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();


            //using (var scope = host.Services.CreateScope())
            //{
            //    using (var db = scope.ServiceProvider.GetService<FinancialAccountDbContext>())
            //    {

            //        // Create
            //        Console.WriteLine("Inserting a new User");
            //        db.Add(new User { LastName = "Кукуев", FirstName = "Антон", CurrentBalance = new Balance() });
            //        db.SaveChanges();

            //        // Read
            //        Console.WriteLine("Querying for a user");
            //        var user = db.User
            //            .OrderBy(b => b.FirstName)
            //            .First();

            //        // Update
            //        Console.WriteLine("Updating the user and adding a balance");
            //        user.FirstName = "Марк";
            //        user.CurrentBalance =
            //            new Balance();
            //        db.SaveChanges();

            //        // Delete
            //        Console.WriteLine("Delete the user");
            //        db.Remove(user);
            //        db.SaveChanges();
            //    }
            //}
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
