using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Moq;
using EntityFrameworkCoreMock;
using System.Collections.Generic;
using FinancialAccountService.Controllers;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Threading;
using System.Diagnostics;
using FinancialAccountService.Database;
using Microsoft.Extensions.DependencyInjection;

namespace FinancialAccount.Tests
{
    [TestFixture]
    public class BalanceControllerTests
    {

        private FinancialAccountDbContext _dbContext;
        private BalanceController _balanceController;
        private Random _random;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FinancialAccountDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            _dbContext = new FinancialAccountDbContext(options);
            var users = GetFakeUsers();

            _dbContext.User.AddRange(users.AsQueryable());
            _dbContext.SaveChanges();
            _balanceController = new BalanceController(_dbContext);
            _random = new Random();
        }

        [TearDown]
        public void Cleanup()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task Test1()
        {
            var controller = new BalanceController(_dbContext);
            var balance = await controller.GetBalance(1);
            
            Assert.AreEqual(0, balance.Value);
        }

        [Test]
        public void TestDepositAndWithdraw_withLock()
        {
            var locker = new Object();

            Parallel.ForEach(_dbContext.User, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, user =>
            {
                // locking object to prevent accessing the same DbContext
                lock (locker)
                {
                    _balanceController.Deposit(new FinancialAccountService.Dto.ChangeBalanceDto()
                    {
                        UserId = user.Id,
                        Summ = _random.Next(1, 100)
                    }).GetAwaiter().GetResult();
                }
                /*
                lock (locker)
                { 
                    _balanceController.Withdraw(new FinancialAccountService.Dto.ChangeBalanceDto()
                    {
                        UserId = user.Id,
                        Summ = _random.Next(1, 100)
                    }).GetAwaiter().GetResult();
                }*/
            });

            Assert.AreEqual(_dbContext.User.ToList().Count(), _dbContext.BalanceTransaction.ToList().Count());
        }

        /// <summary>
        /// Создаёт список пользователей для теста
        /// </summary>
        /// <returns></returns>
        private List<User> GetFakeUsers()
        {
            var users = new List<User>();
            for (int i = 0; i < 50; i++)
            {
                users.Add(new User
                {
                    Id = i + 1,
                    FirstName = $"Name{i}",
                    LastName = $"Lastaname{i}",
                    MiddleName = $"Lastaname{i}"
                });
            }

            return users;
        }
    }
}