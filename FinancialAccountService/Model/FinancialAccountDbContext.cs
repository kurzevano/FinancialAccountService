﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialAccountService.Model
{
    public class FinancialAccountDbContext : DbContext
    {
        /// <summary>
        /// Датасет пользователей
        /// </summary>
        public DbSet<User> User { get; set; }
        
        /// <summary>
        /// Датасет транзакций баланса
        /// </summary>
        public DbSet<BalanceTransaction> BalanceTransaction { get; set; }

        public FinancialAccountDbContext(DbContextOptions<FinancialAccountDbContext> options):base(options)
        {
            
        }
    }
}