﻿using FinancialAccountService.Controllers;
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
    public class BalanceDepositTest_Multithread
    {
        /// <summary>
        /// Тестовая сумма для пополнения
        /// </summary>
        const decimal sumToDeposit = 42;

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
            databaseName = TestUtils.CreateTestDatabase(out fakesUsers);
        }

        /// <summary>
        /// Каждому пользователю один раз пополняет баланс, разные пользователи в разных потоках
        /// </summary>
        [Test]
        public void TestDeposit_multipleDbContext()
        {       
            Parallel.ForEach(fakesUsers, new ParallelOptions() { MaxDegreeOfParallelism = threadNumber }, user =>
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

            fakesUsers.ForEach(user => Assert.AreEqual(balanceController.GetBalance(user.Id).GetAwaiter().GetResult().Value, sumToDeposit));
        }

        /// <summary>
        /// Каждому пользователю многопоточно пополняет баланс
        /// </summary>
        [Test]
        public void BalanceDepositTest()
        {
            foreach (var user in fakesUsers)
            {
                var result = Parallel.For(1, threadNumber + 1, (i, state) =>
                {
                    using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
                    .UseInMemoryDatabase(databaseName: databaseName)
                    .Options);

                    var balanceController = new BalanceController(dbContext);
                    balanceController.Deposit(new FinancialAccountService.Dto.ChangeBalanceDto()
                    {
                        UserId = user.Id,
                        Summ = i
                    }).GetAwaiter().GetResult();
                });
            }

            // Формула арифметической прогрессии
            var sumTotal = (1 + threadNumber) * threadNumber / 2;

            using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
                .UseInMemoryDatabase(databaseName: databaseName).Options);

            var balanceController = new BalanceController(dbContext);

            foreach (var user in fakesUsers)
            {
                var userBalance = balanceController.GetBalance(user.Id).GetAwaiter().GetResult().Value;
                Assert.AreEqual(sumTotal, userBalance);
            }
        }
    }
}