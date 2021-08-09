using FinancialAccountService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinancialAccount.Tests
{
    internal static class TestUtils
    {
        internal const int FakeUsersCount = 50;

        /// <summary>
        /// Создаёт список пользователей для теста
        /// </summary>
        /// <returns></returns>
        internal static List<User> GetFakeUsers(decimal initialBalance = 0)
        {
            var users = new List<User>();
            for (int i = 0; i < FakeUsersCount; i++)
            {
                var user = new User
                {
                    Id = i + 1,
                    FirstName = $"Name{i}",
                    LastName = $"Lastaname{i}",
                    MiddleName = $"Lastaname{i}"
                };

                user.CurrentBalance = new Balance() { Id = user.Id, Summ = initialBalance };
                user.BalanceId = user.CurrentBalance.Id;

                users.Add(user);
            }

            return users;
        }

        /// <summary>
        /// Создаёт новую БД в памяти и наполняет тестовыми данными
        /// </summary>
        /// <param name="fakeUsers">Список тестовых пользователей</param>
        /// <param name="initialBalance">Начальный баланс каждого пользователя</param>
        /// <returns></returns>
        internal static string CreateTestDatabase(out List<User> fakeUsers, decimal initialBalance = 0)
        {
            fakeUsers = new List<User>();
            var dbName = Guid.NewGuid().ToString();
            using var dbContext = new FinancialAccountDbContext(new DbContextOptionsBuilder<FinancialAccountDbContext>()
           .UseInMemoryDatabase(databaseName: dbName)
           .Options);

            if (!dbContext.Set<User>().Any())
            {
                fakeUsers = TestUtils.GetFakeUsers(initialBalance);

                dbContext.User.AddRange(fakeUsers.AsQueryable());
                dbContext.SaveChanges();
            }

            return dbName;
        }
    }
}
