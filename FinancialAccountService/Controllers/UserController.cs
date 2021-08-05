using FinancialAccountService.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FinancialAccountDbContext _dbContext;

        public UserController(FinancialAccountDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        [HttpPost]
        public async void RegisterUser()
        {
            var user = new User()
            {
                FirstName = "Кабан",
                LastName = "Петров",
                MiddleName = "Иванович",
                CurrentBalance = new Balance(),
                DateBirth = new System.DateTime(),
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получает список зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            return _dbContext.Users.Include(x => x.CurrentBalance).ToList();
        }
    }
}