using FinancialAccountService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAccountService.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly FinancialAccountDbContext _dbContext;
        public UserRepository(FinancialAccountDbContext context)
        {
            _dbContext = context;
        }
    }
}
