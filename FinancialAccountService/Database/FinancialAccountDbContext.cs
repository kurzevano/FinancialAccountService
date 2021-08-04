using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialAccountService.Database
{
    public class FinancialAccountDbContext : DbContext
    {
        /// <summary>
        /// Датасет пользователей
        /// </summary>
        public DbSet<User> User { get; set; }

        public FinancialAccountDbContext(DbContextOptions<FinancialAccountDbContext> options):base(options)
        {
            
        }
    }
}