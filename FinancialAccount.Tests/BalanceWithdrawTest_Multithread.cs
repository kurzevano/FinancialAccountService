using FinancialAccountService.Controllers;
using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAccount.Tests
{
    [TestFixture]
    public class BalanceWithdrawTest_Multithread
    {
        /// <summary>
        /// Стартовая сумма баланса у тестовых пользователей
        /// </summary>
        const decimal initialSum = 100;

        /// <summary>
        /// Тестовая сумма для снятия
        /// </summary>
        const decimal sumToWithdraw = 42;

        /// <summary>
        /// Количество потоков в тесте
        /// </summary>
        const int threadNumber = 10;

        /// <summary>
        /// Имя тестовой БД
        /// </summary>
        private string databaseName;

        /// <summary>
        /// Список тестовых пользователей
        /// </summary>
        List<User> fakesUsers;

        [SetUp]
        public void Setup()
        {
            databaseName = TestUtils.CreateTestDatabase(out fakesUsers, initialSum);
        }

        [TearDown]
        public void TearDown()
        {
            TestUtils.DeleteTestDatabase(databaseName);
        }

        /// <summary>
        /// Каждому пользователю один раз пополняет баланс, разные пользователи в разных потоках
        /// </summary>
        [Test]
        public void TestWithdraw_multipleDbContext()
        {
            // act

            Parallel.ForEach(fakesUsers, new ParallelOptions() { MaxDegreeOfParallelism = threadNumber }, user =>
            {
                using var dbContext = TestUtils.CreateDbContext(databaseName);

                var balanceController = new BalanceController(dbContext);
                balanceController.Withdraw(new FinancialAccountService.Dto.ChangeBalanceDto()
                {
                    UserId = user.Id,
                    Summ = sumToWithdraw
                }).GetAwaiter().GetResult();
            });

            // assert

            using var dbContext = TestUtils.CreateDbContext(databaseName);

            var balanceController = new BalanceController((FinancialAccountDbContext)dbContext);

            fakesUsers.ForEach(user => Assert.AreEqual(balanceController.GetBalance(user.Id).GetAwaiter().GetResult().Value, initialSum - sumToWithdraw));
        }

        /// <summary>
        /// У каждого пользователя многопоточно списывает сумму с баланса
        /// </summary>
        [Test]
        public void BalanceWithdrawTest()
        {
            // act
            foreach (var user in fakesUsers)
            {
                var result = Parallel.For(1, threadNumber + 1, (i, state) =>
                {
                    using var dbContext = TestUtils.CreateDbContext(databaseName);

                    var balanceController = new BalanceController(dbContext);
                    balanceController.Withdraw(new FinancialAccountService.Dto.ChangeBalanceDto()
                    {
                        UserId = user.Id,
                        Summ = i
                    }).GetAwaiter().GetResult();
                });
            }

            // assert

            // Общая сумма, которую сняли (формула арифметической прогрессии)
            var sumWithdraw = (1 + threadNumber) * threadNumber / 2;

            // В этом тесте не предусмотрена проверка достаточности средств при снятии
            Assert.IsTrue(initialSum >= sumWithdraw);

            // Остаток баланса
            var restBalance = initialSum - sumWithdraw;

            using var dbContext = TestUtils.CreateDbContext(databaseName);

            var balanceController = new BalanceController(dbContext);

            foreach (var user in fakesUsers)
            {
                var userBalance = balanceController.GetBalance(user.Id).GetAwaiter().GetResult().Value;
                Assert.AreEqual(restBalance, userBalance);
            }
        }
    }
}
