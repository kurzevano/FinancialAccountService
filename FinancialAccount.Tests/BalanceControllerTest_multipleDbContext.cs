using FinancialAccountService.Controllers;
using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FinancialAccount.Tests
{
    [TestFixture]
    public class BalanceControllerTest_multipleDbContext
    {
        /// <summary>
        /// Имя тестовой БД
        /// </summary>
        private string databaseName;

        /// <summary>
        /// Список тестовых пользователей
        /// </summary>
        List<User> fakesUsers;

        /// <summary>
        /// Заполняет БД с указанным именем тестовыми данными
        /// </summary>
        private string CreateTestDatabase()
        {
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options);

            if (!dbContext.Set<User>().Any())
            {
                fakesUsers = TestUtils.GetFakeUsers();

                dbContext.User.AddRange(fakesUsers.AsQueryable());
                dbContext.SaveChanges();
            }

            return dbName;
        }

        [SetUp]
        public void Setup()
        {
           databaseName = CreateTestDatabase();
        }

        [Test]
        public void TestDepositAndWithdraw_multipleDbContext()
        {
            decimal sumToDeposit = 42; 
            Parallel.ForEach(fakesUsers, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, user =>
            {
                using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
                    .UseInMemoryDatabase(databaseName: databaseName)
                    .Options);

                var balanceController = new BalanceController(dbContext);
                balanceController.Deposit(new FinancialAccountService.Dto.ChangeBalanceDto()
                {
                    UserId = user.Id,
                    Summ = sumToDeposit
            }).GetAwaiter().GetResult();
            });

            using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
                    .UseInMemoryDatabase(databaseName: databaseName)
                    .Options);

            var balanceController = new BalanceController(dbContext);

            fakesUsers.ForEach(user => Assert.IsTrue(balanceController.GetBalance(user.Id).GetAwaiter().GetResult().Value == sumToDeposit));
        }
    }
}