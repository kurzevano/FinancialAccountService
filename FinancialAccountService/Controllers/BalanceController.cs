using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinancialAccountService.Dto;
using FinancialAccountService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinancialAccountService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly FinancialAccountDbContext _dbContext;

        public BalanceController(FinancialAccountDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Получает текущий баланс пользователя
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <returns></returns>
        [HttpGet("Balance/{userId}")]
        public async Task<ActionResult<decimal>> GetBalance(int userId)
        {
            var user = await _dbContext.User.Include(x => x.CurrentBalance).FirstOrDefaultAsync(User => User.Id == userId);
            if (user == null)
            {
                return NotFound($"Пользователь с id  {userId} не найден");
            }

            var balance = user.CurrentBalance;
            if (balance == null)
            {
                return NotFound($"Не найдена информация о балансе для пользователя с id  {userId}");
            }

            return balance.Summ;
        }

        /// <summary>
        /// Начисляет сумму на счёт клиента
        /// </summary>
        /// <param name="changeBalanceDto"></param>
        /// <returns></returns>
        [HttpPost("deposit")]
        public async Task<ActionResult> Deposit(ChangeBalanceDto changeBalanceDto)
        {
            var userId = changeBalanceDto.UserId;
            var summ = changeBalanceDto.Summ;

            if (summ <= 0)
            {
                return ValidationProblem($"Неверно указана сумма");
            }

            var user = _dbContext.User.Include(x => x.CurrentBalance).FirstOrDefault(User => User.Id == userId);
            if (user == null)
            {
                return NotFound($"Пользователь с id  {userId} не найден");
            }

            var balance = user.CurrentBalance;
            if (balance == null)
            {
                user.CurrentBalance = new Balance();
                await _dbContext.SaveChangesAsync();
            }

            var balanceId = user.CurrentBalance.Id;

            _dbContext.BalanceTransaction.Add(new BalanceTransaction() { OperationTtype = Convert.ToBoolean(1), Summ = summ, BalanceId = balanceId });
            user.CurrentBalance.Summ += summ;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Списывает сумму со счёта клиента
        /// </summary>
        /// <param name="changeBalanceDto"></param>
        /// <returns></returns>
        [HttpPost("withdrawal")]
        public async Task<ActionResult> Withdraw(ChangeBalanceDto changeBalanceDto)
        {
            var userId = changeBalanceDto.UserId;
            var summ = changeBalanceDto.Summ;

            if (summ <= 0)
            {
                return ValidationProblem($"Неверно указана сумма");
            }

            var user = await _dbContext.User.Include(x => x.CurrentBalance).FirstOrDefaultAsync(User => User.Id == userId);
            if (user == null)
            {
                return NotFound($"Пользователь с id  {userId} не найден");
            }

            var balance = user.CurrentBalance;
            if (balance == null)
            {
                return NotFound($"Не найдена информация о балансе для пользователя с id  {userId}");
            }

            var isEnough = balance.Summ > summ;

            if (!isEnough)
            {
                return BadRequest("Недостаточно средств для списания");
            }

            var balanceId = user.CurrentBalance.Id;
            _dbContext.BalanceTransaction.Add(new BalanceTransaction() { OperationTtype = Convert.ToBoolean(0), Summ = summ, BalanceId = balanceId });
            user.CurrentBalance.Summ -= summ;

            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}