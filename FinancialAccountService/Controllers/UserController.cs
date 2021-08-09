using AutoMapper;
using FinancialAccountService.Dto;
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

        private readonly IMapper _mapper;

        public UserController(FinancialAccountDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        /// <summary>
        /// Регистрирует нового пользователя
        /// </summary>
        [HttpPost]
        public async void RegisterUser(UserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Получает пользователя по id
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<UserDto> GetUser(int userId)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(user => user.Id == userId);
            return _mapper.Map<UserDto>(user);
        }

        /// <summary>
        /// Получает список зарегистрированных пользователей
        /// </summary>
        /// <returns></returns>
        [HttpGet("users")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            // Здесь намеренно делаем Include Balance и Include Transactions, чтобы посмотреть всё, что находится в базе
            return _dbContext.User.Include(x => x.CurrentBalance).ThenInclude(x => x.BalanceTransactions).ToList();
        }
    }
}