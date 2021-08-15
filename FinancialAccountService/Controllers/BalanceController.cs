using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
            var user = await _dbContext.User.AsNoTracking().Include(x => x.CurrentBalance).FirstOrDefaultAsync(User => User.Id == userId);
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

            var isSaved = false;

            var user = await _dbContext.User.Include(x => x.CurrentBalance).ThenInclude(b => b.BalanceTransactions).FirstOrDefaultAsync(User => User.Id == userId);
            if (user == null)
            {
                return NotFound($"Пользователь с id  {userId} не найден");
            }
            using var transaction = _dbContext.Database.BeginTransaction();
            var balance = user.CurrentBalance;
            if (balance == null)
            {
                user.CurrentBalance = new Balance();
            }

            var balanceId = user.CurrentBalance.Id;

            while (!isSaved)
            {
                try
                {
                    user.CurrentBalance.Summ += summ;
                    await _dbContext.SaveChangesAsync();
                    var bt = new BalanceTransaction()
                    {
                        OperationTtype = Convert.ToBoolean(1),
                        Summ = summ,
                        BalanceId = balanceId
                    };

                    _dbContext.BalanceTransaction.Add(bt);
                    await _dbContext.SaveChangesAsync();
                    transaction.Commit();
                    isSaved = true;
                    return Ok();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var cnt = _dbContext.BalanceTransaction.Count();
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Balance)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                proposedValues[property] = databaseValues[property];
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }

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

            using var transaction = _dbContext.Database.BeginTransaction();

            var isSaved = false;
            while (!isSaved)
            {
                try
                {
                    var balanceId = user.CurrentBalance.Id;
                    user.CurrentBalance.Summ -= summ;

                    await _dbContext.SaveChangesAsync();

                    _dbContext.BalanceTransaction.Add(new BalanceTransaction() { OperationTtype = Convert.ToBoolean(0), Summ = summ, BalanceId = balanceId });
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit();
                    isSaved = true;
                    return Ok();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is Balance)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();

                            foreach (var property in proposedValues.Properties)
                            {
                                proposedValues[property] = databaseValues[property];
                            }

                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(
                                "Don't know how to handle concurrency conflicts for "
                                + entry.Metadata.Name);
                        }
                    }
                }
            }

            return Ok();

        }
    }
}